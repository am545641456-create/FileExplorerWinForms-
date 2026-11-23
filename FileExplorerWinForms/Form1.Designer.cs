namespace FileExplorerWinForms;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        topPanel = new TableLayoutPanel();
        btnBack = new Button();
        btnRefresh = new Button();
        lblPath = new Label();
        txtPath = new TextBox();
        splitContainer1 = new SplitContainer();
        treeViewExplorer = new TreeView();
        imageListIcons = new ImageList(components);
        listViewFiles = new ListView();
        columnName = new ColumnHeader();
        columnType = new ColumnHeader();
        columnSize = new ColumnHeader();
        topPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel1.SuspendLayout();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        // 
        // topPanel
        // 
        topPanel.BackColor = Color.White;
        topPanel.ColumnCount = 4;
        topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        topPanel.Controls.Add(btnBack, 0, 0);
        topPanel.Controls.Add(btnRefresh, 1, 0);
        topPanel.Controls.Add(lblPath, 2, 0);
        topPanel.Controls.Add(txtPath, 3, 0);
        topPanel.Dock = DockStyle.Top;
        topPanel.Location = new Point(0, 0);
        topPanel.Name = "topPanel";
        topPanel.Padding = new Padding(10, 8, 10, 8);
        topPanel.RowCount = 1;
        topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        topPanel.Size = new Size(1100, 52);
        topPanel.TabIndex = 0;
        // 
        // btnBack
        // 
        btnBack.AutoSize = true;
        btnBack.BackColor = Color.FromArgb(240, 244, 248);
        btnBack.Cursor = Cursors.Hand;
        btnBack.FlatAppearance.BorderColor = Color.FromArgb(210, 220, 230);
        btnBack.FlatStyle = FlatStyle.Flat;
        btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnBack.ForeColor = Color.Black;
        btnBack.Location = new Point(10, 10);
        btnBack.Margin = new Padding(0, 2, 8, 2);
        btnBack.Name = "btnBack";
        btnBack.Padding = new Padding(8, 2, 8, 2);
        btnBack.Size = new Size(77, 29);
        btnBack.TabIndex = 0;
        btnBack.Text = "← Back";
        btnBack.UseVisualStyleBackColor = false;
        btnBack.Click += btnBack_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.AutoSize = true;
        btnRefresh.BackColor = Color.FromArgb(240, 244, 248);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(210, 220, 230);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnRefresh.ForeColor = Color.Black;
        btnRefresh.Location = new Point(95, 10);
        btnRefresh.Margin = new Padding(0, 2, 12, 2);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Padding = new Padding(8, 2, 8, 2);
        btnRefresh.Size = new Size(74, 29);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // lblPath
        // 
        lblPath.AutoSize = true;
        lblPath.Dock = DockStyle.Fill;
        lblPath.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPath.ForeColor = Color.Black;
        lblPath.Location = new Point(181, 8);
        lblPath.Margin = new Padding(0, 0, 8, 0);
        lblPath.Name = "lblPath";
        lblPath.Size = new Size(36, 36);
        lblPath.TabIndex = 2;
        lblPath.Text = "Path:";
        lblPath.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPath
        // 
        txtPath.BackColor = Color.FromArgb(248, 250, 252);
        txtPath.BorderStyle = BorderStyle.FixedSingle;
        txtPath.Dock = DockStyle.Fill;
        txtPath.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtPath.ForeColor = Color.Black;
        txtPath.Location = new Point(225, 12);
        txtPath.Margin = new Padding(0, 4, 0, 4);
        txtPath.Name = "txtPath";
        txtPath.ReadOnly = true;
        txtPath.Size = new Size(865, 24);
        txtPath.TabIndex = 3;
        // 
        // btnBack
        // 
        btnBack.AutoSize = true;
        btnBack.BackColor = Color.FromArgb(240, 244, 248);
        btnBack.Cursor = Cursors.Hand;
        btnBack.FlatAppearance.BorderColor = Color.FromArgb(210, 220, 230);
        btnBack.FlatStyle = FlatStyle.Flat;
        btnBack.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnBack.Location = new Point(10, 10);
        btnBack.Margin = new Padding(0, 2, 8, 2);
        btnBack.Name = "btnBack";
        btnBack.Padding = new Padding(8, 2, 8, 2);
        btnBack.Size = new Size(77, 29);
        btnBack.TabIndex = 0;
        btnBack.Text = "← Back";
        btnBack.UseVisualStyleBackColor = false;
        btnBack.Click += btnBack_Click;
        // 
        // btnRefresh
        // 
        btnRefresh.AutoSize = true;
        btnRefresh.BackColor = Color.FromArgb(240, 244, 248);
        btnRefresh.Cursor = Cursors.Hand;
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(210, 220, 230);
        btnRefresh.FlatStyle = FlatStyle.Flat;
        btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnRefresh.Location = new Point(95, 10);
        btnRefresh.Margin = new Padding(0, 2, 12, 2);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Padding = new Padding(8, 2, 8, 2);
        btnRefresh.Size = new Size(74, 29);
        btnRefresh.TabIndex = 1;
        btnRefresh.Text = "Refresh";
        btnRefresh.UseVisualStyleBackColor = false;
        btnRefresh.Click += btnRefresh_Click;
        // 
        // lblPath
        // 
        lblPath.AutoSize = true;
        lblPath.Dock = DockStyle.Fill;
        lblPath.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblPath.ForeColor = Color.FromArgb(60, 70, 80);
        lblPath.Location = new Point(181, 8);
        lblPath.Margin = new Padding(0, 0, 8, 0);
        lblPath.Name = "lblPath";
        lblPath.Size = new Size(36, 36);
        lblPath.TabIndex = 2;
        lblPath.Text = "Path:";
        lblPath.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // txtPath
        // 
        txtPath.BackColor = Color.FromArgb(248, 250, 252);
        txtPath.BorderStyle = BorderStyle.FixedSingle;
        txtPath.Dock = DockStyle.Fill;
        txtPath.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        txtPath.Location = new Point(225, 12);
        txtPath.Margin = new Padding(0, 4, 0, 4);
        txtPath.Name = "txtPath";
        txtPath.ReadOnly = true;
        txtPath.Size = new Size(865, 24);
        txtPath.TabIndex = 3;
        // 
        // splitContainer1
        // 
        splitContainer1.BackColor = Color.FromArgb(225, 232, 240);
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 52);
        splitContainer1.Name = "splitContainer1";
        // 
        // splitContainer1.Panel1
        // 
        splitContainer1.Panel1.BackColor = Color.White;
        splitContainer1.Panel1.Controls.Add(treeViewExplorer);
        splitContainer1.Panel1.Padding = new Padding(1);
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.BackColor = Color.White;
        splitContainer1.Panel2.Controls.Add(listViewFiles);
        splitContainer1.Panel2.Padding = new Padding(1);
        splitContainer1.Size = new Size(1100, 598);
        splitContainer1.SplitterDistance = 340;
        splitContainer1.SplitterWidth = 6;
        splitContainer1.TabIndex = 1;
        // 
        // treeViewExplorer
        // 
        treeViewExplorer.BackColor = Color.White;
        treeViewExplorer.BorderStyle = BorderStyle.None;
        treeViewExplorer.Dock = DockStyle.Fill;
        treeViewExplorer.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        treeViewExplorer.ForeColor = Color.Black;
        treeViewExplorer.FullRowSelect = true;
        treeViewExplorer.HideSelection = false;
        treeViewExplorer.ImageIndex = 0;
        treeViewExplorer.ImageList = imageListIcons;
        treeViewExplorer.Location = new Point(1, 1);
        treeViewExplorer.Name = "treeViewExplorer";
        treeViewExplorer.SelectedImageIndex = 0;
        treeViewExplorer.Size = new Size(338, 596);
        treeViewExplorer.TabIndex = 0;
        treeViewExplorer.BeforeExpand += treeViewExplorer_BeforeExpand;
        treeViewExplorer.AfterSelect += treeViewExplorer_AfterSelect;
        // 
        // imageListIcons
        // 
        imageListIcons.ColorDepth = ColorDepth.Depth32Bit;
        imageListIcons.ImageSize = new Size(16, 16);
        imageListIcons.TransparentColor = Color.Transparent;
        // 
        // listViewFiles
        // 
        listViewFiles.BackColor = Color.White;
        listViewFiles.BorderStyle = BorderStyle.None;
        listViewFiles.Columns.AddRange(new ColumnHeader[] { columnName, columnType, columnSize });
        listViewFiles.Dock = DockStyle.Fill;
        listViewFiles.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
        listViewFiles.ForeColor = Color.Black;
        listViewFiles.FullRowSelect = true;
        listViewFiles.GridLines = false;
        listViewFiles.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        listViewFiles.Location = new Point(1, 1);
        listViewFiles.MultiSelect = false;
        listViewFiles.Name = "listViewFiles";
        listViewFiles.Size = new Size(752, 596);
        listViewFiles.SmallImageList = imageListIcons;
        listViewFiles.TabIndex = 0;
        listViewFiles.UseCompatibleStateImageBehavior = false;
        listViewFiles.View = View.Details;
        listViewFiles.DoubleClick += listViewFiles_DoubleClick;
        // 
        // columnName
        // 
        columnName.Text = "Name";
        columnName.Width = 360;
        // 
        // columnType
        // 
        columnType.Text = "Type";
        columnType.Width = 160;
        // 
        // columnSize
        // 
        columnSize.Text = "Size";
        columnSize.Width = 120;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1100, 650);
        Controls.Add(splitContainer1);
        Controls.Add(topPanel);
        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
        ForeColor = Color.Black;
        MinimumSize = new Size(900, 550);
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "File Explorer – مستكشف الملفات";
        Load += Form1_Load;
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        splitContainer1.Panel1.ResumeLayout(false);
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel topPanel;
    private Button btnBack;
    private Button btnRefresh;
    private Label lblPath;
    private TextBox txtPath;
    private SplitContainer splitContainer1;
    private TreeView treeViewExplorer;
    private ImageList imageListIcons;
    private ListView listViewFiles;
    private ColumnHeader columnName;
    private ColumnHeader columnType;
    private ColumnHeader columnSize;
}