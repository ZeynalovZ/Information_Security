using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Enigma
{
    public class Enigma
    {

        private List<int> _rototr1 = new List<int>();
        private List<int> _rototr2 = new List<int>();
        private List<int> _rototr3 = new List<int>();

        private int _rotorRotateCount1;
        private int _rotorRotateCount2;
        private int _rotorRotateCount3;

        private List<int> _reflector = new List<int>();

        public Enigma()
        {
            _rotorRotateCount1 = 0;
            _rotorRotateCount2 = 0;
            _rotorRotateCount3 = 0;

            for (int i = 255; i >= 0; i--)
            {
                _reflector.Add(i);
            }
            var rnd = new Random(10);
            for (int i = 0; i < 256; i++)
            {
                _rototr1.Add(i);
                _rototr2.Add(i);
                _rototr3.Add(i);
            }
            Shuffle(_rototr1, rnd);
            Shuffle(_rototr2, rnd);
            Shuffle(_rototr3, rnd);

        }

        private void Shuffle(List<int> rotor, Random rnd)
        {
            var n = rotor.Count;
            for (int i = n - 1; i >= 1; i--)
{
                int j = rnd.Next(i + 1);
                // exchange perm[j] and perm[i]
                int temp = rotor[j];
                rotor[j] = rotor[i];
                rotor[i] = temp;
            }
        }

        private void RotateRotor(List<int> rotor)
        {
            rotor.Add(rotor.ElementAt(0));
            rotor.RemoveAt(0);
        }

        
        private void RotorOperations()
        {
            _rotorRotateCount1++;
            RotateRotor(_rototr1);
            if (_rotorRotateCount1 == 255)
            {
                _rotorRotateCount1 = 0;
                _rotorRotateCount2++;
                RotateRotor(_rototr2);
                if (_rotorRotateCount2 == 255)
                {
                    _rotorRotateCount2 = 0;
                    _rotorRotateCount3++;
                    RotateRotor(_rototr3);
                    if (_rotorRotateCount2 == 255)
                    {
                        _rotorRotateCount3 = 0;
                    }
                }
                
            }
        }


        public int InputValue(int value)
        {
            RotorOperations();
            var first = _rototr1.ElementAt(value);
            var second = _rototr2.ElementAt(first);
            var third = _rototr3.ElementAt(second);
            var refl = _reflector.ElementAt(third);
            var fourth = _rototr3.IndexOf(refl);
            var fitfh = _rototr2.IndexOf(fourth);
            var end = _rototr1.IndexOf(fitfh);
            return end;
        }


    }
}
