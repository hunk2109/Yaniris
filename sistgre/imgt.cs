using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sistgre
{
    public partial class imgt : Form
    {
        public imgt(Image img)
        {
            InitializeComponent();

            pictureBox1.Image = img;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
        }

        private void Imgt_Load(object sender, EventArgs e)
        {

        }
    }
}
