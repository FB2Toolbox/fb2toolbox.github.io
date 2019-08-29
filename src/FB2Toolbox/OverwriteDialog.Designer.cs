namespace FB2Toolbox
{
    partial class OverwriteDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverwriteDialog));
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.allFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.overwriteButton = new System.Windows.Forms.Button();
            this.skipButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileNameLabel
            // 
            resources.ApplyResources(this.fileNameLabel, "fileNameLabel");
            this.fileNameLabel.AutoEllipsis = true;
            this.fileNameLabel.Name = "fileNameLabel";
            // 
            // allFilesCheckBox
            // 
            resources.ApplyResources(this.allFilesCheckBox, "allFilesCheckBox");
            this.allFilesCheckBox.Name = "allFilesCheckBox";
            this.allFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // overwriteButton
            // 
            this.overwriteButton.DialogResult = System.Windows.Forms.DialogResult.Retry;
            resources.ApplyResources(this.overwriteButton, "overwriteButton");
            this.overwriteButton.Name = "overwriteButton";
            this.overwriteButton.UseVisualStyleBackColor = true;
            // 
            // skipButton
            // 
            this.skipButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
            resources.ApplyResources(this.skipButton, "skipButton");
            this.skipButton.Name = "skipButton";
            this.skipButton.UseVisualStyleBackColor = true;
            // 
            // OverwriteDialog
            // 
            this.AcceptButton = this.overwriteButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.skipButton;
            this.ControlBox = false;
            this.Controls.Add(this.skipButton);
            this.Controls.Add(this.overwriteButton);
            this.Controls.Add(this.allFilesCheckBox);
            this.Controls.Add(this.fileNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OverwriteDialog";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label fileNameLabel;
        private System.Windows.Forms.CheckBox allFilesCheckBox;
        private System.Windows.Forms.Button overwriteButton;
        private System.Windows.Forms.Button skipButton;
    }
}