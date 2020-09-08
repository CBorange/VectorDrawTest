using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;

namespace Hicom.BizDraw.Command
{
    public class CmdDoubleClick : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private vdFigure Figure { get; set; }
        private vdDimension Dimension { get { return Figure as vdDimension; } }

        public CmdDoubleClick(gPoint reference, vdLayout layout) : base(reference, layout)
        {
            Point mousePt = layout.ActiveAction.GdiMousePos;
            vdFigure fig = layout.GetEntityFromPoint(mousePt, layout.Render.GlobalProperties.PickSize, false);
            if (fig is vdDimension || fig is vdText)
            {
                this.Figure = fig;
            }   
        }
        
        protected override void OnMyPositionChanged(gPoint newPosition)
        {

        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            if (this.Figure is vdDimension && this.Figure != null)
            {
                double textHeight = Dimension.TextHeight * Dimension.ScaleFactor;
                switch (Dimension.dimType)
                {
                    case VdConstDimType.dim_Aligned: // 직선(2점)
                    case VdConstDimType.dim_Rotated: // 수직||수평(2점)
                        this.DrawText(Dimension.DefPoint1, "S", textHeight, render);
                        this.DrawText(Dimension.DefPoint2, "E", textHeight, render);
                        break;
                    case VdConstDimType.dim_Angular: // 호(3점)
                        this.DrawText(Dimension.DefPoint3, "S", textHeight, render);
                        this.DrawText(Dimension.DefPoint4, "E", textHeight, render);
                        break;
                }
            }
        }

        public static List<vdFigure> Run(vdDocument doc, string param)
        {
            List<vdFigure> result = new List<vdFigure>();

            gPoint reference = doc.ActiveLayOut.ActiveAction.MouseLocation;
            CmdDoubleClick cmdDoubleClick = new CmdDoubleClick(reference, doc.ActiveLayOut);
            if (cmdDoubleClick.Figure != null)
            {
                // 더블클릭으로 선택한 Entity에 대하여 무조건 리턴할 것인지
                // 수정된 것만 리턴할 것인지 고민했는데 일단 무조건 리턴으로 개발
                // 프로그램단에서 더블클릭한 Entity를 수정해야할 수도 있음으로...

                StatusCode scode = StatusCode.Cancel;
                
                if (cmdDoubleClick.Figure is vdDimension)
                    scode = cmdDoubleClick.EditDimension(param);
                else if (cmdDoubleClick.Figure is vdText || cmdDoubleClick.Figure is vdMText)
                    cmdDoubleClick.EditText();

                if(scode == StatusCode.Success)
                    result.Add(cmdDoubleClick.Figure);
            }
            return result;
        }

        private void EditText()
        {
            this.Document.CommandAction.CmdEditTxT(this.Figure);
        }

        private vdText GetText(vdDimension dimension)
        {
            VectorDraw.Professional.vdCollections.vdEntities explode = dimension.Explode();
            vdMText vMText = null;
            for (int ix = 0; ix < explode.Count; ix++)
            {
                if (explode[ix] is vdMText)
                    vMText = explode[ix] as vdMText;
            }
            
            if (vMText != null)
            {
                explode = vMText.Explode();
                if (explode.Count > 0)
                {
                    vdText vText = explode[0] as vdText;
                    vText.TextString = string.Concat(Math.Round(Dimension.Measurement, Dimension.DecimalPrecision));
                    //vText.HorJustify = VdConstHorJust.VdTextHorCenter;
                    vText.Update();
                    return vText;
                }
                    
            }
            return null;
        }

        private StatusCode EditDimension(string param)
        {
            if (Dimension.dimType == VdConstDimType.dim_Aligned || Dimension.dimType == VdConstDimType.dim_Rotated)
            {
                StatusCode scode = string.IsNullOrEmpty(param) ? StatusCode.Cancel : StatusCode.Success;
                string command = param;
                
                if (string.IsNullOrEmpty(command))
                {
                    this.SetAcceptedStringValues(new string[] { "s;s;", "c;c;", "e;e;" }, param);
                    scode = this.WaitToFinish("Input FixedPoint (S)tart, (C)enter, (E)nd", valueType.POINT | valueType.STRING);
                    command = string.Concat(this.Value);
                }
                    
                if (scode == StatusCode.Success && (command == "s" || command == "e" || command == "c"))
                {
                    Dimension.dimText = " ";
                    Dimension.Invalidate();
                    Dimension.Update();

                    vdText text = GetText(this.Dimension);
                    string orgText = text.TextString;
                    double measure = 0.0;
                    this.Document.CommandAction.CmdEditTxT(text);
                    double value = 0;
                    if(double.TryParse(text.TextString, out value))
                    {
                        if (value <= 0)
                        {
                            Dimension.dimText = orgText;
                            Dimension.Invalidate();
                            Dimension.Update();
                            return StatusCode.Cancel;
                        }
                    }
                    if (string.IsNullOrEmpty(param))
                        this.Document.CommandAction.CmdEditTxT(text);

                    if(orgText != text.TextString)
                    {
                        if (double.TryParse(text.TextString, out measure))
                        {
                            if (Dimension.dimType == VdConstDimType.dim_Angular)
                                EditDimensionArc(Dimension, measure, command);
                            else
                                EditDimensionLine(Dimension, measure, command);

                            scode = StatusCode.Success;
                        }
                    }
                    else
                    {
                        scode = StatusCode.Cancel;
                    }
                    Dimension.dimText = text.TextString;
                    Dimension.Invalidate();
                    Dimension.Update();
                    return scode;
                }
            }

            return StatusCode.Cancel;
        }
        
        private void EditDimensionLine(vdDimension vdim, double measure, string command)
        {
            if(vdim.dimType == VdConstDimType.dim_Rotated) //비율로 증감
                measure = vdim.DefPoint1.Distance2D(vdim.DefPoint2) / vdim.Measurement * measure;

            switch (command.ToLower())
            {
                case "s":
                    vdim.DefPoint2.Set(this.GetDimensionLinePoint(vdim.DefPoint1, vdim.DefPoint2, measure));
                    break;
                case "e":
                    vdim.DefPoint1.Set(this.GetDimensionLinePoint(vdim.DefPoint2, vdim.DefPoint1, measure));
                    break;
                case "c":
                    Vector dir = vdim.DefPoint1.Direction(vdim.DefPoint2);
                    double dist_half = vdim.DefPoint1.Distance2D(vdim.DefPoint2) / 2.0;
                    gPoint cneterpoint = vdim.DefPoint1 + (dir * dist_half);

                    double measure_half = measure / 2.0;
                    vdim.DefPoint1.Set(this.GetDimensionLinePoint(cneterpoint, vdim.DefPoint1, measure_half));
                    vdim.DefPoint2.Set(this.GetDimensionLinePoint(cneterpoint, vdim.DefPoint2, measure_half));
                    break;
            }
        }

        // 차후 개발
        private void EditDimensionArc(vdDimension vdim, double measure, string command)
        {

        }

        private gPoint GetDimensionLinePoint(gPoint start, gPoint end, double measure)
        {
            gPoint retpt = new gPoint();
            Vector dir = start.Direction(end);
            retpt = start + (dir * measure);
            return retpt;
        }

        // 차후 개발
        private gPoint EditDimensionArcPoint()
        {
            return null;
        }
    }
}