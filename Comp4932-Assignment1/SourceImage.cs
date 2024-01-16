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
    public partial class SourceImage : Form
    {
        protected Bitmap?pictureFrame;
        private Line? currentLine;
        public List<Line> lines; // List to store lines

        public SourceImage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            currentLine = null;
            lines = new List<Line>();
            pictureFrame = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (pictureFrame != null)
            {
                e.Graphics.DrawImage(pictureFrame, 0, 0, ClientSize.Width, ClientSize.Height);
            }


            foreach (Line line in lines)
            {
                line.CreateLine(e);
            }

            if (currentLine != null)
            {
                currentLine.CreateLine(e);
            }
        }

        private void SourceImage_Load(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private Bitmap ResizeBitmap(Bitmap originalBitmap, int width, int height)
        {
            Bitmap resizedBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.DrawImage(originalBitmap, new Rectangle(0, 0, width, height));
            }
            return resizedBitmap;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.Title = "Open Image";
                openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff|All files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap image = new Bitmap(openFileDialog.FileName);

                    pictureFrame = ResizeBitmap(image, ClientSize.Width, ClientSize.Height);

                    Refresh();
                }
            }

        }

        private void Source_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentLine == null)
            {
                currentLine = new Line(e.Location);
            }
        }

        private void Source_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentLine != null)
            {
                currentLine.setEnd(e.Location);
                Refresh();
            }
        }

        private void Source_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentLine != null)
            {
                lines.Add(currentLine);
                currentLine.setEnd(e.Location);
                currentLine = null;
                Refresh();
            }
        }
    }
}
