using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hicom.BizDraw.Command
{
    public class Command
    {
        /// <summary>
        /// 개발자용 강제 삭제 커멘드
        /// </summary>
        public const string DELETE_DEVELOPER = "@delete_developer";
        /// <summary>
        /// 더블클릭 이벤트 커맨드
        /// </summary>
        public const string DOUBLECLICK = "dbclick";

        public const string NEW = "new";        
        public const string OPEN = "open";
        public const string SAVE = "save";
        public const string SAVEAS = "saveas";
        public const string PRINT = "print";
        public const string IMPORT = "import";
        public const string EXPORT = "export";
        public const string LAYER = "layer";

        public const string MOVE = "move";
        public const string COPY = "copy";
        public const string DELETE = "delete";
        public const string ROTATE = "rotate";
        
        public const string EXTEND = "extend";
        public const string TRIM = "trim";
        public const string EXTEND_TRIM_FENCE = "fence";
        public const string EXTEND_TRIM_CROSS = "cross";
        public const string EXTEND_TRIM_EDGE = "edge";
        public const string EXTEND_TRIM_PROJECT = "project";
        public const string EXTEND_TRIM_PROJECT_NONE = "none";
        public const string EXTEND_TRIM_PROJECT_UCS = "ucs";
        public const string EXTEND_TRIM_PROJECT_VIEW = "veiw";
        public const string EXPLODE = "explode";
        public const string EXPLODEALL = "explodeall";

        public const string BLOCK = "block";
        public const string BLOCK_WRITE = "writeblock";
        public const string INSERT = "insert";

        public const string REDRAW = "redraw";
        public const string REGEN = "regen";

        public const string ZOOM = "zoom";
        public const string ZOOM_IN = "zoomi";
        public const string ZOOM_OUT = "zoomo";
        public const string ZOOM_WINDOW = "zoomw";
        public const string ZOOM_EXTENDS = "zoome";
        public const string ZOOM_PREVIOUS = "zoomp";

        public const string ORTHO = "ortho";

        public const string OSNAP = "osnap";
        public const string OSNAP_WINDOW = "osnapw";
        public const string OSNAP_ALL = "all";
        public const string OSNAP_NON = "non";
        public const string OSNAP_END = "end";
        public const string OSNAP_MID = "mid";
        public const string OSNAP_CEN = "cen";
        public const string OSNAP_NEA = "near";
        public const string OSNAP_PER = "perpendicular";
        public const string OSNAP_INTERS = "intersection";
        public const string OSNAP_EXTENSION = "extension";

        public const string DIST = "distance";
        public const string DIST_MULTI = "multi";
        public const string DIST_LINE = "line";
        public const string DIST_ARC = "arc";

        public const string LINE = "line";
        public const string CIRCLE = "circle";
        public const string CIRCLE_CEN = "circlecen";
        public const string CIRCLE_3P = "circle3p";
        public const string CIRCLE_2P = "circle2p";
        public const string ARC = "arc";
        public const string ARC_3P = "arc3p";
        public const string ARC_3PC = "arc3pc";
        public const string ARC_2PA = "arc2pa";
        public const string RECTANG = "rectang";
        public const string POLYLINE = "polyline";
        public const string POLYLINE_CLOSE = "close";
        public const string LEADER = "leader";
        public const string LEADER_ANNOTATION = "annotation";
        public const string TEXT = "text";
        public const string MTEXT = "mtext";
        public const string TEXT_STYLE_DLG = "textstyle";

        public const string DIM_ALIGNED = "dimension";
        public const string DIM_VERTICAL = "dimver";
        public const string DIM_HORIZONTAL = "dimhor";
        public const string DIM_ANGULAR = "dimangular";
        public const string DIM_SCALE = "dimscale";
        public const string DIM_OPTIONS = "dimoptions";

        public const string PROPERTY = "property";

        public const string XCOPY = "XCopy";
        public const string XMerge = "XMerge";

        
    }
}
