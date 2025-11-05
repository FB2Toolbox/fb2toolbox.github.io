namespace FB2Toolbox
{
    partial class ChangeProperties
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.authorLastNameText = new System.Windows.Forms.TextBox();
            this.authorLastNameCheck = new System.Windows.Forms.CheckBox();
            this.authorMiddleNameText = new System.Windows.Forms.TextBox();
            this.authorMiddleNameCheck = new System.Windows.Forms.CheckBox();
            this.authorFirstNameText = new System.Windows.Forms.TextBox();
            this.authorFirstNameCheck = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bookTitleText = new System.Windows.Forms.TextBox();
            this.bookTitleCheck = new System.Windows.Forms.CheckBox();
            this.bookNumberText = new System.Windows.Forms.NumericUpDown();
            this.bookGenreText = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bookNumberCheck = new System.Windows.Forms.CheckBox();
            this.bookSeriesText = new System.Windows.Forms.TextBox();
            this.bookSeriesCheck = new System.Windows.Forms.CheckBox();
            this.bookGenreCheck = new System.Windows.Forms.CheckBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookNumberText)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.authorLastNameText);
            this.groupBox1.Controls.Add(this.authorLastNameCheck);
            this.groupBox1.Controls.Add(this.authorMiddleNameText);
            this.groupBox1.Controls.Add(this.authorMiddleNameCheck);
            this.groupBox1.Controls.Add(this.authorFirstNameText);
            this.groupBox1.Controls.Add(this.authorFirstNameCheck);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "&Автор";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "&Фамилия:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "&Отчество:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "&Имя:";
            // 
            // authorLastNameText
            // 
            this.authorLastNameText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.authorLastNameText.Location = new System.Drawing.Point(117, 72);
            this.authorLastNameText.MaxLength = 150;
            this.authorLastNameText.Name = "authorLastNameText";
            this.authorLastNameText.Size = new System.Drawing.Size(279, 21);
            this.authorLastNameText.TabIndex = 5;
            this.authorLastNameText.TextChanged += new System.EventHandler(this.authorLastNameText_TextChanged);
            // 
            // authorLastNameCheck
            // 
            this.authorLastNameCheck.AutoSize = true;
            this.authorLastNameCheck.Location = new System.Drawing.Point(93, 75);
            this.authorLastNameCheck.Name = "authorLastNameCheck";
            this.authorLastNameCheck.Size = new System.Drawing.Size(15, 14);
            this.authorLastNameCheck.TabIndex = 4;
            this.authorLastNameCheck.UseVisualStyleBackColor = true;
            this.authorLastNameCheck.CheckedChanged += new System.EventHandler(this.authorLastNameCheck_CheckedChanged);
            // 
            // authorMiddleNameText
            // 
            this.authorMiddleNameText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.authorMiddleNameText.Location = new System.Drawing.Point(117, 46);
            this.authorMiddleNameText.MaxLength = 150;
            this.authorMiddleNameText.Name = "authorMiddleNameText";
            this.authorMiddleNameText.Size = new System.Drawing.Size(279, 21);
            this.authorMiddleNameText.TabIndex = 3;
            this.authorMiddleNameText.TextChanged += new System.EventHandler(this.authorMiddleNameText_TextChanged);
            // 
            // authorMiddleNameCheck
            // 
            this.authorMiddleNameCheck.AutoSize = true;
            this.authorMiddleNameCheck.Location = new System.Drawing.Point(93, 49);
            this.authorMiddleNameCheck.Name = "authorMiddleNameCheck";
            this.authorMiddleNameCheck.Size = new System.Drawing.Size(15, 14);
            this.authorMiddleNameCheck.TabIndex = 2;
            this.authorMiddleNameCheck.UseVisualStyleBackColor = true;
            this.authorMiddleNameCheck.CheckedChanged += new System.EventHandler(this.authorMiddleNameCheck_CheckedChanged);
            // 
            // authorFirstNameText
            // 
            this.authorFirstNameText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.authorFirstNameText.Location = new System.Drawing.Point(117, 20);
            this.authorFirstNameText.MaxLength = 150;
            this.authorFirstNameText.Name = "authorFirstNameText";
            this.authorFirstNameText.Size = new System.Drawing.Size(279, 21);
            this.authorFirstNameText.TabIndex = 1;
            this.authorFirstNameText.TextChanged += new System.EventHandler(this.authorFirstNameText_TextChanged);
            // 
            // authorFirstNameCheck
            // 
            this.authorFirstNameCheck.AutoSize = true;
            this.authorFirstNameCheck.Location = new System.Drawing.Point(93, 23);
            this.authorFirstNameCheck.Name = "authorFirstNameCheck";
            this.authorFirstNameCheck.Size = new System.Drawing.Size(15, 14);
            this.authorFirstNameCheck.TabIndex = 0;
            this.authorFirstNameCheck.UseVisualStyleBackColor = true;
            this.authorFirstNameCheck.CheckedChanged += new System.EventHandler(this.authorFirstNameCheck_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.bookTitleText);
            this.groupBox2.Controls.Add(this.bookTitleCheck);
            this.groupBox2.Controls.Add(this.bookNumberText);
            this.groupBox2.Controls.Add(this.bookGenreText);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.bookNumberCheck);
            this.groupBox2.Controls.Add(this.bookSeriesText);
            this.groupBox2.Controls.Add(this.bookSeriesCheck);
            this.groupBox2.Controls.Add(this.bookGenreCheck);
            this.groupBox2.Location = new System.Drawing.Point(13, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 133);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "&Книга";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Н&азвание:";
            // 
            // bookTitleText
            // 
            this.bookTitleText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bookTitleText.Location = new System.Drawing.Point(117, 102);
            this.bookTitleText.MaxLength = 150;
            this.bookTitleText.Name = "bookTitleText";
            this.bookTitleText.Size = new System.Drawing.Size(279, 21);
            this.bookTitleText.TabIndex = 11;
            this.bookTitleText.TextChanged += new System.EventHandler(this.bookTitleText_TextChanged);
            // 
            // bookTitleCheck
            // 
            this.bookTitleCheck.AutoSize = true;
            this.bookTitleCheck.Location = new System.Drawing.Point(93, 106);
            this.bookTitleCheck.Name = "bookTitleCheck";
            this.bookTitleCheck.Size = new System.Drawing.Size(15, 14);
            this.bookTitleCheck.TabIndex = 10;
            this.bookTitleCheck.UseVisualStyleBackColor = true;
            this.bookTitleCheck.CheckedChanged += new System.EventHandler(this.bookTitleCheck_CheckedChanged);
            // 
            // bookNumberText
            // 
            this.bookNumberText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bookNumberText.Location = new System.Drawing.Point(117, 75);
            this.bookNumberText.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.bookNumberText.Name = "bookNumberText";
            this.bookNumberText.Size = new System.Drawing.Size(279, 21);
            this.bookNumberText.TabIndex = 9;
            this.bookNumberText.ValueChanged += new System.EventHandler(this.bookNumberText_ValueChanged);
            // 
            // bookGenreText
            // 
            this.bookGenreText.BackColor = System.Drawing.SystemColors.Window;
            this.bookGenreText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookGenreText.FormattingEnabled = true;
            this.bookGenreText.Location = new System.Drawing.Point(117, 20);
            this.bookGenreText.Name = "bookGenreText";
            this.bookGenreText.Size = new System.Drawing.Size(279, 21);
            this.bookGenreText.TabIndex = 4;
            this.bookGenreText.TextChanged += new System.EventHandler(this.bookGenreText_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "&Номер:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "&Серия:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "&Жанр:";
            // 
            // bookNumberCheck
            // 
            this.bookNumberCheck.AutoSize = true;
            this.bookNumberCheck.Location = new System.Drawing.Point(93, 78);
            this.bookNumberCheck.Name = "bookNumberCheck";
            this.bookNumberCheck.Size = new System.Drawing.Size(15, 14);
            this.bookNumberCheck.TabIndex = 7;
            this.bookNumberCheck.UseVisualStyleBackColor = true;
            this.bookNumberCheck.CheckedChanged += new System.EventHandler(this.bookNumberCheck_CheckedChanged);
            // 
            // bookSeriesText
            // 
            this.bookSeriesText.BackColor = System.Drawing.SystemColors.ControlLight;
            this.bookSeriesText.Location = new System.Drawing.Point(117, 46);
            this.bookSeriesText.MaxLength = 150;
            this.bookSeriesText.Name = "bookSeriesText";
            this.bookSeriesText.Size = new System.Drawing.Size(279, 21);
            this.bookSeriesText.TabIndex = 6;
            this.bookSeriesText.TextChanged += new System.EventHandler(this.bookSeriesText_TextChanged);
            // 
            // bookSeriesCheck
            // 
            this.bookSeriesCheck.AutoSize = true;
            this.bookSeriesCheck.Location = new System.Drawing.Point(93, 50);
            this.bookSeriesCheck.Name = "bookSeriesCheck";
            this.bookSeriesCheck.Size = new System.Drawing.Size(15, 14);
            this.bookSeriesCheck.TabIndex = 5;
            this.bookSeriesCheck.UseVisualStyleBackColor = true;
            this.bookSeriesCheck.CheckedChanged += new System.EventHandler(this.bookSeriesCheck_CheckedChanged);
            // 
            // bookGenreCheck
            // 
            this.bookGenreCheck.AutoSize = true;
            this.bookGenreCheck.Location = new System.Drawing.Point(93, 23);
            this.bookGenreCheck.Name = "bookGenreCheck";
            this.bookGenreCheck.Size = new System.Drawing.Size(15, 14);
            this.bookGenreCheck.TabIndex = 3;
            this.bookGenreCheck.UseVisualStyleBackColor = true;
            this.bookGenreCheck.CheckedChanged += new System.EventHandler(this.bookGenreCheck_CheckedChanged);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpdate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonUpdate.Location = new System.Drawing.Point(268, 280);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(78, 23);
            this.buttonUpdate.TabIndex = 10;
            this.buttonUpdate.Text = "Изменить";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(349, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // ChangeProperties
            // 
            this.AcceptButton = this.buttonUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(436, 315);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeProperties";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Изменить метаданные";
            this.Shown += new System.EventHandler(this.ChangeProperties_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bookNumberText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox authorLastNameText;
        private System.Windows.Forms.CheckBox authorLastNameCheck;
        private System.Windows.Forms.TextBox authorMiddleNameText;
        private System.Windows.Forms.CheckBox authorMiddleNameCheck;
        private System.Windows.Forms.TextBox authorFirstNameText;
        private System.Windows.Forms.CheckBox authorFirstNameCheck;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox bookNumberCheck;
        private System.Windows.Forms.TextBox bookSeriesText;
        private System.Windows.Forms.CheckBox bookSeriesCheck;
        private System.Windows.Forms.CheckBox bookGenreCheck;
        private System.Windows.Forms.ComboBox bookGenreText;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown bookNumberText;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox bookTitleText;
        private System.Windows.Forms.CheckBox bookTitleCheck;
    }
}