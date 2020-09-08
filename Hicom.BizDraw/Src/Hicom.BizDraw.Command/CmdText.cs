using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using VectorDraw.Professional.CommandActions;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.Actions;
using VectorDraw.Geometry;
using VectorDraw.Actions;

using Hicom.BizDraw.Geometry;
using Hicom.BizDraw.PlanDraw;
using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.Command
{
    //Text, MText출력 클래스
    //실제 사용되지 않음 (BizDrawCtrl::CommandText 사용)
    class CmdText : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private vdFigure Figure { get; set; }

        public CmdText(gPoint reference, vdLayout layout, bool isMultiLine, double textSize, double rotation) : base(reference, layout)
        {
            if (isMultiLine)
            {
                if (this.Document.CommandAction.CmdMText(null, rotation, reference, textSize, 0, 0))
                    this.Figure = this.Document.ActiveLayOut.Entities.Last; 
            }
            else
            {
                if (this.Document.CommandAction.CmdText(null, reference, textSize, rotation))
                    this.Figure = this.Document.ActiveLayOut.Entities.Last;
            }   
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {

        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {

        }

        public static List<vdFigure> Run(vdDocument doc, bool isMultiLine = false)
        {
            List<vdFigure> result = new List<vdFigure>();

            doc.Prompt("Insertion point");
            gPoint start = doc.ActionUtility.getUserPoint() as gPoint;
            if (start == null)
                return result;

            double textSize = doc.ActiveTextStyle.Height;
            double rotation = 0;

            doc.ActionUtility.SetAcceptedStringValues(new string[] { "Text Size" }, textSize.ToString());
            doc.Prompt(string.Concat("Text Size(", textSize, ")"));            
            StatusCode sCode = doc.ActionUtility.getUserDouble(out textSize);
            if (textSize.AreEqual(0))
                textSize = doc.ActiveTextStyle.Height;

            doc.ActionUtility.SetAcceptedStringValues(new string[] { "Rotation" }, rotation.ToString());
            doc.Prompt(string.Concat("Rotation(", rotation, ")"));
            sCode = doc.ActionUtility.getUserDouble(out rotation);

            if (textSize > 0)
            {
                doc.Prompt("Input Text.");
                CmdText cmdText = new CmdText(start, doc.ActiveLayOut, isMultiLine, textSize, rotation);
                if(cmdText.Figure != null)
                    result.Add(cmdText.Figure);
            }

            return result;
        }
    }
}
