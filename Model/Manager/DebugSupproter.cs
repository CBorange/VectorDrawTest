﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdCommandLine;
using System.Windows.Forms;


namespace MathPractice.Model.Manager
{
    public class DebugSupproter
    {
        private vdDocument document;
        private List<vdCircle> debugCircles;
        private List<vdText> debugCircleTexts;

        public DebugSupproter(vdDocument document)
        {
            this.document = document;
            debugCircles = new List<vdCircle>();
            debugCircleTexts = new List<vdText>();

        }
    }
}
