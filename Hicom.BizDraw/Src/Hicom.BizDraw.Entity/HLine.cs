using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Serialize;
using VectorDraw.Geometry;
using VectorDraw.Render;

namespace Hicom.BizDraw.Entity
{
    public class HLine : vdLine, IEntityBase
    {
        public bool UnRemove { get; set; }
        public object Tag { get; set; }
                
        public HLine() : base()
        {
            this.Init();
        }

        public HLine(vdDocument doc) : base(doc)
        {
            this.Init();
        }

        public HLine(vdDocument doc, gPoint startpoint, gPoint endpoint) : base(doc, startpoint, endpoint)
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
            serializer.Serialize(flag, "HLine.flag");
            serializer.Serialize(this.UnRemove, "HLine.UnRemove");
            serializer.Serialize(this.Tag, "HLine.Tag");
        }

        public override bool DeSerialize(DeSerializer deserializer, string fieldname, object value)
        {
            if (base.DeSerialize(deserializer, fieldname, value)) return true;

            bool ret = true;
            switch (fieldname)
            {
                case "HLine.flag":
                    int flag = (int)value;
                    break;
                case "HLine.UnRemove":
                    this.UnRemove = (bool)value;
                    break;
                case "HLine.Tag":
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
            vdLine vLine = new vdLine(doc, StartPoint, EndPoint);
            vLine.SetUnRegisterDocument(doc);
            vLine.setDocumentDefaults();
            vLine.Layer = this.Layer.Clone(doc) as vdLayer;
            return new List<vdFigure>() { vLine };
        }
        /// <summary>
        /// 객체 선택시 그려주는 함수
        /// </summary>
        /// <param name="index"></param>
        /// <param name="GripBox"></param>
        /// <param name="render"></param>
        protected override void DrawGrip(int index, Box GripBox, vdRender render)
        {
            base.DrawGrip(index, GripBox, render);
        }
    }
}
