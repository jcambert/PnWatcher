using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PnWatcher
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string errors)
        {
            InitializeComponent();
            errorsTxt.Text = errors;
        }

        public void AddError(string error)
        {
            errorsTxt.Text = error + "\n" + errorsTxt.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
