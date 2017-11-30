using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.Web.Common.Gps
{
    public interface IProjection
    {
        double ScaleFactor { get; }
        double TrueOriginLat { get; }
        double TrueOriginLon { get; }
        double TrueOriginEasting { get; }
        double TrueOriginNorthing { get; }
    }
}
