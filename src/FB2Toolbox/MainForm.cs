using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Linq;
using System.Collections;

namespace FB2Toolbox
{
    public partial class MainForm : Form
    {
        #region Private
        private bool inProgress_Value = false;
        private FileProperties itemsFilter = null;
        private Dictionary<string, string> _loadedFileIDs = new Dictionary<string, string>();
        // private int _selectedCount = 0;
        private readonly List<ListViewItem> _itemsCache = new List<ListViewItem>();
        #endregion

        private string _ver;
        public MainForm()
        {
            InitializeComponent();
            Extensions.SetDoubleBuffered(filesView, true);

            IsCancel = false;
            InProgress = false;

            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            _ver = String.Format("(v{0}.{1}{2})", v.Major, v.Minor, v.Build > 0 ? "." + v.Build : "");
            SetApplicationTitle();
        }
        private string _rootPath = "";
        private void SetApplicationTitle(string path = "")
        {
            if (path != String.Empty)
            {
                if (_rootPath == "")
                    _rootPath = path;
                else {
                    if (_rootPath.Length > path.Length || path.Substring(0, _rootPath.Length) != _rootPath)
                    {
                        var fPath = path.Split(Path.DirectorySeparatorChar);
                        var rPath = _rootPath.Split(Path.DirectorySeparatorChar);
                        var result = new List<string>();
                        var len = fPath.Length > rPath.Length ? rPath.Length : fPath.Length;
                        for (var i = 0; i < len; i++)
                        {
                            if (fPath[i] == rPath[i])
                                result.Add(fPath[i]);
                            else
                                break;
                        }
                        _rootPath = String.Join(Path.DirectorySeparatorChar.ToString(), result.ToArray());
                    }
                }
            } else
            {
                _rootPath = "";
            }
            
            Text = string.Format("{0} {1}{2}", Application.ProductName, _ver, _rootPath != "" ? ": " + _rootPath : "" );
        }
        private Dictionary<string, string> LoadedFileIDs
        {
            get
            {
                return _loadedFileIDs;
            }
        }
        private List<ListViewItem> ItemsCache
        {
            get
            {
                return _itemsCache;
            }
        }
        private int SelectedCount
        {
            get
            {
                return filesView.CheckedItems.Count;
            }
        }
        private bool IsCancel { get; set; }
        private bool InProgress
        {
            get
            {
                return inProgress_Value;
            }
            set
            {
                if (inProgress_Value != value)
                {
                    inProgress_Value = value;
                    if (inProgress_Value)
                    {
                        statusCancelProgressLabel.Visible = true;
                        IsCancel = false;
                    }
                    fileToolStripMenuItem.Enabled = !inProgress_Value;
                    otherToolStripMenuItem.Enabled = !inProgress_Value;
                    actionsToolStripMenuItem.Enabled = !inProgress_Value && (SelectedCount > 0);
                    Cursor = inProgress_Value ? Cursors.WaitCursor : Cursors.Default;
                    if (!inProgress_Value)
                    {
                        statusCancelProgressLabel.Visible = false;
                        AddMessageRN(Properties.Resources.Ready);
                    }
                }
            }
        }
        #region File menu events

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Properties.Settings.Default.DefaultAddFiltesPath;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.DefaultAddFiltesPath = openFileDialog.InitialDirectory;
                Properties.Settings.Default.Save();
                AddFiles(openFileDialog.FileNames);
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = Properties.Settings.Default.DefaultAddFolderPath;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bool recursive = true;
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                if (item != null)
                    recursive = Convert.ToBoolean(item.Tag);

