using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {
        // create a Random object to facilitate choosing random icons for the squares
        Random random = new Random();

        // create a list with duplicate entries for each symbol (in Webdings) that needs to be matched
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z"
        };

        // reference variables for first and second Label controls that have been clicked by user, 
        // initially null until something is clicked
        Label firstClicked = null;
        Label secondClicked = null;

        /// <summary>
        /// assign each letter/icon from the list icons to a random square in the layout 
        /// </summary>
        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                // conversion of control to Label
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquares();
        }

        /// <summary>
        /// event handler for every label's click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // if the label color is black then an already revealed icon has been clicked, continue
                if (clickedLabel.ForeColor == Color.Black)
                    return;
                // if firstClicked is null, then this is the first label to be clicked
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }

                // if this part is reached it means firstClicked is not null and timer is not running
                // so this must be the second icons that is being clicked.
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // check if game has been won by player
                CheckForWinner();

                // if two icons match then they should not be hidden again
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                timer1.Start();
                
            }
        }

        /// <summary>
        /// checks every icon to see if it is matched by comparing its foreground color to its background
        /// color. If all of the icons are matched, the player wins the game.
        /// </summary>
        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                // if an icon has a mismatching ForeColor and BackColor it means the game is not over yet.
                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            // if it gets pas the foreach loop it means all icons were matched.
            MessageBox.Show("All icons were matched. Good job!", "Congratulations");
            Close();
        }

        /// <summary>
        /// timer starts when player clicks two icons that don't match, hide both icons and stops timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // stop timer
            timer1.Stop();

            // hide both button by changing ForeColors
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // reset firstClicked and secondClicked to allow for selection new labels
            firstClicked = null;
            secondClicked = null;
        }
    }
}
