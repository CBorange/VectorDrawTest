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
using MathPractice.Model.Manager;

namespace MathPractice.Model.CollisionCalculator
{
    public class BeamCollisionCalculator
    {
        private MathSupporter math;
        private vdDocument document;
        private BeamManager beamManager;
        private ICollisionAlgorithm crossAlgorithm;
        private ICollisionAlgorithm rotationAlgorithm;

        public BeamCollisionCalculator()
        {
            crossAlgorithm = new CrossAlgorithm();
            rotationAlgorithm = new RotationAlgorithm();
        }
        public void Initialize(vdDocument document, BeamManager beamManager)
        {
            this.document = document;
            math = MathSupporter.Instance;
            this.beamManager = beamManager;
        }
        public void CalcCuttingRect_RotationAlgorithm(Beam verBeam, Beam horBeam)
        {
            rotationAlgorithm.CalcAlgorithm_CuttingRect(verBeam, horBeam);
        }
        public void CalcCuttingRect_CrossAlgorithm(Beam verBeam, Beam horBeam)
        {
            crossAlgorithm.CalcAlgorithm_CuttingRect(verBeam, horBeam);
        }
        public void CollisionCheck(Beam horBeam)
        {
            horBeam.RemoveAllCalcTarget();
            for (int i = 0; i < beamManager.VerBeams.Count; ++i)
            {
                if (math.OBBCollision(horBeam, beamManager.VerBeams[i]))
                {
                    horBeam.CalcTargetBeams.Add(beamManager.VerBeams[i]);
                }
            }
        }
    }
}
