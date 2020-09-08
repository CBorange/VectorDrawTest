using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Actions;
using VectorDraw.Generics;

using Hicom.BizDraw.Entity;

namespace Hicom.BizDraw.PlanDraw
{
    static public class Converter
    {
        static public vdDocument ToVDF(vdDocument doc)
        {
            MemoryStream stream = doc.ToStream();
            VectorDraw.Professional.Components.vdDocumentComponent comp = new VectorDraw.Professional.Components.vdDocumentComponent();
            comp.Document.LoadFromMemory(stream);

            ToVDF(comp.Document.ActiveLayOut);

            return comp.Document;
        }
        static public void ToVDF(vdLayout layout)
        {
            for(int ix = 0; ix < layout.Entities.Count; ix++)
            {
                vdFigure figure = layout.Entities[ix];
                
                if (figure is IEntityBase && figure.Deleted == false && figure.visibility == vdFigure.VisibilityEnum.Visible)
                {
                    IEntityBase entBase = figure as IEntityBase;
                    List<vdFigure> vFigure = entBase.TovdFigure(layout.Document);

                    figure.Deleted = true;
                    figure.Update();
                    layout.Entities.AddItems(vFigure.ToArray());
                }
            }
        }
    }
}
