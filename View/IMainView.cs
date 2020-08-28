using VectordrawTest.Controller;
using VectordrawTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CuttingAlgorithm;

namespace VectordrawTest.View
{
    public interface IMainView
    {
        void InitializeAllModels(VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCutter collisionCalculator, BeamBuilder beamBuilder);
        void AttachController(MainController controller);
    }
}
