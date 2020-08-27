using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdCommandLine;
using System.Windows.Forms;
using System.Drawing;
using VectorDraw.Render;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

namespace MathPractice.Model.Manager
{
    public class VectorDrawConfigure
    {
        private static  VectorDrawConfigure instance;
        public static VectorDrawConfigure Instance
        {
            get
            {
                if (instance == null)
                    instance = new VectorDrawConfigure();
                return instance;
            }
        }
        public const int VIEW_WIDTH = 650;
        public const int VIEW_HEIGHT = 650;
        public const int VIEW_HALFWIDTH = 325;
        public const int VIEW_HALFHEIGHT = 325;

        private vdDocument document;
        private vdCommandLine commandLine;

        private BeamManager beamManager;

        private VectorDrawConfigure()
        {

        }
        public void InitializeSystem(vdDocument document, vdCommandLine commandLine, BeamManager beamManager)
        {
            this.document = document;
            this.commandLine = commandLine;
            this.beamManager = beamManager;

            document.ShowUCSAxis = false;
            document.ActiveLayOut.ZoomWindow(new gPoint(-VIEW_HALFWIDTH, -VIEW_HALFHEIGHT), new gPoint(VIEW_HALFWIDTH, VIEW_HALFHEIGHT));
            document.OnDrawOverAll += new vdDocument.DrawOverAllEventHandler(AllDrawOver_Handler);
            document.ActionEnd += new vdDocument.ActionEndEventHandler(ActionEnd_Handler);

            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\";
            if (System.IO.Directory.Exists(path))
            {
                document.SupportPath = path;

                commandLine.SelectDocument(document);
                commandLine.UnLoadCommands();
                bool result = commandLine.LoadCommands(path, "Commands.txt");
                if (!result)
                    Debug.WriteLine("Load Command Error");
            }
        }
        // Line, Circle, Text 등 Custom Class로 제어되지 않는 객체 추가
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
        public void AddLineToDocument(vdLine newLine)
        {
            newLine.SetUnRegisterDocument(document);
            newLine.setDocumentDefaults();
            document.Model.Entities.AddItem(newLine);
        }
        public vdCircle AddCircleToDocument(gPoint center, double radius)
        {
            vdCircle circle = new vdCircle(document, center, radius);
            circle.PenColor.SystemColor = Color.Yellow;
            circle.Update();

            document.Model.Entities.Add(circle);
            document.Redraw(true);
            return circle;
        }
        public vdText AddTextToDocument(gPoint insertPoint, string caption)
        {
            vdText text = new vdText(document, caption, insertPoint, 3);
            text.PenColor.SystemColor = Color.Orange;
            text.Update();

            document.Model.Entities.Add(text);
            document.Redraw(true);
            return text;
        }

        // Event Handler
        public void AllDrawOver_Handler(object sender, vdRender render, ref bool cancel)
        {
            Debug.WriteLine("AllDrawOver");
            beamManager.DrawOutLineFromAllBeam(render);
        }
        public void ActionEnd_Handler(object sender, string actionName)
        {
            Debug.WriteLine("ActionEnd");
            beamManager.RefreshAllBeam();
            document.Redraw(true);
        }
    }
}
