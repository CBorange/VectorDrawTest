using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Geometry;
namespace Hicom.BizDraw.Command
{
    /// <summary>
    /// Vector Draw Read용으로만 사용
    /// 추후 이용 가능성을 위해 각각의 함수 생성
    /// </summary>
    public class BizCommand
    {
        //File
        public static void CmdOpen(vdDocument doc) { }
        public static void CmdSave(vdDocument doc) { }
        public static void CmdExport(vdDocument doc) { }
        public static void CmdImport(vdDocument doc) { }

        // Draw
        public static void CmdLine(vdDocument doc) { }
        public static void CmdCircle(vdDocument doc) { }
        public static void CmdArc(vdDocument doc) { }
        public static void CmdRect(vdDocument doc) { }
        public static void CmdPolyLine(vdDocument doc) { }
        public static void CmdLeader(vdDocument doc) { }
        public static void CmdText(vdDocument doc) { }
        public static void CmdDim(vdDocument doc) { }
        public static void CmdDimScale(vdDocument doc) { }
        public static void CmdDimOptions(vdDocument doc) { }
        public static void CmdLineWidth(vdDocument doc) { }

        // ETC
        public static void CmdOrtho(vdDocument doc) { }
        //Edit
        public static void CmdOsnap(vdDocument doc) { }
        public static void CmdOsnapW(vdDocument doc) { }
        public static void CmdDistance(vdDocument doc) { }

        //Modify
        public static void CmdMove(vdDocument doc) { }
        public static void CmdCopy(vdDocument doc) { }
        public static void CmdDelete(vdDocument doc) { }
        public static void CmdRotate(vdDocument doc) { }
        public static void CmdExtendTrim(vdDocument doc) { }
        public static void CmdExplode(vdDocument doc) { }

        public static void CmdBlock(vdDocument doc) { }
        public static void CmdWirteBlock(vdDocument doc) { }
        public static void CmdInsert(vdDocument doc) { }

        // Draw Ctrl
        public static void CmdProperty(vdDocument doc) { }        

        public static void CmdXCopy(vdDocument doc)
        {
            CmdSelectionMerge.SelectionMerge.Copy(doc);
        }

        public static void CmdXMerge(vdDocument doc)
        {
            CmdSelectionMerge.SelectionMerge.Merge(doc);
        }
    }
}
