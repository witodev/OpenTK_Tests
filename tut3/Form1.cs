using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tut3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Start");

            using (var game = new Game())
            {
                game.Run(30, 30);
            }
        }

        private Color GetColorFromTemp(double curTemp, double min = 0, double max = 99)
        {
            Bitmap temps = (Bitmap)Properties.Resources.temp;

            if (curTemp <= min)
                return temps.GetPixel(0, 10);
            else if (curTemp >= max)
                return temps.GetPixel(99, 10);
            else //if (curTemp>min && curTemp < max)
            {
                var x = max - min;
                var y = (curTemp-min) / x;
                var z = (int)Math.Round(y * 99);
                return temps.GetPixel(z, 10);
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = double.Parse(textBox1.Text);
            BackColor = GetColorFromTemp(t,0,100);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}
