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
        /// <summary>
        /// https://medium.com/delightlearner/c-development-%E7%85%A9%E4%BA%BA%E7%9A%84%E8%B7%A8%E5%9F%B7%E8%A1%8C%E7%B7%92%E4%BD%9C%E6%A5%AD%E7%84%A1%E6%95%88-%E5%A6%82%E4%BD%95%E8%B7%A8%E5%9F%B7%E8%A1%8C%E7%B7%92%E6%8E%A7%E5%88%B6-ui-%E5%85%83%E4%BB%B6-41c7b129f47a
        /// </summary>
        /// 煩人的跨執行緒作業無效-如何跨執行緒控制




        private Point _Point;
        private string rec = "";
        //private string ori = "";
        private bool _stopFishing = false;
        private int _cnt = 0;
        private int _fishingCount = 0;//紀錄釣魚成功幾次
        private int _fishingTarget = 0;//目標要釣幾條魚
        private int _fishingFailTarget = 3;//失敗幾次要中止
        private int _fishingFailCount = 0;//紀錄失敗次數
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
                txtFishTarget.Enabled = false;
                _stopFishing = false;
                _fishingTarget = int.Parse(txtFishTarget.Text);
                _fishingFailCount = 0;

                Thread thread = new Thread(FishingTask);
                thread.Start();
            }
            catch (Exception ex)
            {

            }


        }


        private void FishingTask()
        {
            while (!_stopFishing)
            {
                int count = 0;
                using (Capture capture = new Capture())
                {
                    //capture.LeftClick(new Point(1274, 617));//點擊動作
                }

                Thread.Sleep(3500);


                while (count < 10)
                {
                    if (CatchFish())
                    {
                        _fishingCount++;
                        if(_fishingCount>=_fishingTarget)
                        {
                            _stopFishing = true;
                        }
                        break;
                    }

                    if(count==9)//第9次嘗試還沒釣起來，記1次失敗
                    {
                        _fishingFailCount++;
                    }
                    Thread.Sleep(300);
                    count++;
                }
                Thread.Sleep(5000);

                if(_fishingFailCount==_fishingFailTarget)//失敗次數達到設定值，中止
                {
                    _stopFishing = true;
                }


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

        private bool CatchFish()
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
        private void btnStopFish_Click(object sender, EventArgs e)
        {
            txtFishTarget.Enabled = true;
            _stopFishing = true;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
