using System;
using System.Collections.Generic;
using System.Text;

namespace lab3
{
    public class Word
    {
        public byte b1 { get; set; }
        public byte b2 { get; set; }
        public byte b3 { get; set; }
        public byte b4 { get; set; }

        public Word()
        {

        }

        public Word(byte _b1, byte _b2, byte _b3, byte _b4)
        {
            b1 = _b1;
            b2 = _b2;
            b3 = _b3;
            b4 = _b4;
        }

        public Word(Word word)
        {
            b1 = word.b1;
            b2 = word.b2;
            b3 = word.b3;
            b4 = word.b4;
        }

        public override string ToString()
        {
            return $"{b1} {b2} {b3} {b4}";
        }
    }
}
