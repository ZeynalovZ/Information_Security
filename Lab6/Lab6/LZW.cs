using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab6
{
    class LZW
    {
        public LZW()
        {
        }

        public byte[] Compress(byte[] data)
        {
            var dictionary = CreateInitialCompressionDictionary();

            var w = string.Empty;
            var compressedData = new List<ushort>();

            foreach (char c in data)
            {
                string wc = w + c;
                if (dictionary.ContainsKey(wc))
                {
                    w = wc;
                }
                else
                {
                    compressedData.Add(dictionary[w]);
                    if (dictionary.Count == ushort.MaxValue)
                    {
                        dictionary = CreateInitialCompressionDictionary();
                    }

                    dictionary.Add(wc, (ushort)dictionary.Count);
                    w = c.ToString();
                }
            }

            if (!string.IsNullOrEmpty(w))
                compressedData.Add(dictionary[w]);

            var res = compressedData.SelectMany(BitConverter.GetBytes).ToArray();

            return res;
        }

        public byte[] UnCompress(byte[] data)
        {
            var dictionary = CreateInitialDecompressionDictionary();

            var compressedData = new ushort[data.Length / 2];
            Buffer.BlockCopy(data, 0, compressedData, 0, data.Length);

            var w = dictionary[compressedData[0]];
            var decompressedData = new StringBuilder(w);

            compressedData = compressedData.Skip(1).ToArray();
            foreach (ushort k in compressedData)
            {
                string entry = string.Empty;
                if (dictionary.ContainsKey(k))
                    entry = dictionary[k];
                else if (k == dictionary.Count)
                    entry = w + w[0];

                decompressedData.Append(entry);

                if (dictionary.Count == ushort.MaxValue)
                {
                    dictionary = CreateInitialDecompressionDictionary();
                }
                dictionary.Add((ushort)dictionary.Count(), w + entry[0]);
                w = entry;
            }

            var res = decompressedData.ToString().Select(Convert.ToByte).ToArray();

            return res;
        }

        private Dictionary<ushort, string> CreateInitialDecompressionDictionary()
        {
            var dictionary = new Dictionary<ushort, string>();

            for (ushort i = 0; i < 256; i++)
                dictionary.Add(i, ((char)i).ToString());

            return dictionary;
        }

        private Dictionary<string, ushort> CreateInitialCompressionDictionary()
        {
            var dictionary = new Dictionary<string, ushort>();

            for (ushort i = 0; i < 256; i++)
                dictionary.Add(((char)i).ToString(), i);

            return dictionary;
        }
    }
}
