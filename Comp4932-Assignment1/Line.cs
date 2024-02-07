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
        private bool shift;
        private bool resizeStart;
        private bool resizeEnd;
        private static int counter = 0;
        private int id;

        // Constructs the line to use both start and end points
        public Line(Point start) { 
            this.start = start;
            this.end.X = 0;
            this.end.Y = 0;
            id = counter / 2;
            counter++;
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

        public int UserMovement(MouseEventArgs e) { 
            double centerLineX = (start.X + end.X) / 2;
            double centerLineY = (start.Y + end.Y) / 2;

            // User is shifting the line position
            if (Math.Abs(e.Location.X - centerLineX) < 5 && Math.Abs(e.Location.Y - centerLineY) < 5)
            {
                shift = true;
                resizeStart = false;
                resizeEnd = false;
                return 1;
            }

            // User is changing the size of the line by moving the start point
            else if (Math.Abs(e.Location.X - start.X) < 5 && Math.Abs(e.Location.Y - start.Y) < 5)
            {
                shift = false;
                resizeStart = true;
                resizeEnd = false;
                return 3;
            }

            // User is changing the size of the line by moving the end point
            else if (Math.Abs(e.Location.X - end.X) < 5 && Math.Abs(e.Location.Y - end.Y) < 5)
            {
                shift = false;
                resizeStart = false;
                resizeEnd = true;
                return 3;
            }

            else return 4;
        }

        public int getId()
        {
            return id;
        }

        public void Resize (MouseEventArgs e)
        {
            if (shift) {
                int phaseX = e.Location.X - (start.X + end.X) / 2;
                int phaseY = e.Location.Y - (start.Y + end.Y) / 2;

                start.X += phaseX;
                start.Y += phaseY;
                end.X += phaseX;
                end.Y += phaseY;
            } 

            else if (resizeStart)
            {
                start.X = e.Location.X;
                start.Y = e.Location.Y;
            }
            else
            {
                end.X = e.Location.X;
                end.Y = e.Location.Y;
            }
        }
    }
}
