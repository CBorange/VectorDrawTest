using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.Constants;
using VectorDraw.Geometry;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.PlanDraw
{
    public class DrawObject : DrawBase
    {
        private gPoint LastPoint { get; set; }

        public vdHatchProperties HatchProperties { get; set; }

        public DrawObject(vdDocument doc) : base(doc)
        {
            this.HatchProperties = new vdHatchProperties(VdConstFill.VdFillModeNone);
        }

        public void Initialize()
        {
            this.LastPoint = null;
            this.HatchProperties = new vdHatchProperties(VdConstFill.VdFillModeNone);
        }
        
        public HLine DrawLine(gPoint start, gPoint end)
        {
            HLine vLine = DrawObject.DrawLine(this.Document, start, end);            
            vLine.Layer = this.ActiveLayer;
            vLine.PenColor.ByLayer = true;
            AddItem(vLine);
            this.LastPoint = new gPoint(end);
            return vLine;
        }

        public HLine DrawLineTo(gPoint end)
        {
            if(this.LastPoint != null)
            {
                HLine vLine = DrawObject.DrawLine(this.Document, this.LastPoint, end);
                vLine.Layer = this.ActiveLayer;
                vLine.PenColor.ByLayer = true;
                AddItem(vLine);
                this.LastPoint = new gPoint(end);
                return vLine;
            }
            return null;
        }

        public HCircle DrawCircle(gPoint p1, gPoint p2, gPoint p3)
        {
            Geometry.Circle circle = new Geometry.Circle(p1, p2, p3);
            return this.DrawCircle(circle.Center, circle.Radius);
        }

        public HCircle DrawCircle(gPoint p1, gPoint p2)
        {
            Geometry.Circle circle = new Geometry.Circle(p1, p2);
            return this.DrawCircle(circle.Center, circle.Radius);
        }

        public HCircle DrawCircle(gPoint center, double radius)
        {
            HCircle vCircle = DrawObject.DrawCircle(this.Document, center, radius);
            vCircle.PenColor.ByLayer = true;
            vCircle.HatchProperties = this.HatchProperties.Clone() as vdHatchProperties;
            vCircle.Layer = this.ActiveLayer;
            AddItem(vCircle);
            return vCircle;
        }

        public HPolyline DrawPolyline(Vertexes vertexes, VdConstPlineFlag flag)
        {
            HPolyline vPolyline = new HPolyline();
            vPolyline.PenColor.ByLayer = true;
            vPolyline.VertexList = vertexes;
            vPolyline.Flag = flag;
            vPolyline.HatchProperties = this.HatchProperties.Clone() as vdHatchProperties;
            vPolyline.Layer = this.ActiveLayer;
            AddItem(vPolyline);
            return vPolyline;
        }

        public HArc DrawArc(gPoint center, double radius, double startAngleDeg, double endAngleDeg)
        {
            HArc vArc = new HArc();
            vArc.SetUnRegisterDocument(this.Document);
            vArc.setDocumentDefaults();
            vArc.Center = center;
            vArc.Radius = radius;
            vArc.StartAngle = Globals.DegreesToRadians(startAngleDeg);
            vArc.EndAngle = Globals.DegreesToRadians(endAngleDeg);
            vArc.HatchProperties = this.HatchProperties.Clone() as vdHatchProperties;
            vArc.Layer = this.ActiveLayer;
            vArc.PenColor.ByLayer = true;
            AddItem(vArc);
            return vArc;
        }

        public HArc DrawArc(gPoint p1, gPoint p2, gPoint p3)
        {
            Geometry.Arc arc = new Geometry.Arc(p1, p2, p3);
            if(arc.IsCircle)
                return this.DrawArc(arc.Center, arc.Radius, arc.StartAngle, arc.EndAngle);
            return null;
        }

        public HRect DrawRectangle(gPoint insertPoint, double width, double height, double rotation = 0)
        {
            HRect vRect = new HRect();
            vRect.SetUnRegisterDocument(this.Document);
            vRect.setDocumentDefaults();
            vRect.InsertionPoint = insertPoint;
            vRect.Rotation = rotation;
            vRect.Width = width;
            vRect.Height = height;
            vRect.Layer = this.ActiveLayer;
            vRect.PenColor.ByLayer = true;
            vRect.HatchProperties = this.HatchProperties.Clone() as vdHatchProperties;
            AddItem(vRect);
            return vRect;
        }

        #region static draw
        public static HLine DrawLine(vdDocument doc, gPoint start, gPoint end)
        {
            HLine line = new HLine(doc, start, end);
            line.SetUnRegisterDocument(doc);
            line.setDocumentDefaults();
            return line;
        }
        
        public static HCircle DrawCircle(vdDocument doc, gPoint center, double radius)
        {
            HCircle circle = new HCircle();
            circle.SetUnRegisterDocument(doc);
            circle.setDocumentDefaults();
            circle.Center = center;
            circle.Radius = radius;
            return circle;
        }

        public static HCircle DrawCircle(vdDocument doc, Geometry.Circle circle)
        {
            return DrawObject.DrawCircle(doc, circle.Center, circle.Radius);
        }

        public static HArc DrawArc(vdDocument doc, gPoint center, double radius, double startAngle, double endAngle)
        {
            HArc vArc = new HArc(doc, center, radius, startAngle, endAngle);
            vArc.SetUnRegisterDocument(doc);
            vArc.setDocumentDefaults();
            return vArc;
        }

        public static HArc DrawArc(vdDocument doc, Geometry.Arc arc)
        {
            return DrawObject.DrawArc(doc, arc.Center, arc.Radius, arc.StartAngle, arc.EndAngle);
        }

        public static HRect DrawRectangle(vdDocument doc, gPoint insertPoint, double width, double height, double rotation = 0)
        {
            HRect vRect = new HRect(doc, insertPoint, width, height, rotation);
            vRect.SetUnRegisterDocument(doc);
            vRect.setDocumentDefaults();
            return vRect;
        }

        public static HRect DrawRectangle(vdDocument doc, gPoint startPoint, gPoint endPoint, double rotation = 0)
        {
            double width = endPoint.x - startPoint.x;
            double height = endPoint.y - startPoint.y;
            return DrawRectangle(doc, startPoint, width, height, rotation);
        }
        
        #endregion
    }
}
