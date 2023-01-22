using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Media;

namespace MachingGame_shevchenko
{
    public partial class Form1 : Form
    {
        // firstClicked соответсвует первому Label control 
        // который нажмет пользователь, но его значение будет null 
        // если пользователь еще не нажал label
        Label firstClicked = null;

        // secondClicked 
        Label secondClicked = null;

        // Используем этот Random object чтобы делать рандомные иконки для квадратиков
        Random random = new Random();

        // Each of these letters is an interesting icon
        // in the Webdings font,
        // каждая иконка появляется в листе дважды
        List<string> icons = new List<string>()
    {
        "a", "a", "n", "n", "?", "?", "L", "L",
        "o", "o", "3", "3", "A", "A", "i", "i",
        "<", "<", "j", "j"
    };

        //переменная считает время
        int timeSpend;

        //sound
        private SoundPlayer _soundPlayer;

        private void AssignIconsToSquares()
        {
            // На TableLayoutPanel находится 16 labels,
            // и лист с иконками содержит 16 icons,
            // так что icon is pulled at random from the list
            // and added to each label
            foreach (Control control in tableLayoutPanel1.Controls)
            {
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

            _soundPlayer= new SoundPlayer("zvuk-telegram-uvedomlenie-v-telegram-30454.wav");

            AssignIconsToSquares();

            StartTimer();
        }

        /// Каждый label's Click event обрабатывается этим обработчиком событий 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            // проверяет, запущен ли таймер, обращаясь к значению
            // свойству Enabled
            // The timer is only on after two non-matching 
            // icons have been shown to the player, 
            // so ignore any clicks if the timer is running
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                // If firstClicked is null, this is the first icon 
                // in the pair that the player clicked,
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;

                    return;
                }
                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second icon the player clicked
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                // Check to see if the player won
                CheckForWinner();


                // If the player clicked two matching icons, keep them 
                // black and reset firstClicked and secondClicked 
                // so the player can click another icon
                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    // включить звук
                    _soundPlayer.Play();
                    return;
                }


                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                timer1.Start();

                //Теперь форма готова к тому, чтобы игрок выбрал другую пару значков.
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Спрятать обе иконки
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // Переустановить firstClicked и secondClicked 
            // так чтобы в следующий раз когда label 
            // нажат, программа знала it's the first click
            firstClicked = null;
            secondClicked = null;
        }
        /// Check every icon to see if it is matched, by 
        /// comparing its foreground color to its background color. 
        /// If all of the icons are matched, the player wins
        /// </summary>
        private void CheckForWinner()
        {
            // Проверяет  все labels в TableLayoutPanel, 
            // checking each one matched или нет
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            //Если все норм выводит "Congratulations"
            // If the loop didn’t return, it didn't find
            // any unmatched icons
            // That means the user won. Show a message and close the form
            timer2.Stop();
            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();

        }

        private void StartTimer()
        {
            timeSpend = 0;
            timeLabel.Text = "0 seconds";
            timer2.Start();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timeSpend = timeSpend +1;
            timeLabel.Text = timeSpend + " seconds";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
