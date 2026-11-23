using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;

namespace FileExplorerWinForms;

public partial class Form1 : Form
{
    private const string DummyNodeTag = "__DUMMY__";
    private const int IconDrive = 0;
    private const int IconFolder = 1;
    private const int IconFile = 2;

    private readonly Stack<string> _backHistory = new();
    private string? _currentPath;
    private bool _suppressSelectionChanged;

    public Form1()
    {
        InitializeComponent();
        InitializeIcons();
    }

    private void Form1_Load(object? sender, EventArgs e)
    {
        LoadDrives();
    }

    private void InitializeIcons()
    {
        imageListIcons.Images.Clear();
        imageListIcons.Images.Add(CreateDriveIcon());   // 0
        imageListIcons.Images.Add(CreateFolderIcon());  // 1
        imageListIcons.Images.Add(CreateFileIcon());    // 2
    }

    private static Bitmap CreateIconBitmap(Action<Graphics, Rectangle> draw)
    {
        Bitmap bmp = new(16, 16);
        using Graphics g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);
        draw(g, new Rectangle(0, 0, 15, 15));
        return bmp;
    }

    private static Bitmap CreateDriveIcon()
    {
        return CreateIconBitmap((g, _) =>
        {
            using var bodyBrush = new SolidBrush(Color.FromArgb(96, 125, 139));
            using var bodyPen = new Pen(Color.FromArgb(69, 90, 100), 1);
            using var slotBrush = new SolidBrush(Color.FromArgb(236, 239, 241));
            g.FillRoundedRectangle(bodyBrush, new Rectangle(1, 4, 14, 9), 2);
            g.DrawRoundedRectangle(bodyPen, new Rectangle(1, 4, 14, 9), 2);
            g.FillRectangle(slotBrush, new Rectangle(4, 9, 8, 2));
            g.FillEllipse(Brushes.LightGreen, new Rectangle(12, 6, 2, 2));
        });
    }

    private static Bitmap CreateFolderIcon()
    {
        return CreateIconBitmap((g, _) =>
        {
            using var tabBrush = new SolidBrush(Color.FromArgb(255, 193, 7));
            using var bodyBrush = new SolidBrush(Color.FromArgb(255, 213, 79));
            using var pen = new Pen(Color.FromArgb(200, 150, 0), 1);
            var tab = new Rectangle(1, 3, 6, 3);
            var body = new Rectangle(1, 5, 14, 9);
            g.FillRectangle(tabBrush, tab);
            g.FillRoundedRectangle(bodyBrush, body, 1);
            g.DrawRoundedRectangle(pen, body, 1);
        });
    }

    private static Bitmap CreateFileIcon()
    {
        return CreateIconBitmap((g, _) =>
        {
            using var fill = new SolidBrush(Color.FromArgb(250, 252, 255));
            using var pen = new Pen(Color.FromArgb(120, 144, 156), 1);
            using var foldPen = new Pen(Color.FromArgb(144, 164, 174), 1);
            Point[] page = { new(3, 1), new(10, 1), new(13, 4), new(13, 14), new(3, 14) };
            g.FillPolygon(fill, page);
            g.DrawPolygon(pen, page);
            g.DrawLine(foldPen, 10, 1, 10, 4);
            g.DrawLine(foldPen, 10, 4, 13, 4);
            using var linePen = new Pen(Color.FromArgb(176, 190, 197), 1);
            g.DrawLine(linePen, 5, 7, 11, 7);
            g.DrawLine(linePen, 5, 9, 11, 9);
            g.DrawLine(linePen, 5, 11, 9, 11);
        });
    }

    private void LoadDrives()
    {
        treeViewExplorer.BeginUpdate();
        treeViewExplorer.Nodes.Clear();
        listViewFiles.Items.Clear();
        _backHistory.Clear();
        btnBack.Enabled = false;

        try
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                string displayName = drive.Name;
                string typeText = drive.DriveType.ToString();

                try
                {
                    if (drive.IsReady && !string.IsNullOrWhiteSpace(drive.VolumeLabel))
                    {
                        displayName = $"{drive.VolumeLabel} ({drive.Name.TrimEnd('\\')})";
                    }
                }
                catch
                {
                    // Ignore volume label access issues.
                }

                TreeNode driveNode = new TreeNode(displayName)
                {
                    Tag = drive.Name,
                    ImageIndex = IconDrive,
                    SelectedImageIndex = IconDrive,
                    ToolTipText = $"{drive.Name} - {typeText}"
                };

                try
                {
                    if (drive.IsReady)
                    {
                        driveNode.Nodes.Add(CreateDummyNode());
                    }
                }
                catch
                {
                    // Not ready or inaccessible drive; show without children.
                }

                treeViewExplorer.Nodes.Add(driveNode);
            }

            SetCurrentPath(string.Empty);
        }
        catch (Exception ex)
        {
            ShowError("Failed to load drives.", ex);
        }
        finally
        {
            treeViewExplorer.EndUpdate();
        }
    }

    private static TreeNode CreateDummyNode()
    {
        return new TreeNode("Loading...")
        {
            Tag = DummyNodeTag
        };
    }

    private void treeViewExplorer_BeforeExpand(object? sender, TreeViewCancelEventArgs e)
    {
        TreeNode? node = e.Node;
        if (node == null) return;

        if (node.Nodes.Count == 1 && IsDummyNode(node.Nodes[0]))
        {
            LoadChildDirectories(node);
        }
    }

    private void treeViewExplorer_AfterSelect(object? sender, TreeViewEventArgs e)
    {
        if (_suppressSelectionChanged) return;

        TreeNode? node = e.Node;
        if (node?.Tag is not string path || path == DummyNodeTag)
        {
            return;
        }

        NavigateTo(path, addToHistory: true);
    }

    private void LoadChildDirectories(TreeNode node)
    {
        if (node.Tag is not string path || path == DummyNodeTag)
        {
            return;
        }

        treeViewExplorer.BeginUpdate();
        node.Nodes.Clear();

        try
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                DirectoryInfo info = new(directory);
                if (ShouldSkipDirectory(info))
                {
                    continue;
                }

                TreeNode childNode = new TreeNode(info.Name)
                {
                    Tag = info.FullName,
                    ImageIndex = IconFolder,
                    SelectedImageIndex = IconFolder,
                    ToolTipText = info.FullName
                };

                if (HasAccessibleChildDirectories(info.FullName))
                {
                    childNode.Nodes.Add(CreateDummyNode());
                }

                node.Nodes.Add(childNode);
            }
        }
        catch (UnauthorizedAccessException)
        {
            node.ToolTipText = "Access denied";
        }
        catch (DirectoryNotFoundException)
        {
            node.ToolTipText = "Directory not found";
        }
        catch (IOException)
        {
            node.ToolTipText = "I/O error";
        }
        catch (Exception ex)
        {
            ShowError("Failed to load folders.", ex);
        }
        finally
        {
            treeViewExplorer.EndUpdate();
        }
    }

    private static bool ShouldSkipDirectory(DirectoryInfo info)
    {
        FileAttributes attrs = info.Attributes;
        return (attrs & FileAttributes.System) != 0
            || (attrs & FileAttributes.ReparsePoint) != 0;
    }

    private static bool HasAccessibleChildDirectories(string path)
    {
        try
        {
            using IEnumerator<string> e = Directory.EnumerateDirectories(path).GetEnumerator();
            return e.MoveNext();
        }
        catch
        {
            return false;
        }
    }

    private void NavigateTo(string path, bool addToHistory)
    {
        string normalized = NormalizePath(path);
        if (addToHistory && !string.IsNullOrEmpty(_currentPath) &&
            !PathsEqual(_currentPath, normalized) && Directory.Exists(_currentPath))
        {
            _backHistory.Push(_currentPath);
            btnBack.Enabled = _backHistory.Count > 0;
        }

        SetCurrentPath(normalized);
        ShowDirectoryContents(normalized);
        btnBack.Enabled = _backHistory.Count > 0;
    }

    private void ShowDirectoryContents(string path)
    {
        listViewFiles.BeginUpdate();
        listViewFiles.Items.Clear();

        try
        {
            if (!Directory.Exists(path))
            {
                ShowWarning("Folder not found", "The selected folder no longer exists.");
                return;
            }

            DirectoryInfo[] directories;
            FileInfo[] files;
            try
            {
                DirectoryInfo root = new(path);
                directories = root.GetDirectories();
                files = root.GetFiles();
            }
            catch (UnauthorizedAccessException)
            {
                ShowWarning("Access Denied", "You do not have permission to access this folder.");
                return;
            }

            Array.Sort(directories, static (a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            Array.Sort(files, static (a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

            foreach (DirectoryInfo dir in directories)
            {
                if ((dir.Attributes & FileAttributes.System) != 0) continue;

                ListViewItem item = new(dir.Name)
                {
                    Tag = dir.FullName,
                    ImageIndex = IconFolder
                };
                item.SubItems.Add("Folder");
                item.SubItems.Add(string.Empty);
                listViewFiles.Items.Add(item);
            }

            foreach (FileInfo file in files)
            {
                ListViewItem item = new(file.Name)
                {
                    Tag = file.FullName,
                    ImageIndex = IconFile
                };
                item.SubItems.Add(string.IsNullOrWhiteSpace(file.Extension) ? "File" : file.Extension.ToUpperInvariant());
                item.SubItems.Add(FormatFileSize(file.Length));
                listViewFiles.Items.Add(item);
            }
        }
        catch (DirectoryNotFoundException)
        {
            ShowWarning("Folder Not Found", "The selected folder no longer exists.");
        }
        catch (IOException ex)
        {
            ShowWarning("I/O Error", $"Unable to read folder contents.\n{ex.Message}");
        }
        catch (Exception ex)
        {
            ShowError("Failed to load files.", ex);
        }
        finally
        {
            listViewFiles.EndUpdate();
        }
    }

    private void listViewFiles_DoubleClick(object? sender, EventArgs e)
    {
        if (listViewFiles.SelectedItems.Count == 0) return;

        ListViewItem selectedItem = listViewFiles.SelectedItems[0];
        if (selectedItem.Tag is not string fullPath) return;

        try
        {
            if (Directory.Exists(fullPath))
            {
                SelectTreeNodeByPath(fullPath);
                NavigateTo(fullPath, addToHistory: true);
                return;
            }

            if (File.Exists(fullPath))
            {
                Process.Start(new ProcessStartInfo(fullPath)
                {
                    UseShellExecute = true
                });
                return;
            }

            ShowWarning("Not Found", "The selected item no longer exists.");
        }
        catch (Exception ex)
        {
            ShowError("Unable to open the selected item.", ex);
        }
    }

    private void btnBack_Click(object? sender, EventArgs e)
    {
        while (_backHistory.Count > 0)
        {
            string previous = _backHistory.Pop();
            if (Directory.Exists(previous))
            {
                SelectTreeNodeByPath(previous);
                SetCurrentPath(previous);
                ShowDirectoryContents(previous);
                btnBack.Enabled = _backHistory.Count > 0;
                return;
            }
        }

        btnBack.Enabled = false;
    }

    private void btnRefresh_Click(object? sender, EventArgs e)
    {
        RefreshCurrentView();
    }

    private void RefreshCurrentView()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_currentPath) || !Directory.Exists(_currentPath))
            {
                LoadDrives();
                return;
            }

            string path = _currentPath;
            TreeNode? node = FindNodeByPath(path);
            if (node != null)
            {
                node.Nodes.Clear();
                node.Nodes.Add(CreateDummyNode());
                LoadChildDirectories(node);
                node.Expand();
            }

            ShowDirectoryContents(path);
            SetCurrentPath(path);
        }
        catch (Exception ex)
        {
            ShowError("Failed to refresh the view.", ex);
        }
    }

    private void SelectTreeNodeByPath(string path)
    {
        string normalizedTarget = NormalizePath(path);
        TreeNode? node = EnsureNodeForPath(normalizedTarget);
        if (node == null) return;

        _suppressSelectionChanged = true;
        try
        {
            treeViewExplorer.SelectedNode = node;
            node.EnsureVisible();
        }
        finally
        {
            _suppressSelectionChanged = false;
        }
    }

    private TreeNode? EnsureNodeForPath(string normalizedTarget)
    {
        TreeNode? current = null;
        TreeNodeCollection nodes = treeViewExplorer.Nodes;

        // Walk down from drives, expanding as needed.
        for (int depth = 0; depth < 256; depth++)
        {
            TreeNode? match = null;
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is not string nodePath || nodePath == DummyNodeTag) continue;
                string normalizedNode = NormalizePath(nodePath);
                if (PathsEqual(normalizedNode, normalizedTarget) ||
                    IsSubPath(normalizedTarget, normalizedNode))
                {
                    // Prefer exact match; otherwise pick the node that is on the path.
                    if (PathsEqual(normalizedNode, normalizedTarget))
                    {
                        match = node;
                        break;
                    }

                    if (match == null || NormalizePath((string)match.Tag!).Length < normalizedNode.Length)
                    {
                        match = node;
                    }
                }
            }

            if (match == null) break;

            current = match;
            if (PathsEqual(NormalizePath((string)match.Tag!), normalizedTarget))
            {
                return match;
            }

            if (match.Nodes.Count == 1 && IsDummyNode(match.Nodes[0]))
            {
                LoadChildDirectories(match);
            }
            match.Expand();
            nodes = match.Nodes;
        }

        // Fallback: direct lookup.
        return current != null && PathsEqual(NormalizePath((string)current.Tag!), normalizedTarget)
            ? current
            : FindNodeByPath(normalizedTarget);
    }

    private TreeNode? FindNodeByPath(string path)
    {
        string normalized = NormalizePath(path);
        Stack<TreeNode> stack = new();
        foreach (TreeNode n in treeViewExplorer.Nodes) stack.Push(n);
        while (stack.Count > 0)
        {
            TreeNode node = stack.Pop();
            if (node.Tag is string p && p != DummyNodeTag && PathsEqual(NormalizePath(p), normalized))
            {
                return node;
            }
            foreach (TreeNode child in node.Nodes) stack.Push(child);
        }
        return null;
    }

    private void SetCurrentPath(string path)
    {
        _currentPath = path;
        txtPath.Text = path;
    }

    private static bool IsDummyNode(TreeNode node)
    {
        return node.Tag is string tag && tag == DummyNodeTag;
    }

    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return string.Empty;
        string full = Path.GetFullPath(path);
        return full.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    private static bool PathsEqual(string a, string b)
    {
        return string.Equals(
            NormalizePath(a),
            NormalizePath(b),
            StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsSubPath(string candidate, string parent)
    {
        string c = NormalizePath(candidate);
        string p = NormalizePath(parent);
        if (c.Length <= p.Length) return false;
        if (!c.StartsWith(p, StringComparison.OrdinalIgnoreCase)) return false;
        char sep = c[p.Length];
        return sep == Path.DirectorySeparatorChar || sep == Path.AltDirectorySeparatorChar;
    }

    private static string FormatFileSize(long bytes)
    {
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        double size = bytes;
        int unitIndex = 0;
        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return unitIndex == 0
            ? $"{bytes} {units[unitIndex]}"
            : $"{size:0.##} {units[unitIndex]}";
    }

    private static void ShowError(string message, Exception ex)
    {
        MessageBox.Show(
            $"{message}\n{ex.Message}",
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private static void ShowWarning(string title, string message)
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}

internal static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
    {
        using GraphicsPath path = CreateRoundedPath(rect, radius);
        g.FillPath(brush, path);
    }

    public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
    {
        using GraphicsPath path = CreateRoundedPath(rect, radius);
        g.DrawPath(pen, path);
    }

    private static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
    {
        int d = radius * 2;
        GraphicsPath path = new();
        d = Math.Min(d, Math.Min(rect.Width, rect.Height));
        if (d <= 0)
        {
            path.AddRectangle(rect);
            return path;
        }
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}