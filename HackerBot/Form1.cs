using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HackerBot
{
    public partial class Form1 : Form
    {

        private Point _Point;
        private string rec = "";
        //private string ori = "";
        private bool _stopFishing = false;
        private int _cnt = 0;
        private int _fishingCount = 0;//紀錄釣魚成功幾次
        private int _fishingTarget = 0;//目標要釣幾條魚
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Capture capture = new Capture();
            var res = capture.CaptureScreen(new Point(0, 0), new Size(1560, 885));


            e.Graphics.DrawImage(res, new PointF(0, 0));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            label2.Text = e.X.ToString() + "/" + e.Y.ToString();
            _Point = new Point(e.X, e.Y);
            pictureBox2.Refresh();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Capture capture = new Capture();
            var res = capture.CaptureScreen(_Point, new Size(100, 100));

            e.Graphics.DrawImage(res, new PointF(0, 0));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Capture capture = new Capture();
            //capture.LeftClick(int.Parse(textBox1.Text),int.Parse(textBox2.Text));
        }

        private void Miner_Click(object sender, EventArgs e)
        {
            Capture capture = new Capture();
            capture.LeftClick(new Point(1440, 309));//打開清單
            Thread.Sleep(500);
            capture.LeftClick(new Point(1304, 582));//點擊生活
            Thread.Sleep(500);
            capture.LeftClick(new Point(319, 608));//點選採礦
            Thread.Sleep(500);
            capture.LeftClick(new Point(885, 420));//礦脈lv1
            Thread.Sleep(300000);
            capture.LeftClick(new Point(935, 338));//點擊動作
            Thread.Sleep(500);
            capture.LeftClick(new Point(1274, 617));//點擊動作
            Thread.Sleep(500);
        }

        private void Fishing_Click(object sender, EventArgs e)
        {
            try
            {
                _stopFishing = false;
                _fishingTarget = int.Parse(textBox3.Text);
                Thread thread = new Thread(MyBackgroundTask);
                thread.Start();
            }
            catch (Exception ex)
            {

            }


        }


        private void MyBackgroundTask()
        {
            while (!_stopFishing)
            {
                int count = 0;
                using (Capture capture = new Capture())
                {
                    capture.LeftClick(new Point(1274, 617));//點擊動作
                }

                Thread.Sleep(3500);


                while (count < 10)
                {
                    if (Accuracy_Tick())
                    {
                        _fishingCount++;
                        if(_fishingCount>=_fishingTarget)
                        {
                            _stopFishing = true;
                        }
                        break;
                    }
                    Thread.Sleep(300);
                    count++;
                }
                Thread.Sleep(5000);
            }

        }


        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            Capture capture = new Capture();
            var bitmap = capture.CaptureScreen(new Point(1280, 590), new Size(100, 100));
            var c = capture.GetPixel(bitmap);
            rec += c.ToArgb().ToString() + "/";


            e.Graphics.DrawImage(bitmap, new PointF(0, 0));
        }

        private bool Accuracy_Tick()
        {
            using (Capture capture = new Capture())
            {

                var bitmap = capture.CaptureScreen(new Point(1280, 590), new Size(1, 1));
                var c = capture.GetPixel(bitmap);

                int colorFlag = 230;//判斷的顏色

                if (int.Parse(c.G.ToString())>colorFlag)
                {
                    bitmap.Dispose();
                    capture.LeftClick(new Point(1274, 617));//點擊動作
                    return false;
                }
                
                bitmap.Dispose();
                return false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _stopFishing = true;
        }
    }
}
