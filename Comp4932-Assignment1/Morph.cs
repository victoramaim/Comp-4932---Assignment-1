namespace Comp4932_Assignment1
{
    public partial class Morph : Form
    {
        public Morph()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            SourceImage sourceImage = new SourceImage();
            sourceImage.Show();
            DestinationImage destinationImage = new DestinationImage();
            destinationImage.Show();
        }
    }
}