using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht7MaakEenYahtzeespel
{
    class Yahtzee
    {
        private const int numberOfDice = 5;
        private const int numberOfDiceSides = 6;
        private const int maxTrowNumber = 3;
        private const int sameNumberExtraPointBonusThreshold = 63;
        private const int sameNumberExtraPointBonusPoints = 35;
        private const int threeOfAKindAmountOfDiceWithSameNumberThreshold = 3;

        private int[] dice;
        private List<int> dicePutAway;
        private int trowNumber;
        private List<PointOption> pointOptions;
        private int[] points;
        private List<int> sameNumberExtraPointBonusIndexes;

        public Yahtzee()
        {
            SetupGame();
        }

        public void SetupGame()
        {
            SetupDice();
            SetupDicePutAway();
            SetupPointOptions();
            SetupPoints();
        }
        private void SetupDice()
        {
            if (dice == null)
            {
                dice = new int[numberOfDice];
            }
            else
            {
                for (int i = 0; i < dice.Length; i++)
                {
                    dice[i] = 0;
                }
            }
        }
        private void SetupDicePutAway()
        {
            if (dicePutAway == null)
            {
                dicePutAway = new List<int>();
            }
            else
            {
                dicePutAway.Clear();
            }
        }
        private void SetupPointOptions()
        {
            if (pointOptions == null)
            {
                pointOptions = new List<PointOption>();

                if (sameNumberExtraPointBonusIndexes == null)
                {
                    sameNumberExtraPointBonusIndexes = new List<int>();
                }
                else
                {
                    sameNumberExtraPointBonusIndexes.Clear();
                }

                for (int i=1;i< numberOfDiceSides + 1; i++)
                {
                    string name = "all " + i + "s";
                    string textBeforePoints = "all " + i + " together get to a total of ";
                    int targetNumber = i;
                    Func<int[], int> calculatePoints = (int[] diceResults) =>
                      {
                          int total = 0;
                          foreach(int result in diceResults)
                          {
                              if (result == targetNumber)
                              {
                                  total += result;
                              }
                          }
                          return total;
                      };
                    PointOption option = new PointOption(name, calculatePoints, textBeforePoints);
                    pointOptions.Add(option);
                    for(int o = 0; o < pointOptions.Count; o++)
                    {
                        if (pointOptions[o] == option)
                        {
                            sameNumberExtraPointBonusIndexes.Add(o);
                            break;
                        }
                    }
                }

                string threeOfAKindName = "Three Of A Kind";
                string threeOfAKindTextBeforePoints = "met Three of a kind waarbij 3 cijfers hetzelfde moeten zijn en alle getallen bij de score worden gedaan krijg je de score ";
                Func<int[],int> threeOfAKindCalculatePoints=(int[] diceResults) =>
                {
                    for (int i=1;i<numberOfDiceSides+1; i++)
                    {
                        int amountOfDiceWithNumber = 0;
                        for (int o = 0; o < diceResults.Length; o++)
                        {
                            if (diceResults[o] == i)
                            {
                                amountOfDiceWithNumber++;
                            }
                        }
                        if (amountOfDiceWithNumber >= threeOfAKindAmountOfDiceWithSameNumberThreshold)
                        {
                            int result = 0;
                            foreach(int diceResult in diceResults)
                            {
                                result += diceResult;
                            }
                            return result;
                        }
                    }
                    return 0;
                };
                pointOptions.Add(new PointOption(threeOfAKindName, threeOfAKindCalculatePoints, threeOfAKindTextBeforePoints));
            }
        }
        private void SetupPoints()
        {
            SetupPointOptions();
            if (points == null)
            {
                points = new int[pointOptions.Count];
            }
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = 0;
            }
        }
        public string RunTest()
        {
            string returnString = "";
            for (int i = 0; i < dice.Length; i++)
            {
                dice[i] = Dice.RollDice(numberOfDiceSides);
                returnString+="dice" + i + ": " + dice[i]+"\r\n";
            }
            for (int i = 0; i < pointOptions.Count; i++)
            {
                returnString += pointOptions[i].GetName() + " " + pointOptions[i].GetText(dice) + "\r\n";
            }
            return returnString;
        }

        class PointOption
        {
            private const string notAnOptionText = " is not an option";

            private string name;
            private Func<int[],int> calculatePoints;
            private string textBeforePoints;
            
            internal PointOption(string name, Func<int[],int> calculatePoints, string textBeforePoints)
            {
                this.name = name;
                this.calculatePoints = calculatePoints;
                this.textBeforePoints = textBeforePoints;
            }
            internal string GetName()
            {
                return name;
            }
            internal bool IsOption(int[] diceResults)
            {
                if (GetPoints(diceResults) != 0)
                {
                    return true;
                }
                return false;
            }
            internal string GetText(int[] diceResults)
            {
                if (IsOption(diceResults))
                {
                    return textBeforePoints + calculatePoints.Invoke(diceResults);
                }
                else
                {
                    return name + notAnOptionText;
                }
            }
            internal int GetPoints(int[] diceResults)
            {
                return calculatePoints.Invoke(diceResults);
            }
        }
    }
}
