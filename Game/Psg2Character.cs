using System.Buffers.Binary;

namespace PSG2SaveEditor.Game;

/// <summary>A live view over one character record in eeMemory.bin.</summary>
public sealed class Psg2Character
{
    private readonly byte[] _ee;
    private readonly Psg2Layout _layout;
    private readonly MemberDef _member;
    private readonly Psg2Save _save;

    public Psg2Character(byte[] ee, Psg2Layout layout, MemberDef member, Psg2Save save)
    {
        _ee = ee;
        _layout = layout;
        _member = member;
        _save = save;
    }

    /// <summary>This character's equipped item in each slot (null = empty), Head→Feet.</summary>
    public Psg2Save.ItemEntry? Equipped(EquipSlot slot)
    {
        foreach (var it in _save.Inventory())
        {
            if (it.Equipped && it.OwnerIndex == _member.Slot && it.Slot == slot)
            {
                return it;
            }
        }

        return null;
    }

    public int Slot => _member.Slot;
    public string Name => _member.Name;
    public string Job => string.IsNullOrEmpty(_member.Job) ? "—" : _member.Job!;

    private int RecordBase => _layout.CharArrayBase + _member.Slot * _layout.CharStride;

    /// <summary>
    /// True if this character is in the active party. Eusis and Nei are always present;
    /// any other member is considered recruited once the live inventory holds gear equipped
    /// to them (the +0x30 combat block stays 0 until a character first fights, so it isn't a
    /// reliable membership flag for freshly-joined members like Rudo).
    /// </summary>
    public bool IsRecruited =>
        _member.Slot <= 1 || _save.Inventory().Any(it => it.Equipped && it.OwnerIndex == _member.Slot);

    public bool TryGet(string key, out long value)
    {
        value = 0;
        var f = _layout.Fields.FirstOrDefault(x => x.Key == key);
        if (f is null)
        {
            return false;
        }

        int at = RecordBase + f.Offset;
        if (at < 0 || at + 4 > _ee.Length)
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(at));
        return true;
    }

    public void Set(string key, long value)
    {
        var f = _layout.Fields.FirstOrDefault(x => x.Key == key);
        if (f is null)
        {
            return;
        }

        value = Math.Clamp(value, f.Min, f.Max);
        int at = RecordBase + f.Offset;
        if (at >= 0 && at + 4 <= _ee.Length)
        {
            BinaryPrimitives.WriteUInt32LittleEndian(_ee.AsSpan(at), (uint)value);
        }
    }
}
