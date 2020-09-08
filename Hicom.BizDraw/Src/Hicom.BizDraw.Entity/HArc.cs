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
    public class HArc : vdArc, IEntityBase
    {
        public bool UnRemove { get; set; }
        public object Tag { get; set; }

        public HArc() : base()
        {
            this.Init();
        }

        public HArc(vdDocument doc) : base(doc)
        {
            this.Init();
        }

        public HArc(vdDocument doc, gPoint center, double radius, double startAngle, double endAngle) : base(doc, center, radius, startAngle, endAngle)
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
            serializer.Serialize(flag, "HArc.flag");
            serializer.Serialize(this.UnRemove, "HArc.UnRemove");
            serializer.Serialize(this.Tag, "HArc.Tag");
        }

        public override bool DeSerialize(DeSerializer deserializer, string fieldname, object value)
        {
            if (base.DeSerialize(deserializer, fieldname, value)) return true;

            bool ret = true;
            switch (fieldname)
            {
                case "HArc.flag":
                    int flag = (int)value;
                    break;
                case "HArc.UnRemove":
                    this.UnRemove = (bool)value;
                    break;
                case "HArc.Tag":
                    this.Tag = value;
                    break;
                default:
                    ret = false;
                    break;
            }

            return ret;
        }

        public List<vdFigure> TovdFigure(vdDocument doc)
        {
            vdArc vArc = new vdArc(doc, Center, Radius, StartAngle, EndAngle);
            vArc.Layer = Layer;
            vArc.HatchProperties = HatchProperties;
            return new List<vdFigure>() { vArc };
        }
    }
}
