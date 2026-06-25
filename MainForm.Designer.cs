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
        _mesetaPanel = new Panel();
        _lblMeseta = new Label();
        _numMeseta = new NumericUpDown();
        _btnSave = new Button();
        _lblSerial = new Label();
        _lblFile = new Label();
        _lblBanner = new Label();
        _leftPanel = new Panel();
        _lstChars = new ListBox();
        _lblCharacters = new Label();
        _divider = new Panel();
        _rightPanel = new Panel();
        _tabs = new TabControl();
        _tabStats = new TabPage();
        _fieldTable = new TableLayoutPanel();
        _tabItems = new TabPage();
        _equipTable = new TableLayoutPanel();
        _lblEquipHdr = new Label();
        _tabAllItems = new TabPage();
        _itemList = new ListBox();
        _lblBagHdr = new Label();
        _itemsNote = new Label();
        _namePanel = new Panel();
        _lblJob = new Label();
        _txtName = new Label();
        _menu.SuspendLayout();
        _header.SuspendLayout();
        _mesetaPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_numMeseta).BeginInit();
        _leftPanel.SuspendLayout();
        _rightPanel.SuspendLayout();
        _tabs.SuspendLayout();
        _tabStats.SuspendLayout();
        _tabItems.SuspendLayout();
        _tabAllItems.SuspendLayout();
        _namePanel.SuspendLayout();
        SuspendLayout();
        // 
        // _menu
        // 
        _menu.ImageScalingSize = new Size(24, 24);
        _menu.Items.AddRange(new ToolStripItem[] { _mnuFile, _mnuHelp });
        _menu.Location = new Point(0, 0);
        _menu.Name = "_menu";
        _menu.Padding = new Padding(9, 3, 0, 3);
        _menu.Size = new Size(1452, 35);
        _menu.TabIndex = 5;
        // 
        // _mnuFile
        // 
        _mnuFile.DropDownItems.AddRange(new ToolStripItem[] { _mnuOpen, _mnuSaveAs, _mnuExit });
        _mnuFile.Name = "_mnuFile";
        _mnuFile.Size = new Size(54, 29);
        _mnuFile.Text = "File";
        // 
        // _mnuOpen
        // 
        _mnuOpen.Name = "_mnuOpen";
        _mnuOpen.ShortcutKeys = Keys.Control | Keys.O;
        _mnuOpen.Size = new Size(250, 34);
        _mnuOpen.Text = "Open…";
        // 
        // _mnuSaveAs
        // 
        _mnuSaveAs.Name = "_mnuSaveAs";
        _mnuSaveAs.ShortcutKeys = Keys.Control | Keys.S;
        _mnuSaveAs.Size = new Size(250, 34);
        _mnuSaveAs.Text = "Save As…";
        // 
        // _mnuExit
        // 
        _mnuExit.Name = "_mnuExit";
        _mnuExit.Size = new Size(250, 34);
        _mnuExit.Text = "Exit";
        // 
        // _mnuHelp
        // 
        _mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { _mnuInstructions, _mnuAbout });
        _mnuHelp.Name = "_mnuHelp";
        _mnuHelp.Size = new Size(65, 29);
        _mnuHelp.Text = "Help";
        // 
        // _mnuInstructions
        // 
        _mnuInstructions.Name = "_mnuInstructions";
        _mnuInstructions.ShortcutKeys = Keys.F1;
        _mnuInstructions.Size = new Size(251, 34);
        _mnuInstructions.Text = "How to Use…";
        // 
        // _mnuAbout
        // 
        _mnuAbout.Name = "_mnuAbout";
        _mnuAbout.Size = new Size(251, 34);
        _mnuAbout.Text = "About";
        // 
        // _header
        // 
        _header.Controls.Add(_mesetaPanel);
        _header.Controls.Add(_lblSerial);
        _header.Controls.Add(_lblFile);
        _header.Dock = DockStyle.Top;
        _header.Location = new Point(0, 35);
        _header.Margin = new Padding(4, 5, 4, 5);
        _header.Name = "_header";
        _header.Padding = new Padding(17, 13, 17, 13);
        _header.Size = new Size(1452, 153);
        _header.TabIndex = 4;
        // 
        // _mesetaPanel
        // 
        _mesetaPanel.Controls.Add(_lblMeseta);
        _mesetaPanel.Controls.Add(_numMeseta);
        _mesetaPanel.Controls.Add(_btnSave);
        _mesetaPanel.Location = new Point(20, 87);
        _mesetaPanel.Margin = new Padding(4, 5, 4, 5);
        _mesetaPanel.Name = "_mesetaPanel";
        _mesetaPanel.Size = new Size(1229, 57);
        _mesetaPanel.TabIndex = 0;
        // 
        // _lblMeseta
        // 
        _lblMeseta.AutoSize = true;
        _lblMeseta.Location = new Point(0, 10);
        _lblMeseta.Margin = new Padding(4, 0, 4, 0);
        _lblMeseta.Name = "_lblMeseta";
        _lblMeseta.Size = new Size(69, 25);
        _lblMeseta.TabIndex = 0;
        _lblMeseta.Text = "Meseta";
        // 
        // _numMeseta
        // 
        _numMeseta.Location = new Point(103, 3);
        _numMeseta.Margin = new Padding(4, 5, 4, 5);
        _numMeseta.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
        _numMeseta.Name = "_numMeseta";
        _numMeseta.Size = new Size(200, 31);
        _numMeseta.TabIndex = 1;
        // 
        // _btnSave
        // 
        _btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnSave.Location = new Point(1057, 0);
        _btnSave.Margin = new Padding(4, 5, 4, 5);
        _btnSave.Name = "_btnSave";
        _btnSave.Size = new Size(169, 53);
        _btnSave.TabIndex = 2;
        _btnSave.Text = "Save As…";
        // 
        // _lblSerial
        // 
        _lblSerial.AutoSize = true;
        _lblSerial.Location = new Point(20, 50);
        _lblSerial.Margin = new Padding(4, 0, 4, 0);
        _lblSerial.Name = "_lblSerial";
        _lblSerial.Size = new Size(0, 25);
        _lblSerial.TabIndex = 1;
        // 
        // _lblFile
        // 
        _lblFile.AutoSize = true;
        _lblFile.Location = new Point(20, 17);
        _lblFile.Margin = new Padding(4, 0, 4, 0);
        _lblFile.Name = "_lblFile";
        _lblFile.Size = new Size(302, 25);
        _lblFile.TabIndex = 2;
        _lblFile.Text = "No save state loaded.  File ▸ Open…";
        // 
        // _lblBanner
        // 
        _lblBanner.Dock = DockStyle.Top;
        _lblBanner.Location = new Point(0, 188);
        _lblBanner.Margin = new Padding(4, 0, 4, 0);
        _lblBanner.Name = "_lblBanner";
        _lblBanner.Padding = new Padding(17, 0, 17, 0);
        _lblBanner.Size = new Size(1452, 0);
        _lblBanner.TabIndex = 3;
        _lblBanner.TextAlign = ContentAlignment.MiddleLeft;
        _lblBanner.Visible = false;
        // 
        // _leftPanel
        // 
        _leftPanel.Controls.Add(_lstChars);
        _leftPanel.Controls.Add(_lblCharacters);
        _leftPanel.Dock = DockStyle.Left;
        _leftPanel.Location = new Point(0, 188);
        _leftPanel.Margin = new Padding(4, 5, 4, 5);
        _leftPanel.Name = "_leftPanel";
        _leftPanel.Padding = new Padding(17, 17, 9, 20);
        _leftPanel.Size = new Size(343, 983);
        _leftPanel.TabIndex = 2;
        // 
        // _lstChars
        // 
        _lstChars.BorderStyle = BorderStyle.None;
        _lstChars.Dock = DockStyle.Fill;
        _lstChars.DrawMode = DrawMode.OwnerDrawFixed;
        _lstChars.IntegralHeight = false;
        _lstChars.ItemHeight = 44;
        _lstChars.Location = new Point(17, 64);
        _lstChars.Margin = new Padding(4, 5, 4, 5);
        _lstChars.Name = "_lstChars";
        _lstChars.Size = new Size(317, 899);
        _lstChars.TabIndex = 0;
        // 
        // _lblCharacters
        // 
        _lblCharacters.Dock = DockStyle.Top;
        _lblCharacters.Location = new Point(17, 17);
        _lblCharacters.Margin = new Padding(4, 0, 4, 0);
        _lblCharacters.Name = "_lblCharacters";
        _lblCharacters.Size = new Size(317, 47);
        _lblCharacters.TabIndex = 1;
        _lblCharacters.Text = "Characters";
        // 
        // _divider
        // 
        _divider.Dock = DockStyle.Left;
        _divider.Location = new Point(343, 188);
        _divider.Margin = new Padding(4, 5, 4, 5);
        _divider.Name = "_divider";
        _divider.Size = new Size(3, 983);
        _divider.TabIndex = 1;
        // 
        // _rightPanel
        // 
        _rightPanel.Controls.Add(_tabs);
        _rightPanel.Controls.Add(_namePanel);
        _rightPanel.Dock = DockStyle.Fill;
        _rightPanel.Location = new Point(346, 188);
        _rightPanel.Margin = new Padding(4, 5, 4, 5);
        _rightPanel.Name = "_rightPanel";
        _rightPanel.Padding = new Padding(23, 20, 23, 20);
        _rightPanel.Size = new Size(1106, 983);
        _rightPanel.TabIndex = 0;
        // 
        // _tabs
        // 
        _tabs.Controls.Add(_tabStats);
        _tabs.Controls.Add(_tabItems);
        _tabs.Controls.Add(_tabAllItems);
        _tabs.Dock = DockStyle.Fill;
        _tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        _tabs.ItemSize = new Size(150, 30);
        _tabs.Location = new Point(23, 117);
        _tabs.Margin = new Padding(4, 5, 4, 5);
        _tabs.Name = "_tabs";
        _tabs.Padding = new Point(14, 6);
        _tabs.SelectedIndex = 0;
        _tabs.Size = new Size(1060, 846);
        _tabs.SizeMode = TabSizeMode.Fixed;
        _tabs.TabIndex = 0;
        // 
        // _tabStats
        // 
        _tabStats.AutoScroll = true;
        _tabStats.Controls.Add(_fieldTable);
        _tabStats.Location = new Point(4, 34);
        _tabStats.Margin = new Padding(4, 5, 4, 5);
        _tabStats.Name = "_tabStats";
        _tabStats.Padding = new Padding(23, 27, 23, 27);
        _tabStats.Size = new Size(1052, 808);
        _tabStats.TabIndex = 0;
        _tabStats.Text = "Stats & Experience";
        // 
        // _fieldTable
        // 
        _fieldTable.AutoSize = true;
        _fieldTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _fieldTable.ColumnCount = 2;
        _fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 243F));
        _fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 243F));
        _fieldTable.Dock = DockStyle.Top;
        _fieldTable.Location = new Point(23, 27);
        _fieldTable.Margin = new Padding(4, 5, 4, 5);
        _fieldTable.Name = "_fieldTable";
        _fieldTable.Size = new Size(1006, 0);
        _fieldTable.TabIndex = 0;
        // 
        // _tabItems
        // 
        _tabItems.Controls.Add(_equipTable);
        _tabItems.Controls.Add(_lblEquipHdr);
        _tabItems.Location = new Point(4, 34);
        _tabItems.Margin = new Padding(4, 5, 4, 5);
        _tabItems.Name = "_tabItems";
        _tabItems.Padding = new Padding(23, 20, 23, 20);
        _tabItems.Size = new Size(278, 129);
        _tabItems.TabIndex = 1;
        _tabItems.Text = "Equipment";
        // 
        // _equipTable
        // 
        _equipTable.ColumnCount = 2;
        _equipTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 157F));
        _equipTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _equipTable.Dock = DockStyle.Top;
        _equipTable.Location = new Point(23, 63);
        _equipTable.Margin = new Padding(4, 5, 4, 5);
        _equipTable.Name = "_equipTable";
        _equipTable.Size = new Size(232, 260);
        _equipTable.TabIndex = 0;
        // 
        // _lblEquipHdr
        // 
        _lblEquipHdr.Dock = DockStyle.Top;
        _lblEquipHdr.Location = new Point(23, 20);
        _lblEquipHdr.Margin = new Padding(4, 0, 4, 0);
        _lblEquipHdr.Name = "_lblEquipHdr";
        _lblEquipHdr.Size = new Size(232, 43);
        _lblEquipHdr.TabIndex = 1;
        _lblEquipHdr.Text = "Equipment";
        // 
        // _tabAllItems
        // 
        _tabAllItems.Controls.Add(_itemList);
        _tabAllItems.Controls.Add(_lblBagHdr);
        _tabAllItems.Controls.Add(_itemsNote);
        _tabAllItems.Location = new Point(4, 34);
        _tabAllItems.Margin = new Padding(4, 5, 4, 5);
        _tabAllItems.Name = "_tabAllItems";
        _tabAllItems.Padding = new Padding(23, 20, 23, 20);
        _tabAllItems.Size = new Size(278, 129);
        _tabAllItems.TabIndex = 2;
        _tabAllItems.Text = "Items";
        // 
        // _itemList
        // 
        _itemList.BorderStyle = BorderStyle.None;
        _itemList.Dock = DockStyle.Fill;
        _itemList.DrawMode = DrawMode.OwnerDrawFixed;
        _itemList.IntegralHeight = false;
        _itemList.Location = new Point(23, 63);
        _itemList.Margin = new Padding(4, 5, 4, 5);
        _itemList.Name = "_itemList";
        _itemList.Size = new Size(232, 9);
        _itemList.TabIndex = 0;
        // 
        // _lblBagHdr
        // 
        _lblBagHdr.Dock = DockStyle.Top;
        _lblBagHdr.Location = new Point(23, 20);
        _lblBagHdr.Margin = new Padding(4, 0, 4, 0);
        _lblBagHdr.Name = "_lblBagHdr";
        _lblBagHdr.Size = new Size(232, 43);
        _lblBagHdr.TabIndex = 1;
        _lblBagHdr.Text = "All Items (shared)";
        // 
        // _itemsNote
        // 
        _itemsNote.Dock = DockStyle.Bottom;
        _itemsNote.Location = new Point(23, 72);
        _itemsNote.Margin = new Padding(4, 0, 4, 0);
        _itemsNote.Name = "_itemsNote";
        _itemsNote.Size = new Size(232, 37);
        _itemsNote.TabIndex = 2;
        // 
        // _namePanel
        // 
        _namePanel.Controls.Add(_lblJob);
        _namePanel.Controls.Add(_txtName);
        _namePanel.Dock = DockStyle.Top;
        _namePanel.Location = new Point(23, 20);
        _namePanel.Margin = new Padding(4, 5, 4, 5);
        _namePanel.Name = "_namePanel";
        _namePanel.Size = new Size(1060, 97);
        _namePanel.TabIndex = 1;
        // 
        // _lblJob
        // 
        _lblJob.AutoSize = true;
        _lblJob.Location = new Point(11, 60);
        _lblJob.Margin = new Padding(4, 0, 4, 0);
        _lblJob.Name = "_lblJob";
        _lblJob.Size = new Size(0, 25);
        _lblJob.TabIndex = 0;
        // 
        // _txtName
        // 
        _txtName.AutoSize = true;
        _txtName.Location = new Point(9, 7);
        _txtName.Margin = new Padding(4, 0, 4, 0);
        _txtName.Name = "_txtName";
        _txtName.Size = new Size(0, 25);
        _txtName.TabIndex = 1;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1452, 1171);
        Controls.Add(_rightPanel);
        Controls.Add(_divider);
        Controls.Add(_leftPanel);
        Controls.Add(_lblBanner);
        Controls.Add(_header);
        Controls.Add(_menu);
        MainMenuStrip = _menu;
        Margin = new Padding(4, 5, 4, 5);
        MinimumSize = new Size(1162, 963);
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
        _rightPanel.ResumeLayout(false);
        _tabs.ResumeLayout(false);
        _tabStats.ResumeLayout(false);
        _tabStats.PerformLayout();
        _tabItems.ResumeLayout(false);
        _tabAllItems.ResumeLayout(false);
        _namePanel.ResumeLayout(false);
        _namePanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
