using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;


namespace FIRC
{
    public class InferManager
    {
        public InferManager(string configFile, bool bDet, bool bRec)
        {
            PaddleOCRManager.LoadConfig(configFile, bDet, bRec);
        }
        public List<BoxLine> Detect(byte[] imageData)
        {
            List<BoxLine> boxLineList = new List<BoxLine>();
            var result = PaddleOCRManager.DetectOnly(imageData);
            if (string.IsNullOrEmpty(result))
            {
                return boxLineList;
            }
            result = result.Substring(0, result.Length - 1);
            var pointStr = result.Split(';');
            for (int i = 0; i < pointStr.Length / 4; i++)
            {
                BoxLine bl = new BoxLine();
                var LeftUp = pointStr[4 * i].Split(',');
                bl.LeftUp.X = Convert.ToInt32(LeftUp[0]);
                bl.LeftUp.Y = Convert.ToInt32(LeftUp[1]);

                var RightUp = pointStr[4 * i + 1].Split(',');
                bl.RightUp.X = Convert.ToInt32(RightUp[0]);
                bl.RightUp.Y = Convert.ToInt32(RightUp[1]);

                var RightDown = pointStr[4 * i + 2].Split(',');
                bl.RightDown.X = Convert.ToInt32(RightDown[0]);
                bl.RightDown.Y = Convert.ToInt32(RightDown[1]);

                var LeftDown = pointStr[4 * i + 3].Split(',');
                bl.LeftDown.X = Convert.ToInt32(LeftDown[0]);
                bl.LeftDown.Y = Convert.ToInt32(LeftDown[1]);
                boxLineList.Add(bl);
            }

            return boxLineList;

        }
     
        public List<BoxLine> Detect(string fileName)
        {
            var bytes = File.ReadAllBytes(fileName);
            return Detect(bytes);

        }
        public static string GetCopyrightInfo()
        {
          
           IntPtr info = PaddleOCRManager.GetCopyrightInfo();
            var data= Marshal.PtrToStringAnsi(info);
            return data;
        }
        private byte[] Bitmap2Byte(Bitmap bitmap)
        {
            byte[] data = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));

            }
            return data;
        }
        public List<BoxLine> Detect(Bitmap bmp)
        {
            var bytes = Bitmap2Byte(bmp);
            return Detect(bytes);

        }
        ///// <summary>
        ///// opencvsharp Mat
        ///// </summary>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //public List<BoxLine> Detect(Mat image)
        //{
        //    return Detect(image.tobytes());

        //}
  
        private PointF[] BoxLineToPoinF(BoxLine boxLine)
        {
            List<PointF> ps = new List<PointF>();
            var ps1 = new PointF();
            ps1.X = boxLine.LeftUp.X;
            ps1.Y = boxLine.LeftUp.Y;
            ps.Add(ps1);
            var ps2 = new PointF();
            ps2.X = boxLine.RightUp.X;
            ps2.Y = boxLine.RightUp.Y;
            ps.Add(ps2);
            var ps3 = new PointF();
            ps3.X = boxLine.RightDown.X;
            ps3.Y = boxLine.RightDown.Y;
            ps.Add(ps3);
            var ps4 = new PointF();
            ps4.X = boxLine.LeftDown.X;
            ps4.Y = boxLine.LeftDown.Y;
            ps.Add(ps4);
            return ps.ToArray();

        }
        public Bitmap DrawImage(Bitmap bmp, List<BoxLine> boxLineList)
        {
            Pen pen = new Pen(Color.Blue);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for(int i=0; i<boxLineList.Count;i++)
                {
                    g.DrawLine(pen,boxLineList[i].LeftUp.X, boxLineList[i].LeftUp.Y, boxLineList[i].RightUp.X, boxLineList[i].RightUp.Y);
                    g.DrawLine(pen, boxLineList[i].RightUp.X, boxLineList[i].RightUp.Y, boxLineList[i].RightDown.X, boxLineList[i].RightDown.Y);
                    g.DrawLine(pen, boxLineList[i].LeftDown.X, boxLineList[i].LeftDown.Y, boxLineList[i].RightDown.X, boxLineList[i].RightDown.Y);
                    g.DrawLine(pen, boxLineList[i].LeftUp.X, boxLineList[i].LeftUp.Y, boxLineList[i].LeftDown.X, boxLineList[i].LeftDown.Y);
                }
               
            }
            return bmp;
        }
        public RecResult RecognizeOnly(byte[] imageData)
        {
            RecResult rr = new RecResult();
            var result = PaddleOCRManager.RecognizeOnly(imageData);
            if(string.IsNullOrEmpty(result))
            {
                return rr;
            }
            var data = result.Split(';');
            rr.Text = data[0];
            rr.Score= Convert.ToSingle(data[1]);
            return rr;
        }
        public string DetectAndRecognize(byte[] imageData)
        {
            return PaddleOCRManager.DetectAndRecognize(imageData);
        }
        public string DetectAndRecognize(string fileName)
        {
            var bytes = File.ReadAllBytes(fileName);
            return PaddleOCRManager.DetectAndRecognize(bytes);
        }
        public string DetectAndRecognize(Bitmap bmp)
        {
            var bytes = Bitmap2Byte(bmp);
            return PaddleOCRManager.DetectAndRecognize(bytes);
        }
        public RecResult RecognizeOnly(string fileName)
        {
            RecResult rr = new RecResult();
            var bytes = File.ReadAllBytes(fileName);
            var result = PaddleOCRManager.RecognizeOnly(bytes);
            if (string.IsNullOrEmpty(result))
            {
                return rr;
            }
            var data = result.Split(';');
            rr.Text = data[0];
            rr.Score = Convert.ToSingle(data[1]);
            return rr;
        }
        public RecResult RecognizeOnly(Bitmap bmp)
        {
            RecResult rr = new RecResult();
            var bytes = Bitmap2Byte(bmp);
            var result = PaddleOCRManager.RecognizeOnly(bytes);
            if (string.IsNullOrEmpty(result))
            {
                return rr;
            }
            var data = result.Split(';');
            rr.Text = data[0];
            rr.Score = Convert.ToSingle(data[1]);
            return rr;
        }
        public void Dispose()
        {
            PaddleOCRManager.Dispose();
        }

    }

    public class BoxLine
    {
        public Point LeftUp = new Point();
        public Point RightUp = new Point();
        public Point LeftDown = new Point();
        public Point RightDown = new Point();

    }
    public class RecResult
    {
        public string Text = null;
        public float Score = 0f;

    }


}
