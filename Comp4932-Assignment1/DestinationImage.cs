using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Comp4932_Assignment1
{
    public partial class DestinationImage : Form
    {
        private Bitmap blank;
        public DestinationImage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            blank = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            openFileDialog.Title = "Open Image";
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.ico;*.svg;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                // Create a new Bitmap object from the picture file on disk
                Bitmap image = new Bitmap(openFileDialog.FileName);
                blank = new Bitmap(image, new Size(480, 410));

                // Set the PictureBox Image property
                pictureBox1.Image = blank;

            }
        }
    }
}
