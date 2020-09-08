using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Render;

namespace Hicom.BizDraw.PlanDraw
{
    public class DrawText : DrawBase
    {
        public enum eTextPosition
        {
            Front,
            Middle,
            Behind
        }

        public VdConstVerJust VerJust { get; set; }
        public VdConstHorJust HorJust { get; set; }
        
        public double Rotation { get; set; }
        public double WidthFactor { get; set; }        
        public bool Bold { get; set; }        
        public bool HighLight { get; set; }
        
        public double LeaderArrowSize { get; set; }
        public double LineSpacingFactor { get; set; }
        public vdMText.LineSpaceFlag LineSpaceStyle { get; set; }
        public string TextStyle { get; set; }

        public DrawText(vdDocument doc) : base(doc)
        {
            this.Initialize();
        }

        public void Initialize()
        {
            this.VerJust = VdConstVerJust.VdTextVerCen;
            this.HorJust = VdConstHorJust.VdTextHorCenter;
            
            this.Rotation = 0;
            this.WidthFactor = 1;
            this.Bold = false;
            this.HighLight = false;            
            this.TextStyle = "STANDARD";
            this.LineSpacingFactor = 1;
            this.LineSpaceStyle = vdMText.LineSpaceFlag.TextHeightDepended;
            this.LeaderArrowSize = 5;
        }

        public vdText InsertText(gPoint insertPoint, string text)
        {
            vdText vText = new vdText();
            vText.SetUnRegisterDocument(this.Document);
            vText.setDocumentDefaults();
            vText.TextString = text;
            vText.InsertionPoint = insertPoint;
            vText.VerJustify = this.VerJust;
            vText.HorJustify = this.HorJust;
            vText.Style = this.Document.TextStyles.FindName(this.TextStyle);
            vText.Height = vText.Style.Height;
            vText.Layer = this.ActiveLayer;            
            vText.Rotation = this.Rotation;
            vText.HighLight = this.HighLight;

            AddItem(vText);
            return vText;
        }

        public vdMText InsertMText(gPoint insertPoint, string text)
        {
            vdMText vText = new vdMText();
            vText.SetUnRegisterDocument(this.Document);
            vText.setDocumentDefaults();
            vText.TextString = text;
            vText.InsertionPoint = insertPoint;
            vText.VerJustify = this.VerJust;
            vText.HorJustify = this.HorJust;
            vText.Style = this.Document.TextStyles.FindName(this.TextStyle);
            vText.Height = vText.Style.Height;
            vText.Layer = this.ActiveLayer;
            vText.Rotation = this.Rotation;
            vText.HighLight = this.HighLight;
            vText.LineSpacingFactor = this.LineSpacingFactor;
            vText.LineSpaceStyle = this.LineSpaceStyle;
            AddItem(vText);

            return vText;
        }

        public vdLeader InsertLeaderText(gPoint p1, gPoint p2, gPoint p3, string text, eTextPosition position)
        {
            vdMText vText = new vdMText();
            vText.SetUnRegisterDocument(this.Document);
            vText.setDocumentDefaults();
            vText.TextString = text;
            if(position == eTextPosition.Front)
                vText.InsertionPoint = p2;
            else if(position == eTextPosition.Middle)
                vText.InsertionPoint = (p2 + p3)/2;
            else if(position == eTextPosition.Behind)
                vText.InsertionPoint = p3;
            vText.VerJustify = this.VerJust;
            vText.HorJustify = this.HorJust;
            vText.Layer = this.ActiveLayer;
            vText.Style = this.Document.TextStyles.FindName(this.TextStyle);
            vText.Height = vText.Style.Height;            
            vText.HighLight = this.HighLight;
            vText.LineSpacingFactor = this.LineSpacingFactor;
            vText.LineSpaceStyle = this.LineSpaceStyle;            
            vText.Rotation = p2.GetAngle(p3);
            vText.Update();

            vdLeader vLeader = new vdLeader();
            vLeader.SetUnRegisterDocument(this.Document);
            vLeader.setDocumentDefaults();
            vLeader.VertexList.Add(p1);
            vLeader.VertexList.Add(p2);
            vLeader.VertexList.Add(p3);
            vLeader.ArrowSize = this.LeaderArrowSize;
            vLeader.Layer = this.ActiveLayer;
            vLeader.LeaderMText = vText;
            vLeader.Update();
            vLeader.Invalidate();

            AddItem(vText);
            AddItem(vLeader);

            return vLeader;
        }

        static public vdText InsertText(vdDocument doc, gPoint insertPoint, string text, double height)
        {
            vdText vText = new vdText();
            vText.SetUnRegisterDocument(doc);
            vText.setDocumentDefaults();
            vText.TextString = text;
            vText.InsertionPoint = insertPoint;
            vText.VerJustify = VdConstVerJust.VdTextVerCen;
            vText.HorJustify = VdConstHorJust.VdTextHorCenter;
            vText.Style = doc.ActiveTextStyle;
            vText.Height = height;
            vText.Layer = doc.ActiveLayer;
            vText.Rotation = 0;
            vText.HighLight = false;
            
            return vText;
        }
    }
}
