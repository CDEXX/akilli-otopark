using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GP_Project
{
    public partial class customWarningForm : Form
    {
        public customWarningForm()
        {
            InitializeComponent();
        }

        private void customWarningForm_Load(object sender, EventArgs e)
        {
            UIHelper.SetRoundedRegion(this, 2);
        }

        public void SetPopupStyle(string type, string message)
        {
            lblType.Text = type.ToUpper();
            txtMessage.Text = message;

            Color accentColor;
            Image icon = Properties.Resources.info;

            switch (type.ToLower())
            {
                case "success":
                    accentColor = Color.FromArgb(46, 175, 113);
                    icon = Properties.Resources.success;
                    break;

                case "error":
                    accentColor = Color.FromArgb(171, 46, 50);
                    icon = Properties.Resources.error;
                    break;

                case "warnıng":
                    accentColor = Color.FromArgb(231, 186, 15);
                    icon = Properties.Resources.warning;
                    break;

                default:
                    accentColor = Color.FromArgb(42, 142, 209); // info
                    icon = Properties.Resources.info;
                    break;
            }
            this.BackColor = accentColor;
            btn1.BackColor = accentColor;
            pictureBox1.Image = icon;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
