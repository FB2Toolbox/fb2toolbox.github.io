namespace FB2Toolbox
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.filesView = new System.Windows.Forms.ListView();
            this.columnBookTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnBookSequence = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnBookSequenceNr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAuthorFirstName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAuthorMiddleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAuthorLastName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnGenre = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEncoding = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewDescriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSelectFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.metadataSingleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.commandContextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.booksImages = new System.Windows.Forms.ImageList(this.components);
            this.messagesTextBox = new System.Windows.Forms.RichTextBox();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFolderRecursiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFilesListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.checkAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uncheckAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameInPlaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.compressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripEditBooks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.validateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.commandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameAllZIPTofFB2ZIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createGuidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.view4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusCancelProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusSelectedFilesLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 28);
            this.mainSplitContainer.Name = "mainSplitContainer";
            this.mainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.filesView);
            this.mainSplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.messagesTextBox);
            this.mainSplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            this.mainSplitContainer.Size = new System.Drawing.Size(1198, 518);
            this.mainSplitContainer.SplitterDistance = 394;
            this.mainSplitContainer.TabIndex = 2;
            this.mainSplitContainer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.messagesTextBox_KeyUp);
            // 
            // filesView
            // 
            this.filesView.AllowColumnReorder = true;
            this.filesView.CheckBoxes = true;
            this.filesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnBookTitle,
            this.columnBookSequence,
            this.columnBookSequenceNr,
            this.columnAuthorFirstName,
            this.columnAuthorMiddleName,
            this.columnAuthorLastName,
            this.columnGenre,
            this.columnEncoding,
            this.columnFileSize,
            this.columnVersion});
            this.filesView.ContextMenuStrip = this.contextMenuStrip1;
            this.filesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesView.FullRowSelect = true;
            this.filesView.HideSelection = false;
            this.filesView.LabelWrap = false;
            this.filesView.Location = new System.Drawing.Point(4, 4);
            this.filesView.Name = "filesView";
            this.filesView.ShowItemToolTips = true;
            this.filesView.Size = new System.Drawing.Size(1190, 390);
            this.filesView.SmallImageList = this.booksImages;
            this.filesView.TabIndex = 0;
            this.filesView.UseCompatibleStateImageBehavior = false;
            this.filesView.View = System.Windows.Forms.View.Details;
            this.filesView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.filesView_ItemCheck);
            this.filesView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.filesView_ItemChecked);
            this.filesView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.messagesTextBox_KeyUp);
            // 
            // columnBookTitle
            // 
            this.columnBookTitle.Text = "Название (Книга)";
            this.columnBookTitle.Width = 312;
            // 
            // columnBookSequence
            // 
            this.columnBookSequence.Text = "Серия";
            // 
            // columnBookSequenceNr
            // 
            this.columnBookSequenceNr.Text = "Номер в серии (Книга)";
            this.columnBookSequenceNr.Width = 53;
            // 
            // columnAuthorFirstName
            // 
            this.columnAuthorFirstName.Text = "Имя (Автор)";
            this.columnAuthorFirstName.Width = 160;
            // 
            // columnAuthorMiddleName
            // 
            this.columnAuthorMiddleName.Text = "Отчество (Автор)";
            // 
            // columnAuthorLastName
            // 
            this.columnAuthorLastName.Text = "Фамилия (Автор)";
            this.columnAuthorLastName.Width = 160;
            // 
            // columnGenre
            // 
            this.columnGenre.Text = "Жанр";
            this.columnGenre.Width = 180;
            // 
            // columnEncoding
            // 
            this.columnEncoding.Text = "Кодировка";
            this.columnEncoding.Width = 90;
            // 
            // columnFileSize
            // 
            this.columnFileSize.Text = "Размер";
            // 
            // columnVersion
            // 
            this.columnVersion.Text = "Версия";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDescriptionToolStripMenuItem,
            this.toolStripSeparator1,
            this.openFileToolStripMenuItem,
            this.openSelectFileMenuItem,
            this.openFolderMenuItem,
            this.toolStripMenuItem2,
            this.metadataSingleToolStripMenuItem,
            this.toolStripMenuItem9,
            this.commandContextToolStripMenuItem,
            this.toolStripMenuItem5,
            this.reloadToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuStrip1.Size = new System.Drawing.Size(307, 196);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // viewDescriptionToolStripMenuItem
            // 
            this.viewDescriptionToolStripMenuItem.Name = "viewDescriptionToolStripMenuItem";
            this.viewDescriptionToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.viewDescriptionToolStripMenuItem.Text = "Описание...";
            this.viewDescriptionToolStripMenuItem.Click += new System.EventHandler(this.viewDescriptionToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(303, 6);
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.openFileToolStripMenuItem.Text = "Открыть файл...";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // openSelectFileMenuItem
            // 
            this.openSelectFileMenuItem.Name = "openSelectFileMenuItem";
            this.openSelectFileMenuItem.Size = new System.Drawing.Size(306, 24);
            this.openSelectFileMenuItem.Text = "Выбрать файл в Проводнике...";
            this.openSelectFileMenuItem.Click += new System.EventHandler(this.openSelectFileMenuItem_Click);
            // 
            // openFolderMenuItem
            // 
            this.openFolderMenuItem.Name = "openFolderMenuItem";
            this.openFolderMenuItem.Size = new System.Drawing.Size(306, 24);
            this.openFolderMenuItem.Text = "Выбрать каталог в Проводнике...";
            this.openFolderMenuItem.Click += new System.EventHandler(this.openFolderMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(303, 6);
            // 
            // metadataSingleToolStripMenuItem
            // 
            this.metadataSingleToolStripMenuItem.Name = "metadataSingleToolStripMenuItem";
            this.metadataSingleToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.metadataSingleToolStripMenuItem.Text = "Изменить метаданные...";
            this.metadataSingleToolStripMenuItem.Click += new System.EventHandler(this.metadataSingleToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(303, 6);
            // 
            // commandContextToolStripMenuItem
            // 
            this.commandContextToolStripMenuItem.Name = "commandContextToolStripMenuItem";
            this.commandContextToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.commandContextToolStripMenuItem.Text = "Команды";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(303, 6);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(306, 24);
            this.reloadToolStripMenuItem.Text = "Считать заново";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // booksImages
            // 
            this.booksImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("booksImages.ImageStream")));
            this.booksImages.TransparentColor = System.Drawing.Color.Transparent;
            this.booksImages.Images.SetKeyName(0, "Junior Icon 85.png");
            this.booksImages.Images.SetKeyName(1, "Junior Icon 89.png");
            this.booksImages.Images.SetKeyName(2, "Junior Icon 96.png");
            // 
            // messagesTextBox
            // 
            this.messagesTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messagesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messagesTextBox.Location = new System.Drawing.Point(4, 0);
            this.messagesTextBox.Name = "messagesTextBox";
            this.messagesTextBox.ReadOnly = true;
            this.messagesTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.messagesTextBox.ShowSelectionMargin = true;
            this.messagesTextBox.Size = new System.Drawing.Size(1190, 116);
            this.messagesTextBox.TabIndex = 0;
            this.messagesTextBox.Text = "";
            // 
            // menuMain
            // 
            this.menuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.actionsToolStripMenuItem,
            this.otherToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuMain.Size = new System.Drawing.Size(1198, 28);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFilesToolStripMenuItem,
            this.addFolderToolStripMenuItem,
            this.addFolderRecursiveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.clearFilesListMenuItem,
            this.clearLogOnlyToolStripMenuItem,
            this.toolStripMenuItem4,
            this.checkAllToolStripMenuItem,
            this.uncheckAllToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.fileToolStripMenuItem.Text = "&Список";
            // 
            // addFilesToolStripMenuItem
            // 
            this.addFilesToolStripMenuItem.Name = "addFilesToolStripMenuItem";
            this.addFilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.addFilesToolStripMenuItem.Size = new System.Drawing.Size(392, 24);
            this.addFilesToolStripMenuItem.Text = "Добавить &файлы...";
            this.addFilesToolStripMenuItem.Click += new System.EventHandler(this.addFilesToolStripMenuItem_Click);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(392, 24);
            this.addFolderToolStripMenuItem.Tag = "false";
            this.addFolderToolStripMenuItem.Text = "Добавить &каталог...";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // addFolderRecursiveToolStripMenuItem
            // 
            this.addFolderRecursiveToolStripMenuItem.Name = "addFolderRecursiveToolStripMenuItem";
            this.addFolderRecursiveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.addFolderRecursiveToolStripMenuItem.Size = new System.Drawing.Size(392, 24);
            this.addFolderRecursiveToolStripMenuItem.Tag = "true";
            this.addFolderRecursiveToolStripMenuItem.Text = "Добавить каталог (с подкаталогами)...";
            this.addFolderRecursiveToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(389, 6);
            // 
            // clearFilesListMenuItem
            // 
            this.clearFilesListMenuItem.Name = "clearFilesListMenuItem";
            this.clearFilesListMenuItem.Size = new System.Drawing.Size(392, 24);
            this.clearFilesListMenuItem.Text = "Очистить список файлов и протокол...";
            this.clearFilesListMenuItem.Click += new System.EventHandler(this.clearFilesListMenuItem_Click);
            // 
            // clearLogOnlyToolStripMenuItem
            // 
            this.clearLogOnlyToolStripMenuItem.Name = "clearLogOnlyToolStripMenuItem";
            this.clearLogOnlyToolStripMenuItem.Size = new System.Drawing.Size(392, 24);
            this.clearLogOnlyToolStripMenuItem.Text = "Очистить только протокол";
            this.clearLogOnlyToolStripMenuItem.Click += new System.EventHandler(this.clearLogOnlyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(389, 6);
            // 
            // checkAllToolStripMenuItem
            // 
            this.checkAllToolStripMenuItem.Name = "checkAllToolStripMenuItem";
            this.checkAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.checkAllToolStripMenuItem.Size = new System.Drawing.Size(392, 24);
            this.checkAllToolStripMenuItem.Text = "Отметить все";
            this.checkAllToolStripMenuItem.Click += new System.EventHandler(this.checkAllToolStripMenuItem_Click);
            // 
            // uncheckAllToolStripMenuItem
            // 
            this.uncheckAllToolStripMenuItem.Name = "uncheckAllToolStripMenuItem";
            this.uncheckAllToolStripMenuItem.Size = new System.Drawing.Size(392, 24);
            this.uncheckAllToolStripMenuItem.Text = "Снять все отметки";
            this.uncheckAllToolStripMenuItem.Click += new System.EventHandler(this.uncheckAllToolStripMenuItem_Click);
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.encodeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.copyToToolStripMenuItem,
            this.moveToToolStripMenuItem,
            this.renameInPlaceToolStripMenuItem,
            this.toolStripMenuItem6,
            this.compressToolStripMenuItem,
            this.extractToolStripMenuItem,
            this.toolStripMenuItem8,
            this.toolStripEditBooks,
            this.toolStripSeparator2,
            this.validateToolStripMenuItem,
            this.toolStripMenuItem7,
            this.commandsToolStripMenuItem});
            this.actionsToolStripMenuItem.Enabled = false;
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.actionsToolStripMenuItem.Text = "&Действия";
            // 
            // encodeToolStripMenuItem
            // 
            this.encodeToolStripMenuItem.Name = "encodeToolStripMenuItem";
            this.encodeToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.encodeToolStripMenuItem.Text = "Перекодировать в";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(252, 6);
            // 
            // copyToToolStripMenuItem
            // 
            this.copyToToolStripMenuItem.Name = "copyToToolStripMenuItem";
            this.copyToToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.copyToToolStripMenuItem.Text = "Копировать";
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.moveToToolStripMenuItem.Text = "Переместить";
            // 
            // renameInPlaceToolStripMenuItem
            // 
            this.renameInPlaceToolStripMenuItem.Name = "renameInPlaceToolStripMenuItem";
            this.renameInPlaceToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.renameInPlaceToolStripMenuItem.Text = "Переименовать на месте";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(252, 6);
            // 
            // compressToolStripMenuItem
            // 
            this.compressToolStripMenuItem.Name = "compressToolStripMenuItem";
            this.compressToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.compressToolStripMenuItem.Text = "Архивировать...";
            this.compressToolStripMenuItem.Click += new System.EventHandler(this.compressToolStripMenuItem_Click);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.extractToolStripMenuItem.Text = "Распаковать...";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(252, 6);
            // 
            // toolStripEditBooks
            // 
            this.toolStripEditBooks.Name = "toolStripEditBooks";
            this.toolStripEditBooks.Size = new System.Drawing.Size(255, 24);
            this.toolStripEditBooks.Text = "Изменить метаданные...";
            this.toolStripEditBooks.Click += new System.EventHandler(this.toolStripEditBooks_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(252, 6);
            // 
            // validateToolStripMenuItem
            // 
            this.validateToolStripMenuItem.Name = "validateToolStripMenuItem";
            this.validateToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.validateToolStripMenuItem.Text = "Тестировать...";
            this.validateToolStripMenuItem.Click += new System.EventHandler(this.validateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(252, 6);
            // 
            // commandsToolStripMenuItem
            // 
            this.commandsToolStripMenuItem.Name = "commandsToolStripMenuItem";
            this.commandsToolStripMenuItem.Size = new System.Drawing.Size(255, 24);
            this.commandsToolStripMenuItem.Text = "Команды";
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameAllZIPTofFB2ZIPToolStripMenuItem,
            this.createGuidToolStripMenuItem});
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.otherToolStripMenuItem.Text = "&Прочее";
            // 
            // renameAllZIPTofFB2ZIPToolStripMenuItem
            // 
            this.renameAllZIPTofFB2ZIPToolStripMenuItem.Name = "renameAllZIPTofFB2ZIPToolStripMenuItem";
            this.renameAllZIPTofFB2ZIPToolStripMenuItem.Size = new System.Drawing.Size(329, 24);
            this.renameAllZIPTofFB2ZIPToolStripMenuItem.Text = "Переименовать все *.zip в *.fb2.zip...";
            this.renameAllZIPTofFB2ZIPToolStripMenuItem.Click += new System.EventHandler(this.renameAllZIPTofFB2ZIPToolStripMenuItem_Click);
            // 
            // createGuidToolStripMenuItem
            // 
            this.createGuidToolStripMenuItem.Name = "createGuidToolStripMenuItem";
            this.createGuidToolStripMenuItem.Size = new System.Drawing.Size(329, 24);
            this.createGuidToolStripMenuItem.Text = "Создать идентификатор...";
            this.createGuidToolStripMenuItem.Click += new System.EventHandler(this.createGuidToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.view1ToolStripMenuItem,
            this.view3ToolStripMenuItem,
            this.view2ToolStripMenuItem,
            this.view4ToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.viewToolStripMenuItem.Text = "&Вид";
            // 
            // view1ToolStripMenuItem
            // 
            this.view1ToolStripMenuItem.Checked = true;
            this.view1ToolStripMenuItem.CheckOnClick = true;
            this.view1ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.view1ToolStripMenuItem.Name = "view1ToolStripMenuItem";
            this.view1ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.view1ToolStripMenuItem.Tag = "0";
            this.view1ToolStripMenuItem.Text = "&Фамилия: Серия";
            this.view1ToolStripMenuItem.Click += new System.EventHandler(this.view1ToolStripMenuItem_Click);
            // 
            // view3ToolStripMenuItem
            // 
            this.view3ToolStripMenuItem.CheckOnClick = true;
            this.view3ToolStripMenuItem.Name = "view3ToolStripMenuItem";
            this.view3ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.view3ToolStripMenuItem.Tag = "2";
            this.view3ToolStripMenuItem.Text = "Серия: Фамилия";
            this.view3ToolStripMenuItem.Click += new System.EventHandler(this.view1ToolStripMenuItem_Click);
            // 
            // view2ToolStripMenuItem
            // 
            this.view2ToolStripMenuItem.CheckOnClick = true;
            this.view2ToolStripMenuItem.Name = "view2ToolStripMenuItem";
            this.view2ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.view2ToolStripMenuItem.Tag = "1";
            this.view2ToolStripMenuItem.Text = "Фамилия: &Название";
            this.view2ToolStripMenuItem.Click += new System.EventHandler(this.view1ToolStripMenuItem_Click);
            // 
            // view4ToolStripMenuItem
            // 
            this.view4ToolStripMenuItem.Name = "view4ToolStripMenuItem";
            this.view4ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.view4ToolStripMenuItem.Tag = "3";
            this.view4ToolStripMenuItem.Text = "Серия: Название";
            this.view4ToolStripMenuItem.Click += new System.EventHandler(this.view1ToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusCancelProgressLabel,
            this.statusSelectedFilesLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 546);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip.Size = new System.Drawing.Size(1198, 22);
            this.statusStrip.TabIndex = 1;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(1183, 17);
            this.statusLabel.Spring = true;
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusCancelProgressLabel
            // 
            this.statusCancelProgressLabel.IsLink = true;
            this.statusCancelProgressLabel.Name = "statusCancelProgressLabel";
            this.statusCancelProgressLabel.Size = new System.Drawing.Size(214, 20);
            this.statusCancelProgressLabel.Text = "Нажмите Esc чтобы прервать";
            this.statusCancelProgressLabel.Visible = false;
            this.statusCancelProgressLabel.Click += new System.EventHandler(this.statusCancelProgressLabel_Click);
            // 
            // statusSelectedFilesLabel
            // 
            this.statusSelectedFilesLabel.Name = "statusSelectedFilesLabel";
            this.statusSelectedFilesLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "FictionBook (*.fb2;*.fb2.zip)|*.fb2;*.fb2.zip";
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.Title = "Выбрать FB2 файлы";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Выберите каталог";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 568);
            this.Controls.Add(this.mainSplitContainer);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "MainForm";
            this.Text = "FB2 Toolbox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.messagesTextBox_KeyUp);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            this.mainSplitContainer.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripMenuItem addFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem clearFilesListMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openSelectFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFolderMenuItem;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ImageList booksImages;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem encodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripStatusLabel statusSelectedFilesLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem checkAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uncheckAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem renameInPlaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createGuidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addFolderRecursiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commandContextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripStatusLabel statusCancelProgressLabel;
        private System.Windows.Forms.ToolStripMenuItem clearLogOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameAllZIPTofFB2ZIPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem viewDescriptionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripEditBooks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem metadataSingleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.RichTextBox messagesTextBox;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem view1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem view2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem view3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem view4ToolStripMenuItem;
        private System.Windows.Forms.ListView filesView;
        private System.Windows.Forms.ColumnHeader columnBookTitle;
        private System.Windows.Forms.ColumnHeader columnBookSequence;
        private System.Windows.Forms.ColumnHeader columnBookSequenceNr;
        private System.Windows.Forms.ColumnHeader columnAuthorFirstName;
        private System.Windows.Forms.ColumnHeader columnAuthorMiddleName;
        private System.Windows.Forms.ColumnHeader columnAuthorLastName;
        private System.Windows.Forms.ColumnHeader columnGenre;
        private System.Windows.Forms.ColumnHeader columnEncoding;
        private System.Windows.Forms.ColumnHeader columnFileSize;
        private System.Windows.Forms.ColumnHeader columnVersion;
    }
}

