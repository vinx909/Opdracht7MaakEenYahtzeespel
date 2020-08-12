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
        private const int buttonHeightLeftOver = 2;

        private int diceButtonWith;
        private int buttonHeight;
        private Button[] diceButtons;
        private Label[] diceLabels;
        private List<Dice> diceToPutAway;
        private ButtonFunction[] diceButtonFunctions;

        private Button[] scoreOptionButtons;
        private ButtonFunction[] scoreOptionButtonFunction;

        private Yahtzee yahtzee;
        public Form1()
        {
            InitializeComponent();
            yahtzee = new Yahtzee();
            diceButtonWith = this.ClientRectangle.Width / Yahtzee.GetNumberOfDice();
            buttonHeight = this.ClientRectangle.Height / yahtzee.GetNumberOfScoreOptions() + 1 + buttonHeightLeftOver;
            diceToPutAway = new List<Dice>();

            UpdateDice();
        }

        private void UpdateDice()
        {
            if (diceButtons == null && diceButtonFunctions == null && diceLabels == null && scoreOptionButtons==null && scoreOptionButtonFunction == null)
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

                scoreOptionButtons = new Button[yahtzee.GetNumberOfScoreOptions()];
                scoreOptionButtonFunction = new ButtonFunction[scoreOptionButtons.Length];
            }
            else if (diceButtons == null || diceButtonFunctions == null || diceLabels == null || scoreOptionButtons == null || scoreOptionButtonFunction == null)
            {
                throw new Exception("ether diceButtons, diceButtonFunctions or diceLabels does not exist");
            }
            else
            {
                yahtzee.Reroll(diceToPutAway.ToArray());
                diceToPutAway.Clear();
                for (int i = 0; i < diceButtons.Length; i++)
                {
                    Controls.Remove(diceButtons[i]);
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

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateDice();
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
    }
}
