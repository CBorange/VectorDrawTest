using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace Hicom.BizDraw.Command
{
    public class CmdLeader : ActionBase
    {
        public override vdFigure Entity { get { return Figure; } }
        private vdLeader Figure { get; set; }
        
        public CmdLeader(gPoint reference, vdLayout layout, DrawDimension.DimensionOptions opt) : base(reference, layout)
        {
            this.Figure = new vdLeader();
            this.Figure.SetUnRegisterDocument(layout.Document);
            this.Figure.setDocumentDefaults();
            this.Figure.Layer = this.Document.ActiveLayer;
            this.Figure.VertexList.Add(new gPoint(reference));
            
            this.Figure.ArrowSize = opt.ArrowSize;
            this.Figure.ScaleOverall = opt.ScaleFactor;
        }

        protected override void OnMyPositionChanged(gPoint newPosition)
        {
            
        }

        public override void DrawReference(gPoint orthoPoint, ActionWrapperRender render)
        {
            
        }

        public static List<vdFigure> Run(vdDocument doc, DrawDimension.DimensionOptions opt)
        {
            List<vdFigure> result = new List<vdFigure>();

            // 지시선 입력
            doc.Prompt("Pick start point");
            gPoint start = doc.ActionUtility.getUserPoint() as gPoint;
            if (start == null)
                return result;

            CmdLeader cmdLeader = new CmdLeader(start, doc.ActiveLayOut, opt);
            StatusCode sCode = cmdLeader.WaitToFinish("Pick Next point", valueType.REFPOINT, true);
            if (sCode == StatusCode.Success)
            {
                gPoint point = cmdLeader.Value as gPoint;
                cmdLeader.Figure.VertexList.Add(point);
                
                while (sCode == StatusCode.Success)
                {
                    string annot = string.Format("{0};a;", Command.LEADER_ANNOTATION);
                    cmdLeader.SetAcceptedStringValues(new string[] { annot }, null);
                    sCode = cmdLeader.WaitToFinish("Pick Next point[Annotation(a)]", valueType.REFPOINT | valueType.STRING, true);
                    if (sCode == StatusCode.Success)
                    {
                        if (cmdLeader.Value is string)
                        {
                            string command = cmdLeader.Value.ToString().ToLower();
                            if (command.Equals(Command.LEADER_ANNOTATION, StringComparison.OrdinalIgnoreCase))
                                sCode = StatusCode.Cancel;
                        }
                        else if (cmdLeader.Value is gPoint)
                        {
                            point = cmdLeader.Value as gPoint;
                            cmdLeader.Figure.VertexList.Add(point);
                        }
                    }
                }

                doc.ActionLayout.Entities.AddItem(cmdLeader.Entity);
                doc.ActionDrawFigure(cmdLeader.Entity);
                result.Add(cmdLeader.Entity);

                // 텍스트 입력
                vdMText vmtext = null;
                string inputStr = string.Empty;
                StringBuilder inputSB = new StringBuilder();
                do
                {
                    doc.Prompt("Input TEXT Line");
                    inputStr = doc.ActionUtility.getUserString();
                    if (string.IsNullOrEmpty(inputStr))
                    {
                        string textString = inputSB.ToString();
                        if (!string.IsNullOrEmpty(textString))
                        {
                            gPoint insertPoint = cmdLeader.Figure.VertexList.Last().AsgPoint();

                            vmtext = new vdMText();
                            vmtext.SetUnRegisterDocument(doc);
                            vmtext.setDocumentDefaults();
                            vmtext.InsertionPoint = new gPoint(insertPoint);
                            vmtext.TextString = textString;
                            vmtext.VerJustify = VdConstVerJust.VdTextVerBottom;
                            vmtext.HorJustify = VdConstHorJust.VdTextHorLeft;
                            vmtext.Height = opt.TextHeight * opt.ScaleFactor; // Leader는 축적 적용을 수동으로 해줘야함
                            vmtext.Update();

                            doc.ActionLayout.Entities.AddItem(vmtext);
                            doc.ActionDrawFigure(vmtext);
                        }
                    }
                    else
                    {
                        inputSB.AppendLine(inputStr);
                    }
                } while (!string.IsNullOrEmpty(inputStr));
                
                cmdLeader.Figure.LeaderMText = vmtext;
                cmdLeader.Figure.Invalidate();
                cmdLeader.Figure.Update();
            }
            
            return result;
        }
    }
}
