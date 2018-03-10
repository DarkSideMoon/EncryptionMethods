using System;
using System.Collections.Generic;
using System.Text;

namespace MD5.Library
{
    /// <summary>
    /// Summary description for MD5.
    /// </summary>
    public class MD5
    {
        /// <summary>
        /// lookup table 4294967296*sin(i)
        /// </summary>
        protected readonly static uint[] T = new uint[64]
            {   0xd76aa478,0xe8c7b756,0x242070db,0xc1bdceee,
                0xf57c0faf,0x4787c62a,0xa8304613,0xfd469501,
                0x698098d8,0x8b44f7af,0xffff5bb1,0x895cd7be,
                0x6b901122,0xfd987193,0xa679438e,0x49b40821,
                0xf61e2562,0xc040b340,0x265e5a51,0xe9b6c7aa,
                0xd62f105d,0x2441453,0xd8a1e681,0xe7d3fbc8,
                0x21e1cde6,0xc33707d6,0xf4d50d87,0x455a14ed,
                0xa9e3e905,0xfcefa3f8,0x676f02d9,0x8d2a4c8a,
                0xfffa3942,0x8771f681,0x6d9d6122,0xfde5380c,
                0xa4beea44,0x4bdecfa9,0xf6bb4b60,0xbebfbc70,
                0x289b7ec6,0xeaa127fa,0xd4ef3085,0x4881d05,
                0xd9d4d039,0xe6db99e5,0x1fa27cf8,0xc4ac5665,
                0xf4292244,0x432aff97,0xab9423a7,0xfc93a039,
                0x655b59c3,0x8f0ccc92,0xffeff47d,0x85845dd1,
                0x6fa87e4f,0xfe2ce6e0,0xa3014314,0x4e0811a1,
                0xf7537e82,0xbd3af235,0x2ad7d2bb,0xeb86d391
            };

        /// <summary>
        /// X used to proces data in 
        ///	512 bits chunks as 16 32 bit word
        /// </summary>
        protected uint[] X = new uint[16];

        /// <summary>
        /// the finger print obtained. 
        /// </summary>
        protected Digest dgFingerPrint;

        /// <summary>
        /// the input bytes
        /// </summary>
        protected byte[] m_byteInput;


        /// <summary>
        ///gets or sets as string
        /// </summary>
        public string Value
        {
            get
            {
                string st;
                char[] tempCharArray = new Char[m_byteInput.Length];

                for (int i = 0; i < m_byteInput.Length; i++)
                    tempCharArray[i] = (char)m_byteInput[i];

                st = new String(tempCharArray);
                return st;
            }
            set
            {
                m_byteInput = new byte[value.Length];
                for (int i = 0; i < value.Length; i++)
                    m_byteInput[i] = (byte)value[i];
                dgFingerPrint = CalculateMD5Value();
            }
        }

