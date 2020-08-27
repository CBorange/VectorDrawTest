using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using System.Drawing;
using MathPractice.Model.CustomFigure;

namespace MathPractice.Model.CuttingAlgorithm
{
    public class CuttingResult
    {
        public CuttingResult()
        {
            CutAngle = 0;
            ExtendLength = 0;
            FirstCutPoint = null;
            SecondCutPoint = null;
            ExceptionShape = false;
        }
        public double CutAngle;
        public double ExtendLength;
        public gPoint FirstCutPoint;
        public gPoint SecondCutPoint;
        public bool ExceptionShape;
    }
    public interface IUpCuttingAlgorithm
    {
        CuttingResult CalcAlgorithm_CuttingRect(Beam cuttedBeam, Beam upBeam);
    }
}
