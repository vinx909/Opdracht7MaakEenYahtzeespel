﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht7MaakEenYahtzeespel
{
    class Yahtzee
    {
        private const string exceptionArrayLengthsNotSameSize = "the length of pointOptions and points is not the same, something has gone wrong and the program can't continue";
        private const string exceptionScoreOptionNotFound = "a given score option was not found";
        private const string sameNumberExtraPointBonusTextPartOne = "as a bonus you also get an aditional ";
        private const string sameNumberExtraPointBonusTextPartTwo = " points";
        private const string sameNumberExtraPointBonusUnreachedTextPartOne = "for the bonus points for the same dice you need ";
        private const string sameNumberExtraPointBonusUnreachedTextPartTwo = " more points";
        private const string pointTotalString = "which gives you a total of ";
        private const string stringNextLine = "\r\n";
        private const string optionNameToScoreDevider = ": ";

        private const int numberOfDice = 5;
        private const int numberOfDiceSides = 6;
        private const int maxTrowNumber = 3;
        private const int sameNumberExtraPointBonusThreshold = 63;
        private const int sameNumberExtraPointBonusPoints = 35;
        private const int threeOfAKindAmountOfDiceWithSameNumberThreshold = 3;
        private const int carréAmountOfDiceWithSameNumberThreshold = 4;
        private const int fullHouseAmountOfDiceWithSameNumberThresholdOne = 3;
        private const int fullHouseAmountOfDiceWithSameNumberThresholdTwo = 2;
        private const int yahtzeeAmountOfDiceWithSameNumberThreshold = 5;

        private const int fullHousePoints = 25;
        private const int kleineStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold = 4;
        private const int kleineStraatPoints = 30;
        private const int grooteStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold = 5;
        private const int grooteStraatPoints = 40;
        private const int yahtzeePointsInitial = 50;
        private const int yahtzeePointsSuccessive = 100;

        private Dice[] dice;
        private List<int> dicePutAway;
        private int trowNumber;
        private List<PointOption> pointOptions;
        private int[] points;
        private List<int> sameNumberExtraPointBonusIndexes;

        public Yahtzee()
        {
            SetupGame();
        }
        public static int GetNumberOfDice()
        {
            return numberOfDice;
        }
        public int GetNumberOfScoreOptions()
        {
            return pointOptions.Count;
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
                dice = new Dice[numberOfDice];
                for(int i = 0; i < dice.Length; i++)
                {
                    dice[i] = new Dice(numberOfDiceSides);
                }
            }
            else
            {
                for (int i = 0; i < dice.Length; i++)
                {
                    dice[i].RollDie();
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

                for (int i = 1; i < numberOfDiceSides + 1; i++)
                {
                    string name = "all " + i + "s";
                    string textBeforePoints = "all " + i + " together get to a total of ";
                    int targetNumber = i;
                    Func<int[], int> calculatePoints = (int[] diceResults) =>
                      {
                          int total = 0;
                          foreach (int result in diceResults)
                          {
                              if (result == targetNumber)
                              {
                                  total += result;
                              }
                          }
                          return total;
                      };
                    PointOption option = new PointOption(name, calculatePoints, textBeforePoints, false);
                    pointOptions.Add(option);
                    for (int o = 0; o < pointOptions.Count; o++)
                    {
                        if (pointOptions[o] == option)
                        {
                            sameNumberExtraPointBonusIndexes.Add(o);
                            break;
                        }
                    }
                }

                string threeOfAKindName = "Three Of A Kind";
                string threeOfAKindTextBeforePoints = threeOfAKindName + " where " + threeOfAKindAmountOfDiceWithSameNumberThreshold + " dice must be the same and all numbers are put together for a score of ";
                Func<int[], int> threeOfAKindCalculatePoints = (int[] diceResults) =>
                   {
                       for (int i = 1; i < numberOfDiceSides + 1; i++)
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
                               foreach (int diceResult in diceResults)
                               {
                                   result += diceResult;
                               }
                               return result;
                           }
                       }
                       return 0;
                   };
                pointOptions.Add(new PointOption(threeOfAKindName, threeOfAKindCalculatePoints, threeOfAKindTextBeforePoints, false));

                string carréName = "Carré";
                string carréTextBeforePoints = carréName + " where " + carréAmountOfDiceWithSameNumberThreshold + " dice must be the same and all numbers are put together for a score of ";
                Func<int[], int> carréCalculatePoints = (int[] diceResults) =>
                {
                    for (int i = 1; i < numberOfDiceSides + 1; i++)
                    {
                        int amountOfDiceWithNumber = 0;
                        for (int o = 0; o < diceResults.Length; o++)
                        {
                            if (diceResults[o] == i)
                            {
                                amountOfDiceWithNumber++;
                            }
                        }
                        if (amountOfDiceWithNumber >= carréAmountOfDiceWithSameNumberThreshold)
                        {
                            int result = 0;
                            foreach (int diceResult in diceResults)
                            {
                                result += diceResult;
                            }
                            return result;
                        }
                    }
                    return 0;
                };
                pointOptions.Add(new PointOption(carréName, carréCalculatePoints, carréTextBeforePoints, false));

                string fullHouseName = "Full House";
                string fullHouseTextBeforePoints = fullHouseName + " where " + fullHouseAmountOfDiceWithSameNumberThresholdOne + " dice must be the same and " + fullHouseAmountOfDiceWithSameNumberThresholdTwo + " other dice must be the same for a total of ";
                Func<int[], int> fullHouseCalculatePoints = (int[] diceResults) =>
                 {
                     for (int diceSideToTestForOne = 1; diceSideToTestForOne < numberOfDiceSides + 1; diceSideToTestForOne++)
                     {
                         int amountOfDiceWithNumberOne = 0;
                         for (int i = 0; i < diceResults.Length; i++)
                         {
                             if (diceResults[i] == diceSideToTestForOne)
                             {
                                 amountOfDiceWithNumberOne++;
                             }
                         }
                         if (amountOfDiceWithNumberOne >= fullHouseAmountOfDiceWithSameNumberThresholdOne)
                         {
                             for (int diceSideToTestForTwo = 1; diceSideToTestForTwo < numberOfDiceSides + 1; diceSideToTestForTwo++)
                             {
                                 int amountOfDiceWithNumberTwo = 0;
                                 for (int i = 0; i < diceResults.Length; i++)
                                 {
                                     if (diceResults[i] == diceSideToTestForTwo && diceSideToTestForOne != diceSideToTestForTwo)
                                     {
                                         amountOfDiceWithNumberTwo++;
                                     }
                                 }
                                 if (amountOfDiceWithNumberTwo >= fullHouseAmountOfDiceWithSameNumberThresholdTwo)
                                 {
                                     return fullHousePoints;
                                 }
                             }
                         }
                     }
                     return 0;
                 };
                pointOptions.Add(new PointOption(fullHouseName, fullHouseCalculatePoints, fullHouseTextBeforePoints, false));

                string kleineStraatName = "Kleine Straat";
                string kleineStraatTextBeforePoints = kleineStraatName + " where " + kleineStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold + " dice have to successively increase for a total of ";
                Func<int[], int> kleineStraatCalculatePoints = (int[] diceResults) =>
                 {
                     for (int startOfStreetNumber = 1; startOfStreetNumber <= numberOfDiceSides - kleineStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold + 1; startOfStreetNumber++)
                     {
                         bool isStreet = true;
                         for (int increment = 0; increment < kleineStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold; increment++)
                         {
                             bool incrementFound = false;
                             for (int i = 0; i < diceResults.Length; i++)
                             {
                                 if (diceResults[i] == startOfStreetNumber + increment)
                                 {
                                     incrementFound = true;
                                     break;
                                 }
                             }
                             if (incrementFound == false)
                             {
                                 isStreet = false;
                                 break;
                             }
                         }
                         if (isStreet == true)
                         {
                             return kleineStraatPoints;
                         }
                     }
                     return 0;
                 };
                pointOptions.Add(new PointOption(kleineStraatName, kleineStraatCalculatePoints, kleineStraatTextBeforePoints, false));

                string grooteStraatName = "Grote Straat";
                string grooteStraatTextBeforePoints = kleineStraatName + " where " + grooteStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold + " dice have to successively increase for a total of ";
                Func<int[], int> grooteStraatCalculatePoints = (int[] diceResults) =>
                {
                    for (int startOfStreetNumber = 1; startOfStreetNumber <= numberOfDiceSides - grooteStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold + 1; startOfStreetNumber++)
                    {
                        bool isStreet = true;
                        for (int increment = 0; increment < grooteStraatAmountOfDiceThatHaveToIncrementallyIncreaseTheshold; increment++)
                        {
                            bool incrementFound = false;
                            for (int i = 0; i < diceResults.Length; i++)
                            {
                                if (diceResults[i] == startOfStreetNumber + increment)
                                {
                                    incrementFound = true;
                                    break;
                                }
                            }
                            if (incrementFound == false)
                            {
                                isStreet = false;
                                break;
                            }
                        }
                        if (isStreet == true)
                        {
                            return grooteStraatPoints;
                        }
                    }
                    return 0;
                };
                pointOptions.Add(new PointOption(grooteStraatName, grooteStraatCalculatePoints, grooteStraatTextBeforePoints, false));

                string yahtzeeName = "Yahtzee";
                string yahtzeeTextBeforePoints = yahtzeeName + " where " + yahtzeeAmountOfDiceWithSameNumberThreshold + " dice must be the same for a total yahtzee score of ";
                int amountOfTimesYahtzeeHasBeenTrown = 0;
                Func<int[], int> yahtzeeCalculatePoints = (int[] diceResults) =>
                {
                    for (int i = 1; i < numberOfDiceSides + 1; i++)
                    {
                        int amountOfDiceWithNumber = 0;
                        for (int o = 0; o < diceResults.Length; o++)
                        {
                            if (diceResults[o] == i)
                            {
                                amountOfDiceWithNumber++;
                            }
                        }
                        if (amountOfDiceWithNumber >= yahtzeeAmountOfDiceWithSameNumberThreshold)
                        {
                            int result = yahtzeePointsInitial+ yahtzeePointsSuccessive* amountOfTimesYahtzeeHasBeenTrown;
                            amountOfTimesYahtzeeHasBeenTrown++;
                            return result;
                        }
                    }
                    return 0;
                };
                pointOptions.Add(new PointOption(yahtzeeName, yahtzeeCalculatePoints, yahtzeeTextBeforePoints, true));
            }
        }

        internal string GetScoreBoardString()
        {
            if (pointOptions.Count == points.Length)
            {
                string toReturn = "";
                int totalScore = 0;
                for(int i = 0; i < points.Length; i++)
                {
                    if (points[i] > 0)
                    {
                        for (int o=0;o< sameNumberExtraPointBonusIndexes.Count; o++)
                        {
                            if (sameNumberExtraPointBonusIndexes[o] == i)
                            {
                                toReturn += pointOptions[i].GetName() + optionNameToScoreDevider + points[i] + stringNextLine;
                                totalScore += points[i];
                            }
                        }
                    }
                }

                if(totalScore>= sameNumberExtraPointBonusThreshold)
                {
                    toReturn += sameNumberExtraPointBonusTextPartOne + sameNumberExtraPointBonusPoints + sameNumberExtraPointBonusTextPartTwo;
                    totalScore += sameNumberExtraPointBonusThreshold;
                }
                else
                {
                    toReturn += sameNumberExtraPointBonusUnreachedTextPartOne + (sameNumberExtraPointBonusThreshold - totalScore) + sameNumberExtraPointBonusUnreachedTextPartTwo + stringNextLine + stringNextLine;
                }
                

                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i] > 0)
                    {
                        bool inSameNumbersForExtraPointsIndexes = false;
                        for (int o = 0; o < sameNumberExtraPointBonusIndexes.Count; o++)
                        {
                            if (sameNumberExtraPointBonusIndexes[o] == i)
                            {
                                inSameNumbersForExtraPointsIndexes = true;
                            }
                        }
                        if(inSameNumbersForExtraPointsIndexes == false)
                        {
                            toReturn += pointOptions[i].GetName() + optionNameToScoreDevider + points[i] + stringNextLine;
                            totalScore += points[i];
                        }
                    }
                }

                toReturn += stringNextLine + pointTotalString + totalScore;
                return toReturn;
            }
            else
            {
                throw new Exception(exceptionArrayLengthsNotSameSize);
            }
            throw new NotImplementedException();
        }

        internal bool GetCanReroll()
        {
            return trowNumber < maxTrowNumber;
        }

        internal bool GetIfOption(object scoreOption)
        {
            if (typeof(PointOption).IsInstanceOfType(scoreOption)){
                return ((PointOption)scoreOption).IsOption(GetDiceResults());
            }
            return false;
        }

        public string[] GetScoreOptionsText()
        {
            string[] toReturn = new string[pointOptions.Count];
            for(int i = 0; i < toReturn.Length; i++)
            {
                toReturn[i] = pointOptions[i].GetText(GetDiceResults());
            }
            return toReturn;
        }
        public Object[] GetScoreOptions()
        {
            if (pointOptions.Count == points.Length)
            {
                PointOption[] toReturn = new PointOption[points.Length];
                for (int i = 0; i < toReturn.Length; i++)
                {
                    if (points[i] == 0 || pointOptions[i].CanBeUsedAgain())
                    {
                        toReturn[i] = pointOptions[i];
                    }
                }
                return toReturn;
            }
            else
            {
                throw new Exception(exceptionArrayLengthsNotSameSize);
            }
        }

        public void NextRound()
        {
            dicePutAway.Clear();
            trowNumber = 0;
        }

        public void UseScoreOption(object scoreOption)
        {
            if (pointOptions.Count == points.Length)
            {
                bool scoreOptionFound = false;
                for(int i = 0; i < points.Length; i++)
                {
                    if(pointOptions[i] == scoreOption)
                    {
                        scoreOptionFound = true;
                        points[i] = pointOptions[i].GetPoints(GetDiceResults());
                        break;
                    }
                }
                if (scoreOptionFound == false)
                {
                    throw new Exception(exceptionScoreOptionNotFound);
                }
            }
            else
            {
                throw new Exception(exceptionArrayLengthsNotSameSize);
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
                dice[i].RollDie();
                returnString+="dice" + i + ": " + dice[i].GetDieValue()+"\r\n";
            }
            for (int i = 0; i < pointOptions.Count; i++)
            {
                returnString += pointOptions[i].GetName() + " " + pointOptions[i].GetText(GetDiceResults()) + "\r\n";
            }
            return returnString;
        }
        private int[] GetDiceResults()
        {
            int[] toReturn = new int[dice.Length];
            for(int i = 0; i < dice.Length; i++)
            {
                toReturn[i] = dice[i].GetDieValue();
            }
            return toReturn;
        }
        public void Reroll(object[] diceToPutAway)
        {
            foreach(object supposedlyDice in diceToPutAway)
            {
                if (!typeof(Dice).IsInstanceOfType(supposedlyDice))
                {
                    throw new Exception("object given isn't a dice");
                }
                bool inDice = false;
                for (int i = 0; i < dice.Length; i++)
                {
                    if (dice[i] == supposedlyDice)
                    {
                        dicePutAway.Add(i);
                        inDice = true;
                    }
                }
                if (inDice == false)
                {
                    throw new Exception("dice to put away isn't in dice");
                }
            }
            for (int i = 0; i < dice.Length; i++)
            {
                bool putaway = false;
                foreach(int putAwayIndex in dicePutAway)
                {
                    if (i == putAwayIndex)
                    {
                        putaway = true;
                    }
                }
                if (putaway == false)
                {
                    dice[i].RollDie();
                }
            }

            trowNumber++;
        }
        public Dice[] GetRerollableDice()
        {
            Dice[] returnDice = new Dice[dice.Length - dicePutAway.Count];
            int currentIndex = 0;
            for(int i = 0; i < dice.Length; i++)
            {
                bool putAway = false;
                foreach(int putAwayIndex in dicePutAway)
                {
                    if (i == putAwayIndex)
                    {
                        putAway = true;
                    }
                }
                if (putAway == false)
                {
                    returnDice[currentIndex] = dice[i];
                    currentIndex++;
                }
            }
            return returnDice;
        }
        public int[] GetNonrerollableDice()
        {
            int[] returnDice = new int[dicePutAway.Count];
            int currentIndex = 0;
            for (int i = 0; i < dice.Length; i++)
            {
                bool putAway = false;
                foreach (int putAwayIndex in dicePutAway)
                {
                    if (i == putAwayIndex)
                    {
                        putAway = true;
                    }
                }
                if (putAway == true)
                {
                    returnDice[currentIndex] = dice[i].GetDieValue();
                    currentIndex++;
                }
            }
            return returnDice;
        }
        private int[] orderIntArray(int[] array)
        {
            if (array!=null&&array.Length > 0) {
                int[] returnArray = new int[array.Length];

                int minimum = array[0];
                for(int i = 0; i < array.Length; i++)
                {
                    if (minimum > array[i])
                    {
                        minimum = array[i];
                    }
                }
                int maximum = array[0];
                for (int i = 0; i < array.Length; i++)
                {
                    if (maximum < array[i])
                    {
                        maximum = array[i];
                    }
                }

                int currentIndex = 0;
                for (int numberToTestFor = minimum; numberToTestFor <= maximum; numberToTestFor++)
                {
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (array[i] == numberToTestFor)
                        {
                            returnArray[currentIndex] = numberToTestFor;
                            currentIndex++;
                        }
                    }
                }
                return returnArray;
            }
            else
            {
                throw new Exception("the given array doesn't have numbers to order");
            }
        }

        private class PointOption
        {
            private const string notAnOptionText = " is not an option";

            private string name;
            private Func<int[],int> calculatePoints;
            private string textBeforePoints;
            private bool canBeUsedAgain;
            
            internal PointOption(string name, Func<int[],int> calculatePoints, string textBeforePoints,bool canBeUsedAgain)
            {
                this.name = name;
                this.calculatePoints = calculatePoints;
                this.textBeforePoints = textBeforePoints;
                this.canBeUsedAgain = canBeUsedAgain;
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
            internal bool CanBeUsedAgain()
            {
                return canBeUsedAgain;
            }
        }
    }
}