        //gets the signature/figner print as string
        public string FingerPrint
        {
            get
            {
                return dgFingerPrint.ToString();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MD5()
        {
            Value = "";
        }

        /// <summary>
        /// calculat md5 signature of the string in Input
        /// </summary>
        /// <returns> Digest: the finger print of msg</returns>
        protected Digest CalculateMD5Value()
        {
            byte[] bMsg;    //buffer to hold bits
            uint N;         //N is the size of msg as  word (32 bit) 
            Digest dg = new Digest();           //  the value to be returned

            // create a buffer with bits padded and length is alos padded
            bMsg = CreatePaddedBuffer();

            N = (uint)(bMsg.Length * 8) / 32;       //no of 32 bit blocks

            for (uint i = 0; i < N / 16; i++)
            {
                CopyBlock(bMsg, i);
                PerformTransformation(ref dg.A, ref dg.B, ref dg.C, ref dg.D);
            }
            return dg;
        }


        #region Transforms
        /********************************************************
		 * TRANSFORMATIONS :  FF , GG , HH , II  acc to RFC 1321
		 * where each Each letter represnets the aux function used
		 *********************************************************/

        /// <summary>
        /// perform transformatio using f(((b&c) | (~(b)&d))
        /// </summary>
        protected void TransF(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + MD5Helper.RotateLeft((a + ((b & c) | (~(b) & d)) + X[k] + T[i - 1]), s);
        }

        /// <summary>
        /// perform transformatio using g((b&d) | (c & ~d) )
        /// </summary>
        protected void TransG(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + MD5Helper.RotateLeft((a + ((b & d) | (c & ~d)) + X[k] + T[i - 1]), s);
        }

        /// <summary>
        /// perform transformatio using h(b^c^d)
        /// </summary>
        protected void TransH(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + MD5Helper.RotateLeft((a + (b ^ c ^ d) + X[k] + T[i - 1]), s);
        }

        /// <summary>
        /// perform transformatio using i (c^(b|~d))
        /// </summary>
        protected void TransI(ref uint a, uint b, uint c, uint d, uint k, ushort s, uint i)
        {
            a = b + MD5Helper.RotateLeft((a + (c ^ (b | ~d)) + X[k] + T[i - 1]), s);
        }
        #endregion

        /// <summary>
        /// Perform All the transformation on the data
        /// </summary>
        /// <param name="A">A</param>
        /// <param name="B">B </param>
        /// <param name="C">C</param>
        /// <param name="D">D</param>
        protected void PerformTransformation(ref uint A, ref uint B, ref uint C, ref uint D)
        {
            //// saving  ABCD  to be used in end of loop
            uint AA, BB, CC, DD;

            // На этом шаге инициализируется буффер
            AA = A;
            BB = B;
            CC = C;
            DD = D;

            /*
            Затем происходят «чудесные» преобразования-раунды, которых всего будет 4. 
            Каждый раунд состоит из 16 элементарных преобразований, которые в общем виде можно 
            представить в виде [abcd k s i], которое, в свою очередь, можно представить как 
            A = B + ((A + F(B,C,D) + X[k] + T[i]) <<< s), где
            A, B, C, D — регистры
            F(B,C,D) — одна из логических функций
            X[k] — k-тый элемент 16-битного блока.
            T[i] — i-тый элемент таблицы «белого шума»
            <<< s — операция циклического сдвига на s позиций влево.
            */
            /* Round 1 
				* [ABCD  0  7  1]  [DABC  1 12  2]  [CDAB  2 17  3]  [BCDA  3 22  4]
				* [ABCD  4  7  5]  [DABC  5 12  6]  [CDAB  6 17  7]  [BCDA  7 22  8]
				* [ABCD  8  7  9]  [DABC  9 12 10]  [CDAB 10 17 11]  [BCDA 11 22 12]
				* [ABCD 12  7 13]  [DABC 13 12 14]  [CDAB 14 17 15]  [BCDA 15 22 16]
                *  * */
            TransformNumber numb1 = new TransformNumber(0, 7, 1);
            TransformNumber numb2 = new TransformNumber(1, 12, 2);
            TransformNumber numb3 = new TransformNumber(2, 17, 3);
            TransformNumber numb4 = new TransformNumber(3, 22, 4);

            Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber> nums =
                new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformF(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(4, 7, 5);
            numb2 = new TransformNumber(5, 12, 6);
            numb3 = new TransformNumber(6, 17, 7);
            numb4 = new TransformNumber(7, 22, 8);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformF(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(8, 7, 9);
            numb2 = new TransformNumber(9, 12, 10);
            numb3 = new TransformNumber(10, 17, 11);
            numb4 = new TransformNumber(11, 22, 12);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformF(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(12, 7, 13);
            numb2 = new TransformNumber(13, 12, 14);
            numb3 = new TransformNumber(14, 17, 15);
            numb4 = new TransformNumber(15, 22, 16);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformF(ref A, ref B, ref C, ref D, nums);
            /** Round 2
				*[ABCD  1  5 17]  [DABC  6  9 18]  [CDAB 11 14 19]  [BCDA  0 20 20]
				*[ABCD  5  5 21]  [DABC 10  9 22]  [CDAB 15 14 23]  [BCDA  4 20 24]
				*[ABCD  9  5 25]  [DABC 14  9 26]  [CDAB  3 14 27]  [BCDA  8 20 28]
				*[ABCD 13  5 29]  [DABC  2  9 30]  [CDAB  7 14 31]  [BCDA 12 20 32]
			*/
            numb1 = new TransformNumber(1, 5, 17);
            numb2 = new TransformNumber(6, 9, 18);
            numb3 = new TransformNumber(11, 14, 19);
            numb4 = new TransformNumber(0, 20, 20);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformG(ref A, ref B, ref C, ref D, nums);


            numb1 = new TransformNumber(5, 5, 21);
            numb2 = new TransformNumber(10, 9, 22);
            numb3 = new TransformNumber(15, 14, 23);
            numb4 = new TransformNumber(4, 20, 24);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformG(ref A, ref B, ref C, ref D, nums);


            numb1 = new TransformNumber(9, 5, 25);
            numb2 = new TransformNumber(14, 9, 26);
            numb3 = new TransformNumber(3, 14, 27);
            numb4 = new TransformNumber(8, 20, 28);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformG(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(13, 5, 29);
            numb2 = new TransformNumber(2, 9, 30);
            numb3 = new TransformNumber(7, 14, 31);
            numb4 = new TransformNumber(12, 20, 32);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformG(ref A, ref B, ref C, ref D, nums);

            /*  Round 3
				* [ABCD  5  4 33]  [DABC  8 11 34]  [CDAB 11 16 35]  [BCDA 14 23 36]
				* [ABCD  1  4 37]  [DABC  4 11 38]  [CDAB  7 16 39]  [BCDA 10 23 40]
				* [ABCD 13  4 41]  [DABC  0 11 42]  [CDAB  3 16 43]  [BCDA  6 23 44]
				* [ABCD  9  4 45]  [DABC 12 11 46]  [CDAB 15 16 47]  [BCDA  2 23 48]
			 * */
            numb1 = new TransformNumber(5, 4, 33);
            numb2 = new TransformNumber(8, 11, 34);
            numb3 = new TransformNumber(11, 16, 35);
            numb4 = new TransformNumber(14, 23, 36);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformH(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(1, 4, 37);
            numb2 = new TransformNumber(4, 11, 38);
            numb3 = new TransformNumber(7, 16, 39);
            numb4 = new TransformNumber(10, 23, 40);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformH(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(13, 4, 41);
            numb2 = new TransformNumber(0, 11, 42);
            numb3 = new TransformNumber(3, 16, 43);
            numb4 = new TransformNumber(6, 23, 44);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformH(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(9, 4, 45);
            numb2 = new TransformNumber(12, 11, 46);
            numb3 = new TransformNumber(15, 16, 47);
            numb4 = new TransformNumber(2, 23, 48);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformH(ref A, ref B, ref C, ref D, nums);

            /*  Round  4
				*[ABCD  0  6 49]  [DABC  7 10 50]  [CDAB 14 15 51]  [BCDA  5 21 52]
				*[ABCD 12  6 53]  [DABC  3 10 54]  [CDAB 10 15 55]  [BCDA  1 21 56]
				*[ABCD  8  6 57]  [DABC 15 10 58]  [CDAB  6 15 59]  [BCDA 13 21 60]
				*[ABCD  4  6 61]  [DABC 11 10 62]  [CDAB  2 15 63]  [BCDA  9 21 64]
                * */

            numb1 = new TransformNumber(0, 6, 49);
            numb2 = new TransformNumber(7, 10, 50);
            numb3 = new TransformNumber(14, 15, 51);
            numb4 = new TransformNumber(5, 21, 52);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformI(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(12, 6, 53);
            numb2 = new TransformNumber(3, 10, 54);
            numb3 = new TransformNumber(10, 15, 55);
            numb4 = new TransformNumber(1, 21, 56);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformI(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(8, 6, 57);
            numb2 = new TransformNumber(15, 10, 58);
            numb3 = new TransformNumber(6, 15, 59);
            numb4 = new TransformNumber(13, 21, 60);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformI(ref A, ref B, ref C, ref D, nums);

            numb1 = new TransformNumber(4, 6, 61);
            numb2 = new TransformNumber(11, 10, 62);
            numb3 = new TransformNumber(2, 15, 63);
            numb4 = new TransformNumber(9, 21, 64);

            nums = new Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber>(numb1, numb2, numb3, numb4);

            GenericTransformI(ref A, ref B, ref C, ref D, nums);

            A = A + AA;
            B = B + BB;
            C = C + CC;
            D = D + DD;
        }

        private void GenericTransformF(ref uint A, ref uint B, ref uint C, ref uint D,
            Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber> nums)
        {
            // K, S, I
            // Проходимся 4 раза по одному раунду
            // То есть то количество номеров сколько есть в списке
            TransF(ref A, B, C, D, nums.Item1.NumberK, nums.Item1.NumberS, nums.Item1.NumberI);
            TransF(ref D, A, B, C, nums.Item2.NumberK, nums.Item2.NumberS, nums.Item2.NumberI);
            TransF(ref C, D, A, B, nums.Item3.NumberK, nums.Item3.NumberS, nums.Item3.NumberI);
            TransF(ref B, C, D, A, nums.Item4.NumberK, nums.Item4.NumberS, nums.Item4.NumberI);
        }

        private void GenericTransformG(ref uint A, ref uint B, ref uint C, ref uint D,
            Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber> nums)
        {
            // K, S, I
            // Проходимся 4 раза по одному раунду
            // То есть то количество номеров сколько есть в списке
            TransG(ref A, B, C, D, nums.Item1.NumberK, nums.Item1.NumberS, nums.Item1.NumberI);
            TransG(ref D, A, B, C, nums.Item2.NumberK, nums.Item2.NumberS, nums.Item2.NumberI);
            TransG(ref C, D, A, B, nums.Item3.NumberK, nums.Item3.NumberS, nums.Item3.NumberI);
            TransG(ref B, C, D, A, nums.Item4.NumberK, nums.Item4.NumberS, nums.Item4.NumberI);
        }

        private void GenericTransformH(ref uint A, ref uint B, ref uint C, ref uint D,
            Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber> nums)
        {
            // K, S, I
            // Проходимся 4 раза по одному раунду
            // То есть то количество номеров сколько есть в списке
            TransH(ref A, B, C, D, nums.Item1.NumberK, nums.Item1.NumberS, nums.Item1.NumberI);
            TransH(ref D, A, B, C, nums.Item2.NumberK, nums.Item2.NumberS, nums.Item2.NumberI);
            TransH(ref C, D, A, B, nums.Item3.NumberK, nums.Item3.NumberS, nums.Item3.NumberI);
            TransH(ref B, C, D, A, nums.Item4.NumberK, nums.Item4.NumberS, nums.Item4.NumberI);
        }

        private void GenericTransformI(ref uint A, ref uint B, ref uint C, ref uint D,
            Tuple<TransformNumber, TransformNumber, TransformNumber, TransformNumber> nums)
        {
            // K, S, I
            // Проходимся 4 раза по одному раунду
            // То есть то количество номеров сколько есть в списке
            TransI(ref A, B, C, D, nums.Item1.NumberK, nums.Item1.NumberS, nums.Item1.NumberI);
            TransI(ref D, A, B, C, nums.Item2.NumberK, nums.Item2.NumberS, nums.Item2.NumberI);
            TransI(ref C, D, A, B, nums.Item3.NumberK, nums.Item3.NumberS, nums.Item3.NumberI);
            TransI(ref B, C, D, A, nums.Item4.NumberK, nums.Item4.NumberS, nums.Item4.NumberI);
        }


        /// <summary>
        /// Create Padded buffer for processing , buffer is padded with 0 along 
        /// with the size in the end
        /// 
        /// 1 ЕТАП
        /// В исходную строку дописывают единичный байт 0х80, 
        /// а затем дописывают нулевые биты, до тех пор, пока длина сообщения 
        /// не будет сравнима с 448 по модулю 512. То есть дописываем нули до тех пор, 
        /// пока длина нового сообщения не будет равна [длина] = (512*N+448),
        /// где N — любое натуральное число, такое, что это выражение будет наиболее близко к длине блока.
        /// </summary>
        /// <returns>the padded buffer as byte array</returns>
        protected byte[] CreatePaddedBuffer()
        {
            uint pad;       //no of padding bits for 448 mod 512 
            byte[] bMsg;    //buffer to hold bits
            ulong sizeMsg;      //64 bit size pad
            uint sizeMsgBuff;   //buffer size in multiple of bytes
            int temp = (448 - ((m_byteInput.Length * 8) % 512)); //temporary 


            pad = (uint)((temp + 512) % 512);       //getting no of bits to  be pad
            if (pad == 0)				///pad is in bits
				pad = 512;          //at least 1 or max 512 can be added

            sizeMsgBuff = (uint)((m_byteInput.Length) + (pad / 8) + 8);
            sizeMsg = (ulong)m_byteInput.Length * 8;
            bMsg = new byte[sizeMsgBuff];   ///no need to pad with 0 coz new bytes 
            // are already initialize to 0 :)

            ////copying string to buffer 
            for (int i = 0; i < m_byteInput.Length; i++)
                bMsg[i] = m_byteInput[i];

            bMsg[m_byteInput.Length] |= 0x80;       ///making first bit of padding 1,

            //wrting the size value
            for (int i = 8; i > 0; i--)
                bMsg[sizeMsgBuff - i] = (byte)(sizeMsg >> ((8 - i) * 8) & 0x00000000000000ff);

            return bMsg;
        }

        /// <summary>
        /// Copies a 512 bit block into X as 16 32 bit words
        /// Далее в сообщение дописывается 64-битное представление длины исходного сообщения.
        /// </summary>
        /// <param name="bMsg"> source buffer</param>
        /// <param name="block">no of block to copy starting from 0</param>
        protected void CopyBlock(byte[] bMsg, uint block)
        {

            block = block << 6;
            for (uint j = 0; j < 61; j += 4)
            {
                X[j >> 2] = (((uint)bMsg[block + (j + 3)]) << 24) |
                        (((uint)bMsg[block + (j + 2)]) << 16) |
                        (((uint)bMsg[block + (j + 1)]) << 8) |
                        (((uint)bMsg[block + (j)]));

            }
        }
    }
}
