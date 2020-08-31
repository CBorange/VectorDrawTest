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
using VectordrawTest.Model.CustomFigure;

namespace VectordrawTest.Model.CuttingAlgorithm
{
    public interface IUpCuttingAlgorithm
    {
        CuttingResult CalcAlgorithm_CuttingRect(Beam cuttedBeam, Beam upBeam);
    }
}
