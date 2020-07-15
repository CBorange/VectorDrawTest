using MathPractice.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathPractice.Controller;
using MathPractice.Model;

namespace MathPractice.Controller
{
    public class MainController
    {
        // View
        private IMainView mainView;

        // Models
        private VectorDrawConfigure drawConfigure;
        private BeamManager beamManager;
        private BeamCollisionCalculator collisionCalculator;
        private BeamBuilder beamBuilder;

        public MainController(IMainView mainView, VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCollisionCalculator collisionCalculator, BeamBuilder beamBuilder)
        {
            this.mainView = mainView;
            this.drawConfigure = drawConfigure;
            this.beamManager = beamManager;
            this.collisionCalculator = collisionCalculator;
            this.beamBuilder = beamBuilder;
        }

        public void RotateBeam_Plus()
        {
            beamManager.RotateBeam(5);
        }
        public void RotateBeam_Minus()
        {
            beamManager.RotateBeam(-5);
        }
        public void CuttingBeam_HorizontalUp()
        {
            beamManager.CuttingBeam_HorizontalUp();
        }
        public void CreateNewHorBeam()
        {

        }
        public void CreateNewVerBeam()
        {

        }
    }
}
