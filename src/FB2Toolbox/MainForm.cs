using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace FB2Toolbox
{
    public partial class MainForm : Form
    {
        #region Private
        private bool inProgress_Value = false;
        private Dictionary<string, string> _loadedFileIDs = new Dictionary<string, string>();
        private int _selectedCount = 0;
        #endregion
        public MainForm()
        {
            InitializeComponent();
            Extensions.SetDoubleBuffered(filesView, true);

            IsCancel = false;
            InProgress = false;
            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (v.Build > 0)
                Text = string.Format("{0} (v{1}.{2}.{3})", Text, v.Major, v.Minor, v.Build);
            else
                Text = string.Format("{0} (v{1}.{2})", Text, v.Major, v.Minor);
        }
        private Dictionary<string, string> LoadedFileIDs
        {
            get
            {
                return _loadedFileIDs;
            }
        }
        private int SelectedCount
        {
            get
            {
                return _selectedCount;
            }
            set
            {
                _selectedCount = value;
                CheckMenus();
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
        private List<FB2File> ReadListOfFiles(string[] fileNames)
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
        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Properties.Settings.Default.DefaultAddFiltesPath;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InProgress = true;
                Properties.Settings.Default.DefaultAddFiltesPath = openFileDialog.InitialDirectory;
                Properties.Settings.Default.Save();
                var _listOfFiles = ReadListOfFiles(openFileDialog.FileNames);
                AddItemsToList(_listOfFiles);
                InProgress = false;
                UpdateStatus();
            }
        }
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
            list.Sort();
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
                }
            }
            filesView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            filesView.EndUpdate();
            CheckMenus();
        }
        private void ClearLists()
        {
            filesView.BeginUpdate();
            filesView.Items.Clear();
            filesView.Groups.Clear();
            filesView.EndUpdate();
            messagesTextBox.Clear();
            LoadedFileIDs.Clear();
            SelectedCount = 0;
            UpdateStatus();
        }
        private void CheckMenus()
        {
            actionsToolStripMenuItem.Enabled = _selectedCount > 0;
            clearFilesListMenuItem.Enabled = filesView.Items.Count > 0;
            checkAllToolStripMenuItem.Enabled = clearFilesListMenuItem.Enabled;
            uncheckAllToolStripMenuItem.Enabled = clearFilesListMenuItem.Enabled;
        }
        private void clearFilesListMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.ConfirmationClearFileList, Properties.Resources.ConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                ClearLists();
        }
        private void AddFolder(DirectoryInfo folder, List<FB2File> containers, bool recursive)
        {
            if (IsCancel)
                return;
            UpdateStatus(String.Format(Properties.Resources.ProgressScanFolder, folder.FullName));
            try
            {
                FileInfo[] files = folder.GetFiles("*" + FB2Config.Current.FB2Extension);
                FileInfo[] zipFiles = folder.GetFiles("*" + FB2Config.Current.FB2ZIPExtension);
                if (files.Length > 0)
                {
                    List<string> list = new List<string>();
                    foreach (FileInfo fi in files)
                    {
                        list.Add(fi.FullName);
                    }
                    var fclist = ReadListOfFiles(list.ToArray());
                    containers.AddRange(fclist);
                }
                if (IsCancel)
                    return;
                if (zipFiles.Length > 0)
                {
                    List<string> list = new List<string>();
                    foreach (FileInfo fi in zipFiles)
                    {
                        list.Add(fi.FullName);
                    }
                    var fclist = ReadListOfFiles(list.ToArray());
                    containers.AddRange(fclist);
                }
                if (IsCancel)
                    return;
                if (recursive)
                {
                    DirectoryInfo[] folders = folder.GetDirectories();
                    foreach (DirectoryInfo di in folders)
                    {
                        AddFolder(di, containers, recursive);
                    }
                }
            }
            catch(Exception ex)
            {
                AddErrorRN(ex.Message);
            }
        }
        private void UpdateStatus()
        {
            statusLabel.Text = String.Empty;
            statusSelectedFilesLabel.Text = String.Format(Properties.Resources.StatusBarFilesCount, SelectedCount, filesView.Items.Count);
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
                InProgress = true;
                Properties.Settings.Default.DefaultAddFolderPath = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.Save();
                List<FB2File> list = new List<FB2File>();
                AddFolder(new DirectoryInfo(folderBrowserDialog.SelectedPath), list, recursive);
                AddItemsToList(list);
                InProgress = false;
                UpdateStatus();
            }
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
            if (e.NewValue == CheckState.Unchecked)
                SelectedCount--;
            if (!fb2.IsValid)
                e.NewValue = CheckState.Unchecked;
            if (e.NewValue == CheckState.Checked)
                SelectedCount++;
        }
        private void filesView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateStatus();
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
                    catch
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
            if (typeGroup == 0)
            {
                seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : item.BookAuthorLastName + ": " + seq;
                return seq;
            }
            else
                if (typeGroup == 1)
                {
                    seq = item.BookTitle;
                    seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : item.BookAuthorLastName + ": " + seq;
                    return seq;
                }
                else
                    if (typeGroup == 2)
                    {
                        seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                        seq = String.IsNullOrEmpty(item.BookAuthorLastName) ? seq : seq + ": " + item.BookAuthorLastName;
                        return seq;
                    }
                    else
                        if (typeGroup == 3)
                        {
                            seq = String.IsNullOrEmpty(item.BookSequenceName) ? Properties.Resources.DisplayNoSerie : item.BookSequenceName;
                            seq = seq + ": " + item.BookTitle;
                            return seq;
                        }
            return seq;
        }

        private int GetGroupType()
        {
            object o = 0;
            if (view1ToolStripMenuItem.CheckState == CheckState.Checked)
                o = view1ToolStripMenuItem.Tag;
            if (view2ToolStripMenuItem.CheckState == CheckState.Checked)
                o = view2ToolStripMenuItem.Tag;
            if (view3ToolStripMenuItem.CheckState == CheckState.Checked)
                o = view3ToolStripMenuItem.Tag;
            if (view4ToolStripMenuItem.CheckState == CheckState.Checked)
                o = view4ToolStripMenuItem.Tag;
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

            view1ToolStripMenuItem.CheckState = CheckState.Unchecked;
            view2ToolStripMenuItem.CheckState = CheckState.Unchecked;
            view3ToolStripMenuItem.CheckState = CheckState.Unchecked;
            view4ToolStripMenuItem.CheckState = CheckState.Unchecked;
            if (Convert.ToInt32(view1ToolStripMenuItem.Tag) == groupType)
                view1ToolStripMenuItem.CheckState = CheckState.Checked;
            if (Convert.ToInt32(view2ToolStripMenuItem.Tag) == groupType)
                view2ToolStripMenuItem.CheckState = CheckState.Checked;
            if (Convert.ToInt32(view3ToolStripMenuItem.Tag) == groupType)
                view3ToolStripMenuItem.CheckState = CheckState.Checked;
            if (Convert.ToInt32(view4ToolStripMenuItem.Tag) == groupType)
                view4ToolStripMenuItem.CheckState = CheckState.Checked;

            filesView.BeginUpdate();
            filesView.Groups.Clear();
            foreach (ListViewItem item in filesView.Items)
            {
                UpdateGroup(item);
            }

            filesView.EndUpdate();
        }
    }
}
