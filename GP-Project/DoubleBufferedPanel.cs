using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GP_Project
{
    // Bu ': Panel' kısmı, bu sınıfın bir Panel olduğunu belirtir
    public class DoubleBufferedPanel : Panel
    {
        // Constructor (Yapıcı Metot)
        public DoubleBufferedPanel()
        {
            // Bu, "titremeyi engelle" komutudur.
            this.DoubleBuffered = true;

            // Bu iki satır da çizim performansını ve pürüzsüzlüğünü
            // en üst düzeye çıkarmak içindir.
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
    }
}
