using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Comp4932_Assignment1
{
    public class Line
    {
        public Point start;
        public Point end;
        

        // Constructs the line to use both start and end points
        public Line(Point start) { 
            this.start = start;
            this.end.X = 0;
            this.end.Y = 0;
        }

        
        Pen pen = new Pen(Color.Black, 5); // Create the pen

        public void setEnd(Point end)
        {
            this.end = end;
        }
        
        // Draws the line between the two points
        public void CreateLine (PaintEventArgs e)
        {
            
            e.Graphics.DrawLine(pen, start, end);
        }
    }
}
