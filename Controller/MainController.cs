using VectordrawTest.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectordrawTest.Controller;
using VectordrawTest.Model;
using VectorDraw.Geometry;
using VectordrawTest.Model.Manager;
using VectordrawTest.Model.CuttingAlgorithm;
using VectordrawTest.Model.CustomFigure;

namespace VectordrawTest.Controller
{
    public class MainController
    {
        // View
        private IMainView mainView;

        // Models
        private VectorDrawConfigure drawConfigure;
        private BeamManager beamManager;
        private BeamCutter collisionCalculator;
        private BeamBuilder beamBuilder;

        public MainController(IMainView mainView, VectorDrawConfigure drawConfigure, BeamManager beamManager,
            BeamCutter collisionCalculator, BeamBuilder beamBuilder)
        {
            this.mainView = mainView;
            this.drawConfigure = drawConfigure;
            this.beamManager = beamManager;
            this.collisionCalculator = collisionCalculator;
            this.beamBuilder = beamBuilder;
        }

        public void UpBeam_HorizontalUp()
        {
            beamManager.CuttingBeam_HorizontalUp();
        }
        public void UpBeam_Vertical()
        {
            beamManager.CuttingBeam_VerticalUp();
        }
        public void CuttingBeam_Midline()
        {
            beamManager.CuttingBeam_MidlineCutting();
        }
        public void CreateNewHorBeam(gPoint point)
        {
            Beam newBeam = beamBuilder.CreateHorBeam(point, 0);
            beamManager.AddNewHorBeam(newBeam);
        }
        public void CreateNewVerBeam(gPoint point)
        {
            Beam newBeam = beamBuilder.CreateVerBeam(point, 90);
            beamManager.AddNewVerBeam(newBeam);
        }
        
    }
}
