# Phantasy Star Generation 2 — Save State Editor

A Windows Forms (.NET 10) editor for **PCSX2** save states (`.p2s`) of *Phantasy Star
Generation:2* (the PS2 remake of Phantasy Star II; Japanese release **SLPM-625.53**).

It edits **Meseta** and each character's **level, HP, TP, attributes (Attack, Defense,
Stamina, Intellect, Agility, Luck, Skill) and Experience**, and **displays** each
character's equipment and the shared item list. It always writes a **new `*_edited.p2s`**
file — your original is never modified.

## Usage

1. `File ▸ Open…` and pick your `PhantasyStarGeneration2.p2s`.
2. Edit Meseta at the top; pick a character to edit their stats/experience.
3. `File ▸ Save As…` → writes a new `*_edited.p2s`.
4. Load the edited state in PCSX2.

The character list shows all eight roster members. The two recruited members
(**Eusis**, **Nei**) are marked *“● in party”*; the rest are *“○ reserve”* — they
exist in the save pre-seeded at Lv1 and are editable, but aren't in your party yet.

## How it works

`.p2s` files are ZIP containers whose regions use **Zstandard (ZIP method 93)**. The game's
PS2 RAM lives in the `eeMemory.bin` entry. `SaveFormat/P2SArchive.cs` parses the container
manually, decompresses with `ZstdSharp`, and repacks — reusing untouched entries verbatim and
recompressing only `eeMemory.bin`.

### Verified memory map (`eeMemory.bin`)

Anchored empirically against known in-game values. All character fields are **u32 little-endian**.

* **Character array**: base `0x247700`, stride `0x84` (132 bytes), **8 slots** in roster order:
  Eusis, Nei, Rudo, Amy, Hugh, Anna, Kain, Shir.
* **Per-record offsets**: `+0x00` Level, `+0x04`/`+0x08` HP cur/max, `+0x0C`/`+0x10` TP cur/max,
  `+0x14` Attack, `+0x18` Defense, `+0x1C` Stamina, `+0x20` Intellect, `+0x24` Agility,
  `+0x28` Luck, `+0x2C` Skill, `+0x54` Experience.
* **“Recruited” flag**: the block at `+0x30` is non-zero only for active party members.
* **Meseta**: lone u32 at absolute `0x248314`.
* **Inventory**: array at absolute `0x247B14`, stride 4, each entry `[id u16][flags u16]`,
  ending at the first empty entry. Holds every owned item (equipped and bag). An item is
  **equipped** iff `(flags & 0x3F) != 0x3F`; then `owner = flags & 0x3F` (character index)
  and `slot = (flags >> 6) & 7` (0 Head, 1 Body, 2 R.Hand, 3 L.Hand, 4 Feet). Confirmed
  item IDs so far: 1 Knife, 53 Steel Claw, 66 Ribbon, 82 Carbon Suit, 83 Carbon Vest,
  128 Leather Shoes, 129 Espadrilles, 138 Monomate.

Offsets are overridable at runtime via a `psg2_layout.json` placed next to the executable.

## Verified end-to-end

Editing Meseta and stats (e.g. Experience) was confirmed to take effect in PCSX2. Equipment and
inventory were mapped from a controlled menus-closed save diff (buy one item + unequip two
weapons) and decode exactly.

## Still display-only / not yet mapped

* **Equipment & items are read-only** for now (display only); editing them is a possible extension.
* The complete **item-ID → name** table is unknown — unmapped IDs render as `Item #N`.
  You can name items yourself by editing **`psg2_items.json`** next to the exe (a simple
  `{ "id": "Name" }` map); your entries override/extend the built-in table, no rebuild needed.
* The **Job** field is shown from a known table (only Eusis = *Agent*, Nei = *—* confirmed),
  not read from the save.

> **Note on TP / Attack:** Eusis's stored TP reads 18 (you may see 10 in a menu — likely current
> vs. max), and the stored Attack is a base value the game adds the weapon bonus to (Nei stores 11,
> displays 16 with Steel Claws). Stamina/Intellect/Agility/Luck/Skill/Defense/Level/HP/Exp match
> the in-game numbers exactly.

## Project layout

* `SaveFormat/P2SArchive.cs` — `.p2s` (zip + zstd) reader/writer.
* `Game/Psg2Layout.cs` — offsets, field defs, roster (JSON-overridable).
* `Game/Psg2Character.cs`, `Game/Psg2Save.cs` — read/write model.
* `MainForm.*` — UI (designer pattern: static layout in `.Designer.cs`, theme/behaviour in `.cs`).
* `Analyzer/` — a separate console project used for offset reverse-engineering and round-trip tests
  (excluded from the app build).
