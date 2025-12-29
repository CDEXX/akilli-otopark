using System;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class settingsMenuForm : Form
    {
        private readonly Panel parentPanel;
        public settingsMenuForm(Panel parentPanel)
        {
            InitializeComponent();
            this.parentPanel = parentPanel;
        }

        private void settingsMenuForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            parentPanel.Visible = false;
            this.Close();
        }

        private void settingsMenuForm_Load_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
