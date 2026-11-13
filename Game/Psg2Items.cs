using System.Text.Json;

namespace PSG2SaveEditor.Game;

/// <summary>Equipment slot, encoded as (flags >> 6) &amp; 7 in an inventory entry.</summary>
public enum EquipSlot { Head = 0, Body = 1, RightHand = 2, LeftHand = 3, Feet = 4 }

/// <summary>
/// Item-ID → name table for Phantasy Star Generation 2.
/// <para>
/// IDs 1–185 come straight from the game's own item-definition table in the SLPM-625.53
/// executable (0x20-byte records at ELF file offset 0x18B1B0; the record index is the
/// item ID, word 0 points at the Japanese name, u16 at +4 is the equip kind:
/// 0 item, 1 one-handed, 2 two-handed, 3 head, 4 shield, 5 body, 6 feet).
/// English names come from the English fan-translation patch's own item table: names
/// without a (?) are byte-for-byte what the patch displays. Names marked (?) contain a
/// dictionary-compressed word that isn't decoded yet, so that word is a best-effort
/// translation (override it via psg2_items.json if the game shows something different).
/// </para>
/// <para>
/// A saved item ID can carry flag bits above the base ID (e.g. 0x0429 = Fire Slicer,
/// 0x4014 = Silent Shot); <see cref="BaseId"/> strips them before lookup.
/// </para>
/// The table can be extended/overridden at runtime by a <c>psg2_items.json</c> next to the
/// executable — a simple { "id": "Name", ... } map keyed by base ID.
/// </summary>
public static class Psg2Items
{
    /// <summary>Mask for the base item ID; higher bits are per-item flags.</summary>
    public const int IdMask = 0xFF;

    public static int BaseId(int rawId) => rawId & IdMask;

