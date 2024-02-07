using System.Numerics;

namespace Comp4932_Assignment1
{
    public partial class Morpher : Form
    {
        private Image source;
        private Image destination;
        private Image morphed;
        private Manager manager;
        private int framesNum;
        private List<Bitmap> frames;

        public Morpher()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            frames = new List<Bitmap>();

            source = new Image(Constants.SOURCE);
            source.MdiParent = this;
            source.Show();

            destination = new Image(Constants.DESTINATION);
            destination.MdiParent = this;
            destination.Show();

            morphed = new Image(Constants.MORPHED);
            morphed.MdiParent = this;
            morphed.Show();

            manager = new Manager(frames, morphed);
            manager.MdiParent = this;
            manager.Show();
        }

        public void Reflect(Line line, int origin)
        {
            Line copiedLine = new Line(line.start);
            copiedLine.setEnd(line.end);

            if (origin == Constants.SOURCE)
            {
                destination.AddLines(copiedLine);
            }

            else if (origin == Constants.DESTINATION)
            {
                source.AddLines(copiedLine);
            }

            source.Invalidate();
            destination.Invalidate();
        }

        public void UpdateTransition(int frame)
        {
            if (morphed == null) return;
            morphed.SetImage(frames[frame]);

        }

        private void Morph_Load(object sender, EventArgs e)
        {

        }

        // Get the number of frames
        public void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            framesNum = (int)numericUpDown1.Value;
        }

        public void GenerateIntermediateFrames(List<Vector2> dest_points, List<Vector2> source_points, Bitmap final_image, Bitmap destination_image, List<Color> dest_colors, List<Color> source_colors)
        {
            List<Vector2> new_dest_points = new List<Vector2>(dest_points);
            frames.Add(destination_image);
            for (int frameIndex = 0; frameIndex < framesNum - 1; frameIndex++)
            {
                Bitmap frame = new Bitmap(destination_image.Width, destination_image.Height);

                for (int i = 0; i < dest_points.Count; i++)
                {
                    float diff_X = (dest_points[i].X - source_points[i].X) / framesNum;
                    float diff_Y = (dest_points[i].Y - source_points[i].Y) / framesNum;
                    Vector2 diffVector = new Vector2(diff_X, diff_Y);

                    new_dest_points[i] = Vector2.Subtract(new_dest_points[i], diffVector);
                    new_dest_points[i] = destination.validatePixel(new_dest_points[i], frame.Width, frame.Height);

                    frame.SetPixel((int)dest_points[i].X, (int)dest_points[i].Y, destination_image.GetPixel((int)new_dest_points[i].X, (int)new_dest_points[i].Y));
                }

                frames.Add(frame);
            }
            frames.Add(final_image);
        }

        public List<Bitmap> GetFrames()
        {
            return frames;
        }

        // Morphing button
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            destination.Morph(source.GetLines(), framesNum);
        }

        public void TransitionInit(Bitmap img)
        {
            morphed.SetImage(img);
        }
    }
}