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
    public class HCircle : vdCircle, IEntityBase
    {
        public bool UnRemove { get; set; }
        public object Tag { get; set; }

        public HCircle() : base()
        {
            this.Init();
        }

        public HCircle(vdDocument doc) : base(doc)
        {
            this.Init();
        }

        public HCircle(vdDocument doc, gPoint center, double radius) : base(doc, center, radius)
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
            serializer.Serialize(flag, "HCircle.flag");
            serializer.Serialize(this.UnRemove, "HCircle.UnRemove");
            serializer.Serialize(this.Tag, "HCircle.Tag");
        }

        public override bool DeSerialize(DeSerializer deserializer, string fieldname, object value)
        {
            if (base.DeSerialize(deserializer, fieldname, value)) return true;

            bool ret = true;
            switch (fieldname)
            {
                case "HCircle.flag":
                    int flag = (int)value;
                    break;
                case "HCircle.UnRemove":
                    this.UnRemove = (bool)value;
                    break;
                case "HCircle.Tag":
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
            vdCircle vCircle = new vdCircle(doc, Center, Radius);
            vCircle.Layer = Layer;
            vCircle.HatchProperties = HatchProperties;
            return new List<vdFigure>() { vCircle};
        }      
    }
}