    private static readonly Dictionary<int, string> BuiltIn = new()
    {
        { 1,   "Knife" },                     // Weapon (1H)  ナイフ
        { 2,   "Ceramic Knife" },             // Weapon (1H)  セラミックナイフ
        { 3,   "Laser Knife" },               // Weapon (1H)  レーザーナイフ
        { 4,   "Silver Knife" },              // Weapon (1H)  シルバーナイフ
        { 5,   "Dagger" },                    // Weapon (1H)  ダガー  (?)
        { 6,   "Ceramic Dagger" },            // Weapon (1H)  セラミックダガー  (?)
        { 7,   "Wind Dagger" },               // Weapon (1H)  ウィンドダガー  (?)
        { 8,   "Laconia Dagger" },            // Weapon (1H)  ラコニアダガー  (?)
        { 9,   "Scalpel" },                   // Weapon (1H)  メス
        { 10,  "Laser Scalpel" },             // Weapon (1H)  レーザーメス
        { 11,  "Sword" },                     // Weapon (2H)  ソード
        { 12,  "Ceramic Sword" },             // Weapon (2H)  セラミックソード
        { 13,  "Flame Sword" },               // Weapon (2H)  フレイムソード  (?)
        { 14,  "Laser Sword" },               // Weapon (2H)  レーザーソード
        { 15,  "Anger Sword" },               // Weapon (2H)  怒りの剣
        { 16,  "Plasma Sword" },              // Weapon (2H)  プラズマソード  (?)
        { 17,  "Laconia Sword" },             // Weapon (2H)  ラコニアソード  (?)
        { 18,  "Chain Sword" },               // Weapon (2H)  チェインソード  (?)
        { 19,  "Nei Sword" },                 // Weapon (2H)  ネイソード  (?)
        { 20,  "Silent Shot" },               // Weapon (2H)  サイレントショット
        { 21,  "Poison Shot" },               // Weapon (1H)  ポイズンショット
        { 22,  "Acid Shot" },                 // Weapon (1H)  アシドショット
        { 23,  "Laser Shot" },                // Weapon (2H)  レーザーショット
        { 24,  "Napalm Shot" },               // Weapon (2H)  ナパームショット  (?)
        { 25,  "Nei Shot" },                  // Weapon (2H)  ネイショット  (?)
        { 26,  "Sonic Gun" },                 // Weapon (1H)  ソニックガン  (?)
        { 27,  "Wave Gun" },                  // Weapon (1H)  ウェーブガン  (?)
        { 28,  "Shotgun" },                   // Weapon (2H)  ショットガン
        { 29,  "Vulcan" },                    // Weapon (2H)  バルカン  (?)
        { 30,  "Laser Vulcan" },              // Weapon (2H)  レーザーバルカン  (?)
        { 31,  "Pulse Vulcan" },              // Weapon (2H)  パルスバルカン  (?)
        { 32,  "Cannon" },                    // Weapon (2H)  カノン
        { 33,  "Laser Cannon" },              // Weapon (2H)  レーザーカノン
        { 34,  "Pulse Cannon" },              // Weapon (2H)  パルスカノン  (?)
        { 35,  "Plasma Cannon" },             // Weapon (2H)  プラズマカノン  (?)
        { 36,  "Needle Gun" },                // Weapon (1H)  ニードルガン
        { 37,  "Boomerang" },                 // Weapon (1H)  ブーメラン  (?)
        { 38,  "Slicer" },                    // Weapon (1H)  スライサー
        { 39,  "Titanium Slicer" },           // Weapon (1H)  チタンスライサー
        { 40,  "Laser Slicer" },              // Weapon (1H)  レーザースライサー
        { 41,  "Fire Slicer" },               // Weapon (1H)  ファイアスライサー
        { 42,  "Ice Slicer" },                // Weapon (1H)  アイススライサー  (?)
        { 43,  "Laconia Slicer" },            // Weapon (1H)  ラコニアスライサー  (?)
        { 44,  "Nei Slicer" },                // Weapon (1H)  ネイスライサー  (?)
        { 45,  "Whip" },                      // Weapon (1H)  ムチ  (?)
        { 46,  "Lightning Whip" },            // Weapon (1H)  電光のムチ  (?)
        { 47,  "Blazing Cane" },              // Weapon (1H)  火炎の杖
        { 48,  "Freezing Cane" },             // Weapon (1H)  氷結の杖
        { 49,  "Windblade Cane" },            // Weapon (1H)  風刃の杖  (?)
        { 50,  "Steel Scale" },               // Weapon (1H)  スチールスケール  (?)
        { 51,  "Ceramic Scale" },             // Weapon (1H)  セラミックスケール  (?)
        { 52,  "Laconia Scale" },             // Weapon (1H)  ラコニアスケール  (?)
        { 53,  "Steel Claw" },                // Weapon (1H)  スチールクロー
        { 54,  "Saber Claw" },                // Weapon (1H)  サーベルクロー  (?)
        { 55,  "Ceramic Claw" },              // Weapon (1H)  セラミッククロー
        { 56,  "Laser Claw" },                // Weapon (1H)  レーザークロー
        { 57,  "Silent Claw" },               // Weapon (1H)  サイレントクロー
        { 58,  "Animal Claw" },               // Weapon (1H)  アニマルクロー
        { 59,  "Headgear" },                  // Head         ヘッドギア
        { 60,  "Fiberglass Gear" },           // Head         グラスギア
        { 61,  "Titanium Gear" },             // Head         チタンギア
        { 62,  "Crescent Gear" },             // Head         クレセントギア  (?)
        { 63,  "Storm Gear" },                // Head         ストームギア
        { 64,  "Ceramic Gear" },              // Head         セラミックギア
        { 65,  "Laconia Gear" },              // Head         ラコニアギア  (?)
        { 66,  "Ribbon" },                    // Head         リボン
        { 67,  "Silver Ribbon" },             // Head         シルバーリボン
        { 68,  "Jeweled Ribbon" },            // Head         ジュエルリボン
        { 69,  "Silver Crown" },              // Head         シルバークラウン
        { 70,  "Jeweled Crown" },             // Head         ジュエルクラウン
        { 71,  "Snow Crown" },                // Head         雪の冠  (?)
        { 72,  "Thunder Crown" },             // Head         雷の冠
        { 73,  "Nei Crown" },                 // Head         ネイクラウン  (?)
        { 74,  "Titanium Helmet" },           // Head         チタンメット
        { 75,  "Laconia Helmet" },            // Head         ラコニアメット  (?)
        { 76,  "Nei Helmet" },                // Head         ネイメット  (?)
        { 77,  "Wind Bandana" },              // Head         風のバンダナ  (?)
        { 78,  "Rainbow Bandana" },           // Head         虹のバンダナ  (?)
        { 79,  "Gale Bandana" },              // Head         疾風のバンダナ  (?)
        { 80,  "Magic Hat" },                 // Head         マジックハット  (?)
        { 81,  "Mogic Hat" },                 // Head         モジックハット  (?)
        { 82,  "Carbon Suit" },               // Body         カーボンスーツ
        { 83,  "Carbon Vest" },               // Body         カーボンベスト
        { 84,  "Fiberglass Vest" },           // Body         グラスベスト
        { 85,  "Fiberglass Coat" },           // Body         グラスコート  (?)
        { 86,  "Fiberglass Mantle" },         // Body         グラスマント
        { 87,  "White Mantle" },              // Body         ホワイトマント  (?)
        { 88,  "Titanium Chestplate" },       // Body         チタンチェスト
        { 89,  "Ceramic Chestplate" },        // Body         セラミックチェスト
        { 90,  "Zirconium Chestplate" },      // Body         ジルコニウムチェスト  (?)
        { 91,  "Crystal Chestplate" },        // Body         クリスタチェスト  (?)
        { 92,  "Laconia Armor" },             // Body         ラコニアチェスト  (?)
        { 93,  "Titanium Armor" },            // Body         チタンアーマー
        { 94,  "Ceramic Armor" },             // Body         セラミックアーマー
        { 95,  "Zirconium Armor" },           // Body         ジルコニウムアーマー  (?)
        { 96,  "Fibrillae" },                 // Body         フィブリラ
        { 97,  "Titanium Fibrillae" },        // Body         チタンフィブリラ
        { 98,  "Ceramic Fibrillae" },         // Body         セラミックフィブリラ
        { 99,  "Laconia Fibrillae" },         // Body         ラコニアフィブリラ  (?)
        { 100, "Titanium Harness" },          // Body         チタンハーニッシュ  (?)
        { 101, "Crystal Harness" },           // Body         クリスタハーニッシュ  (?)
        { 102, "Laconia Harness" },           // Body         ラコニアハーニッシュ  (?)
        { 103, "Nei Harness" },               // Body         ネイハーニッシュ  (?)
        { 104, "Crystal Field" },             // Body         クリスタフィールド  (?)
        { 105, "Plasma Field" },              // Body         プラズマフィールド  (?)
        { 106, "Nei Field" },                 // Body         ネイフィールド  (?)
        { 107, "Amber Mantle" },              // Body         琥珀の法衣  (?)
        { 108, "Prison Uniform" },            // Body         囚人服
        { 109, "Carbon Shield" },             // Shield       カーボンシールド  (?)
        { 110, "Fiberglass Shield" },         // Shield       グラスシールド  (?)
        { 111, "Titanium Shield" },           // Shield       チタンシールド  (?)
        { 112, "Mirror Shield" },             // Shield       ミラーシールド  (?)
        { 113, "Ceramic Shield" },            // Shield       セラミックシールド  (?)
        { 114, "Serene Shield" },             // Shield       静謐の盾  (?)
        { 115, "Laser Shield" },              // Shield       レーザーシールド  (?)
        { 116, "Laconia Shield" },            // Shield       ラコニアシールド  (?)
        { 117, "Nei Shield" },                // Shield       ネイシールド  (?)
        { 118, "Carbon Armel" },              // Shield       カーボンエーメル  (?)
        { 119, "Fiberglass Armel" },          // Shield       グラスエーメル  (?)
        { 120, "Titanium Armel" },            // Shield       チタンエーメル  (?)
        { 121, "Mirror Armel" },              // Shield       ミラーエーメル  (?)
        { 122, "Ceramic Armel" },             // Shield       セラミックエーメル  (?)
        { 123, "Luminous Armel" },            // Shield       ルミナスエーメル  (?)
        { 124, "Laconia Armel" },             // Shield       ラコニアエーメル  (?)
        { 125, "Nei Armel" },                 // Shield       ネイエーメル  (?)
        { 126, "Green Sleeves" },             // Shield       グリーンスリーブ  (?)
        { 127, "Sincere Sleeves" },           // Shield       誠意の袖  (?)
        { 128, "Leather Shoes" },             // Feet         レザーシューズ
        { 129, "Espadrilles" },               // Feet         エスパドリュー
        { 130, "Leather Boots" },             // Feet         レザーブーツ
        { 131, "Knife Boots" },               // Feet         ナイフブーツ
        { 132, "Long Boots" },                // Feet         ロングブーツ
        { 133, "Heilsam Boots" },             // Feet         ハイルザムブーツ  (?)
        { 134, "Schnella Boots" },            // Feet         シュネラブーツ  (?)
        { 135, "Black Boots" },               // Feet         ブラックブーツ  (?)
        { 136, "White Boots" },               // Feet         ホワイトブーツ  (?)
        { 137, "Garda Boots" },               // Feet         ガーダブーツ  (?)
        { 138, "Monomate" },                  // Item         モノメイト
        { 139, "Dimate" },                    // Item         ディメイト
        { 140, "Trimate" },                   // Item         トリメイト
        { 141, "Monofluid" },                 // Item         モノフルイド
        { 142, "Difluid" },                   // Item         ディフルイド
        { 143, "Trifluid" },                  // Item         トリフルイド
        { 144, "Antidote" },                  // Item         アンティポイズン
        { 145, "Moon Atomizer" },             // Item         ムーンアトマイザー  (?)
        { 146, "Star Atomizer" },             // Item         スターアトマイザー
        { 147, "Sol Atomizer" },              // Item         ソルアトマイザー  (?)
        { 148, "Traveling Ocarina" },         // Item         旅のオカリナ
        { 149, "Hesitant Ocarina" },          // Item         迷いのオカリナ
        { 150, "Covert Ocarina" },            // Item         忍びのオカリナ
        { 151, "Charming Ocarina" },          // Item         魅惑のオカリナ  (?)
        { 152, "Shortcake" },                 // Item         ショートケーキ  (?)
        { 153, "Mont Blanc Cake" },           // Item         モンブランケーキ  (?)
        { 154, "Fruit Cake" },                // Item         フルーツタルト  (?)
        { 155, "Chiffon Cake" },              // Item         シフォンケーキ  (?)
        { 156, "Naura Shortcake" },           // Item         Ｓケーキ・ナウラ風  (?)
        { 157, "LandMaster" },                // Item         ランドマスター
        { 158, "Jet Scooter" },               // Item         ジェットスクーター
        { 159, "IceDecker" },                 // Item         アイスデッカー  (?)
        { 160, "Dynamite" },                  // Item         ダイナマイト
        { 161, "Atlas" },                     // Item         アトラス
        { 162, "Ransom Note" },               // Item         脅迫状
        { 163, "System Recorder" },           // Item         システムレコーダー
        { 164, "Maerla Leaves" },             // Item         マルエラリーブ
        { 165, "Maerla Gum" },                // Item         マルエラガム
        { 166, "Aeroprism" },                 // Item         エアロプリズム
        { 167, "Plasma Ring" },               // Item         プラズマリング  (?)
        { 168, "Healing Ring" },              // Item         ヒールリング
        { 169, "Golden Stone" },              // Item         黄金の石
        { 170, "Enhancer" },                  // Item         ちから・もりもりん
        { 171, "Main Key" },                  // Item         メインキー
        { 172, "Digital Music Score" },       // Item         電子楽譜
        { 173, "Motavian Ring" },             // Item         モタビアンリング
        { 174, "Maerla Berries" },            // Item         マルエラベリー
        { 175, "Happiness Ring" },            // Item         幸福の指輪
        { 176, "Visiphone" },                 // Item         ビジフォン
        { 177, "Red Card" },                  // Item         レッドカード
        { 178, "Yellow Card" },               // Item         イエローカード
        { 179, "Blue Card" },                 // Item         ブルーカード
        { 180, "Green Card" },                // Item         グリーンカード
        { 181, "Container Key" },             // Item         コンテナのカギ
        { 182, "Tube Key" },                  // Item         チューブキー
        { 183, "Polymeteral" },               // Item         ポリメテラール
        { 184, "Silver Bullet Necklace" },    // Item         銀の弾丸ネックレス
        { 185, "Nanette's Letter" },          // Item         ナネットの手紙
    };

    public static IReadOnlyDictionary<int, string> Names { get; } = LoadMerged();

    public static string Name(int rawId) =>
        Names.TryGetValue(BaseId(rawId), out var n) ? n : $"Item #{rawId}";

    /// <summary>
    /// Base IDs of two-handed weapons (equip kind 2 in the game's item table). The save stores
    /// these only in the right-hand slot, but the game fills both hands, so the UI mirrors them
    /// into the left hand for display.
    /// </summary>
    public static readonly HashSet<int> TwoHanded = new() { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 25, 28, 29, 30, 31, 32, 33, 34, 35 };

    public static bool IsTwoHanded(int rawId) => TwoHanded.Contains(BaseId(rawId));

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
                            map[BaseId(id)] = v;   // user file wins over built-in
                        }
                    }
                }
            }
        }
        catch { /* ignore a malformed override file; keep built-in names */ }

        return map;
    }
}
