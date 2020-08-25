using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Opdracht7MaakEenYahtzeespel
{
    public partial class Form1 : Form
    {
        private const string exceptionUpdateDiceDoesntFullyExist = "ether diceButtons, diceButtonFunctions or diceLabels does not exist";
        private const string exceptionUpdateScoreOptionButtonsDoesntFullyExist = "ether scoreOptionButtons or scoreOptionButtonFunction does not exist";
        private const string rerollButtonString = "reroll";
        private const string messageBoxScoreOptionNotPossible = "the chosen option isn't possible";

        private const int buttonHeightLeftOver = 2;
        
        private int diceButtonWith;
        private int buttonHeight;
        private Button[] diceButtons;
        private Label[] diceLabels;
        private List<Dice> diceToPutAway;
        private ButtonFunction[] diceButtonFunctions;

        private Button rerollButton;
        private Label scoreBoard;

        private Button[] scoreOptionButtons;
        private ButtonFunction[] scoreOptionButtonFunction;

        private Yahtzee yahtzee;
        public Form1()
        {
            InitializeComponent();
            yahtzee = new Yahtzee();
            diceButtonWith = this.ClientRectangle.Width / (Yahtzee.GetNumberOfDice() + 1);
            buttonHeight = this.ClientRectangle.Height / (yahtzee.GetNumberOfScoreOptions() + 1 + buttonHeightLeftOver);
            diceToPutAway = new List<Dice>();

            UpdateDice();
            UpdateScoreOptionButtons();
            UpdateRerollButton();
            UpdateScoreBoard();
        }

        private void UpdateDice()
        {
            if (diceButtons == null && diceButtonFunctions == null && diceLabels == null)
            {
                diceButtons = new Button[Yahtzee.GetNumberOfDice()];
                diceLabels = new Label[diceButtons.Length];

                diceButtonFunctions = new ButtonFunction[diceButtons.Length];

                Action<Object> diceButtonFunction = (Object dice) =>
                {
                    if (typeof(Dice).IsInstanceOfType(dice))
                    {
                        bool found = false;
                        for(int i=0;i< diceToPutAway.Count; i++)
                        {
                            if (diceToPutAway[i] == dice)
                            {
                                for(int o=0;o< diceButtons.Length; o++)
                                {
                                    if (diceButtonFunctions[o].GetFunctionParameter() == dice)
                                    {
                                        diceButtons[o].Font = new Font(diceButtons[o].Font, FontStyle.Regular);
                                    }
                                }
                                diceToPutAway.RemoveAt(i);
                                found = true;
                                break;
                            }
                        }
                        if (found == false)
                        {
                            for (int i = 0; i < diceButtons.Length; i++)
                            {
                                if (diceButtonFunctions[i].GetFunctionParameter() == dice)
                                {
                                    diceButtons[i].Font = new Font(diceButtons[i].Font, FontStyle.Bold);
                                }
                            }
                            diceToPutAway.Add((Dice)dice);
                        }
                    }
                    else
                    {
                        throw new Exception("object given to the button function isn't of the type dice");
                    }
                };

                for (int i = 0; i < diceButtons.Length; i++)
                {
                    diceButtons[i] = new Button();
                    diceLabels[i] = new Label();

                    Point point = new Point(diceButtonWith * i, 0);

                    diceButtons[i].Location = point;
                    diceButtons[i].Width = diceButtonWith;
                    diceButtons[i].Height = buttonHeight;

                    diceLabels[i].Location = point;
                    diceLabels[i].Width = diceButtonWith;
                    diceLabels[i].Height = buttonHeight;

                    diceButtons[i].Click += new EventHandler(UseDiceButton);

                    diceButtonFunctions[i]=new ButtonFunction(diceButtons[i], diceButtonFunction, null);

                    Controls.Add(diceButtons[i]);
                }
            }
            else if (diceButtons == null || diceButtonFunctions == null || diceLabels == null)
            {
                throw new Exception(exceptionUpdateDiceDoesntFullyExist);
            }
            else
            {
                yahtzee.Reroll(diceToPutAway.ToArray());
                diceToPutAway.Clear();
                for (int i = 0; i < diceButtons.Length; i++)
                {
                    Controls.Remove(diceButtons[i]);
                    diceButtons[i].Font = new Font(diceButtons[i].Font, FontStyle.Regular);
                    Controls.Remove(diceLabels[i]);
                }
            }
            Dice[] rerollableDice = yahtzee.GetRerollableDice();
            int[] nonRerollableDice = yahtzee.GetNonrerollableDice();
            for(int i = 0; i < diceButtons.Length; i++)
            {
                if (i < rerollableDice.Length)
                {
                    diceButtons[i].Text = ""+rerollableDice[i].GetDieValue();
                    diceButtonFunctions[i].SetFunctionParameter(rerollableDice[i]);
                    Controls.Add(diceButtons[i]);
                }
                else
                {
                    diceLabels[i].Text = "" + nonRerollableDice[i - rerollableDice.Length];
                    Controls.Add(diceLabels[i]);
                }
            }
        }
        private void UpdateScoreOptionButtons()
        {
            if(scoreOptionButtons == null && scoreOptionButtonFunction == null)
            {
                scoreOptionButtons = new Button[yahtzee.GetNumberOfScoreOptions()];
                scoreOptionButtonFunction = new ButtonFunction[scoreOptionButtons.Length];

                Action<Object> scoreButtonFunction = (Object scoreOption) =>
                {
                    if (yahtzee.GetIfOption(scoreOption))
                    {
                        diceToPutAway.Clear();
                        yahtzee.UseScoreOption(scoreOption);
                        yahtzee.NextRound();
                        UpdateDice();
                        UpdateScoreOptionButtons();
                        UpdateScoreBoard();
                        UpdateRerollButton();
                    }
                    else
                    {
                        MessageBox.Show(messageBoxScoreOptionNotPossible);
                    }
                };

                for(int i=0;i< scoreOptionButtons.Length; i++)
                {
                    scoreOptionButtons[i] = new Button();
                    
                    Point point = new Point(0, buttonHeight * (i + 1));

                    scoreOptionButtons[i].Location = point;
                    scoreOptionButtons[i].Width = this.ClientRectangle.Width;
                    scoreOptionButtons[i].Height = buttonHeight;

                    scoreOptionButtons[i].Click += new EventHandler(UseScoreButton);

                    scoreOptionButtonFunction[i] = new ButtonFunction(scoreOptionButtons[i], scoreButtonFunction, null);

                    Controls.Add(scoreOptionButtons[i]);
                }
            }
            else if (scoreOptionButtons == null || scoreOptionButtonFunction == null)
            {
                throw new Exception(exceptionUpdateScoreOptionButtonsDoesntFullyExist);
            }
            else
            {
                for(int i = 0; i < scoreOptionButtons.Length; i++)
                {
                    Controls.Remove(scoreOptionButtons[i]);
                }
            }

            string[] scoreTexts = yahtzee.GetScoreOptionsText();
            object[] scoreOptions = yahtzee.GetScoreOptions();
            int numberOfButton = 0;
            for (int i = 0; i < scoreOptions.Length; i++)
            {
                if (scoreOptions[i] != null && numberOfButton < scoreOptionButtons.Length)
                {
                    scoreOptionButtons[numberOfButton].Text = scoreTexts[i];
                    scoreOptionButtons[numberOfButton].Location = new Point(0, buttonHeight * (numberOfButton + 1));
                    Controls.Add(scoreOptionButtons[numberOfButton]);
                    scoreOptionButtonFunction[numberOfButton].SetFunctionParameter(scoreOptions[i]);
                    numberOfButton++;
                }
            }
        }
        private void UpdateRerollButton()
        {
            if (rerollButton == null)
            {
                rerollButton = new Button();
                rerollButton.Text = rerollButtonString;
                rerollButton.Click += new EventHandler(rerollButtonFunction);
                rerollButton.Location = new Point(diceButtonWith * Yahtzee.GetNumberOfDice(), 0);
                rerollButton.Width = diceButtonWith;
                rerollButton.Height = buttonHeight;
            }

            Controls.Remove(rerollButton);
            if (yahtzee.GetCanReroll())
            {
                Controls.Add(rerollButton);
            }
        }
        private void UpdateScoreBoard()
        {
            if (scoreBoard == null)
            {
                scoreBoard = new Label();
                scoreBoard.Width = this.ClientRectangle.Width;
                Controls.Add(scoreBoard);
            }

            int extraRows = 0;
            object[] scoreOptionsToLookForNull = yahtzee.GetScoreOptions();
            foreach(object checkForNull in scoreOptionsToLookForNull)
            {
                if (checkForNull == null)
                {
                    extraRows++;
                }
            }
            scoreBoard.Height = buttonHeight * (buttonHeightLeftOver + extraRows);
            scoreBoard.Location = new Point(0, buttonHeight * (yahtzee.GetNumberOfScoreOptions() + 1 - extraRows));
            scoreBoard.Text = yahtzee.GetScoreBoardString();
        }

        private void rerollButtonFunction(object sender, EventArgs e)
        {
            UpdateDice();
            UpdateScoreOptionButtons();
            UpdateRerollButton();
        }
        private void UseDiceButton(object sender, EventArgs e)
        {
            foreach (ButtonFunction buttonFunction in diceButtonFunctions)
            {
                if (buttonFunction.IsSameButton(sender))
                {
                    buttonFunction.DoFunction();
                    break;
                }
            }
        }
        private void UseScoreButton(object sender, EventArgs e)
        {
            foreach (ButtonFunction buttonFunction in scoreOptionButtonFunction)
            {
                if (buttonFunction.IsSameButton(sender))
                {
                    buttonFunction.DoFunction();
                    break;
                }
            }
        }
    }
}
