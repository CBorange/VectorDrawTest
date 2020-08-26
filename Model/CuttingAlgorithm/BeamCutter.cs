﻿using System;
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
using MathPractice.Model.Manager;

namespace MathPractice.Model.CuttingAlgorithm
{
    public class BeamCutter
    {
        private MathSupporter math;
        private vdDocument document;
        private BeamManager beamManager;
        private IUpCuttingAlgorithm crossAlgorithm;

        public BeamCutter()
        {
            crossAlgorithm = new UpCutting_Cross();
        }
        public void Initialize(vdDocument document, BeamManager beamManager)
        {
            this.document = document;
            math = MathSupporter.Instance;
            this.beamManager = beamManager;
        }
        public void CalcCuttingRect_CrossAlgorithm(Beam upBeam, Beam cuttedBeam)
        {
            crossAlgorithm.CalcAlgorithm_CuttingRect(upBeam, cuttedBeam);
        }
        public void CheckCollisionHorToVer(Beam horBeam)
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
        public void CheckCollisionVerToHor(Beam verBeam)
        {
            verBeam.RemoveAllCalcTarget();
            for (int i = 0; i < beamManager.HorBeams.Count; ++i)
            {
                if (math.OBBCollision(verBeam, beamManager.HorBeams[i]))
                {
                    verBeam.CalcTargetBeams.Add(beamManager.HorBeams[i]);
                }
            }
        }
    }
}