using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FB2Toolbox
{
    public partial class GuidForm : Form
    {
        public GuidForm()
        {
            InitializeComponent();
            button1_Click(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            guidTextBox.Text = Guid.NewGuid().ToString().ToUpper();
        }
    }
}
