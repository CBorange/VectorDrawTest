using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Geometry;
using VectorDraw.Actions;

namespace Hicom.BizDraw.PlanDraw
{
    public abstract class DrawBase
    {
        protected vdDocument Document { get; set; }
        private vdLayer activeLayer;
        protected vdLayer ActiveLayer
        {
            get
            {
                if (this.activeLayer == null)
                    return this.Document.ActiveLayer;
                return this.activeLayer;
            }
            set
            {
                this.activeLayer = value;
            }
        }
        public bool AddEntity { get; set; }
        public DrawBase(vdDocument doc)
        {
            AddEntity = true;
            this.Document = doc;
        }

        protected void AddItem(object value)
        {
            if(AddEntity)
                this.Document.ActiveLayOut.Entities.AddItem(value);
        }

        public void SetDocument(vdDocument doc)
        {
            this.Document = doc;
        }

        public void SetActiveLayer(string layerName)
        {
            if(this.Document != null)
            {
                vdLayer layer = this.Document.Layers.FindName(layerName);
                if (layer != null)
                    this.ActiveLayer = layer;
                else
                    this.ActiveLayer = this.Document.Layers[0];
            }
        }

        public void SetActiveLayer(vdLayer layer)
        {
            this.ActiveLayer = layer;
        }

        public override string ToString()
        {
            return this.Document != null ? this.Document.ActiveLayOut.Name : string.Empty;
        }
    }
}
