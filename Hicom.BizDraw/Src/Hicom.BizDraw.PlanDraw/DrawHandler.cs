using System.Linq;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdCollections;

namespace Hicom.BizDraw.PlanDraw
{
    public class DrawHandler
    {
        public vdDocument Document { get { return this.document; } }
        private vdDocument document;

        private DrawDimension drawDimension;
        public DrawDimension Dimension { get { return this.drawDimension; } }

        private DrawObject drawObject;
        public DrawObject Object { get { return this.drawObject; } }

        private DrawText drawText;
        public DrawText Text { get { return this.drawText; } }

        public DrawHandler(vdDocument doc)
        {
            this.document = doc;
            this.drawDimension = new DrawDimension(this.Document);
            this.drawObject = new DrawObject(this.Document);
            this.drawText = new DrawText(this.Document);
        }

        public void Initialize(vdDocument doc)
        {
            this.document = doc;
            this.drawDimension.Initialize();
            this.drawDimension.SetDocument(doc);
            this.drawObject.Initialize();
            this.drawObject.SetDocument(doc);
            this.drawText.Initialize();
            this.drawText.SetDocument(doc);
        }
    
        public void Redraw()
        {
            this.Document.Redraw(true);
        }

        public vdFigure FindID(int id)
        {
            return this.Document.ActiveLayOut.Entities.FindFromId(id);
        } 

        public vdFigure FindHandleId(ulong id)
        {
            //List<vdFigure> list1 = this.Document.ActiveLayOut.Entities.Cast<vdFigure>().ToList();
            //*********************************서로 Count가 다름 NULL 제외 필요*********************************
            //vdArray<vdFigure> list2 = Document.ActiveLayOut.Entities.ArrayItems;        // Count: 3
            //vdFigure[] list3 = Document.ActiveLayOut.Entities.ArrayItems.ArrayItems;    // Count: 8
            //*********************************서로 Count가 다름 NULL 제외 필요*********************************

            return Document.ActiveLayOut.Entities.ArrayItems.ArrayItems.Where(t => t?.HandleId == id).FirstOrDefault();  //[] 항목중 NULL 나올수 있음으로 NULL처리
            //return Document.ActiveLayOut.Entities.ArrayItems.ArrayItems.Where(t => t.HandleId == id).FirstOrDefault();  //속도 측정:00:00:16.2378359

            //return Document.ActiveLayOut.Entities.ArrayItems.ArrayItems.ToList().Find(item => item.HandleId == id);     //속도 측정:00:00:21.7851872
            //속도 저하 문제위치
            //List<vdFigure> list = this.Document.ActiveLayOut.Entities.Cast<vdFigure>().ToList();
            //return list.Find(item => item.HandleId == id);
        }

        public void SetDocument(vdDocument doc)
        {
            this.document = doc;
            this.drawObject.SetDocument(doc);
            this.drawDimension.SetDocument(doc);
            this.drawText.SetDocument(doc);
        }

        /// <summary>
        /// 다른 vdDocument에서 생성된 entities를 현재 vdDocument에 추가한다.
        /// </summary>
        /// <param name="selection"></param>
        public void InsertEntities(vdSelection selection)
        {

        }
        
        public override string ToString()
        {
            return this.Document != null ? this.Document.ActiveLayOut.Name : string.Empty;
        }

        public void ClearUnusedItems(string prefix)
        {
            // 블럭 데이터 삭제
            vdBlocks blocks = Document.GetUsedBlocks();

            for(int loop = Document.Blocks.Count-1; loop >= 0; loop--)
            {
                if(Document.Blocks[loop].Name.Contains(prefix) && blocks.FindName(Document.Blocks[loop].Name) == null)
                {
                    Document.Blocks.RemoveItem(Document.Blocks[loop]);
                }
            }

            Document.ClearEraseItems();
        }
    }
}
