using MathPractice.Controller;
using MathPractice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPractice.View
{
    public interface IMainView
    {
        void InitializeAllModels(VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCollisionCalculator collisionCalculator, BeamBuilder beamBuilder);
        void AttachController(MainController controller);
    }
}
