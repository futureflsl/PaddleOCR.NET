using FIRC;
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

namespace PaddleOCRTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap("D:\\1.jpg");
            Bitmap b = new Bitmap(bmp);
            bmp.Dispose();
            InferManager infer = new InferManager("config.txt",true,false);
            var result = infer.Detect("D:\\1.jpg");
            pictureBox1.Image =  infer.DrawImage(b,result);
            infer.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InferManager infer = new InferManager("config.txt", false, true);
            Bitmap bmp = new Bitmap("D:\\line.jpg");
            var result = infer.RecognizeOnly(bmp);
            infer.Dispose();
            MessageBox.Show(result.Text+"|"+result.Score);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InferManager infer = new InferManager("config.txt", true, true);
            var result = infer.DetectAndRecognize("D:\\22.jpg");
            Console.WriteLine(result);
            infer.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(InferManager.GetCopyrightInfo());
        }
    }
}
