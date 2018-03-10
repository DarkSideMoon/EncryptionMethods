using System;

namespace MD5.Library
{
    public class TransformNumber
    {
        public uint NumberK { get; set; }
        public ushort NumberS { get; set; }
        public uint NumberI { get; set; }

        public TransformNumber(uint k, ushort s, uint i)
        {
            NumberK = k;
            NumberS = s;
            NumberI = i;
        }
    }
}
