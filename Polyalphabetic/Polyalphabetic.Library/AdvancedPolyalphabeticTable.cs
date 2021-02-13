using System;
using System.Text;
using System.Linq;

namespace Polyalphabetic.Library
{
    public class AdvancedPolyalphabeticTable
    {
        public string RawStringRow { get; }

        public byte[][] Table => Create();

        public static AdvancedPolyalphabeticTable operator 
            +(AdvancedPolyalphabeticTable tbl1, AdvancedPolyalphabeticTable tbl2)
        {
            var fullString = new StringBuilder(tbl1.RawStringRow.Length + tbl2.RawStringRow.Length);
            fullString.Append(tbl1.RawStringRow);
            fullString.Append(tbl2.RawStringRow);

            return new AdvancedPolyalphabeticTable(fullString.ToString());
        }

        public AdvancedPolyalphabeticTable(string rawStringRow)
        {
            RawStringRow = rawStringRow;
        }

        internal bool VerifyString(string data) => data.All(c => RawStringRow.Contains(c) || char.IsWhiteSpace(c));

        private byte[][] Create()
        {
            var tbl = new byte[RawStringRow.Length][];
            var row = Encoding.Default.GetBytes(RawStringRow);

            for (var i = 0; i < RawStringRow.Length; i++)
            {
                var pushedRow = new byte[row.Length];
                Buffer.BlockCopy(row, 0, pushedRow, 0, row.Length);

                if (i == 0)
                {
                    pushedRow = ShiftOne(pushedRow);
                    tbl[i] = pushedRow;
                    continue;
                }

                for (var j = 0; j < i + 1; j++)
                {
                    pushedRow = ShiftOne(pushedRow);
                    tbl[i] = pushedRow;
                }
            }

            return tbl;
        }

        private byte[] ShiftOne(byte[] orig)
        {
            byte[] bytes = new byte[orig.Length];
            Buffer.BlockCopy(orig, 0, bytes, 0, orig.Length);

            var first = bytes[0];
            for (var i = 0; i < bytes.Length - 1; i++)
                orig[i] = bytes[i + 1];

            orig[orig.Length - 1] = first;
            return orig;
        }

        public override string ToString()
        {
            byte[][] tbl = Create();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("plain\t");

            foreach (char c in RawStringRow)
                stringBuilder.Append(c + " ");

            stringBuilder.AppendLine();

            var curLen = stringBuilder.Length;
            for (var i = 0; i < curLen; i++)
                stringBuilder.Append("_");

            stringBuilder.AppendLine();

            for (var i = 0; i < tbl.Length; i++)
            {
                stringBuilder.Append(i + "\t");
                for (var j = 0; j < tbl[i].Length; j++)
                    stringBuilder.Append((char)(tbl[i][j]) + " ");
                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
