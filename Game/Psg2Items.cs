using System.Text.Json;

namespace PSG2SaveEditor.Game;

/// <summary>Equipment slot, encoded as (flags >> 6) &amp; 7 in an inventory entry.</summary>
public enum EquipSlot { Head = 0, Body = 1, RightHand = 2, LeftHand = 3, Feet = 4 }

/// <summary>
/// Item-ID → name table for Phantasy Star Generation 2, mapped empirically from save
/// diffs. The built-in table can be extended/overridden at runtime by dropping a
/// <c>psg2_items.json</c> next to the executable — a simple { "id": "Name", ... } map —
/// so new items can be named without rebuilding. Unknown IDs render as "Item #N".
/// </summary>
public static class Psg2Items
{
    private static readonly Dictionary<int, string> BuiltIn = new()
    {
        // Weapons
        { 1,     "Knife" },
        { 2,     "Ceramic Knife" },
        { 3,     "Laser Knife" },      // 1-handed (dual-wieldable)
        { 9,     "Scalpel" },
        { 11,    "Sword" },            // 2-handed
        { 12,    "Ceramic Sword" },    // 2-handed
        { 21,    "Poison Shot" },      // 1-handed (dual-wieldable)
        { 28,    "Shotgun" },          // 2-handed (occupies right hand only)
        { 36,    "Needle Gun" },
        { 53,    "Steel Claw" },
        { 55,    "Ceramic Claw" },     // 1-handed (dual-wieldable)
        { 16404, "Silent Shot" },      // 2-handed; raw id is 0x4014 (base 0x14 + a high-byte attribute)
        // Head
        { 59,    "Headgear" },
        { 60,    "Fiberglass Gear" },
        { 61,    "Titanium Gear" },
        { 66,    "Ribbon" },
        { 67,    "Silver Ribbon" },
        { 69,    "Silver Crown" },
        // Body
        { 82,    "Carbon Suit" },
        { 83,    "Carbon Vest" },
        { 84,    "Fiberglass Vest" },
        { 86,    "Fiberglass Mantle" },
        { 93,    "Titanium Armor" },
        // Feet
        { 128,   "Leather Shoes" },
        { 129,   "Espadrilles" },
        // Consumables
        { 138,   "Monomate" },
        { 139,   "Dimate" },
        { 140,   "Trimate" },
        { 141,   "Monofluid" },
        { 144,   "Antidote" },
        { 146,   "Star Atomizer" },
        { 148,   "Traveling Ocarina" },
        { 149,   "Hesitant Ocarina" },
        // Key items
        { 181,   "Container Key" },
        { 182,   "Tube Key" },
    };

    public static IReadOnlyDictionary<int, string> Names { get; } = LoadMerged();

    public static string Name(int id) =>
        Names.TryGetValue(id, out var n) ? n : $"Item #{id}";

    /// <summary>
    /// IDs of two-handed weapons. The save stores these only in the right-hand slot, but
    /// the game fills both hands, so the UI mirrors them into the left hand for display.
    /// </summary>
    public static readonly HashSet<int> TwoHanded = new() { 11, 12, 28, 16404 }; // Sword, Ceramic Sword, Shotgun, Silent Shot

    public static bool IsTwoHanded(int id) => TwoHanded.Contains(id);

    private static Dictionary<int, string> LoadMerged()
    {
        var map = new Dictionary<int, string>(BuiltIn);
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, "psg2_items.json");
            if (File.Exists(path))
            {
                var overrides = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
                if (overrides is not null)
                {
                    foreach (var (k, v) in overrides)
                    {
                        if (int.TryParse(k, out int id) && !string.IsNullOrWhiteSpace(v))
                        {
                            map[id] = v;   // user file wins over built-in
                        }
                    }
                }
            }
        }
        catch { /* ignore a malformed override file; keep built-in names */ }

        return map;
    }
}
