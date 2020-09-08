using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.PlanDraw
{
    public class DrawDimension : DrawBase
    {
        // 치수선 진행방향 기준
        public enum eDirection
        {
            Left,
            Right,
            VerLeft,
            VerRight,
            HorLeft,
            HorRight,
            None
        }

        private DimensionOptions _option;
        public DimensionOptions Options { get { return _option; } }
        public eDirection Direction { get; set; }        
        public int Level { get; set; }

        private gPoint PreStart { get; set; }
        private gPoint PreEnd { get; set; }

        #region Dimension Arrow options
        public const string ARROWTYPE_ARROW = "VDDIM_DEFAULT";
        public const string ARROWTYPE_CIRCLE = "VDDIM_ARROW_CIRCLE";
        public const string ARROWTYPE_DIAGONAL = "VDDIM_ARROW_DIAGONAL";
        public string ArrowBlock
        {
            get
            {
                if (this.arrowBlock != null)
                    return this.arrowBlock.Name;
                return string.Empty;
            }
            set
            {
                string name = (string)value;
                this.arrowBlock = this.Document.Blocks.FindName(name);
            }
        }
        public string ArrowBlock2
        {
            get
            {
                if (this.arrowBlock2 != null)
                    return this.arrowBlock2.Name;
                return string.Empty;
            }
            set
            {
                string name = (string)value;
                this.arrowBlock2 = this.Document.Blocks.FindName(name);
            }
        }
        private vdBlock arrowBlock { get; set; }
        private vdBlock arrowBlock2 { get; set; }
        #endregion

        public DrawDimension(vdDocument doc) : base(doc)
        {
            _option = new DimensionOptions();
            this.Initialize();
        }

        public void Initialize()
        {
            _option.SetDefault();
            Level = 1;
            Direction = eDirection.Left;
            AddArrowBlock(DrawDimension.ARROWTYPE_CIRCLE);
            AddArrowBlock(DrawDimension.ARROWTYPE_DIAGONAL);
            ArrowBlock = DrawDimension.ARROWTYPE_ARROW;
            ArrowBlock2 = DrawDimension.ARROWTYPE_ARROW;
        }
        
        public void SetDirection(eDirection direction)
        {
            this.Direction = direction;
        }

        public void SetLevel(int level)
        {
            this.Level = level;
        }

        public void SetDimension(eDirection direction, int level)
        {
            this.Direction = direction;
            this.Level = level;
        }
        
        public void SetDimScale(double scaleFactor)
        {
            this.Options.ScaleFactor = scaleFactor;
        }

        public void SetDecimalPrecision(short decimalPrecision)
        {
            this.Options.DecimalPrecision = decimalPrecision;
        }

        public void AddArrowBlock(vdBlock vBlock)
        {
            if(this.Document.Blocks.FindItem(vBlock.Name) == false)
            {
                this.Document.Blocks.AddItem(vBlock);
            }
        }

        public void AddArrowBlock(string name)
        {
            switch(name)
            {
                case DrawDimension.ARROWTYPE_CIRCLE:
                    vdCircle vCircle = new vdCircle();
                    vCircle.SetUnRegisterDocument(this.Document);
                    vCircle.setDocumentDefaults();
                    vCircle.Center = new gPoint(0, 0);
                    vCircle.Radius = 0.5;
                    vCircle.Layer = this.ActiveLayer;
                    vCircle.HatchProperties = new vdHatchProperties(VdConstFill.VdFillModeSolid);
                    this.AddArrowBlock(name, new gPoint(0, 0), vCircle);
                    break;
                case DrawDimension.ARROWTYPE_DIAGONAL:
                    vdLine vLine = new vdLine();
                    vLine.SetUnRegisterDocument(this.Document);
                    vLine.setDocumentDefaults();
                    vLine.StartPoint = new gPoint(0, 0);
                    vLine.EndPoint = new gPoint(1, 1);
                    vLine.Layer = this.ActiveLayer;
                    this.AddArrowBlock(name, (vLine.StartPoint + vLine.EndPoint) / 2, vLine);
                    break;
            }
        }

        public void AddArrowBlock(string name, gPoint origin, vdFigure vFigure)
        {
            if (this.Document.Blocks.FindItem(name) == false)
            {
                vdBlock vBlock = new vdBlock();
                vBlock.SetUnRegisterDocument(this.Document);
                vBlock.setDocumentDefaults();
                vBlock.Name = name;
                vBlock.Entities.AddItem(vFigure);
                vBlock.Origin = new gPoint(origin);
                this.AddArrowBlock(vBlock);
            }
        }
        
        /// <summary>
        /// 이전 치수선의 끝점에서 거리 만큼 이동한 치수선
        /// </summary>
        /// <param name="length"></param>
        public void DrawTo(double length)
        {
            if (this.PreStart != null && this.PreEnd != null)
            {
                Vector vec = new Vector(this.PreEnd - this.PreStart);
                vec.Normalize();
                gPoint end = this.PreEnd + vec * length;
                this.Draw(this.PreEnd, end);
                this.PreEnd = new gPoint(end);
            }
        }

        private short GetTrimDecimalPrecision(double len)
        {
            string dimString = len.ToString();
            int decimalPrecision = this.Options.DecimalPrecision;
            int index = dimString.LastIndexOf('.');
            decimalPrecision = (index != -1) ? decimalPrecision = Math.Min(dimString.Length - index -1, decimalPrecision) : 0;
            
            return Convert.ToInt16(decimalPrecision);
        }

        public void Draw(vdDimension dimension)
        {
            dimension.SetUnRegisterDocument(this.Document);
            dimension.setDocumentDefaults();

            dimension.ArrowBlock = this.arrowBlock;
            dimension.ArrowBlock2 = this.arrowBlock2;
            dimension.AngularType = VdConstDimAngularType.Length;
            dimension.Rotation = 0;
            dimension.LinePosition = DrawDimension.GetLinePosition(dimension.DefPoint1, dimension.DefPoint2, this.Direction, this.Options.FirstSpace, this.Options.DimSpace, this.Level);
            dimension.TextPosition = DrawDimension.GetTextPosition(dimension.TextPosition, dimension.LinePosition, this.Direction, this.Options.FirstSpace, this.Options.DimSpace, this.Level);
            switch (this.Direction)
            {
                case eDirection.Left:
                case eDirection.Right:
                    dimension.dimType = VdConstDimType.dim_Aligned;
                    break;
                case eDirection.VerLeft:
                case eDirection.VerRight:
                    dimension.dimType = VdConstDimType.dim_Rotated; dimension.Rotation = Globals.HALF_PI;
                    break;
                case eDirection.HorLeft:
                case eDirection.HorRight:
                    dimension.dimType = VdConstDimType.dim_Rotated;
                    break;
            }

            dimension.ArrowSize = 100;// this.Options.ArrowSize;
            dimension.TextHeight = this.Options.TextHeight;
            dimension.ScaleFactor = 1;// this.Options.ScaleFactor;
            dimension.DecimalPrecision = GetTrimDecimalPrecision(Math.Round(dimension.Measurement, 3)); // 소수 3째 짜리에서 반올림한 것을 기준으로 함
            dimension.ExtLineDist1 = this.Options.ExtLineDist1;
            dimension.ExtLineDist2 = this.Options.ExtLineDist2;
            dimension.TextDist = this.Options.TextDist;
            dimension.Layer = this.ActiveLayer;
            

            // 도면에서 치수선 옵션이 적용된 경우가 있으므로 기본값으로 변경해준다.
            dimension.DimLineColor.ColorFlag = vdColor.ColorType.ByBlock;
            dimension.ExtLineColor.ColorFlag = vdColor.ColorType.ByBlock;
            dimension.TextMovement = VdConstDimTextMovement.KeepDimLineWithText;
            dimension.TextStyle = this.Document.TextStyles.FindName(CommonActionString.ACTION_DIM_TEXTSTYLE);
            AddItem(dimension);
            this.PreStart = new gPoint(dimension.DefPoint1);
            this.PreEnd = new gPoint(dimension.DefPoint2);
        }

        /// <summary>
        /// 두점을 사이의 치수선
        /// </summary>
        /// <param name="level"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public vdDimension Draw(gPoint p1, gPoint p2)
        {
            vdDimension dimension = new vdDimension();
            dimension.DefPoint1 = p1;
            dimension.DefPoint2 = p2;
            Draw(dimension);
            
            return dimension;
        }

        /// <summary>
        /// 두 선분의 사잇각
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="line2"></param>
        /// <param name="radius"></param>
        public vdDimension DrawAngular(Line line1, Line line2, double radius)
        {
            gPoint center = new gPoint();
            if(line1.Intersection(line2, out center) == 1)
            {
                gPoint p1 = new gPoint(center);
                p1 += line1.Direction * radius;
                gPoint p2 = new gPoint(center);
                p2 += line2.Direction * radius;
                return this.DrawAngular(center, p1, p2);
            }
            return null;
        }

        /// <summary>
        /// 두 선분의 사잇각
        /// </summary>
        /// <param name="center"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public vdDimension DrawAngular(gPoint center, gPoint p1, gPoint p2)
        {
            Vector vec1 = new Vector(p1 - center);
            Vector vec2 = new Vector(p2 - center);
            vec1.Normalize();
            vec2.Normalize();
            Line line1 = new Line(center, vec1);
            Line line2 = new Line(center, vec2);
            double endAng = center.GetAngle(p2);
            Arc arc = new Arc(center, p1, endAng);

            gPoint linePosition = arc.MiddlePoint();
            gPoints defPoint = DrawDimension.GetAnglurDimensionPoint(center, line1, line2, linePosition);
            vdDimension vDimension = new vdDimension();
            vDimension.SetUnRegisterDocument(this.Document);
            vDimension.setDocumentDefaults();
            if(defPoint.Count > 3)
            {
                vDimension.ArrowBlock = this.arrowBlock;
                vDimension.ArrowBlock2 = this.arrowBlock2;
                vDimension.DefPoint1 = defPoint[0];
                vDimension.DefPoint2 = defPoint[1];
                vDimension.DefPoint3 = defPoint[2];
                vDimension.DefPoint4 = defPoint[3];
                vDimension.LinePosition = linePosition;
                vDimension.Rotation = 0;
                vDimension.dimType = VdConstDimType.dim_Angular;
                vDimension.AngularType = VdConstDimAngularType.Angle;
                vDimension.DimAunit = AUnits.AUnitType.au_Degrees;
                vDimension.ArrowSize = this.Options.ArrowSize;
                vDimension.TextHeight = this.Options.TextHeight;
                vDimension.ScaleFactor = this.Options.ScaleFactor;
                vDimension.DecimalPrecision = this.Options.DecimalPrecision;
                vDimension.ExtLineDist1 = this.Options.ExtLineDist1;
                vDimension.ExtLineDist2 = this.Options.ExtLineDist2;
                vDimension.TextDist = this.Options.TextDist;
                vDimension.Layer = this.ActiveLayer;

                AddItem(vDimension);
            }

            return vDimension;
        }

        public void DrawRectangular(gPoint origin, double width, double height)
        {
            if (this.Direction == eDirection.Left)
            {
                gPoint p1 = origin + Geometry.Geometry.Yaxis * height;
                gPoint p2 = p1 + Geometry.Geometry.Xaxis * width;
                this.Draw(origin, p1);
                this.Draw(p1, p2);
            }
            else if(this.Direction == eDirection.Right)
            {
                gPoint p1 = origin + Geometry.Geometry.Xaxis * width;
                gPoint p2 = p1 + Geometry.Geometry.Yaxis * height;
                this.Draw(origin, p1);
                this.Draw(p1, p2);
            }
        }

        public void SetDimensionOptions(DimensionOptions opt)
        {
            Options.ScaleFactor = opt.ScaleFactor;
            Options.TextHeight = opt.TextHeight;
            Options.FirstSpace = opt.FirstSpace;
            Options.DimSpace = opt.DimSpace;
            Options.DecimalPrecision = opt.DecimalPrecision;
            Options.ArrowSize = opt.ArrowSize;
            Options.ExtLineDist1 = opt.ExtLineDist1;
            Options.ExtLineDist2 = opt.ExtLineDist2;
            Options.TextDist = opt.TextDist;
        }

        public static gPoint GetLinePosition(gPoint p1, gPoint p2, eDirection direction, double firstSpace, double dimSpace, int level)
        {
            Vector dir = new Vector(p2 - p1);
            dir.Normalize();
            dir.Rotate90(true);
            gPoint result = new gPoint();

            switch (direction)
            {
                case eDirection.Left:
                    result = p1 + dir * (firstSpace + dimSpace * (level - 1));
                    break;
                case eDirection.Right:
                    result = p1 - dir * (firstSpace + dimSpace * (level - 1));
                    break;
                case eDirection.VerLeft:
                    result = p1 - Geometry.Geometry.Xaxis * (firstSpace + dimSpace * (level - 1));
                    break;
                case eDirection.VerRight:
                    result = p1 + Geometry.Geometry.Xaxis * (firstSpace + dimSpace * (level - 1));
                    break;
                case eDirection.HorLeft:
                    result = p1 + Geometry.Geometry.Yaxis * (firstSpace + dimSpace * (level - 1));
                    break;
                case eDirection.HorRight:
                    result = p1 - Geometry.Geometry.Yaxis * (firstSpace + dimSpace * (level - 1));
                    break;
            }

            return result;
        }

        public static gPoint GetTextPosition(gPoint p1, gPoint p2, eDirection direction, double firstSpace, double dimSpace, int level)
        {
            Vector dir = new Vector(p2 - p1);
            dir.Normalize();
            dir.Rotate90(true);
            gPoint result = new gPoint();

            switch (direction)
            {
                case eDirection.Left:
                    result = p1 + dir * (firstSpace + dimSpace * (level - 1) );
                    break;
                case eDirection.Right:
                    result = p1 - dir * (firstSpace + dimSpace * (level - 1) );
                    break;
                case eDirection.VerLeft:
                    result = p1 - Geometry.Geometry.Xaxis * (firstSpace + dimSpace * (level - 1) );
                    break;
                case eDirection.VerRight:
                    result = p1 + Geometry.Geometry.Xaxis * (firstSpace + dimSpace * (level - 1) );
                    break;
                case eDirection.HorLeft:
                    result = p1 + Geometry.Geometry.Yaxis * (firstSpace + dimSpace * (level - 1) );
                    break;
                case eDirection.HorRight:
                    result = p1 - Geometry.Geometry.Yaxis * (firstSpace + dimSpace * (level - 1) );
                    break;
            }

            return result;
        }

        public static gPoints GetAnglurDimensionPoint(gPoint center, Line line1, Line line2, gPoint pointOnArc)
        {
            gPoints defPoint = new gPoints();
            double radius = center.Distance2D(pointOnArc);
            
            linesegment segCross1;
            linesegment segCross2;
            Circle circle = new Circle(center, radius);
            int result1 = circle.Intersection(line1, out segCross1);
            int result2 = circle.Intersection(line2, out segCross2);

            List<linesegment> listSeg = new List<linesegment>();
            listSeg.Add(new linesegment(center, segCross1.StartPoint));
            listSeg.Add(new linesegment(center, segCross2.StartPoint));
            listSeg.Add(new linesegment(center, segCross1.EndPoint));
            listSeg.Add(new linesegment(center, segCross2.EndPoint));

            for (int ix = 0; ix < listSeg.Count; ix++)
            {
                linesegment seg1 = listSeg[ix];
                linesegment seg2 = ix >= listSeg.Count - 1 ? listSeg[0] : listSeg[ix + 1];

                bool between = Geometry.Geometry.IsBetween(seg1, seg2, pointOnArc);
                if (between)
                {
                    defPoint.Add(center);
                    defPoint.Add(seg1.EndPoint);
                    defPoint.Add(seg1.EndPoint);
                    defPoint.Add(seg2.EndPoint);
                    break;
                }
            }
            return defPoint;
        }

        public static gPoints GetAnglurDimensionLine(gPoint center, linesegment lineSeg1, linesegment lineSeg2, gPoint pointOnArc)
        {            
            gPoints defPoint = new gPoints();
            gPoints targets = new gPoints();
            targets.Add(lineSeg1.StartPoint);
            targets.Add(lineSeg2.StartPoint);
            targets.Add(lineSeg1.EndPoint);
            targets.Add(lineSeg2.EndPoint);
            
            // 연장선 만들기
            linesegment segCross1;
            linesegment segCross2;
            double radius = center.Distance2D(pointOnArc);
            Line line1 = new Line(lineSeg1.StartPoint, lineSeg1.EndPoint);
            Line line2 = new Line(lineSeg2.StartPoint, lineSeg2.EndPoint);
            Circle circle = new Circle(center, radius);
            int result1 = circle.Intersection(line1, out segCross1);
            int result2 = circle.Intersection(line2, out segCross2);

            List<linesegment> listSeg = new List<linesegment>();
            listSeg.Add(new linesegment(center, segCross1.StartPoint));
            listSeg.Add(new linesegment(center, segCross2.StartPoint));
            listSeg.Add(new linesegment(center, segCross1.EndPoint));
            listSeg.Add(new linesegment(center, segCross2.EndPoint));
            //

            for (int ix = 0; ix < listSeg.Count; ix++)
            {
                linesegment seg1 = listSeg[ix];
                linesegment seg2 = ix >= listSeg.Count - 1 ? listSeg[0] : listSeg[ix + 1];
                
                if (Geometry.Geometry.IsBetween(seg1, seg2, pointOnArc))
                {
                    Vector v1 = new Vector(center, seg1.EndPoint);
                    Vector v2 = new Vector(center, seg2.EndPoint);
                    
                    defPoint.Add(center);
                    
                    gPoints results = Geometry.Geometry.GetEqulsDirectionPoints(center, targets, v1);
                    if (results.Count > 0)
                    {
                        defPoint.Add(results[0]);
                        defPoint.Add(results[0]);
                    }
                    else
                    {
                        defPoint.Add(center + v1);
                        defPoint.Add(center + v1);
                    }
                    
                    results = Geometry.Geometry.GetEqulsDirectionPoints(center, targets, v2);
                    if (results.Count > 0)
                        defPoint.Add(results[0]);
                    else
                        defPoint.Add(center + v2);
                    
                    break;
                }
            }
            return defPoint;
        }
        
        public override string ToString()
        {
            return string.Concat(this.Direction.ToString(), ",", this.Level);
        }

        #region /**** 치수선 옵션 **************************************************/
        [Serializable]
        public class DimensionOptions : ISerializable
        {
            public double ScaleFactor { get; set; }
            public double TextHeight { get; set; }
            public double FirstSpace { get; set; }
            public double DimSpace { get; set; }
            public short DecimalPrecision { get; set; }
            public double ArrowSize { get; set; }
            public double ExtLineDist1 { get; set; }
            public double ExtLineDist2 { get; set; }
            public double TextDist { get; set; }

            public DimensionOptions()
            {

            }

            public DimensionOptions(SerializationInfo info, StreamingContext context)
            {
                int flag = (int)info.GetValue("DimensionOptions.Flag", typeof(int));
                ScaleFactor = (double)info.GetValue("DimensionOptions.ScaleFactor", typeof(double));
                TextHeight = (double)info.GetValue("DimensionOptions.TextHeight", typeof(double));
                FirstSpace = (double)info.GetValue("DimensionOptions.FirstSpace", typeof(double));
                DimSpace = (double)info.GetValue("DimensionOptions.DimSpace", typeof(double));
                DecimalPrecision = (short)info.GetValue("DimensionOptions.DecimalPrecision", typeof(short));
                ArrowSize = (double)info.GetValue("DimensionOptions.ArrowSize", typeof(double));
                ExtLineDist1 = (double)info.GetValue("DimensionOptions.ExtLineDist1", typeof(double));
                ExtLineDist2 = (double)info.GetValue("DimensionOptions.ExtLineDist2", typeof(double));
                TextDist = (double)info.GetValue("DimensionOptions.TextDist", typeof(double));
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                int flag = 1;
                info.AddValue("DimensionOptions.Flag", flag, typeof(int));
                info.AddValue("DimensionOptions.ScaleFactor", ScaleFactor, typeof(double));
                info.AddValue("DimensionOptions.TextHeight", TextHeight, typeof(double));
                info.AddValue("DimensionOptions.FirstSpace", FirstSpace, typeof(double));
                info.AddValue("DimensionOptions.DimSpace", DimSpace, typeof(double));
                info.AddValue("DimensionOptions.ArrowSize", DecimalPrecision, typeof(short));
                info.AddValue("DimensionOptions.DecimalPrecision", ArrowSize, typeof(double));
                info.AddValue("DimensionOptions.ExtLineDist1", ExtLineDist1, typeof(double));
                info.AddValue("DimensionOptions.ExtLineDist2", ExtLineDist2, typeof(double));
                info.AddValue("DimensionOptions.TextDist", TextDist, typeof(double));
            }

            public DimensionOptions(DimensionOptions other)
            {
                SetValue(other);
            }

            public void SetValue(DimensionOptions other)
            {
                ScaleFactor = other.ScaleFactor;
                TextHeight = other.TextHeight;
                FirstSpace = other.FirstSpace;
                DimSpace = other.DimSpace;
                DecimalPrecision = other.DecimalPrecision;
                ArrowSize = other.ArrowSize;
                ExtLineDist1 = other.ExtLineDist1;
                ExtLineDist2 = other.ExtLineDist2;
                TextDist = other.TextDist;
            }

            public void ApplyOptions(vdDimension dimension)
            {
                dimension.ScaleFactor = ScaleFactor;
                dimension.TextHeight = TextHeight;
                dimension.DecimalPrecision = DecimalPrecision;
                dimension.ArrowSize = ArrowSize;
                dimension.ExtLineDist1 = ExtLineDist1;
                dimension.ExtLineDist2 = ExtLineDist2;
                dimension.TextDist = TextDist;
                dimension.Update();
            }

            public void SetDefault()
            {
                ScaleFactor = 50;
                TextHeight = 2.5;
                FirstSpace = 200;
                DimSpace = 150;
                DecimalPrecision = 0;
                ArrowSize = 2.5;
                ExtLineDist1 = 1;
                ExtLineDist2 = 1.5;
                TextDist = 1.0;
            }
            
        }
        #endregion
    }
}
