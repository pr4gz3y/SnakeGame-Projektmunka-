using Snake_Game_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Game_Project
{
    public partial class Form1 : Form
    {
        private List<Kör> Snake = new List<Kör>();
        private Kör food = new Kör();
        int maxWidth;
        int maxHeight;
        int score;
        int highScore;
        Random rand = new Random();
        bool goLeft, goRight, goDown, goUp;

        public Form1()
        {
            InitializeComponent();

            new Beállítások();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Beállítások.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Beállítások.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Beállítások.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Beállítások.directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartJatek(object sender, EventArgs e)
        {
            RestartJatek();
        }

        private void FrissitPicBox(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColour;
            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColour = Brushes.Black;
                }
                else
                {
                    snakeColour = Brushes.MediumPurple;
                }
                canvas.FillEllipse(snakeColour, new Rectangle
                    (
                    Snake[i].x * Beállítások.Width,
                    Snake[i].y * Beállítások.Height,
                    Beállítások.Width, Beállítások.Height
                    ));
            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle
            (
            food.x * Beállítások.Width,
            food.y * Beállítások.Height,
            Beállítások.Width, Beállítások.Height
            ));
        }

        private void KepernyoFoto(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = " Az elért pontszámom: " + score + " és a legtöbb elért pontszámom: " + highScore;
            caption.Font = new Font("Ariel", 12, FontStyle.Bold);
            caption.ForeColor = Color.Black;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Snake Game Képernyőfotó(saját)";
            dialog.DefaultExt = "jpg";
            dialog.Filter = "JPG Image File | *.jpg";
            dialog.ValidateNames = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(width, height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void JatekIdozito(object sender, EventArgs e)
        {
            if (goLeft)
            {
                Beállítások.directions = "left";
            }
            if (goRight)
            {
                Beállítások.directions = "right";
            }
            if (goDown)
            {
                Beállítások.directions = "down";
            }
            if (goUp)
            {
                Beállítások.directions = "up";
            }
            // end of directions
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Beállítások.directions)
                    {
                        case "left":
                            Snake[i].x--;
                            break;
                        case "right":
                            Snake[i].x++;
                            break;
                        case "down":
                            Snake[i].y++;
                            break;
                        case "up":
                            Snake[i].y--;
                            break;
                    }
                    if (Snake[i].x < 0)
                    {
                        Snake[i].x = maxWidth;
                    }
                    if (Snake[i].x > maxWidth)
                    {
                        Snake[i].x = 0;
                    }
                    if (Snake[i].y < 0)
                    {
                        Snake[i].y = maxHeight;
                    }
                    if (Snake[i].y > maxHeight)
                    {
                        Snake[i].y = 0;
                    }
                    if (Snake[i].x == food.x && Snake[i].y == food.y)
                    {
                        EatEtel();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            JatekVege();
                        }
                    }
                }
                else
                {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
            }
            picCanvas.Invalidate();
        }

        private void RestartJatek()
        {
            maxWidth = picCanvas.Width / Beállítások.Width - 1;
            maxHeight = picCanvas.Height / Beállítások.Height - 1;
            Snake.Clear();
            startButton.Enabled = false;
            snapButton.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;
            Kör head = new Kör { x = 10, y = 5 };
            Snake.Add(head); //A kigyó fej része itt kerül hozzáadásra
            for (int i = 0; i < 10; i++)
            {
                Kör body = new Kör();
                Snake.Add(body);
            }
            food = new Kör { x = rand.Next(2, maxWidth), y = rand.Next(2, maxHeight) };
            gameTimer.Start();
        }

        private void EatEtel()
        {
            score += 1;
            txtScore.Text = "Score: " + score;
            Kör body = new Kör
            {
                x = Snake[Snake.Count - 1].x,
                y = Snake[Snake.Count - 1].y
            };
            Snake.Add(body);
            food = new Kör { x = rand.Next(2, maxWidth), y = rand.Next(2, maxHeight) };
        }

        private void JatekVege()
        {
            gameTimer.Stop();
            startButton.Enabled = true;
            snapButton.Enabled = true;
            if (score > highScore)
            {
                highScore = score;
                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
    }
}
