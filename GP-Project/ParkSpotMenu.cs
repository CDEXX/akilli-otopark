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
    public partial class ParkSpotMenu : Form
    {
        public ParkSpotMenu()
        {
            InitializeComponent();
            SecilenEylem = "NONE";
        }

        // Ana formun, kullanıcının neye tıkladığını bilmesi için
        public string SecilenEylem { get; private set; } = "NONE";
        public event EventHandler EylemSecildi;
        // Ana formdan, hangi park yerinin menüsü olduğunu almak için
        public string ParkYeriID { get; set; }
        public string MevcutDurum { get; set; }


        private void ParkSpotMenu_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            // Butonları mevcut duruma göre etkinleştir/devre dışı bırak
            // Örneğin, zaten 'DOLU' ise 'Dolu İşaretle' butonunu kapat
            btnDoluIsaretle.Enabled = (MevcutDurum != "DOLU");
            btnBosIsaretle.Enabled = (MevcutDurum != "BOŞ");
            btnArizaliBildir.Enabled = (MevcutDurum != "ARIZALI");
        }

 

        private void btnDoluIsaretle_Click_1(object sender, EventArgs e)
        {
            SecilenEylem = "DOLU";
            EylemSecildi?.Invoke(this, EventArgs.Empty); // Ana forma haber ver
            this.Close();
        }

        private void btnBosIsaretle_Click(object sender, EventArgs e)
        {
            SecilenEylem = "BOŞ";
            EylemSecildi?.Invoke(this, EventArgs.Empty); // Ana forma haber ver
            this.Close();
        }

        private void btnArizaliBildir_Click(object sender, EventArgs e)
        {
            SecilenEylem = "ARIZALI";
            EylemSecildi?.Invoke(this, EventArgs.Empty); // Ana forma haber ver
            this.Close();
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            SecilenEylem = "NONE";
            this.Close();
        }


        private void ParkSpotMenu_Deactivate_1(object sender, EventArgs e)
        {
            SecilenEylem = "NONE";
            this.Close();
        }

        private void Engellibuton_Click(object sender, EventArgs e)
        {
            SecilenEylem = "ENGELLİ";
            EylemSecildi?.Invoke(this, EventArgs.Empty); 
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
