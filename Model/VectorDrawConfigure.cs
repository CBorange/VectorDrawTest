using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spatial;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdCommandLine;
using System.Windows.Forms;
using System.Drawing;

namespace MathPractice.Model
{
    public class VectorDrawConfigure
    {
        public const int VIEW_WIDTH = 650;
        public const int VIEW_HEIGHT = 650;
        public const int VIEW_HALFWIDTH = 325;
        public const int VIEW_HALFHEIGHT = 325;

        private vdDocument document;
        private vdCommandLine commandLine;

        public void InitializeSystem(vdDocument document, vdCommandLine commandLine)
        {
            this.document = document;
            this.commandLine = commandLine;

            document.ShowUCSAxis = false;
            document.ActiveLayOut.ZoomWindow(new gPoint(-VIEW_HALFWIDTH, -VIEW_HALFHEIGHT), new gPoint(VIEW_HALFWIDTH, VIEW_HALFHEIGHT));
            commandLine.LoadCommands(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "Commands.txt");
        }
        public void AddLineToDocument(gPoint startPoint, gPoint endPoint)
        {
            vdLine newLine = new vdLine();
            newLine.SetUnRegisterDocument(document);
            newLine.setDocumentDefaults();

            newLine.StartPoint = startPoint;
            newLine.EndPoint = endPoint;
            newLine.PenColor.ColorIndex = 3;
            newLine.PenWidth = 1;
            newLine.LineType = document.LineTypes.FindName("DASHDOT0");
            newLine.Update();

            document.Model.Entities.AddItem(newLine);
            document.Redraw(true);
        }
        public void AddCircleToDocument(gPoint center, double radius)
        {
            vdCircle circle = new vdCircle(document, center, radius);
            circle.PenColor.SystemColor = Color.Yellow;
            circle.Update();

            document.Model.Entities.Add(circle);
            document.Redraw(true);
        }
    }
}
