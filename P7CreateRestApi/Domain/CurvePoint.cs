using System;

namespace Dot.Net.WebApi.Domain
{
    public class CurvePoint
    {
        // TODO: Map columns in data table CURVEPOINT with corresponding fields

        public int Id;
        public byte? CurveId;
        public DateTime? AsOfDate;
        public double? Term;
        public double? CurvePointValue;
        public DateTime? CreationDate;
    }
}