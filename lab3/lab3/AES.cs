using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lab3
{
    public class AES
    {
        static private int Nb = 4;
        static private int Nr = 10;
        static private int Nk = 4;

        private byte[] _bytes;
        private List<Word> wKey = new List<Word>();
        private int wKeyLenght = Nb * (Nr + 1);

        private int wordLenght = 4;

        private Matrix _matrix = new Matrix();

        byte[,] bytesMatrix = new byte[4, 4];

        public int _additionalBytesCount = 0;

        List<byte[,]> blockBytes = new List<byte[,]>();

        private int[] Sbox =
        {
            0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
            0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
            0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
            0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
            0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
            0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
            0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
            0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
            0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
            0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
            0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
            0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
            0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
            0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
            0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
            0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
        };

        private int[] invSbox =
        {
            0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
            0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
            0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
            0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
            0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
            0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
            0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
            0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
            0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
            0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
            0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
            0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
            0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
            0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
            0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
        };

        static List<Word> Rcon = new List<Word>
        {
            new Word(0x00, 0x00, 0x00, 0x00),
            new Word(0x01, 0x00, 0x00, 0x00),
            new Word(0x02, 0x00, 0x00, 0x00),
            new Word(0x04, 0x00, 0x00, 0x00),
            new Word(0x08, 0x00, 0x00, 0x00),
            new Word(0x10, 0x00, 0x00, 0x00),
            new Word(0x20, 0x00, 0x00, 0x00),
            new Word(0x40, 0x00, 0x00, 0x00),
            new Word(0x80, 0x00, 0x00, 0x00),
            new Word(0x1b, 0x00, 0x00, 0x00),
            new Word(0x36, 0x00, 0x00, 0x00)
        };

        public AES(byte[] bytesToEncript, byte[] key)
        {
            _bytes = bytesToEncript;
            ExpandKey(key);

            var str = "";
            foreach (var bit in key)
            {
                str += $"{bit} ";
            }
            Console.WriteLine(str);

        }

        public void ShiftRow(Word word)
        {
            var tmp = word.b1;
            word.b1 = word.b2;
            word.b2 = word.b3;
            word.b3 = word.b4;
            word.b4 = tmp;
        }

        public byte SubByte(byte b)
        {
            string tmp = b.ToString("X2");
            int row = Convert.ToInt32(tmp[0].ToString(), 16);
            int column = Convert.ToInt32(tmp[1].ToString(), 16);
            byte newByte = (byte)Sbox[row * 16 + column];
            return newByte;
        }

        public void SubBytes(Word word)
        {
            word.b1 = SubByte(word.b1);
            word.b2 = SubByte(word.b2);
            word.b3 = SubByte(word.b3);
            word.b4 = SubByte(word.b4);
        }


        public void Xor(Word word1, Word word2)
        {
            word1.b1 ^= word2.b1;
            word1.b2 ^= word2.b2;
            word1.b3 ^= word2.b3;
            word1.b4 ^= word2.b4;
        }

        public void ExpandKey(byte[] key)
        {
            int i = 0;
            // Начальная инициализация (первые 4 слова в массиве W - сам ключ)
            for (; i < Nb; i++)
            {
                var word = new Word(key[(i * Nb)], key[(i * Nb) + 1], key[(i * Nb) + 2], key[(i * Nb) + 3]);
                wKey.Add(word);
            }


            for (; i < wKeyLenght; i++)
            {
                Word tmp;
                tmp = new Word(wKey[i - 1]);
                if (i % Nb == 0)
                {
                    ShiftRow(tmp);
                    SubBytes(tmp);
                    Xor(tmp, Rcon[i / Nk]);
                    Xor(tmp, wKey[i - Nb]);
                }
                else
                {
                    ;
                    Xor(tmp, wKey[i - Nb]);
                }
                wKey.Add(new Word(tmp));

            }

            foreach (var k in wKey)
            {
                Console.WriteLine(k.ToString());
            }
            Console.WriteLine("current lenght is " + wKey.Count);
        }


        public void SubBytesMatrix(byte[,] s)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    s[i, j] = SubByte(s[i, j]);
                }
            }

        }

        public void GenerateMatrixes()
        {
            var bytesLenght = _bytes.Length;
            var blocksCount = bytesLenght / 16;
            var additionalBytesCount = bytesLenght % 16;
            _additionalBytesCount = additionalBytesCount;
            var missingBytesCount = 16 - additionalBytesCount;
            

            int index = 0;
            if (blocksCount != 0)
            {
                for (int k = 0; k < blocksCount; k++)
                {
                    byte[,] tmp = new byte[4, 4];
                    
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            tmp[j, i] = _bytes[index];
                            index++;
                        }
                    }
                    blockBytes.Add(tmp);
                }

                if (additionalBytesCount != 0)
                {
                    byte[,] tmp = new byte[4, 4];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (index < bytesLenght)
                            {
                                tmp[j, i] = _bytes[index];
                                index++;
                            }
                            else
                            {
                                tmp[j, i] = 0;
                            }
                            
                        }
                    }
                    blockBytes.Add(tmp);
                }
            }
            else
            {
                if (additionalBytesCount != 0)
                {
                    byte[,] tmp = new byte[4, 4];
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (index < bytesLenght)
                            {
                                tmp[j, i] = _bytes[index];
                                index++;
                            }
                            else
                            {
                                tmp[j, i] = 0;
                            }

                        }
                    }
                    blockBytes.Add(tmp);
                }
            }
        }

        public byte[] GetBytes()
        {
            byte[] bytes = new byte[blockBytes.Count * 16 + 1];
            int index = 0;
            foreach (var blk in blockBytes)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bytes[index] = (blk[j, i]);
                        index++;
                    }
                }
            }
            Console.WriteLine(_additionalBytesCount);
            bytes[index] = Convert.ToByte(_additionalBytesCount);

            return (byte[])bytes.Clone();
        }

        public byte[] EncryptBytes()
        {

            GenerateMatrixes();
            foreach (var matrix in blockBytes)
            {
                _matrix.PrintMatrix(matrix);
            }

            

            foreach (var matrix in blockBytes)
            {
                _matrix.AddRoundKey(matrix, wKey[0], wKey[1], wKey[2], wKey[3]);

                for (int i = 1; i < Nr; i++)
                {
                    SubBytesMatrix(matrix);

                    _matrix.ShiftRowsMatrix(matrix);

                    _matrix.MixColumns(matrix);
                    _matrix.AddRoundKey(matrix, wKey[i * Nb], wKey[i * Nb + 1], wKey[i * Nb + 2], wKey[i * Nb + 3]);
                }
                SubBytesMatrix(matrix);
                _matrix.ShiftRowsMatrix(matrix);
                _matrix.AddRoundKey(matrix, wKey[Nr * Nb], wKey[Nr * Nb + 1], wKey[Nr * Nb + 2], wKey[Nr * Nb + 3]);
                _matrix.PrintMatrix(matrix);
            }

            return GetBytes();
            
        }

        public void DecryptBytes(byte[] key)
        {


        }


    }
}
