using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Serialize;
using VectorDraw.Geometry;

namespace Hicom.BizDraw.Entity
{
    public class HPolyline : vdPolyline, IEntityBase
    {
        public bool UnRemove { get; set; }
        public object Tag { get; set; }

        public HPolyline() : base()
        {
            this.Init();
        }

        public HPolyline(vdDocument doc) : base(doc)
        {
            this.Init();
        }
        
        public HPolyline(vdDocument doc, Vertexes vertexlist) : base(doc, vertexlist)
        {
            this.Init();
        }

        public HPolyline(vdDocument doc, gPoints vertexlist) : base(doc, vertexlist)
        {
            this.Init();
        }

        private void Init()
        {
            this.UnRemove = false;
            this.Tag = null;
        }

        public override void Serialize(Serializer serializer)
        {
            base.Serialize(serializer);

            int flag = 1;
            serializer.Serialize(flag, "HPolyline.flag");
            serializer.Serialize(this.UnRemove, "HPolyline.UnRemove");
            serializer.Serialize(this.Tag, "HPolyline.Tag");
        }

        public List<vdFigure> TovdFigure(vdDocument doc)
        {
            vdPolyline vPolyline = new vdPolyline(doc, VertexList);
            vPolyline.Flag = Flag;
            vPolyline.HatchProperties = HatchProperties;
            return new List<vdFigure>() { vPolyline };
        }

        public override bool DeSerialize(DeSerializer deserializer, string fieldname, object value)
        {
            if (base.DeSerialize(deserializer, fieldname, value)) return true;

            bool ret = true;
            switch (fieldname)
            {
                case "HPolyline.flag":
                    int flag = (int)value;
                    break;
                case "HPolyline.UnRemove":
                    this.UnRemove = (bool)value;
                    break;
                case "HPolyline.Tag":
                    this.Tag = value;
                    break;
                default:
                    ret = false;
                    break;
            }

            return ret;
        }
    }
}
