using System.Buffers.Binary;
using PSG2SaveEditor.SaveFormat;

namespace PSG2SaveEditor.Game;

/// <summary>
/// High-level model: opens a .p2s, exposes meseta and the 8 character records for
/// editing, and repacks to a new file. Reads/writes eeMemory.bin in place.
/// </summary>
public sealed class Psg2Save
{
    private readonly P2SArchive _archive;
    private readonly byte[] _ee;
    private readonly Psg2Layout _layout;

    public string SourcePath { get; }
    public string? DetectedSerial { get; }
    public IReadOnlyList<Psg2Character> AllCharacters { get; }

    private Psg2Save(string path, P2SArchive archive, byte[] ee, Psg2Layout layout)
    {
        SourcePath = path;
        _archive = archive;
        _ee = ee;
        _layout = layout;
        DetectedSerial = FindSerial(ee);
        AllCharacters = layout.Members.Select(m => new Psg2Character(ee, layout, m, this)).ToList();
    }

    public static Psg2Save Open(string path, Psg2Layout layout)
    {
        var archive = P2SArchive.Load(path);
        if (!archive.Has("eeMemory.bin"))
        {
            throw new InvalidDataException("This .p2s has no eeMemory.bin region.");
        }

        return new Psg2Save(path, archive, archive.GetData("eeMemory.bin"), layout);
    }

    /// <summary>Recruited members (in the active party), in roster order.</summary>
    public IReadOnlyList<Psg2Character> RecruitedCharacters =>
        AllCharacters.Where(c => c.IsRecruited).ToList();

    /// <summary>True if the character array looks sane (Eusis level/HP in range).</summary>
    public bool StructureFound
    {
        get
        {
            int b = _layout.CharArrayBase;
            if (b < 0 || b + 0x0C > _ee.Length)
            {
                return false;
            }

            long level = BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(b + 0x00));
            long maxHp = BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(b + 0x08));
            return level is >= 1 and <= 99 && maxHp is >= 1 and <= 99999;
        }
    }

    public long Meseta
    {
        get
        {
            int at = _layout.MesetaOffset;
            return at >= 0 && at + 4 <= _ee.Length
                ? BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(at))
                : 0;
        }
        set
        {
            long v = Math.Clamp(value, 0, _layout.MesetaMax);
            int at = _layout.MesetaOffset;
            if (at >= 0 && at + 4 <= _ee.Length)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(_ee.AsSpan(at), (uint)v);
            }
        }
    }

    /// <summary>One owned item: id, name, and (if equipped) the owner + equip slot.</summary>
    public readonly record struct ItemEntry(int Index, int Id, int Flags)
    {
        public string Name => Psg2Items.Name(Id);
        public bool Equipped => (Flags & 0x3F) != 0x3F;
        public int OwnerIndex => Flags & 0x3F;
        public EquipSlot Slot => (EquipSlot)((Flags >> 6) & 7);
    }

    /// <summary>Every owned item (equipped pieces and bag items), in storage order.</summary>
    public IReadOnlyList<ItemEntry> Inventory()
    {
        var list = new List<ItemEntry>();
        int baseOff = _layout.InventoryOffset, stride = _layout.InventoryStride;
        for (int i = 0; i < _layout.InventoryMaxEntries; i++)
        {
            int o = baseOff + i * stride;
            if (o + 4 > _ee.Length)
            {
                break;
            }

            int id = BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(o));
            int flags = BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(o + 2));
            if (id == 0 && flags == 0)
            {
                break;   // first empty slot ends the list
            }

            list.Add(new ItemEntry(i, id, flags));
        }

        return list;
    }

    public void SaveAs(string destPath)
    {
        _archive.SetData("eeMemory.bin", _ee);
        _archive.Save(destPath);
    }

    /// <summary>Best-effort PS2 serial detection (SLxS_nnn.nn) for the status bar.</summary>
    private static string? FindSerial(byte[] ee)
    {
        for (int i = 0; i < ee.Length - 11; i++)
        {
            if (ee[i] != (byte)'S' || ee[i + 1] != (byte)'L')
            {
                continue;
            }

            char c2 = (char)ee[i + 2], c3 = (char)ee[i + 3];
            if (c2 is 'U' or 'E' or 'P' or 'A' && c3 is 'S' or 'L' or 'M' or 'B'
                && ee[i + 4] == (byte)'_' && char.IsDigit((char)ee[i + 5]))
            {
                return System.Text.Encoding.ASCII.GetString(ee, i, 11);
            }
        }

        return null;
    }
}
