using System.Text.Json;

namespace PSG2SaveEditor.Game;

/// <summary>One editable numeric field within a character's record (all u32 LE in PSG2).</summary>
public sealed class FieldDef
{
    public string Key { get; set; } = "";
    public string Label { get; set; } = "";
    /// <summary>Byte offset relative to the character record base.</summary>
    public int Offset { get; set; } = -1;
    public long Min { get; set; }
    public long Max { get; set; } = 9999;
}

/// <summary>A roster slot: canonical name + known class/"job" (display only).</summary>
public sealed class MemberDef
{
    public int Slot { get; set; }
    public string Name { get; set; } = "";
    /// <summary>Known class/role shown as "Job". Null/empty shows as "—".</summary>
    public string? Job { get; set; }
}

/// <summary>
/// Memory map for Phantasy Star Generation:2 party data, verified empirically against a
/// real PCSX2 save (eeMemory.bin). The character array is 8 records of 0x84 bytes each in
/// the canonical roster order; meseta is a lone u32 in a nearby header region. Offsets are
/// JSON-overridable via psg2_layout.json next to the executable.
/// </summary>
public sealed class Psg2Layout
{
    /// <summary>Absolute offset of character[0] (Eusis) in eeMemory.bin.</summary>
    public int CharArrayBase { get; set; } = 0x247700;
    /// <summary>Bytes between consecutive character records.</summary>
    public int CharStride { get; set; } = 0x84;
    public int CharCount { get; set; } = 8;

    /// <summary>Absolute offset of the party meseta (u32).</summary>
    public int MesetaOffset { get; set; } = 0x248314;
    public long MesetaMax { get; set; } = 9_999_999;

    /// <summary>
    /// Relative offset whose value is non-zero only for recruited/active members
    /// (the extended combat block) — used to flag who is actually in the party.
    /// </summary>
    public int RecruitedFlagOffset { get; set; } = 0x30;

    /// <summary>
    /// Absolute offset of the owned-items array. Each entry is [id u16][flags u16]
    /// (stride 4); scanning stops at the first empty entry. Holds every owned item,
    /// equipped or not. Flags: equipped iff (flags &amp; 0x3F) != 0x3F, then
    /// owner = flags &amp; 0x3F (character index) and slot = (flags >> 6) &amp; 7
    /// (0 Head, 1 Body, 2 R.Hand, 3 L.Hand, 4 Feet).
    /// </summary>
    public int InventoryOffset { get; set; } = 0x247B14;
    public int InventoryStride { get; set; } = 4;
    public int InventoryMaxEntries { get; set; } = 128;

    public List<FieldDef> Fields { get; set; } = new();
    public List<MemberDef> Members { get; set; } = new();

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public static Psg2Layout Default() => new()
    {
        Fields =
        {
            new() { Key = "level",      Label = "Level",        Offset = 0x00, Min = 1, Max = 99 },
            new() { Key = "experience", Label = "Experience",   Offset = 0x54, Min = 0, Max = 9_999_999 },
            new() { Key = "curHp",      Label = "HP (current)", Offset = 0x04, Min = 0, Max = 9999 },
            new() { Key = "maxHp",      Label = "HP (max)",     Offset = 0x08, Min = 1, Max = 9999 },
            new() { Key = "curTp",      Label = "TP (current)", Offset = 0x0C, Min = 0, Max = 9999 },
            new() { Key = "maxTp",      Label = "TP (max)",     Offset = 0x10, Min = 0, Max = 9999 },
            new() { Key = "attack",     Label = "Attack",       Offset = 0x14, Min = 0, Max = 999 },
            new() { Key = "defense",    Label = "Defense",      Offset = 0x18, Min = 0, Max = 999 },
            new() { Key = "stamina",    Label = "Stamina",      Offset = 0x1C, Min = 0, Max = 999 },
            new() { Key = "intellect",  Label = "Intellect",    Offset = 0x20, Min = 0, Max = 999 },
            new() { Key = "agility",    Label = "Agility",      Offset = 0x24, Min = 0, Max = 999 },
            new() { Key = "luck",       Label = "Luck",         Offset = 0x28, Min = 0, Max = 999 },
            new() { Key = "skill",      Label = "Skill",        Offset = 0x2C, Min = 0, Max = 999 },
        },
        // Canonical PSG2 roster order (matches the in-RAM array). Job is the character's
        // class where known; only Eusis (Agent) and Nei (—) are confirmed from the save.
        Members =
        {
            new() { Slot = 0, Name = "Eusis", Job = "Agent" },
            new() { Slot = 1, Name = "Nei",   Job = null },
            new() { Slot = 2, Name = "Rudo",  Job = null },
            new() { Slot = 3, Name = "Amy",   Job = null },
            new() { Slot = 4, Name = "Hugh",  Job = null },
            new() { Slot = 5, Name = "Anna",  Job = null },
            new() { Slot = 6, Name = "Kain",  Job = null },
            new() { Slot = 7, Name = "Shir",  Job = null },
        }
    };

    public static Psg2Layout Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "psg2_layout.json");
        if (File.Exists(path))
        {
            try
            {
                var loaded = JsonSerializer.Deserialize<Psg2Layout>(File.ReadAllText(path), JsonOpts);
                if (loaded is { Fields.Count: > 0, Members.Count: > 0 })
                {
                    return loaded;
                }
            }
            catch { /* fall back to baked-in default */ }
        }

        return Default();
    }
}
