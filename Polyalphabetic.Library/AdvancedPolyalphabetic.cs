using System;
using System.Text;
using System.Linq;

namespace Polyalphabetic.Library
{
    #region Algorithm
    /*
     * Step1:
     * 
     * Step2:
     * 
     * Step3:
     * 
    */
    #endregion

    /// <summary>
    /// Advanced implementation of polyalphabetic cipher
    /// </summary>
    public class AdvancedPolyalphabetic
    {
        [Flags]
        public enum LegalCharacters : byte
        {
            Alphabetical = 0x0,
            Alphanumerical = 0x1,
            Symbols = 0x2
        }

        private const string Alphabetical = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Alphanumerical = "0123456789";
        private const string Symbols = "!\"#¤%&/()=?@£$€{[]}:^´`½§_,.";

        private AdvancedPolyalphabeticTable CompleteTable { get; set; }
        private string KeyPhrase { get; set; }

        public AdvancedPolyalphabetic(LegalCharacters chars, string keyPhrase)
        {
            KeyPhrase = keyPhrase.Replace(" ", "");
            CompleteTable = new AdvancedPolyalphabeticTable("");

            if ((chars & LegalCharacters.Alphabetical) == LegalCharacters.Alphabetical)
                CompleteTable += new AdvancedPolyalphabeticTable(Alphabetical);

            if ((chars & LegalCharacters.Alphanumerical) == LegalCharacters.Alphanumerical)
                CompleteTable += new AdvancedPolyalphabeticTable(Alphanumerical);

            if ((chars & LegalCharacters.Symbols) == LegalCharacters.Symbols)
                CompleteTable += new AdvancedPolyalphabeticTable(Symbols);
        }

        private string ExtendKeyPhrase(int total)
        {
            var extended = new StringBuilder(total);
            var i = KeyPhrase.Length;

            extended.Append(KeyPhrase);
            while ((i += KeyPhrase.Length) < total)
                extended.Append(KeyPhrase);

            for (var j = 0; j < total - (i - KeyPhrase.Length); j++)
                extended.Append(KeyPhrase[j]);

            return extended.ToString();
        }

        public string Encode(string data)
        {
            var extKey = ExtendKeyPhrase(data.Length);
            var encrypted = new byte[data.Length];

            if (!CompleteTable.VerifyString(data))
                throw new ArgumentException("Plaintext data contains characters that does not exist in the cipher table");

            if (!CompleteTable.VerifyString(KeyPhrase))
                throw new ArgumentException("Key data contains characters that does not exist in the cipher table");

            var tbl = CompleteTable.Table;
            for (var i = 0; i < data.Length; i++)
            {
                if (char.IsWhiteSpace(data[i]))
                {
                    encrypted[i] = (byte)' ';
                    continue;
                }

                int plainPos = 0;

                for (var j = 0; j < CompleteTable.RawStringRow.Length; j++)
                    if (data[i] == CompleteTable.RawStringRow[j])
                    {
                        plainPos = j;
                        break;
                    }

                foreach (var row in tbl.Where(row => row[0] == extKey[i]))
                {
                    encrypted[i] = row[plainPos];
                    break;
                }
            }

            return Encoding.Default.GetString(encrypted);
        }

        public string Decode(string encryptedData)
        {
            var extKey = ExtendKeyPhrase(encryptedData.Length);
            var decrypted = new byte[encryptedData.Length];
            var tbl = CompleteTable.Table;

            for (var i = 0; i < encryptedData.Length; i++)
            {
                if (char.IsWhiteSpace(encryptedData[i]))
                {
                    decrypted[i] = (byte)' ';
                    continue;
                }

                for (var j = 0; j < tbl.Length; j++)
                {
                    if (tbl[j][0] != extKey[i]) continue;
                    for (var x = 0; x < tbl[j].Length; x++)
                        if (tbl[j][x] == encryptedData[i])
                            decrypted[i] = (byte)CompleteTable.RawStringRow[x];
                }
            }

            return Encoding.Default.GetString(decrypted);
        }
    }
}
