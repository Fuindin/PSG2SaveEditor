namespace PSG2SaveEditor;

/// <summary>Phantasy Star sci-fi palette — deep space navy, cyan/teal accents, pale text.</summary>
internal static class Theme
{
    public static readonly Color SpaceDark  = Color.FromArgb(0x0B, 0x12, 0x24); // outer frame
    public static readonly Color SpaceMid   = Color.FromArgb(0x14, 0x20, 0x3A); // panels
    public static readonly Color SpacePanel = Color.FromArgb(0x1B, 0x2A, 0x4A); // fields / cards
    public static readonly Color PanelHi    = Color.FromArgb(0x24, 0x37, 0x5E); // lighter fields
    public static readonly Color TextBright = Color.FromArgb(0xE6, 0xF2, 0xFF); // body text
    public static readonly Color TextMuted  = Color.FromArgb(0x8C, 0xA6, 0xC8); // secondary text
    public static readonly Color Cyan       = Color.FromArgb(0x3D, 0xE0, 0xE6); // selection / accents
    public static readonly Color CyanDeep   = Color.FromArgb(0x1F, 0x9A, 0xA8);
    public static readonly Color Amber      = Color.FromArgb(0xF2, 0xC1, 0x4E); // meseta / highlights

    public static readonly string Family = "Segoe UI"; // crisp, sci-fi-leaning sans

    public static Font Body(float size, FontStyle style = FontStyle.Regular)
        => new(Family, size, style);
}

/// <summary>Themed colors for the menu strip (dark popups, cyan highlight).</summary>
internal sealed class PsMenuColors : System.Windows.Forms.ProfessionalColorTable
{
    public override Color MenuStripGradientBegin => Theme.SpaceDark;
    public override Color MenuStripGradientEnd   => Theme.SpaceDark;
    public override Color ToolStripDropDownBackground => Theme.SpaceMid;
    public override Color ImageMarginGradientBegin => Theme.SpaceMid;
    public override Color ImageMarginGradientMiddle => Theme.SpaceMid;
    public override Color ImageMarginGradientEnd => Theme.SpaceMid;
    public override Color MenuItemSelected => Theme.CyanDeep;
    public override Color MenuItemSelectedGradientBegin => Theme.CyanDeep;
    public override Color MenuItemSelectedGradientEnd => Theme.CyanDeep;
    public override Color MenuItemBorder => Theme.Cyan;
    public override Color MenuItemPressedGradientBegin => Theme.PanelHi;
    public override Color MenuItemPressedGradientEnd => Theme.SpaceMid;
    public override Color MenuBorder => Theme.CyanDeep;
    public override Color SeparatorDark => Theme.CyanDeep;
}
