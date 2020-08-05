using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht7MaakEenYahtzeespel
{
    class Dice
    {
        private static Random random;
        private const int defaultNumberOfSides = 6;

        private static void SetRandom()
        {
            if (random == null)
            {
                random = new Random();
            }
        }
        public static int RollDice(int sides)
        {
            SetRandom();
            return random.Next(sides) + 1;
        }
        public static int RollDice()
        {
            return RollDice(defaultNumberOfSides);
        }
    }
}
