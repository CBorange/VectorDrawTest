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
    public class HRect : vdRect, IEntityBase
    {
        public bool UnRemove { get; set; }
        public object Tag { get; set; }

        public HRect() : base()
        {
            this.Init();
        }

        public HRect(vdDocument doc) : base(doc)
        {
            this.Init();
        }

        public HRect(vdDocument doc, gPoint insertionpoint, double width, double height, double rotation) : base(doc, insertionpoint, width, height, rotation)
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
            serializer.Serialize(flag, "HRect.flag");
            serializer.Serialize(this.UnRemove, "HRect.UnRemove");
            serializer.Serialize(this.Tag, "HRect.Tag");
        }
        
        public List<vdFigure> TovdFigure(vdDocument doc)
        {
            vdRect vRect = new vdRect(doc, InsertionPoint, Width, Height, Rotation);
            vRect.Layer = Layer;
            return new List<vdFigure>() { vRect };
        }

        public override bool DeSerialize(DeSerializer deserializer, string fieldname, object value)
        {
            if (base.DeSerialize(deserializer, fieldname, value)) return true;

            bool ret = true;
            switch (fieldname)
            {
                case "HRect.flag":
                    int flag = (int)value;
                    break;
                case "HRect.UnRemove":
                    this.UnRemove = (bool)value;
                    break;
                case "HRect.Tag":
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