                Properties.Settings.Default.DefaultAddFolderPath = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
                AddDir(folderBrowserDialog.SelectedPath, recursive);
            }
        }

        private void AddFiles(string[] files)
        {
            InProgress = true;
            var fs = ImportFiles(files);
            AddItemsToList(fs);
            InProgress = false;
            UpdateStatus();
        }

        private void AddDir(string dir, bool recursive)
        {
            InProgress = true;
            messagesTextBox.Enabled = false;
            var fs = ReadFolder(dir, recursive);
            if (fs != null && fs.Count > 0) AddItemsToList(fs);
            messagesTextBox.Enabled = true;
            InProgress = false;
            UpdateStatus();
        }

        private List<FB2File> ReadFolder(string folder, bool recursive)
        {
            if (IsCancel || !Directory.Exists(folder))
                return null;

            var result = new List<FB2File>();
            var dirInfo = new DirectoryInfo(folder);

            UpdateStatus(String.Format(Properties.Resources.ProgressScanFolder, dirInfo.FullName));
            try
            {
                FileInfo[] files = dirInfo.GetFiles("*" + FB2Config.Current.FB2Extension);
                FileInfo[] zipFiles = dirInfo.GetFiles("*" + FB2Config.Current.FB2ZIPExtension);
                if (files.Length > 0)
                {
                    List<string> list = new List<string>();
                    foreach (FileInfo fi in files)
                    {
                        list.Add(fi.FullName);
                    }
                    var fclist = ImportFiles(list.ToArray());
                    result.AddRange(fclist);
                }
                
                if (IsCancel)
                    return null;

                if (zipFiles.Length > 0)
                {
                    List<string> list = new List<string>();
                    foreach (FileInfo fi in zipFiles)
                    {
                        list.Add(fi.FullName);
                    }
                    var fclist = ImportFiles(list.ToArray());
                    result.AddRange(fclist);
                }
                
                if (IsCancel)
                    return null;

                if (recursive)
                {
                    DirectoryInfo[] directories = dirInfo.GetDirectories();
                    foreach (DirectoryInfo di in directories)
                    {
                        List<FB2File> r = ReadFolder(di.FullName, recursive);
                        if (r != null && r.Count > 0) result.AddRange(r);
                    }
                }
            }
            catch (Exception ex)
            {
                AddErrorRN(ex.Message);
            }

            return result;
        }

        private List<FB2File> ImportFiles(string[] fileNames)
        {
            List<FB2File> containers = new List<FB2File>();
            foreach (string fileName in fileNames)
            {
                try
                {
                    FB2File fc = new FB2File(fileName);
                    containers.Add(fc);
                    UpdateStatus(String.Format(Properties.Resources.ProgressFileLoaded, fc.FileInformation.Name));
                    if (CheckCancel())
                        break;
                }
                catch (Exception ex)
                {
                    AddErrorRN(String.Format(Properties.Resources.ProgressFileLoadError, fileName, ex.Message));
                }
                if (CheckCancel())
                    break;
            }
            return containers;
        }

        private bool _dragStarted = false;
        private int _dragX = -1;
        private int _dragY = -1;
        private void filesView_MouseDown(object sender, MouseEventArgs e)
        {
            _dragX = MousePosition.X;
            _dragY = MousePosition.Y;
        }

        private void filesView_MouseUp(object sender, MouseEventArgs e)
        {
            _dragStarted = false;
            _dragX = -1;
            _dragY = -1;
            Cursor = Cursors.Default;
        }

        private void filesView_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragX < 0 || _dragY < 0)
                return;

            if (!_dragStarted && Math.Sqrt((_dragX - MousePosition.X) * (_dragX - MousePosition.X) + (_dragY - MousePosition.Y) * (_dragY - MousePosition.Y)) > 100)
            {
                _dragStarted = true;
                StartDragFiles();
            }
        }

        private void filesView_MouseLeave(object sender, EventArgs e)
        {
            if (!_dragStarted)
                return;

            _dragStarted = true;
            StartDragFiles();
        }

        private void StartDragFiles()
        {
            var items = filesView.CheckedItems.Count > 0 ? filesView.CheckedItems : (filesView.SelectedItems.Count > 0 ? (IList)filesView.SelectedItems : null);
            if (items == null || items.Count == 0)
            {
                return;
            }

            var files = new List<string>();
            foreach (ListViewItem item in items)
            {
                files.Add(((FB2File)item.Tag).FileInformation.FullName);
            }

            this.DoDragDrop(new DataObject(DataFormats.FileDrop, files.ToArray()), DragDropEffects.Copy);

        }

        private void filesView_DragEnter(object sender, DragEventArgs e)
        {
            if (!_dragStarted && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void filesView_DragDrop(object sender, DragEventArgs e)
        {
            if (_dragStarted)
                return;

            var _list = (string[])e.Data.GetData(DataFormats.FileDrop);
            var _files = new List<string>();
            var _dirs = new List<string>();
            if (filesView.Items.Count > 0 && MessageBox.Show(Properties.Resources.ConfirmationClearFileList, Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                ClearLists();
            foreach (var fi in _list)
            {
                if (File.Exists(fi)) _files.Add(fi);
                else if (Directory.Exists(fi)) _dirs.Add(fi);
            }
            InProgress = true;
            if (_files.Count > 0)
            {
                AddItemsToList(ImportFiles(_files.ToArray()));
            }
            if (_dirs.Count > 0)
                foreach (var dir in _dirs)
                {
                    var fs = ReadFolder(dir, true);
                    if (fs != null && fs.Count > 0) AddItemsToList(fs);
                    if (IsCancel) break;
                }
            UpdateStatus();
            InProgress = false;
        }

        #endregion

        private void AddErrorRN(string message)
        {
            System.Drawing.Color prevColor = messagesTextBox.SelectionColor;
            messagesTextBox.SelectionColor = System.Drawing.Color.Red;
            messagesTextBox.AppendText(String.Format("{0}\r\n", message));
            messagesTextBox.SelectionColor = prevColor;
            messagesTextBox.SelectionStart = messagesTextBox.Text.Length;
            messagesTextBox.ScrollToCaret();
            messagesTextBox.Update();
        }
        private void AddMessage(string message)
        {
            messagesTextBox.AppendText(String.Format("{0}", message));
            messagesTextBox.Update();
        }
        private void AddMessageRN(string message)
        {
            messagesTextBox.AppendText(String.Format("{0}\r\n", message));
            messagesTextBox.SelectionStart = messagesTextBox.Text.Length;
            messagesTextBox.ScrollToCaret();
            messagesTextBox.Update();
        }
        private void UpdateItem(ListViewItem item)
        {
            if (item.SubItems.Count < 8)
            {
                item.SubItems.Clear();
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
                item.SubItems.Add(String.Empty);
            }
            FB2File fc = item.Tag as FB2File;
            item.Text = fc.BookTitle;
            item.SubItems[columnBookSequence.Index].Text = String.IsNullOrEmpty(fc.BookSequenceName) ? Properties.Resources.DisplayNoSerie : fc.BookSequenceName;
            item.SubItems[columnBookSequenceNr.Index].Text = fc.BookSequenceNr.HasValue ? fc.BookSequenceNr.Value.ToString() : String.Empty;
            item.SubItems[columnAuthorFirstName.Index].Text = fc.BookAuthorFirstName;
            item.SubItems[columnAuthorMiddleName.Index].Text = fc.BookAuthorMiddleName;
            item.SubItems[columnAuthorLastName.Index].Text = fc.BookAuthorLastName;
            item.SubItems[columnGenre.Index].Text = fc.BookGenre;
            item.SubItems[columnEncoding.Index].Text = fc.BookEncoding;
            item.SubItems[columnFileSize.Index].Text = fc.BookSizeText;
            item.SubItems[columnVersion.Index].Text = fc.BookVersion;
            item.ToolTipText = fc.FileInformation.FullName;

            UpdateGroup(item);

            item.ImageIndex = 0;
            if (fc.IsZIP())
                item.ImageIndex = 1;
            if (!fc.IsValid)
            {
                string error = String.Format(Properties.Resources.ProgressFileLoadError, fc.FileInformation.FullName, fc.Error());
                item.ToolTipText = error;
                item.ImageIndex = 2;
            }
        }
        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
            statusStrip.Update();
        }
        private void AddItemsToList(List<FB2File> list)
        {
            UpdateStatus(String.Format(Properties.Resources.ParseFileListLoad, list.Count));
            // list.Sort();
            filesView.BeginUpdate();
            foreach (FB2File fc in list)
            {
                string key = fc.FileInformation.FullName;
                if (LoadedFileIDs.ContainsKey(key))
                {
                    AddMessageRN(String.Format(Properties.Resources.ProgressFileAlreadyLoaded, key));
                }
                else
                {
                    LoadedFileIDs.Add(key, String.Empty);
                    ListViewItem lvi = new ListViewItem(fc.BookTitle);
                    lvi.Tag = fc;
                    UpdateItem(lvi);
                    if (!fc.IsValid)
                        AddMessageRN(lvi.ToolTipText);
                    filesView.Items.Add(lvi);
                    ItemsCache.Add(lvi);
                    AddMessage(String.Format(Properties.Resources.ProgressFileLoaded + "\r\n", key));
                    SetApplicationTitle(fc.FileInformation.FullName);
                }
            }
            // filesView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            filesView.EndUpdate();
            CheckMenus();
        }
        private void ClearLists()
        {
            filesView.BeginUpdate();
            filesView.Items.Clear();
            ItemsCache.Clear();
            filesView.Groups.Clear();
            filesView.EndUpdate();
            messagesTextBox.Clear();
            LoadedFileIDs.Clear();
            UpdateStatus();
            SetApplicationTitle();
        }
        private void CheckMenus()
        {
            actionsToolStripMenuItem.Enabled = SelectedCount > 0;
            clearFilesListMenuItem.Enabled = filesView.Items.Count > 0;
            checkAllToolStripMenuItem.Enabled = clearFilesListMenuItem.Enabled;
            uncheckAllToolStripMenuItem.Enabled = clearFilesListMenuItem.Enabled;
        }
        private void clearFilesListMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.ConfirmationClearFileList, Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                ClearLists();
        }
        private void UpdateStatus()
        {
            statusLabel.Text = String.Empty;
            statusSelectedFilesLabel.Text = String.Format(Properties.Resources.StatusBarFilesCount, SelectedCount, filesView.Items.Count);
        }
        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = InProgress;
        }
        private void openSelectFileMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                if (fc != null)
                   System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + fc.FileInformation.FullName + "\"");
            }
        }
        private void openFolderMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                if (fc != null)
                   System.Diagnostics.Process.Start("explorer.exe", "/select,\"" + fc.FileInformation.DirectoryName + "\"");
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (InProgress)
                e.Cancel = true;
            else
            {
                viewDescriptionToolStripMenuItem.Enabled = filesView.FocusedItem != null;
                openSelectFileMenuItem.Enabled = filesView.FocusedItem != null;
                openFolderMenuItem.Enabled = openSelectFileMenuItem.Enabled;
                openFileToolStripMenuItem.Enabled = openSelectFileMenuItem.Enabled;
                reloadToolStripMenuItem.Enabled = openSelectFileMenuItem.Enabled;
                metadataSingleToolStripMenuItem.Enabled = filesView.FocusedItem != null;
                commandContextToolStripMenuItem.Enabled = openSelectFileMenuItem.Enabled;
            }
        }
        private void moveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverwriteDialog.Reset();
            RenameProfileElement pe = (sender as ToolStripMenuItem).Tag as RenameProfileElement;
            bool useTranslit = (sender as ToolStripMenuItem).Text.StartsWith(Properties.Resources.TranslitMenuText);
            if (pe != null)
            {
                folderBrowserDialog.SelectedPath = Properties.Settings.Default.DefaultMoveToPath;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    InProgress = true;
                    Properties.Settings.Default.DefaultMoveToPath = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                    foreach (ListViewItem item in filesView.CheckedItems)
                    {
                        FB2File fc = item.Tag as FB2File;
                        string oldName = fc.FileInformation.FullName;
                        try
                        {
                            var processed = fc.MoveTo(folderBrowserDialog.SelectedPath, pe, useTranslit);
                            if (!processed.Skipped)
                            {
                                LoadedFileIDs.Remove(oldName);
                                if (!LoadedFileIDs.ContainsKey(fc.FileInformation.FullName))
                                    LoadedFileIDs.Add(fc.FileInformation.FullName, string.Empty);
                                AddMessageRN(String.Format(Properties.Resources.MoveFileSuccess, oldName, fc.FileInformation.FullName));
                            }
                            else
                            {
                                AddErrorRN(String.Format(Properties.Resources.MoveFileSkip, oldName, processed.NewFullName));
                            }
                        }
                        catch (Exception ex)
                        {
                            AddErrorRN(String.Format(Properties.Resources.MoveFileError, oldName, ex.Message));
                        }
                        UpdateItem(item);
                        if (CheckCancel())
                            break;
                    }
                    InProgress = false;
                }
            }
        }
        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverwriteDialog.Reset();
            RenameProfileElement pe = (sender as ToolStripMenuItem).Tag as RenameProfileElement;
            bool useTranslit = (sender as ToolStripMenuItem).Text.StartsWith(Properties.Resources.TranslitMenuText);
            if (pe != null)
            {
                folderBrowserDialog.SelectedPath = Properties.Settings.Default.DefaultCopyToPath;
                if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    InProgress = true;
                    Properties.Settings.Default.DefaultCopyToPath = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                    foreach (ListViewItem item in filesView.CheckedItems)
                    {
                        FB2File fc = item.Tag as FB2File;
                        string oldName = fc.FileInformation.FullName;
                        try
                        {
                            var processed = fc.CopyTo(folderBrowserDialog.SelectedPath, pe, useTranslit);
                            if (!processed.Skipped)
                            {
                                LoadedFileIDs.Remove(oldName);
                                if (!LoadedFileIDs.ContainsKey(fc.FileInformation.FullName))
                                    LoadedFileIDs.Add(fc.FileInformation.FullName, string.Empty);
                                AddMessageRN(String.Format(Properties.Resources.CopyFileSuccess, oldName, fc.FileInformation.FullName));
                            }
                            else
                                AddErrorRN(String.Format(Properties.Resources.CopyFileSkipped, oldName, processed.NewFullName));
                        }
                        catch (Exception ex)
                        {
                            AddErrorRN(String.Format(Properties.Resources.CopyFileError, oldName, ex.Message));
                        }
                        UpdateItem(item);
                        if (CheckCancel())
                            break;
                    }
                    InProgress = false;
                }
            }
        }
        private void renameToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverwriteDialog.Reset();
            RenameProfileElement pe = (sender as ToolStripMenuItem).Tag as RenameProfileElement;
            bool useTranslit = (sender as ToolStripMenuItem).Text.StartsWith(Properties.Resources.TranslitMenuText);
            if (pe != null)
            {
                if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationRename, SelectedCount, pe.Name), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    InProgress = true;
                    foreach (ListViewItem item in filesView.CheckedItems)
                    {
                        FB2File fc = item.Tag as FB2File;
                        string oldName = fc.FileInformation.Name;
                        string oldFullName = fc.FileInformation.FullName;
                        try
                        {
                            var processed = fc.RenameTo(pe, useTranslit);
                            if (!processed.Skipped)
                            {
                                LoadedFileIDs.Remove(oldFullName);
                                if (!LoadedFileIDs.ContainsKey(fc.FileInformation.FullName))
                                    LoadedFileIDs.Add(fc.FileInformation.FullName, string.Empty);
                                AddMessageRN(String.Format(Properties.Resources.RenameFileSuccess, oldName, fc.FileInformation.Name));
                            }
                            else
                            {
                                AddErrorRN(String.Format(Properties.Resources.RenameFileSkip, oldName, processed.NewFullName));
                            }
                        }
                        catch (Exception ex)
                        {
                            AddErrorRN(String.Format(Properties.Resources.RenameFileError, fc.FileInformation.FullName, ex.Message));
                        }
                        UpdateItem(item);
                        if (CheckCancel())
                            break;
                    }
                    InProgress = false;
                }
            }
        }
        private void ExecuteCommand(ListViewItem item, CommandElement command)
        {
            string commandString = String.Empty;
            try
            {
                FB2File fc = item.Tag as FB2File;
                if (!String.IsNullOrEmpty(command.OnlyWithExtension))
                {
                    if (!fc.FileInformation.FullName.ToLowerInvariant().EndsWith(command.OnlyWithExtension.ToLowerInvariant()))
                    {
                        AddMessageRN(String.Format(Properties.Resources.ExecuteCommandWrongExtension, fc.FileInformation.FullName, command.OnlyWithExtension));
                        return;
                    }
                }

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = command.CreateNoWindow;
                startInfo.FileName = command.FileName;
                startInfo.UseShellExecute = false;
                commandString = String.Format(command.Arguments, fc.FileInformation.FullName);
                startInfo.Arguments = commandString;

                if (string.IsNullOrEmpty(startInfo.FileName) || startInfo.FileName.Trim() == "")
                {
                    AddMessageRN(String.Format(Properties.Resources.ExecuteCommandErrorEmpty, command.FileName));
                    return;
                }

                if (!startInfo.FileName.StartsWith("/") && startInfo.FileName.Substring(1, 2) != ":\\")
                {
                    var enviromentPath = System.Environment.GetEnvironmentVariable("PATH");
                    var paths = enviromentPath.Split(';');
                    var exePath = paths.Select(x => Path.Combine(x, startInfo.FileName))
                                       .Where(x => File.Exists(x))
                                       .FirstOrDefault();
                    if (exePath != null)
                    {
                        startInfo.FileName = exePath;
                    }
                }

                AddMessageRN(String.Format(Properties.Resources.ExecuteCommand, command.FileName, commandString));
                using (Process exeProcess = Process.Start(startInfo))
                {
                    if (command.WaitAndReload)
                    {
                        exeProcess.WaitForExit();
                        fc.Reload();
                        UpdateItem(item);
                    }
                }
            }
            catch (Exception ex)
            {
                AddMessageRN(String.Format(Properties.Resources.ExecuteCommandError, command.FileName, ex.Message));
            }
        }
        private void commandToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommandElement pe = (sender as ToolStripMenuItem).Tag as CommandElement;
            if (pe != null)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    ExecuteCommand(item, pe);
                    if (CheckCancel())
                        break;
                }
                InProgress = false;
            }
        }
        private void commandContextToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                CommandElement pe = (sender as ToolStripMenuItem).Tag as CommandElement;
                if (pe != null)
                {
                    ExecuteCommand(filesView.FocusedItem, pe);
                    filesView.FocusedItem.EnsureVisible();
                }
            }
        }
        private void windows1251ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encoding enc = (sender as ToolStripMenuItem).Tag as Encoding;
            if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationRecode, SelectedCount, enc.EncodingName), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    FB2File fc = item.Tag as FB2File;
                    if (fc.BookEncoding == enc.EncodingName)
                    {
                        string message = String.Format(Properties.Resources.ProgressEncodingAlreadyEncoded, fc.FileInformation.Name, enc.EncodingName);
                        AddMessageRN(message);
                    }
                    else
                    {
                        string olfFileSize = fc.BookSizeText;
                        string message = String.Format(Properties.Resources.ProgressEncoding, fc.FileInformation.Name, enc.EncodingName);
                        AddMessage(message);
                        try
                        {
                            fc.EncodeTo(enc);
                            AddMessageRN(String.Format(Properties.Resources.ProgressFileSizeChange, olfFileSize, fc.BookSizeText));
                        }
                        catch (Exception ex)
                        {
                            message = String.Format(Properties.Resources.ProgressEncodingError, fc.FileInformation.Name, ex.Message);
                            AddErrorRN(message);
                            fc.Reload();
                        }
                        UpdateItem(item);
                    }
                    if (CheckCancel())
                        break;
                }
                UpdateStatus();
                InProgress = false;
            }
        }
        private void filesView_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (InProgress)
            {
                e.NewValue = e.CurrentValue;
                return;
            }
            ListViewItem item = filesView.Items[e.Index];
            FB2File fb2 = item.Tag as FB2File;
            if (!fb2.IsValid)
                e.NewValue = CheckState.Unchecked;
        }
        private void filesView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!InProgress)
            {
                UpdateStatus();
                CheckMenus();
            }
        }
        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in filesView.Items)
                item.Checked = true;
        }
        private void uncheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in filesView.Items)
                item.Checked = false;
        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                if (fc != null)
                    try
                    {
                        System.Diagnostics.Process.Start(fc.FileInformation.FullName);
                    }
                    catch (Exception ex)
                    {
                    }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (EncodingElement ee in FB2Config.Current.Encodings)
            {
                Encoding enc = Encoding.GetEncoding(ee.Name);
                ToolStripMenuItem item = new System.Windows.Forms.ToolStripMenuItem(FB2Config.Current.Encodings.TranslateEncodings ? enc.EncodingName : ee.Name);
                item.Click += new EventHandler(windows1251ToolStripMenuItem_Click);
                item.Tag = enc;
                encodeToolStripMenuItem.DropDownItems.Add(item);
            }
            foreach (RenameProfileElement rp in FB2Config.Current.RenameProfiles)
            {
                ToolStripMenuItem copyItem = new System.Windows.Forms.ToolStripMenuItem(String.Format(Properties.Resources.RenameMenuName, rp.Name));
                copyItem.Click += new EventHandler(copyToToolStripMenuItem_Click);
                copyItem.Tag = rp;
                copyToToolStripMenuItem.DropDownItems.Add(copyItem);

                ToolStripMenuItem moveItem = new System.Windows.Forms.ToolStripMenuItem(String.Format(Properties.Resources.RenameMenuName, rp.Name));
                moveItem.Click += new EventHandler(moveToolStripMenuItem_Click);
                moveItem.Tag = rp;
                moveToToolStripMenuItem.DropDownItems.Add(moveItem);

                ToolStripMenuItem renameItem = new System.Windows.Forms.ToolStripMenuItem(String.Format(Properties.Resources.RenameMenuName, rp.Name));
                renameItem.Click += new EventHandler(renameToToolStripMenuItem_Click);
                renameItem.Tag = rp;
                renameInPlaceToolStripMenuItem.DropDownItems.Add(renameItem);
            }
            ToolStripSeparator i1 = new System.Windows.Forms.ToolStripSeparator();
            copyToToolStripMenuItem.DropDownItems.Add(i1);
            ToolStripSeparator i2 = new System.Windows.Forms.ToolStripSeparator();
            moveToToolStripMenuItem.DropDownItems.Add(i2);
            ToolStripSeparator i3 = new System.Windows.Forms.ToolStripSeparator();
            renameInPlaceToolStripMenuItem.DropDownItems.Add(i3);
            foreach (RenameProfileElement rp in FB2Config.Current.RenameProfiles)
            {
                ToolStripMenuItem copyItem = new System.Windows.Forms.ToolStripMenuItem(Properties.Resources.TranslitMenuText + " " + String.Format(Properties.Resources.RenameMenuName, rp.Name));
                copyItem.Click += new EventHandler(copyToToolStripMenuItem_Click);
                copyItem.Tag = rp;
                copyToToolStripMenuItem.DropDownItems.Add(copyItem);

                ToolStripMenuItem moveItem = new System.Windows.Forms.ToolStripMenuItem(Properties.Resources.TranslitMenuText + " " + String.Format(Properties.Resources.RenameMenuName, rp.Name));
                moveItem.Click += new EventHandler(moveToolStripMenuItem_Click);
                moveItem.Tag = rp;
                moveToToolStripMenuItem.DropDownItems.Add(moveItem);

                ToolStripMenuItem renameItem = new System.Windows.Forms.ToolStripMenuItem(Properties.Resources.TranslitMenuText + " " + String.Format(Properties.Resources.RenameMenuName, rp.Name));
                renameItem.Click += new EventHandler(renameToToolStripMenuItem_Click);
                renameItem.Tag = rp;
                renameInPlaceToolStripMenuItem.DropDownItems.Add(renameItem);
            }
            foreach (CommandElement command in FB2Config.Current.Commands.CheckedFilesCommands)
            {
                ToolStripMenuItem commandItem = new System.Windows.Forms.ToolStripMenuItem(command.Name);
                commandItem.Click += new EventHandler(commandToToolStripMenuItem_Click);
                commandItem.Tag = command;
                commandsToolStripMenuItem.DropDownItems.Add(commandItem);
            }
            foreach (CommandElement command in FB2Config.Current.Commands.FocusedFileCommand)
            {
                ToolStripMenuItem commandItem = new System.Windows.Forms.ToolStripMenuItem(command.Name);
                commandItem.Click += new EventHandler(commandContextToToolStripMenuItem_Click);
                commandItem.Tag = command;
                commandContextToolStripMenuItem.DropDownItems.Add(commandItem);
            }
            CheckMenus();
            UpdateStatus();
        }
        private void createGuidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (GuidForm form = new GuidForm())
            {
                form.ShowDialog();
            }
        }
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                fc.Reload();
                UpdateItem(filesView.FocusedItem);
            }
        }
        private void CompressFile(ListViewItem item)
        {
            string fileName = String.Empty;
            try
            {
                FB2File fc = item.Tag as FB2File;
                fileName = fc.FileInformation.Name;
                if (fileName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
                {
                    AddMessageRN(String.Format(Properties.Resources.ProgressCompressFileAlreadyCompressed, fileName));
                }
                else
                {
                    string oldFileSize = fc.BookSizeText;
                    AddMessage(String.Format(Properties.Resources.ProgressCompressFile, fileName));
                    fc.Compress();
                    UpdateItem(item);
                    AddMessageRN(String.Format(Properties.Resources.ProgressFileSizeChange, oldFileSize, fc.BookSizeText));
                }
            }
            catch (Exception ex)
            {
                AddErrorRN(String.Format(Properties.Resources.ProgressCompressFileError, fileName, ex.Message));
            }
        }
        private void compressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationCompress, SelectedCount), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    CompressFile(item);
                    if (CheckCancel())
                        break;
                }
                InProgress = false;
            }
        }
        private void UpdateFileProperties(ListViewItem item, FileProperties props)
        {
            string fileName = String.Empty;
            try
            {
                FB2File fc = item.Tag as FB2File;
                fileName = fc.FileInformation.Name;

                fc.UpdateProperties(props);
            }
            catch (Exception ex)
            {
                AddErrorRN(String.Format(Properties.Resources.ProgressExtractFileError, fileName, ex.Message));
            }
        }
        private void ExtractFile(ListViewItem item)
        {
            string fileName = String.Empty;
            try
            {
                FB2File fc = item.Tag as FB2File;
                fileName = fc.FileInformation.Name;
                if (fileName.ToLower().EndsWith(FB2Config.Current.FB2Extension))
                {
                    AddMessageRN(String.Format(Properties.Resources.ProgressExtractFileAlreadyExtracted, fileName));
                }
                else
                {
                    string oldFileSize = fc.BookSizeText;
                    AddMessage(String.Format(Properties.Resources.ProgressExtractFile, fileName));
                    fc.Extract();
                    UpdateItem(item);
                    AddMessageRN(String.Format(Properties.Resources.ProgressFileSizeChange, oldFileSize, fc.BookSizeText));
                }
            }
            catch (Exception ex)
            {
                AddErrorRN(String.Format(Properties.Resources.ProgressExtractFileError, fileName, ex.Message));
            }
        }
        private bool CheckCancel()
        {
            Application.DoEvents();
            if (IsCancel)
            {
                if (MessageBox.Show(Properties.Resources.ConfirmationCancel, Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    AddMessageRN(Properties.Resources.ProgressCancelledByUser);
                    return true;
                }
                else
                    IsCancel = false;
            }
            return false;
        }
        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationExtract, SelectedCount), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    ExtractFile(item);
                    if (CheckCancel())
                        break;
                }
                InProgress = false;
            }
        }
        private void messagesTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (InProgress)
                if (e.KeyCode == Keys.Escape)
                {
                    e.Handled = true;
                    IsCancel = true;
                }
        }
        private void statusCancelProgressLabel_Click(object sender, EventArgs e)
        {
            IsCancel = true;
        }
        private void clearLogOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            messagesTextBox.Clear();
        }
        private void RenameFolderZIPtoFB2ZIP(DirectoryInfo folder)
        {
            if (IsCancel)
                return;
            FileInfo[] zipFiles = folder.GetFiles("*.zip");
            if (zipFiles.Length > 0)
            {
                foreach (FileInfo fi in zipFiles)
                {
                    string fileName = fi.FullName;
                    if (!fileName.ToLower().EndsWith(FB2Config.Current.FB2ZIPExtension))
                    {
                        AddMessageRN(string.Format(Properties.Resources.RenameZIPtoFB2ZIP, fi.Name));
                        fileName = fileName.Substring(0, fileName.Length - ".zip".Length) + FB2Config.Current.FB2ZIPExtension;
                        fi.MoveTo(fileName);
                    }
                    if (CheckCancel())
                        break;
                }
            }
            if (IsCancel)
                return;
            DirectoryInfo[] folders = folder.GetDirectories();
            foreach (DirectoryInfo di in folders)
            {
                RenameFolderZIPtoFB2ZIP(di);
            }
        }
        private void renameAllZIPTofFB2ZIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = Properties.Settings.Default.DefaultAddFolderPath;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InProgress = true;
                RenameFolderZIPtoFB2ZIP(new DirectoryInfo(folderBrowserDialog.SelectedPath));
                InProgress = false;
                UpdateStatus();
            }
        }
        private void ValidateSchema(ListViewItem item)
        {
            string fileName = String.Empty;
            try
            {
                FB2File fc = item.Tag as FB2File;
                fileName = fc.FileInformation.Name;
                AddMessage(string.Format(Properties.Resources.ProgressValidation, fc.FileInformation.Name));
                List<string> errors = fc.ValidateSchema();
                if (errors.Count > 0)
                {
                    AddMessageRN(string.Empty);
                    foreach (string s in errors)
                    {
                        AddErrorRN(s);
                    }
                }
                else
                {
                    AddMessageRN(Properties.Resources.ValidationOk);
                }
                errors.Clear();
            }
            catch (Exception ex)
            {
                AddErrorRN(String.Format(Properties.Resources.ValidationFileError, fileName, ex.Message));
            }
        }
        private void validateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationValidation, SelectedCount), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    ValidateSchema(item);
                    if (CheckCancel())
                        break;
                }
                InProgress = false;
            }
        }

        private void viewDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                try
                {
                    FB2File fc = filesView.FocusedItem.Tag as FB2File;
                    AddMessageRN(string.Format(Properties.Resources.FileDescriptionView, fc.FileInformation.FullName));
                    string temp = string.Empty;
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(fc.Metadata.Description);
                    using (var stringWriter = new StringWriter())
                    {
                        var xmlTextWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Indent = true, NewLineChars = "\r\n", CloseOutput = true, OmitXmlDeclaration = true });
                        xmlDoc.WriteTo(xmlTextWriter);
                        xmlTextWriter.Flush();
                        temp = stringWriter.ToString();
                    }
                    AddMessageRN(temp);
                }
                catch (Exception ex)
                {
                    AddErrorRN(ex.Message);
                }
            }
        }

        private void toolStripEditBooks_Click(object sender, EventArgs e)
        {
            var dialog = new ChangeProperties();
            FB2File fc = null;
            if (filesView.FocusedItem != null)
            {
                fc = filesView.FocusedItem.Tag as FB2File;
            }
            dialog.LoadFileProperties(fc, filesView.CheckedItems.Count);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                InProgress = true;
                var fp = dialog.GetFileProperties();
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    FB2File fi = item.Tag as FB2File;
                    try
                    {
                        UpdateFileProperties(item, fp);
                        UpdateItem(item);
                        AddMessageRN(String.Format(Properties.Resources.ProgressEditFile, fi.FileInformation.FullName));
                        if (CheckCancel())
                            break;
                    }
                    catch (Exception ex)
                    {
                        AddErrorRN(String.Format(Properties.Resources.ProgressEditFileError, fi.FileInformation.FullName, ex.Message));
                    }
                }
                InProgress = false;
            }
        }

        private void metadataSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                var dialog = new ChangeProperties();
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                dialog.LoadFileProperties(fc, 1);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var fp = dialog.GetFileProperties();
                    try
                    {
                            UpdateFileProperties(filesView.FocusedItem, fp);
                            UpdateItem(filesView.FocusedItem);
                            AddMessageRN(String.Format(Properties.Resources.ProgressEditFile, fc.FileInformation.FullName));
                    }
                    catch (Exception ex)
                    {
                        AddErrorRN(String.Format(Properties.Resources.ProgressEditFileError, fc.FileInformation.FullName, ex.Message));
                    }
                }
            }
        }

        private string GetGroupName(FB2File item, int typeGroup = 0)
        {
            string seq = string.Empty;
            switch(typeGroup)
            {
                // Фамилия Имя
                case 5:
                    seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                    seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : item.BookAuthorLastName + (String.IsNullOrEmpty(item.BookAuthorFirstName) ? " "+ item.BookAuthorFirstName : "");

                    return seq;
                // Фамилия: Серия
                case 0:
                    seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                    seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : item.BookAuthorLastName + ": " + seq;
                    return seq;
                // Фамилия: Название
                case 1:
                    seq = item.BookTitle;
                    seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : item.BookAuthorLastName + ": " + seq;
                    return seq;
                // Серия
                case 4:
                    seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                    return seq;
                // Серия: Фамилия
                case 2:
                    seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                    seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : seq + ": " + item.BookAuthorLastName;
                    return seq;
                // Серия: Название
                case 3:
                    seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                    seq = seq + ": " + item.BookTitle;
                    return seq;
            }
            return seq;
        }

        private int GetGroupType()
        {
            object o = 0;
            List<ToolStripMenuItem> toolItems = new List<ToolStripMenuItem>() {
                view1ToolStripMenuItem,
                view2ToolStripMenuItem,
                view3ToolStripMenuItem,
                view4ToolStripMenuItem,
                view5ToolStripMenuItem,
                view6ToolStripMenuItem
            };
            foreach (ToolStripMenuItem toolItem in toolItems)
            {
                if (toolItem.CheckState == CheckState.Checked)
                    o = toolItem.Tag;
            }
            return Convert.ToInt32(o);
        }

        private void UpdateGroup(ListViewItem item)
        {
            FB2File fc = item.Tag as FB2File;
            var groupName = GetGroupName(fc, GetGroupType());

            ListViewGroup group = filesView.Groups[groupName.ToUpper()];
            if (group == null)
                group = filesView.Groups.Add(groupName.ToUpper(), groupName);
            item.Group = group;
        }

        private void view1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupType = Convert.ToInt32((sender as ToolStripMenuItem).Tag);
            List<ToolStripMenuItem> toolItems = new List<ToolStripMenuItem>() { 
                view1ToolStripMenuItem, 
                view2ToolStripMenuItem, 
                view3ToolStripMenuItem, 
                view4ToolStripMenuItem, 
                view5ToolStripMenuItem, 
                view6ToolStripMenuItem
            };
            foreach (ToolStripMenuItem toolItem in toolItems)
            {
                toolItem.CheckState = CheckState.Unchecked;
                if (Convert.ToInt32(toolItem.Tag) == groupType)
                    toolItem.CheckState = CheckState.Checked;
            }
            filesView.BeginUpdate();
            filesView.Groups.Clear();
            foreach (ListViewItem item in filesView.Items)
            {
                UpdateGroup(item);
            }
            filesView.EndUpdate();
        }

        private void deleteSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationDeleteFile, fc.FileInformation.Name), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    return;
                DeleteFile(filesView.FocusedItem);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckCheckedFilesItems();
            if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationDelete, SelectedCount), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    DeleteFile(item);
                    if (CheckCancel())
                        break;
                }
                InProgress = false;
            }
        }

        private void CheckCheckedFilesItems()
        {
            if (filesView.CheckedItems.Count == 0 && filesView.SelectedItems.Count > 0)
            {
                foreach ( ListViewItem item in filesView.SelectedItems)
                {
                    item.Checked = true;
                }
            }
        }

        private void DeleteFile(ListViewItem item)
        {
            FB2File fc = item.Tag as FB2File;
            if (fc != null)
            {
                File.Delete(fc.FileInformation.FullName);
                string path = Path.GetDirectoryName(fc.FileInformation.FullName);
                FB2File.RemoveFolder(new DirectoryInfo(path));
                if (!File.Exists(fc.FileInformation.FullName))
                {
                    filesView.Items.Remove(item);
                    ItemsCache.Remove(item);
                    LoadedFileIDs.Remove(fc.FileInformation.FullName);
                    CheckMenus();
                }
            }
        }

        private void removeSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (filesView.FocusedItem != null)
            {
                FB2File fc = filesView.FocusedItem.Tag as FB2File;
                if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationRemoveFile, fc.FileInformation.Name), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                    return;
                if (fc != null)
                {
                    filesView.Items.Remove(filesView.FocusedItem);
                    ItemsCache.Remove(filesView.FocusedItem);
                    LoadedFileIDs.Remove(fc.FileInformation.FullName);
                }
                CheckMenus();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(String.Format(Properties.Resources.ConfirmationRemove, SelectedCount), Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                InProgress = true;
                foreach (ListViewItem item in filesView.CheckedItems)
                {
                    FB2File fc = item.Tag as FB2File;
                    filesView.Items.Remove(item);
                    ItemsCache.Remove(item);
                    LoadedFileIDs.Remove(fc.FileInformation.FullName);
                    CheckMenus();
                }
                InProgress = false;
            }
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (itemsFilter == null) itemsFilter = new FileProperties();
            var dialog = new ChangeProperties(itemsFilter);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                itemsFilter = dialog.GetFileProperties();
                if (itemsFilter.AuthorFirstNameChange && itemsFilter.AuthorFirstName.Trim() == string.Empty) itemsFilter.AuthorFirstNameChange = false;
                if (itemsFilter.AuthorMiddleNameChange && itemsFilter.AuthorMiddleName.Trim() == string.Empty) itemsFilter.AuthorMiddleNameChange = false;
                if (itemsFilter.AuthorLastNameChange && itemsFilter.AuthorLastName.Trim() == string.Empty) itemsFilter.AuthorLastNameChange = false;
                if (itemsFilter.GengeChange && itemsFilter.Genre == null || itemsFilter.GengeChange && itemsFilter.Genre.Trim() == string.Empty) itemsFilter.GengeChange = false;
                if (itemsFilter.SeriesChange && itemsFilter.Series.Trim() == string.Empty) itemsFilter.SeriesChange = false;
                if (itemsFilter.NumberChange && itemsFilter.Number.Trim() == string.Empty) itemsFilter.NumberChange = false;
                if (itemsFilter.TitleChange && itemsFilter.Title.Trim() == string.Empty) itemsFilter.TitleChange= false;

                if (!itemsFilter.AuthorFirstNameChange
                    && !itemsFilter.AuthorMiddleNameChange
                    && !itemsFilter.AuthorLastNameChange
                    && !itemsFilter.GengeChange
                    && !itemsFilter.SeriesChange
                    && !itemsFilter.NumberChange
                    && !itemsFilter.TitleChange)
                {
                    clearFilterToolStripMenuItem_Click(sender, e);
                    return;
                }

                var filteredItems = ItemsCache.Where(item =>
                {
                    var fb2 = item.Tag as FB2File;
                    var result = false;
                    var result2 = false;
                    var first = true;
                    if (itemsFilter.AuthorFirstNameChange)
                    {
                        result = fb2.BookAuthorFirstName.ToLower().IndexOf(itemsFilter.AuthorFirstName.ToLower()) >= 0;
                        result2 = (first || result2) && result;
                        first = false;
                    }
                    if (itemsFilter.AuthorMiddleNameChange)
                    {
                        result = fb2.BookAuthorMiddleName.ToLower().IndexOf(itemsFilter.AuthorMiddleName.ToLower()) >= 0;
                        result2 = (first || result2) && result;
                        first = false;
                    }
                    if (itemsFilter.AuthorLastNameChange)
                    {
                        result = fb2.BookAuthorLastName.ToLower().IndexOf(itemsFilter.AuthorLastName.ToLower()) >= 0;
                        result2 = (first || result2) && result;
                        first = false;
                    }
                    if (itemsFilter.GengeChange)
                    {
                        result = fb2.BookGenre.ToLower() == itemsFilter.GenreTitle.ToLower();
                        result2 = (first || result2) && result;
                        first = false;
                    }
                    if (itemsFilter.SeriesChange)
                    {
                        result = fb2.BookSequenceName.ToLower().IndexOf(itemsFilter.Series.ToLower()) >= 0;
                        result2 = (first || result2) && result;
                        first = false;
                    }
                    if (itemsFilter.TitleChange)
                    {
                        result = fb2.BookTitle.ToLower().IndexOf(itemsFilter.Title.ToLower()) >= 0;
                        result2 = (first || result2) && result;
                        first = false;
                    }
                    return result2;
                });
                filesView.BeginUpdate();
                var items = filteredItems.ToArray();
                filesView.Items.Clear();
                filesView.Groups.Clear();
                if (items.Length > 0) filesView.Items.AddRange(items);
                foreach (ListViewItem item in filesView.Items) UpdateGroup(item);
                UpdateStatus();
                filesView.EndUpdate();
                filterToolStripMenuItem.Checked = true;
            }
        }

        private void clearFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filesView.BeginUpdate();
            filesView.Items.Clear();
            filesView.Groups.Clear();
            filesView.Items.AddRange(ItemsCache.ToArray());
            foreach (ListViewItem item in filesView.Items) {
                UpdateGroup(item);
                //item.Checked = false;
            }
            //SelectedCount = 0;
            UpdateStatus();
            filesView.EndUpdate();
            itemsFilter = null;
            filterToolStripMenuItem.Checked = false;
        }
    }
}
