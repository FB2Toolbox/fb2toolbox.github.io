using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FB2Toolbox
{
    public partial class OverwriteDialog : Form
    {
        public static bool OverwriteAll = false;
        public static bool SkipAll = false;
        public static void Reset()
        {
            OverwriteAll = false;
            SkipAll = false;
        }
        public OverwriteDialog(string fileName)
        {
            InitializeComponent();
            fileNameLabel.Text = string.Format(Properties.Resources.OverwriteDialofFileAlreadyExists, fileName);
        }
        public bool CheckSkip()
        {
            if (OverwriteAll)
                return false;
            if (SkipAll)
                return true;
            bool skip = ShowDialog() == System.Windows.Forms.DialogResult.Abort;
            if (allFilesCheckBox.Checked)
            {
                if (skip)
                    SkipAll = true;
                else
                    OverwriteAll = true;
            }
            return skip;
        }
    }
}
