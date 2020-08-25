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
            LeftCutAngle = 0;
            RightCutAngle = 0;
            LeftExtendLength = 0;
            RightExtendLength = 0;
        }
        public double LeftCutAngle;
        public double RightCutAngle;
        public double LeftExtendLength;
        public double RightExtendLength;
    }
    public interface IUpCuttingAlgorithm
    {
        CuttingResult CalcAlgorithm_CuttingRect(Beam cuttedBeam, Beam upBeam);
    }
}
