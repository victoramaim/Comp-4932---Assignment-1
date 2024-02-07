using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Comp4932_Assignment1
{
    public partial class Image : Form
    {
        protected Bitmap? pictureFrame;
        private Line? currentLine;
        private Line? movingLine;
        public List<Line> lines; // List to store lines
        protected int type;

        public Image(int type)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            currentLine = null;
            lines = new List<Line>();
            pictureFrame = null;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            this.type = type;
            Text = TypeToString();
        }

        public void SetImage(Bitmap image)
        {
            pictureFrame = image;
            //ResizeBitmap(loaded, ClientSize.Width, ClientSize.Height);
            Refresh();
        }

        private string TypeToString()
        {
            if (type != 2) {
                return type == Constants.SOURCE ? Constants.SOURCE_STR : Constants.DESTINATION_STR;
            } else { return Constants.MORPHED_STR; }
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
                    if (type == Constants.SOURCE) ((Morpher)MdiParent).TransitionInit(pictureFrame);
                    Refresh();
                }
            }
        }

        private void Source_MouseDown(object sender, MouseEventArgs e)
        {
            if (type != 2)
            {
                bool hoverLine = false;

                foreach (Line line in lines)
                {
                    if (line.UserMovement(e) != 4)
                    {
                        hoverLine = true;
                        movingLine = line;
                    }
                }

                if (!hoverLine)
                {
                    if (pictureFrame != null && e.Button == MouseButtons.Left)
                    {
                        currentLine = new Line(e.Location);
                    }
                }
            }
            else return;
        }

        private void Source_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentLine != null) {
                currentLine.setEnd(e.Location);
                Refresh();
            }

            else if (movingLine != null) {
                movingLine.Resize(e);
                Refresh();
            }
        }

        private void Source_MouseUp(object sender, MouseEventArgs e)
        {
            if (pictureFrame != null && e.Button == MouseButtons.Left && currentLine != null)
            {
                lines.Add(currentLine);
                currentLine.setEnd(e.Location);
                ((Morpher)MdiParent).Reflect(currentLine, this.type);
                currentLine = null;
                movingLine = null;
                Refresh();
            }

            else if (movingLine != null) {
                movingLine = null;
                Refresh();
            }
        }

        public Line getLine(int lineId)
        {
            return lines.Find(line => line.getId() == lineId);
        }

        public void AddLines(Line line)
        {
            lines.Add(line);
        }

        public void Morph(List<Line> sourceLines, int framesNum)
        {
            if (((Morpher)MdiParent).GetFrames().Count != 0)
            {
                ((Morpher)MdiParent).GetFrames().Clear();
            }
            Bitmap transition = new Bitmap(pictureFrame.Width, pictureFrame.Height);
            List<Vector2> sourcePoints = new List<Vector2>();
            List<Color> colors = new List<Color>();
            List<Vector2> dest_points = new List<Vector2>();
            List<Color> dest_colors = new List<Color>();

            for (int y = 0; y < pictureFrame.Height; ++y)
            {
                for (int x = 0; x < pictureFrame.Width; ++x)
                {
                    double weight_sum = 0;
                    Vector2 delta_sum = new Vector2(0, 0);
                    for (int k = 0; k < lines.Count; k++)
                    {
                        Line line = lines[k];

                        Vector2 P = new Vector2(line.start.X, line.start.Y);
                        Vector2 Q = new Vector2(line.end.X, line.end.Y);
                        Vector2 PQ = new Vector2(line.end.X - line.start.X, line.end.Y - line.start.Y);
                        Vector2 n = new Vector2(-PQ.Y, PQ.X);
                        Vector2 XP = new Vector2(line.start.X - x, line.start.Y - y);
                        Vector2 PX = new Vector2(x - line.start.X, y - line.start.Y);

                        float d = Vector2.Dot(XP, n) / n.Length();

                        float f = Vector2.Dot(PX, PQ) / PQ.Length();

                        float fl = f / PQ.Length();

                        Line sourceLine = sourceLines[k];

                        Vector2 PPrime = new Vector2(sourceLine.start.X, sourceLine.start.Y);
                        Vector2 NPrime = new Vector2(-1 * (sourceLine.end.Y - sourceLine.start.Y), sourceLine.end.X - sourceLine.start.X);
                        Vector2 PQPrime = new Vector2(sourceLine.end.X - sourceLine.start.X, sourceLine.end.Y - sourceLine.start.Y);

                        Vector2 XPrime = PPrime + Vector2.Multiply(fl, PQPrime) - Vector2.Multiply(d, Vector2.Divide(NPrime, NPrime.Length()));

                        Vector2 X = new Vector2(x, y);
                        Vector2 delta1 = XPrime - X;
                        double weight = 0;
                        if (fl >= 0 && fl <= 1) weight = Math.Pow(1 / (d + 0.01), 2);
                        else if (fl < 0)
                        {
                            float dxp = Vector2.Distance(X, P);
                            weight = Math.Pow(1 / (dxp + 0.01), 2);
                        }
                        else if (fl > 1)
                        {
                            float dxq = Vector2.Distance(X, Q);
                            weight = Math.Pow(1 / (dxq + 0.01), 2);
                        }
                        weight_sum += weight;
                        delta_sum += Vector2.Multiply((float)weight, delta1);

                    }
                    Vector2 delta_avg = Vector2.Divide(delta_sum, (float)weight_sum);

                    Vector2 XPrime_avg = new Vector2(x, y) + delta_avg;
                    XPrime_avg = validatePixel(XPrime_avg, pictureFrame.Width, pictureFrame.Height);
                    transition.SetPixel(x, y, pictureFrame.GetPixel((int)XPrime_avg.X, (int)XPrime_avg.Y));

                    dest_points.Add(new Vector2(x, y));
                    dest_colors.Add(pictureFrame.GetPixel(x, y));
                    sourcePoints.Add(new Vector2((int)XPrime_avg.X, (int)XPrime_avg.Y));
                    colors.Add(pictureFrame.GetPixel((int)XPrime_avg.X, (int)XPrime_avg.Y));
                }
            }
            ((Morpher)MdiParent).GenerateIntermediateFrames(dest_points, sourcePoints, transition, pictureFrame, dest_colors, colors);
            ((Morpher)MdiParent).UpdateTransition(0);
        }

        public Vector2 validatePixel(Vector2 coord, int width, int height)
        {
            if (coord.X < 0)
            {
                coord.X = 0;
            }
            else if (coord.X >= width)
            {
                coord.X = width - 1;
            }
            if (coord.Y < 0)
            {
                coord.Y = 0;
            }
            else if (coord.Y >= height)
            {
                coord.Y = height - 1;
            }
            return coord;
        }

        public List<Line> GetLines()
        {
            return lines;
        }

        public Bitmap GetImage()
        {
            return pictureFrame;
        }
    }
}
