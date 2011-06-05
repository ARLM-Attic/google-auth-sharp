using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class Base32Encoder
    {
        //THIS CODE TAKEN AND THEN AUGMENTED FROM:
        //http://www.codeproject.com/KB/recipes/Base32Encoding.aspx
        //My modifcations were to make this a static class that takes no constructors or customizations.

        private const string ENCODING_TABLE = "abcdefghijklmnopqrstuvwxyz234567";
        private const char PADDING = '=';

        public static string ToBase32String(byte[] input)
        {
            var output = new StringBuilder();
            int specialLength = input.Length % 5;
            int normalLength = input.Length - specialLength;
            for (int i = 0; i < normalLength; i += 5)
            {
                int b1 = input[i];
                int b2 = input[i + 1];
                int b3 = input[i + 2];
                int b4 = input[i + 3];
                int b5 = input[i + 4];

                output.Append(ENCODING_TABLE[(b1 >> 3) & 0x1f]);
                output.Append(ENCODING_TABLE[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                output.Append(ENCODING_TABLE[(b2 >> 1) & 0x1f]);
                output.Append(ENCODING_TABLE[((b2 << 4) | (b3 >> 4)) & 0x1f]);
                output.Append(ENCODING_TABLE[((b3 << 1) | (b4 >> 7)) & 0x1f]);
                output.Append(ENCODING_TABLE[(b4 >> 2) & 0x1f]);
                output.Append(ENCODING_TABLE[((b4 << 3) | (b5 >> 5)) & 0x1f]);
                output.Append(ENCODING_TABLE[b5 & 0x1f]);
            }

            switch (specialLength)
            {
                case 1:
                    {
                        int b1 = input[normalLength] & 0xff;
                        output.Append(ENCODING_TABLE[(b1 >> 3) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b1 << 2) & 0x1f]);
                        output.Append(new string(PADDING, 6));
                        break;
                    }

                case 2:
                    {
                        int b1 = input[normalLength];
                        int b2 = input[normalLength + 1];
                        output.Append(ENCODING_TABLE[(b1 >> 3) & 0x1f]);
                        output.Append(ENCODING_TABLE[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b2 >> 1) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b2 << 4) & 0x1f]);
                        output.Append(new string(PADDING, 4));
                        break;
                    }
                case 3:
                    {
                        int b1 = input[normalLength];
                        int b2 = input[normalLength + 1];
                        int b3 = input[normalLength + 2];
                        output.Append(ENCODING_TABLE[(b1 >> 3) & 0x1f]);
                        output.Append(ENCODING_TABLE[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b2 >> 1) & 0x1f]);
                        output.Append(ENCODING_TABLE[((b2 << 4) | (b3 >> 4)) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b3 << 1) & 0x1f]);
                        output.Append(new string(PADDING, 3));
                        break;
                    }
                case 4:
                    {
                        int b1 = input[normalLength];
                        int b2 = input[normalLength + 1];
                        int b3 = input[normalLength + 2];
                        int b4 = input[normalLength + 3];
                        output.Append(ENCODING_TABLE[(b1 >> 3) & 0x1f]);
                        output.Append(ENCODING_TABLE[((b1 << 2) | (b2 >> 6)) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b2 >> 1) & 0x1f]);
                        output.Append(ENCODING_TABLE[((b2 << 4) | (b3 >> 4)) & 0x1f]);
                        output.Append(ENCODING_TABLE[((b3 << 1) | (b4 >> 7)) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b4 >> 2) & 0x1f]);
                        output.Append(ENCODING_TABLE[(b4 << 3) & 0x1f]);
                        output.Append(PADDING);
                        break;
                    }
            }

            return output.ToString();
        }

        public static byte[] FromBase32String(string data)
        {
            var outStream = new List<Byte>();
            byte[] dTable = InitialiseDecodingTable();

            int length = data.Length;
            while (length > 0)
            {
                if (!Ignore(data[length - 1])) break;
                length--;
            }

            int i = 0;
            int finish = length - 8;
            for (i = NextI(data, i, finish); i < finish; i = NextI(data, i, finish))
            {
                byte b1 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b2 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b3 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b4 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b5 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b6 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b7 = dTable[data[i++]];
                i = NextI(data, i, finish);
                byte b8 = dTable[data[i++]];

                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
                outStream.Add((byte)((b7 << 5) | b8));
            }
            DecodeLastBlock(outStream,
                data[length - 8], data[length - 7], data[length - 6], data[length - 5],
                data[length - 4], data[length - 3], data[length - 2], data[length - 1],
                dTable);

            return outStream.ToArray();
        }

        private static int DecodeLastBlock(ICollection<byte> outStream,
            char c1, char c2, char c3, char c4, char c5, char c6, char c7, char c8,
            byte[] dTable)
        {
            if (c3 == PADDING)
            {
                byte b1 = dTable[c1];
                byte b2 = dTable[c2];
                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                return 1;
            }

            if (c5 == PADDING)
            {
                byte b1 = dTable[c1];
                byte b2 = dTable[c2];
                byte b3 = dTable[c3];
                byte b4 = dTable[c4];
                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                return 2;
            }

            if (c6 == PADDING)
            {
                byte b1 = dTable[c1];
                byte b2 = dTable[c2];
                byte b3 = dTable[c3];
                byte b4 = dTable[c4];
                byte b5 = dTable[c5];

                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                return 3;
            }

            if (c8 == PADDING)
            {
                byte b1 = dTable[c1];
                byte b2 = dTable[c2];
                byte b3 = dTable[c3];
                byte b4 = dTable[c4];
                byte b5 = dTable[c5];
                byte b6 = dTable[c6];
                byte b7 = dTable[c7];

                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
                return 4;
            }

            else
            {
                byte b1 = dTable[c1];
                byte b2 = dTable[c2];
                byte b3 = dTable[c3];
                byte b4 = dTable[c4];
                byte b5 = dTable[c5];
                byte b6 = dTable[c6];
                byte b7 = dTable[c7];
                byte b8 = dTable[c8];
                outStream.Add((byte)((b1 << 3) | (b2 >> 2)));
                outStream.Add((byte)((b2 << 6) | (b3 << 1) | (b4 >> 4)));
                outStream.Add((byte)((b4 << 4) | (b5 >> 1)));
                outStream.Add((byte)((b5 << 7) | (b6 << 2) | (b7 >> 3)));
                outStream.Add((byte)((b7 << 5) | b8));
                return 5;
            }
        }

        private static int NextI(string data, int i, int finish)
        {
            while ((i < finish) && Ignore(data[i])) i++;
            return i;
        }

        private static bool Ignore(char c)
        {
            return (c == '\n') || (c == '\r') || (c == '\t') || (c == ' ') || (c == '-');
        }

        private static byte[] InitialiseDecodingTable()
        {
            byte[] dt = new byte[0x80];
            for (int i = 0; i < ENCODING_TABLE.Length; i++)
            {
                dt[ENCODING_TABLE[i]] = (byte)i;
            }
            return dt;
        }
    }
}
