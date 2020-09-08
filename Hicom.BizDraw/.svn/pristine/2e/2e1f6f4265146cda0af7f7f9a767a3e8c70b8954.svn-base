using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Geometry;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdCollections;

namespace Hicom.BizDraw.Command
{
    public class CmdSelectionMerge
    {
        private static CmdSelectionMerge _merge = new CmdSelectionMerge();
        public static CmdSelectionMerge SelectionMerge { get { return _merge; } }

        private vdSelection _vdSelection { get; set; }
        private gPoint _origin;
        public List<vdFigure> Figures { get; set; }

        public CmdSelectionMerge()
        {
            _vdSelection = null;
            _origin = null;
            Figures = new List<vdFigure>();
        }

        public void Copy(vdDocument document)
        {
            document.Prompt("Select Entities.");
            vdSelection selection = document.ActionUtility.getUserSelection();
            if (selection != null)
            {
                if (_vdSelection != null && _vdSelection.Count > 0)
                {
                    _vdSelection.RemoveAll();
                }

                _vdSelection = selection;
                document.Prompt("Pick origin point.");
                gPoint point;
                VectorDraw.Actions.StatusCode scode = document.ActionUtility.getUserPoint(out point);
                if (scode == VectorDraw.Actions.StatusCode.Success)
                    _origin = point;
            }
            document.Prompt("Command:");
        }

        public void Merge(vdDocument document)
        {
            if (_vdSelection != null && _origin != null)
            {
                document.Prompt("Pick Paste point.");
                gPoint point;
                VectorDraw.Actions.StatusCode scode = document.ActionUtility.getUserPoint(out point);

                if (scode == VectorDraw.Actions.StatusCode.Success)
                {
                    vdSelection mergeSelection = new vdSelection("mergeSelection");
                    mergeSelection.SetUnRegisterDocument(document);
                    foreach (vdFigure fig in _vdSelection)
                    {
                        mergeSelection.AddItem((vdFigure)fig.Clone(document), false, vdSelection.AddItemCheck.Nochecking);
                    }

                  
                    List<vdFigure> beforeAllEntities = this.GetFigures(document);
                    //document.MergeSelection(mergeSelection);
                    foreach (vdFigure f in mergeSelection)
                    {
                        document.ActiveLayOut.Entities.AddItem(f);
                    }
                    List<vdFigure> afterAllEntities = this.GetFigures(document);
                    this.Figures = afterAllEntities.Where(item => beforeAllEntities.Contains(item) == false).ToList();

                    Vector vec = new Vector(point - _origin);
                    Matrix matrix = new Matrix();
                    matrix.TranslateMatrix(vec);
                    mergeSelection.Transformby(matrix);

                    mergeSelection.RemoveAll();

                    document.Redraw(true);
                }
                document.Prompt("Command:");
            }
        }

        //private void OnAfterAddItem(object obj)
        //{
        //    if(obj is vdFigure)
        //    {
        //        vdFigure fig = obj as vdFigure;
        //        if(Figures.Exists(item => item.HandleId == fig.HandleId) == false)
        //            Figures.Add(obj as vdFigure);
        //    }

        //}

        private List<vdFigure> GetFigures(vdDocument doc)
        {
            List<vdFigure> entities = new List<vdFigure>();
            for (int ix = 0; ix < doc.ActiveLayOut.Entities.Count; ix++)
                entities.Add(doc.ActiveLayOut.Entities[ix]);

            return entities;
        }
    }
}
