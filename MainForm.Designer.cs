namespace PSG2SaveEditor;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    private MenuStrip _menu;
    private ToolStripMenuItem _mnuFile, _mnuOpen, _mnuSaveAs, _mnuExit, _mnuHelp, _mnuInstructions, _mnuAbout;

    private Panel _header;
    private Label _lblFile, _lblSerial;
    private Panel _mesetaPanel;
    private Label _lblMeseta;
    private NumericUpDown _numMeseta;
    private Button _btnSave;

    private Label _lblBanner;

    private Panel _leftPanel;
    private Label _lblCharacters;
    private ListBox _lstChars;

    private Panel _divider;

    private Panel _rightPanel;
    private Panel _namePanel;
    private Label _txtName, _lblJob;
    private TabControl _tabs;
    private TabPage _tabStats, _tabItems, _tabAllItems;
    private TableLayoutPanel _fieldTable;
    private Label _lblEquipHdr, _lblBagHdr, _itemsNote;
    private TableLayoutPanel _equipTable;
    private ListBox _itemList;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        _menu = new MenuStrip();
        _mnuFile = new ToolStripMenuItem();
        _mnuOpen = new ToolStripMenuItem();
        _mnuSaveAs = new ToolStripMenuItem();
        _mnuExit = new ToolStripMenuItem();
        _mnuHelp = new ToolStripMenuItem();
        _mnuInstructions = new ToolStripMenuItem();
        _mnuAbout = new ToolStripMenuItem();
        _header = new Panel();
        _lblFile = new Label();
        _lblSerial = new Label();
        _mesetaPanel = new Panel();
        _lblMeseta = new Label();
        _numMeseta = new NumericUpDown();
        _btnSave = new Button();
        _lblBanner = new Label();
        _leftPanel = new Panel();
        _lblCharacters = new Label();
        _lstChars = new ListBox();
        _divider = new Panel();
        _rightPanel = new Panel();
        _namePanel = new Panel();
        _txtName = new Label();
        _lblJob = new Label();
        _tabs = new TabControl();
        _tabStats = new TabPage();
        _tabItems = new TabPage();
        _tabAllItems = new TabPage();
        _fieldTable = new TableLayoutPanel();
        _lblEquipHdr = new Label();
        _lblBagHdr = new Label();
        _itemsNote = new Label();
        _equipTable = new TableLayoutPanel();
        _itemList = new ListBox();

        _menu.SuspendLayout();
        _header.SuspendLayout();
        _mesetaPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_numMeseta).BeginInit();
        _leftPanel.SuspendLayout();
        _rightPanel.SuspendLayout();
        _namePanel.SuspendLayout();
        _tabs.SuspendLayout();
        _tabStats.SuspendLayout();
        _tabItems.SuspendLayout();
        _tabAllItems.SuspendLayout();
        SuspendLayout();

        // _menu
        _menu.Items.AddRange(new ToolStripItem[] { _mnuFile, _mnuHelp });
        _menu.Location = new Point(0, 0);
        _menu.Name = "_menu";
        _menu.Size = new Size(900, 24);
        // _mnuFile
        _mnuFile.DropDownItems.AddRange(new ToolStripItem[] { _mnuOpen, _mnuSaveAs, _mnuExit });
        _mnuFile.Text = "File";
        _mnuOpen.Text = "Open…";
        _mnuOpen.ShortcutKeys = Keys.Control | Keys.O;
        _mnuSaveAs.Text = "Save As…";
        _mnuSaveAs.ShortcutKeys = Keys.Control | Keys.S;
        _mnuExit.Text = "Exit";
        // _mnuHelp
        _mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { _mnuInstructions, _mnuAbout });
        _mnuHelp.Text = "Help";
        _mnuInstructions.Text = "How to Use…";
        _mnuInstructions.ShortcutKeys = Keys.F1;
        _mnuAbout.Text = "About";

        // _header
        _header.Controls.Add(_mesetaPanel);
        _header.Controls.Add(_lblSerial);
        _header.Controls.Add(_lblFile);
        _header.Dock = DockStyle.Top;
        _header.Location = new Point(0, 24);
        _header.Size = new Size(900, 92);
        _header.Padding = new Padding(12, 8, 12, 8);
        // _lblFile
        _lblFile.AutoSize = true;
        _lblFile.Location = new Point(14, 10);
        _lblFile.Text = "No save state loaded.  File ▸ Open…";
        // _lblSerial
        _lblSerial.AutoSize = true;
        _lblSerial.Location = new Point(14, 30);
        _lblSerial.Text = "";
        // _mesetaPanel
        _mesetaPanel.Controls.Add(_lblMeseta);
        _mesetaPanel.Controls.Add(_numMeseta);
        _mesetaPanel.Controls.Add(_btnSave);
        _mesetaPanel.Location = new Point(14, 52);
        _mesetaPanel.Size = new Size(860, 34);
        // _lblMeseta
        _lblMeseta.AutoSize = true;
        _lblMeseta.Location = new Point(0, 6);
        _lblMeseta.Text = "Meseta";
        // _numMeseta
        _numMeseta.Location = new Point(72, 2);
        _numMeseta.Size = new Size(140, 27);
        _numMeseta.Maximum = 9999999;
        // _btnSave
        _btnSave.Location = new Point(740, 0);
        _btnSave.Size = new Size(118, 32);
        _btnSave.Text = "Save As…";
        _btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;

        // _lblBanner
        _lblBanner.Dock = DockStyle.Top;
        _lblBanner.Location = new Point(0, 116);
        _lblBanner.Size = new Size(900, 0);
        _lblBanner.TextAlign = ContentAlignment.MiddleLeft;
        _lblBanner.Padding = new Padding(12, 0, 12, 0);
        _lblBanner.AutoSize = false;
        _lblBanner.Visible = false;

        // _leftPanel
        _leftPanel.Controls.Add(_lstChars);
        _leftPanel.Controls.Add(_lblCharacters);
        _leftPanel.Dock = DockStyle.Left;
        _leftPanel.Location = new Point(0, 116);
        _leftPanel.Size = new Size(240, 460);
        _leftPanel.Padding = new Padding(12, 10, 6, 12);
        // _lblCharacters
        _lblCharacters.Dock = DockStyle.Top;
        _lblCharacters.Height = 28;
        _lblCharacters.Text = "Characters";
        // _lstChars
        _lstChars.Dock = DockStyle.Fill;
        _lstChars.BorderStyle = BorderStyle.None;
        _lstChars.DrawMode = DrawMode.OwnerDrawFixed;
        _lstChars.ItemHeight = 44;
        _lstChars.IntegralHeight = false;

        // _divider
        _divider.Dock = DockStyle.Left;
        _divider.Location = new Point(240, 116);
        _divider.Size = new Size(2, 460);

        // _rightPanel  (tabs fill; name strip docked on top)
        _rightPanel.Controls.Add(_tabs);
        _rightPanel.Controls.Add(_namePanel);
        _rightPanel.Dock = DockStyle.Fill;
        _rightPanel.Location = new Point(242, 116);
        _rightPanel.Size = new Size(658, 544);
        _rightPanel.Padding = new Padding(16, 12, 16, 12);
        // _namePanel
        _namePanel.Controls.Add(_lblJob);
        _namePanel.Controls.Add(_txtName);
        _namePanel.Dock = DockStyle.Top;
        _namePanel.Height = 58;
        // _txtName
        _txtName.AutoSize = true;
        _txtName.Location = new Point(6, 4);
        _txtName.Text = "";
        // _lblJob
        _lblJob.AutoSize = true;
        _lblJob.Location = new Point(8, 36);
        _lblJob.Text = "";
        // _tabs
        _tabs.Controls.Add(_tabStats);
        _tabs.Controls.Add(_tabItems);
        _tabs.Controls.Add(_tabAllItems);
        _tabs.Dock = DockStyle.Fill;
        _tabs.Padding = new Point(14, 6);
        _tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        _tabs.SizeMode = TabSizeMode.Fixed;
        _tabs.ItemSize = new Size(150, 30);
        // _tabStats
        _tabStats.Controls.Add(_fieldTable);
        _tabStats.Text = "Stats & Experience";
        _tabStats.Padding = new Padding(16, 16, 16, 16);
        _tabStats.AutoScroll = true;
        // _fieldTable  (Top + AutoSize so the tab page can scroll if ever too short)
        _fieldTable.Dock = DockStyle.Top;
        _fieldTable.AutoSize = true;
        _fieldTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _fieldTable.ColumnCount = 2;
        _fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
        _fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
        // _tabItems = "Equipment"  (equip table fills; header docked top)
        _tabItems.Controls.Add(_equipTable);
        _tabItems.Controls.Add(_lblEquipHdr);
        _tabItems.Text = "Equipment";
        _tabItems.Padding = new Padding(16, 12, 16, 12);
        // _lblEquipHdr
        _lblEquipHdr.Dock = DockStyle.Top;
        _lblEquipHdr.Height = 26;
        _lblEquipHdr.Text = "Equipment";
        // _equipTable  (5 slot rows)
        _equipTable.Dock = DockStyle.Top;
        _equipTable.Height = 156;
        _equipTable.ColumnCount = 2;
        _equipTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        _equipTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // _tabAllItems = "Items"  (full owned-items list)
        _tabAllItems.Controls.Add(_itemList);
        _tabAllItems.Controls.Add(_lblBagHdr);
        _tabAllItems.Controls.Add(_itemsNote);
        _tabAllItems.Text = "Items";
        _tabAllItems.Padding = new Padding(16, 12, 16, 12);
        // _lblBagHdr
        _lblBagHdr.Dock = DockStyle.Top;
        _lblBagHdr.Height = 26;
        _lblBagHdr.Text = "All Items (shared)";
        // _itemsNote
        _itemsNote.Dock = DockStyle.Bottom;
        _itemsNote.Height = 22;
        _itemsNote.Text = "";
        // _itemList
        _itemList.Dock = DockStyle.Fill;
        _itemList.BorderStyle = BorderStyle.None;
        _itemList.DrawMode = DrawMode.OwnerDrawFixed;
        _itemList.ItemHeight = 24;
        _itemList.IntegralHeight = false;

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 660);
        Controls.Add(_rightPanel);
        Controls.Add(_divider);
        Controls.Add(_leftPanel);
        Controls.Add(_lblBanner);
        Controls.Add(_header);
        Controls.Add(_menu);
        MainMenuStrip = _menu;
        MinimumSize = new Size(820, 600);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Phantasy Star Generation 2 — Save Editor";

        _menu.ResumeLayout(false);
        _menu.PerformLayout();
        _header.ResumeLayout(false);
        _header.PerformLayout();
        _mesetaPanel.ResumeLayout(false);
        _mesetaPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)_numMeseta).EndInit();
        _leftPanel.ResumeLayout(false);
        _namePanel.ResumeLayout(false);
        _namePanel.PerformLayout();
        _rightPanel.ResumeLayout(false);
        _tabs.ResumeLayout(false);
        _tabStats.ResumeLayout(false);
        _tabItems.ResumeLayout(false);
        _tabAllItems.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
