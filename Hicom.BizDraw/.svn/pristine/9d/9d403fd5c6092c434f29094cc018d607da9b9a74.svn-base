using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Serialize;
using VectorDraw.Professional.vdObjects;

namespace Hicom.BizDraw.Entity
{
    public interface IEntityBase : IvdProxyFigure
    {
        bool UnRemove { get; set; }
        object Tag { get; set; }

        List<vdFigure> TovdFigure(vdDocument doc);
    }
}
