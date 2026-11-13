using PSG2SaveEditor.Game;

namespace PSG2SaveEditor;

public sealed partial class MainForm : Form
{
    private readonly Psg2Layout _layout = Psg2Layout.Load();
    private Psg2Save? _save;
    private readonly List<Psg2Character> _chars = new();   // characters shown in the list
    private readonly Dictionary<string, NumericUpDown> _fieldEditors = new();
    private bool _dirty;
    private bool _loading;   // suppress write-back while populating controls
    private string? _pendingOpen;

    public MainForm() : this(null) { }

    public MainForm(string? openPath)
    {
        InitializeComponent();   // visual layout (MainForm.Designer.cs)
        ApplyTheme();            // colours, fonts, owner-draw renderer
        WireEvents();
        BuildFieldEditors();     // generate the stat rows from the offset config
        UpdateState();
        _pendingOpen = openPath;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        UpdateCharListItemHeight();
        UpdateTabSize();

        if (_pendingOpen is not null)
        {
            string path = _pendingOpen;
            _pendingOpen = null;
            LoadPath(path);
        }
    }

    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        UpdateCharListItemHeight();
        UpdateTabSize();
    }

    /// <summary>
    /// Size the owner-drawn tabs to fit the widest label at the current DPI. The
    /// tabs are OwnerDrawFixed (every tab shares ItemSize), so a fixed width clipped
    /// the longest caption ("Stats &amp; Experience") on high-DPI displays.
    /// </summary>
    private void UpdateTabSize()
    {
        int w = 0, h = 0;
        foreach (TabPage p in _tabs.TabPages)
        {
            Size sz = TextRenderer.MeasureText(p.Text, _tabs.Font);
            w = Math.Max(w, sz.Width);
            h = Math.Max(h, sz.Height);
        }

        _tabs.ItemSize = new Size(w + 28, h + 14);
    }

    // ---- Theme & wiring (kept out of the designer file so it stays editable) ----

    private void ApplyTheme()
    {
        BackColor = Theme.SpaceDark;
        ForeColor = Theme.TextBright;
        Font = Theme.Body(9.75f);

        _menu.Renderer = new ToolStripProfessionalRenderer(new PsMenuColors()) { RoundedEdges = false };
        _menu.BackColor = Theme.SpaceDark;
        _menu.ForeColor = Theme.TextBright;
        _menu.Font = Theme.Body(9.75f);

        _header.BackColor = Theme.SpaceMid;
        _lblFile.ForeColor = Theme.TextBright;
        _lblFile.BackColor = Color.Transparent;
        _lblSerial.ForeColor = Theme.TextMuted;
        _lblSerial.BackColor = Color.Transparent;

        _mesetaPanel.BackColor = Theme.SpaceMid;
        _lblMeseta.ForeColor = Theme.Amber;
        _lblMeseta.BackColor = Color.Transparent;
        _lblMeseta.Font = Theme.Body(12f, FontStyle.Bold);
        _numMeseta.Maximum = _layout.MesetaMax;
        _numMeseta.Font = Theme.Body(12f, FontStyle.Bold);
        StyleNumeric(_numMeseta);
        StyleButton(_btnSave);

        _lblBanner.Font = Theme.Body(9.5f, FontStyle.Bold);

        _leftPanel.BackColor = Theme.SpaceMid;
        _lblCharacters.ForeColor = Theme.Cyan;
        _lblCharacters.BackColor = Color.Transparent;
        _lblCharacters.Font = Theme.Body(12f, FontStyle.Bold);
        _lstChars.BackColor = Theme.SpaceMid;
        _lstChars.ForeColor = Theme.TextBright;
        _lstChars.Font = Theme.Body(11f, FontStyle.Bold);

        _divider.BackColor = Theme.CyanDeep;

        _rightPanel.BackColor = Theme.SpaceMid;
        _namePanel.BackColor = Theme.SpaceMid;
        _txtName.BackColor = Color.Transparent;
        _txtName.ForeColor = Theme.Cyan;
        _txtName.Font = Theme.Body(17f, FontStyle.Bold);
        _lblJob.BackColor = Color.Transparent;
        _lblJob.ForeColor = Theme.TextMuted;
        _lblJob.Font = Theme.Body(10.5f, FontStyle.Italic);

        _tabs.Font = Theme.Body(10f, FontStyle.Bold);
        foreach (TabPage page in _tabs.TabPages)
        {
            page.BackColor = Theme.SpacePanel;
        }

        _fieldTable.BackColor = Theme.SpacePanel;

        foreach (var p in new[] { _equipTable })
        {
            p.BackColor = Theme.SpacePanel;
            p.AutoSize = true;                              // grow with DPI-scaled rows
            p.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        _lblEquipHdr.BackColor = _lblBagHdr.BackColor = Theme.SpacePanel;
        _lblEquipHdr.ForeColor = _lblBagHdr.ForeColor = Theme.Cyan;
        _lblEquipHdr.Font = _lblBagHdr.Font = Theme.Body(11.5f, FontStyle.Bold);
        _lblEquipHdr.Padding = _lblBagHdr.Padding = new Padding(2, 4, 0, 0);
        _itemsNote.BackColor = Theme.SpacePanel;
        _itemsNote.ForeColor = Theme.TextMuted;
        _itemsNote.Font = Theme.Body(8.5f, FontStyle.Italic);
        _itemList.BackColor = Theme.SpacePanel;
        _itemList.ForeColor = Theme.TextBright;
        _itemList.Font = Theme.Body(10.5f);

        _portraitPanel.BackColor = Theme.SpaceMid;
        _picPortrait.BackColor = Theme.SpacePanel;
    }

    private void WireEvents()
    {
        _mnuOpen.Click += (_, _) => OpenFile();
        _mnuSaveAs.Click += (_, _) => SaveAs();
        _mnuExit.Click += (_, _) => Close();
        _mnuInstructions.Click += (_, _) => ShowInstructions();
        _mnuAbout.Click += (_, _) => MessageBox.Show(this,
            "Phantasy Star Generation 2 — Save State Editor\n\n" +
            "Edits PCSX2 (.p2s) save states for Phantasy Star Generation:2.\n\n" +
            "Always writes to a new copy; your original is never modified.",
            "About", MessageBoxButtons.OK, MessageBoxIcon.Information);

        _numMeseta.ValueChanged += (_, _) => { if (!_loading && _save is not null) { _save.Meseta = (long)_numMeseta.Value; MarkDirty(); } };
        _btnSave.Click += (_, _) => SaveAs();

        _lstChars.DrawItem += DrawCharacterItem;
        _lstChars.SelectedIndexChanged += (_, _) => LoadSelectedCharacter();
        _tabs.DrawItem += DrawTab;
        _itemList.DrawItem += DrawItemRow;
    }

    private void DrawItemRow(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= _itemList.Items.Count)
        {
            return;
        }

        e.Graphics.FillRectangle(new SolidBrush(Theme.SpacePanel), e.Bounds);
        string text = _itemList.Items[e.Index].ToString() ?? "";
        // Bag/unequipped rows are prefixed with a tab marker; tint them muted.
        bool equipped = !text.StartsWith("·");
        TextRenderer.DrawText(e.Graphics, text, _itemList.Font, Rectangle.Inflate(e.Bounds, -8, 0),
            equipped ? Theme.TextBright : Theme.TextMuted,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
    }

    private static void StyleNumeric(NumericUpDown n)
    {
        n.BorderStyle = BorderStyle.FixedSingle;
        n.BackColor = Theme.PanelHi;
        n.ForeColor = Theme.TextBright;
    }

    private static void StyleButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.BackColor = Theme.CyanDeep;
        b.ForeColor = Theme.TextBright;
        b.Font = Theme.Body(10f, FontStyle.Bold);
        b.FlatAppearance.BorderColor = Theme.Cyan;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.MouseOverBackColor = Theme.Cyan;
        b.Cursor = Cursors.Hand;
    }

    // ---- Owner drawing ---------------------------------------------------

    private void DrawTab(object? sender, DrawItemEventArgs e)
    {
        var rect = _tabs.GetTabRect(e.Index);
        bool selected = e.Index == _tabs.SelectedIndex;
        using var bg = new SolidBrush(selected ? Theme.CyanDeep : Theme.SpaceMid);
        e.Graphics.FillRectangle(bg, rect);
        TextRenderer.DrawText(e.Graphics, _tabs.TabPages[e.Index].Text, _tabs.Font, rect,
            selected ? Theme.TextBright : Theme.TextMuted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
    }

    /// <summary>Dark rows with a cyan highlight + recruited/reserve marker.</summary>
    private void DrawCharacterItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0 || e.Index >= _chars.Count)
        {
            return;
        }

        var g = e.Graphics;
        var rect = e.Bounds;
        g.FillRectangle(new SolidBrush(Theme.SpaceMid), rect);

        bool selected = (e.State & DrawItemState.Selected) != 0;
        if (selected)
        {
            var hi = Rectangle.Inflate(rect, -4, -3);
            using var fill = new SolidBrush(Color.FromArgb(60, Theme.Cyan));
            using var pen = new Pen(Theme.Cyan, 1);
            g.FillRectangle(fill, hi);
            g.DrawRectangle(pen, hi);
        }

        var c = _chars[e.Index];

        // Derive the two rows from the actual font heights so they scale with DPI
        // instead of overlapping at >100% (fixed pixel offsets do not scale).
        using var subFont = Theme.Body(8.5f, FontStyle.Regular);
        int x = rect.X + 14;
        int w = rect.Width - 18;
        int nameH = (int)Math.Ceiling(_lstChars.Font.GetHeight(g));
        int subH = (int)Math.Ceiling(subFont.GetHeight(g));
        var nameRect = new Rectangle(x, rect.Y + 5, w, nameH);
        var subRect = new Rectangle(x, rect.Y + 5 + nameH, w, subH);

        TextRenderer.DrawText(g, c.Name, _lstChars.Font, nameRect,
            Theme.TextBright, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        string sub = c.IsRecruited ? $"● in party · {c.Job}" : "○ reserve";
        TextRenderer.DrawText(g, sub, subFont, subRect,
            c.IsRecruited ? Theme.Cyan : Theme.TextMuted,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }

    /// <summary>
    /// Size each character row to fit both text lines at the current DPI. Called on
    /// show and on DPI change; the fixed ItemHeight overlapped on high-DPI displays.
    /// </summary>
    private void UpdateCharListItemHeight()
    {
        if (!_lstChars.IsHandleCreated)
        {
            return;
        }

        using var subFont = Theme.Body(8.5f, FontStyle.Regular);
        using var g = _lstChars.CreateGraphics();
        _lstChars.ItemHeight = (int)Math.Ceiling(_lstChars.Font.GetHeight(g) + subFont.GetHeight(g)) + 12;
        _lstChars.Invalidate();
    }

    // ---- Dynamic stat rows (generated from the offset config) ------------

    private void BuildFieldEditors()
    {
        _fieldTable.Controls.Clear();
        _fieldEditors.Clear();
        _fieldTable.RowStyles.Clear();
        _fieldTable.RowCount = _layout.Fields.Count;
        for (int i = 0; i < _layout.Fields.Count; i++)
        {
            // AutoSize rows grow with the (DPI-scaled) font; a fixed pixel height
            // clipped the labels on high-DPI displays.
            _fieldTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }

        int row = 0;
        foreach (var f in _layout.Fields)
        {
            // Anchor=Left (no Top/Bottom) centers the label vertically in the row so
            // it lines up with the numeric without a fixed top-margin hack.
            var lbl = new Label
            {
                Text = f.Label, AutoSize = true, Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 12, 4), ForeColor = Theme.TextBright,
                BackColor = Color.Transparent, Font = Theme.Body(10.5f),
            };
            var num = new NumericUpDown
            {
                Minimum = f.Min,
                Maximum = f.Max,
                Width = 140,
                ThousandsSeparator = f.Max > 9999,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 0, 4),
                Tag = f.Key,
                Font = Theme.Body(10.5f),
            };
            StyleNumeric(num);
            num.ValueChanged += (_, _) =>
            {
                if (_loading)
                {
                    return;
                }

                int idx = _lstChars.SelectedIndex;
                if (idx < 0 || idx >= _chars.Count)
                {
                    return;
                }

                _chars[idx].Set(f.Key, (long)num.Value);
                MarkDirty();
            };
            _fieldEditors[f.Key] = num;
            _fieldTable.Controls.Add(lbl, 0, row);
            _fieldTable.Controls.Add(num, 1, row);
            row++;
        }
    }

    private static readonly (EquipSlot slot, string label)[] EquipSlots =
    {
        (EquipSlot.Head, "Head"), (EquipSlot.Body, "Body"), (EquipSlot.RightHand, "Right Hand"),
        (EquipSlot.LeftHand, "Left Hand"), (EquipSlot.Feet, "Feet"),
    };

    /// <summary>Fills the Equipment &amp; Items tab for the selected character (read-only).</summary>
    private void RenderEquipmentAndItems(Psg2Character c)
    {
        // Equipment: this character's five slots. A two-handed right-hand weapon is
        // stored once but fills both hands in-game, so mirror it into the left hand.
        _equipTable.Controls.Clear();
        _equipTable.RowStyles.Clear();
        _equipTable.RowCount = EquipSlots.Length;
        var rightHand = c.Equipped(EquipSlot.RightHand);
        bool twoHander = rightHand is { } rh && Psg2Items.IsTwoHanded(rh.Id);
        int row = 0;
        foreach (var (slot, label) in EquipSlots)
        {
            // AutoSize rows + vertical-centered labels scale with DPI; the fixed
            // 28px rows overlapped (and mis-spaced) the slots on high-DPI displays.
            _equipTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var lblSlot = new Label
            {
                Text = label, AutoSize = true, Anchor = AnchorStyles.Left,
                Margin = new Padding(2, 4, 12, 4), ForeColor = Theme.TextMuted,
                BackColor = Color.Transparent, Font = Theme.Body(10f),
            };

            var worn = c.Equipped(slot);
            // Show a two-handed weapon in both hand rows.
            bool mirroredTwoHand = slot is EquipSlot.RightHand or EquipSlot.LeftHand && twoHander;
            string text = mirroredTwoHand ? $"{rightHand!.Value.Name}  (2-handed)"
                                          : worn?.Name ?? "— none —";
            bool filled = mirroredTwoHand || worn is not null;
            var lblItem = new Label
            {
                Text = text,
                AutoSize = true, Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 4, 0, 4), BackColor = Color.Transparent,
                ForeColor = filled ? Theme.TextBright : Theme.TextMuted,
                Font = Theme.Body(10f, filled ? FontStyle.Bold : FontStyle.Italic),
            };
            _equipTable.Controls.Add(lblSlot, 0, row);
            _equipTable.Controls.Add(lblItem, 1, row);
            row++;
        }

        // All owned items (shared), with equipped-by annotation.
        _itemList.BeginUpdate();
        _itemList.Items.Clear();
        var inv = _save?.Inventory() ?? Array.Empty<Psg2Save.ItemEntry>();
        foreach (var it in inv)
        {
            if (it.Equipped)
            {
                string owner = OwnerName(it.OwnerIndex);
                string slot = EquipSlots.FirstOrDefault(s => s.slot == it.Slot).label ?? it.Slot.ToString();
                _itemList.Items.Add($"{it.Name}   — {owner}, {slot}");
            }
            else
            {
                _itemList.Items.Add($"· {it.Name}   — bag");
            }
        }

        if (_itemList.Items.Count == 0)
        {
            _itemList.Items.Add("· — no items —");
        }

        _itemList.EndUpdate();

        _itemsNote.Text = $"{inv.Count} item(s) owned.  “bag” = carried/unequipped.  Equipment shown is read-only.";
    }

    private string OwnerName(int index) =>
        _save?.AllCharacters.FirstOrDefault(c => c.Slot == index)?.Name ?? $"Char {index}";

    private void ShowInstructions()
    {
        MessageBox.Show(this,
            "How to use\r\n\r\n" +
            "1.  File ▸ Open… and pick your PhantasyStarGeneration2.p2s save state.\r\n" +
            "2.  Edit Meseta at the top, and select a character to edit their level,\r\n" +
            "    HP / TP, attributes and experience.\r\n" +
            "3.  File ▸ Save As… — the editor writes a NEW *_edited.p2s file and never\r\n" +
            "    touches your original.\r\n" +
            "4.  In PCSX2, load the edited save state to use your changes.\r\n\r\n" +
            "Tip: make your first edit a small one (e.g. Meseta) and confirm it shows\r\n" +
            "in-game before making big changes.",
            "How to Use", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ---- Actions ---------------------------------------------------------

    private void OpenFile()
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "PCSX2 save state (*.p2s)|*.p2s|All files (*.*)|*.*",
            Title = "Open PCSX2 save state",
        };

        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        LoadPath(dlg.FileName);
    }

    private void LoadPath(string path)
    {
        try
        {
            _save = Psg2Save.Open(path, _layout);
            _dirty = false;
            PopulateCharacters();
            UpdateState();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Could not open save state:\n\n{ex.Message}", "Open failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SaveAs()
    {
        if (_save is null)
        {
            return;
        }

        string suggested = Path.GetFileNameWithoutExtension(_save.SourcePath) + "_edited.p2s";
        using var dlg = new SaveFileDialog
        {
            Filter = "PCSX2 save state (*.p2s)|*.p2s",
            Title = "Save edited copy",
            FileName = suggested,
            InitialDirectory = Path.GetDirectoryName(_save.SourcePath),
        };

        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (string.Equals(Path.GetFullPath(dlg.FileName), Path.GetFullPath(_save.SourcePath),
                StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Please choose a different file name — the editor never overwrites the original.",
                "Choose a new file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _save.SaveAs(dlg.FileName);
            _dirty = false;
            UpdateTitle();
            MessageBox.Show(this, $"Saved:\n{dlg.FileName}\n\nLoad this state in PCSX2 to use it.",
                "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Could not save:\n\n{ex.Message}", "Save failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---- Population / state ---------------------------------------------

    private void PopulateCharacters()
    {
        _lstChars.Items.Clear();
        _chars.Clear();
        if (_save is null)
        {
            return;
        }

        // Recruited members first, then reserve — all are shown and editable.
        foreach (var c in _save.AllCharacters.OrderByDescending(c => c.IsRecruited).ThenBy(c => c.Slot))
        {
            _chars.Add(c);
            _lstChars.Items.Add(c.Name);
        }

        if (_lstChars.Items.Count > 0)
        {
            _lstChars.SelectedIndex = 0;
        }
    }

    private void LoadSelectedCharacter()
    {
        int idx = _lstChars.SelectedIndex;
        if (idx < 0 || idx >= _chars.Count)
        {
            return;
        }

        var c = _chars[idx];

        _loading = true;
        _txtName.Text = c.Name;
        _lblJob.Text = c.IsRecruited
            ? $"Job: {c.Job}"
            : $"Job: {c.Job}   (reserve — not yet recruited)";

        foreach (var (key, num) in _fieldEditors)
        {
            if (c.TryGet(key, out long v))
            {
                num.Value = Math.Clamp(v, num.Minimum, num.Maximum);
                num.Enabled = true;
            }
            else
            {
                num.Enabled = false;
            }
        }

        _loading = false;
        RenderEquipmentAndItems(c);
        LoadPortrait(c.Name);
    }

    /// <summary>
    /// Shows the selected character's portrait from a <c>portraits/&lt;Name&gt;.png</c> file
    /// next to the executable (user-swappable, like psg2_items.json). Missing or
    /// unreadable files just leave the panel blank.
    /// </summary>
    private void LoadPortrait(string name)
    {
        _picPortrait.Image?.Dispose();
        _picPortrait.Image = null;

        string path = Path.Combine(AppContext.BaseDirectory, "portraits", name + ".png");
        if (!File.Exists(path))
        {
            return;
        }

        try
        {
            // Decode into a detached copy so the file isn't locked for the app's lifetime.
            using var ms = new MemoryStream(File.ReadAllBytes(path));
            using var loaded = Image.FromStream(ms);
            _picPortrait.Image = new Bitmap(loaded);
        }
        catch { /* leave blank on any decode error */ }
    }

    private void UpdateState()
    {
        bool loaded = _save is not null;
        _btnSave.Enabled = loaded;
        _numMeseta.Enabled = loaded;

        if (!loaded)
        {
            _lblFile.Text = "No save state loaded.  File ▸ Open…";
            _lblSerial.Text = "";
            _lblBanner.Visible = false;
            UpdateTitle();
            return;
        }

        _lblFile.Text = _save!.SourcePath;
        string serial = _save.DetectedSerial ?? "unknown";
        int recruited = _save.RecruitedCharacters.Count;
        _lblSerial.Text = $"Detected game: {serial}   •   {recruited} in party / {_chars.Count} known";

        _loading = true;
        _numMeseta.Value = Math.Clamp(_save.Meseta, _numMeseta.Minimum, _numMeseta.Maximum);
        _loading = false;

        if (!_save.StructureFound)
        {
            _lblBanner.Visible = true;
            _lblBanner.Height = 28;
            _lblBanner.BackColor = Color.FromArgb(60, 20, 20);
            _lblBanner.ForeColor = Color.FromArgb(255, 150, 150);
            _lblBanner.Text = "⚠  Character data not found at the expected offsets — this may be a different game, version, or an unusual save point.";
        }
        else
        {
            _lblBanner.Visible = false;
            _lblBanner.Height = 0;
        }

        UpdateTitle();
    }

    private void MarkDirty()
    {
        if (_dirty)
        {
            return;
        }

        _dirty = true;
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        const string baseTitle = "Phantasy Star Generation 2 — Save Editor";
        Text = _save is null
            ? baseTitle
            : $"{baseTitle} — {Path.GetFileName(_save.SourcePath)}{(_dirty ? " *" : "")}";
    }
}
