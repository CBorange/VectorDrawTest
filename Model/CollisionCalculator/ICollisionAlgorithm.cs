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
using System.Drawing;
using MathPractice.Model.CustomFigure;

namespace MathPractice.Model.CollisionCalculator
{
    public interface ICollisionAlgorithm
    {
        void CalcAlgorithm_CuttingRect(Beam verBeam, Beam horBeam);
    }
}
