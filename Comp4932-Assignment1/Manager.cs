using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Timer = System.Windows.Forms.Timer;

namespace Comp4932_Assignment1
{
    public partial class Manager : Form
    {
        private Image morphed;
        private int counter = 0;
        private Timer timer;

        public Manager(List<Bitmap> frames, Image morphed)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.morphed = morphed;
        }

        private void Cycle()
        {
            if (counter + 1 == ((Morpher)MdiParent).GetFrames().Count)
            {
                counter = 0;
            }
            else counter++;
            morphed.SetImage(((Morpher)MdiParent).GetFrames()[counter]);
        }

        public void InitTimer()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100; // in miliseconds
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Cycle();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (((Morpher)MdiParent).GetFrames().Count == 0)
            {
                return;
            }
            else InitTimer();
        }
    }
}
