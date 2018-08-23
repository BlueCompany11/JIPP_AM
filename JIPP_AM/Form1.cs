using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JIPP_AM
{
    public partial class Form1 : Form
    {
        int dx;
        int dy;
        int dxDest;
        int dyDest;
        System.Timers.Timer timer;
        delegate void ReachDestination();
        event ReachDestination DestinationReached;
        int pictureWidth = 100;
        int pictureHeight = 100;
        public Form1()
        {
            InitializeComponent();
            //timer, jesli wlaczony bedzie trigerowal zdarzenie elapsed co 30 ms
            timer = new System.Timers.Timer(30);
            //dopisuje metode do zdarzenia
            timer.Elapsed += Timer_Elapsed;
            //dopisuje do wlasnego zdarzenia jakim jest DestinationReached metode lambda ktora wylaczy mi stoper
            //wyrazenie lambda mozna traktowac jako metode anonimowa
            //alternatywa dla tego zapisu byloby zdefiniowanie funkcji ktora zwraca void i nie przyjmuje zadnych argumentow
            DestinationReached += (() => 
            {
                timer.Enabled = false;
            });
            // druga reakcja na zdarzenie dotarcia do punktu przeznaczenia jest wyswietlenie obrazka do gory nogami
            DestinationReached += RotatePicture;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MoveToTheDirection();
            //sprawdzamy czy powinnismy wywolac zdarzenie DestinationReached
            if (dx == dxDest && dy == dyDest)
            {
                DestinationReached();
            }
        }
        //metoda wywolywana przy wcisnieciu myszki
        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Clicks == 1)
            {
                Console.WriteLine(e.Location.X.ToString() + " " + e.Location.Y.ToString());
                //pobieram lokalizacje gdzie zostala wcisnieta myszka
                //odejmowanie ma spowodowac, ze obrazek poruszy sie w swoje centrum
                dxDest = e.Location.X - pictureWidth / 2;
                dyDest = e.Location.Y - pictureHeight / 2;
                timer.Enabled = true;
            }
        }
        private void MoveToTheDirection()
        {
            if (dx < dxDest)
            {
                dx++;
            }
            else if (dx > dxDest)
            {
                dx--;
            }
            if (dy < dyDest)
            {
                dy++;
            }
            else if (dy > dyDest)
            {
                dy--;
            }
            Draw();
        }
        void Draw()
        {
            Graphics graphics = tableLayoutPanel1.CreateGraphics();
            graphics.Clear(tableLayoutPanel1.BackColor);
            //TRZEBA PODMIENIC SCIEZKE
            Image image = Image.FromFile(@"C:\Users\BlueCompany\source\repos\JIPP_AM\JIPP_AM\person.png");
            var bmp = new Bitmap(image, new Size(pictureWidth, pictureHeight));
            graphics.DrawImage(bmp, dx, dy);
        }

        void RotatePicture()
        {
            Graphics graphics = tableLayoutPanel1.CreateGraphics();
            graphics.Clear(tableLayoutPanel1.BackColor);
            //TRZEBA PODMIENIC SCIEZKE
            Image image = Image.FromFile(@"C:\Users\BlueCompany\source\repos\JIPP_AM\JIPP_AM\person.png");
            var bmp = new Bitmap(image, new Size(pictureWidth, pictureHeight));
            bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            graphics.DrawImage(bmp, dx, dy);
        }
    }
}
