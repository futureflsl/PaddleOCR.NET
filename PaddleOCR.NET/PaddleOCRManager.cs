using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FIRC {
    public class PaddleOCRManager
    {
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "Detect", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr Detect(byte[] input, int height, int width,IntPtr result);  //out string
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "Detect_image", SetLastError = true)]
    public static extern IntPtr DetectImage(IntPtr ptr, int size);    
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "DetectAndRecognize", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr DetectAndRecognize(byte[] input, int height, int width);  //out string
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "DetectAndRecognize_image", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr DetectAndRecognize_image(IntPtr input, int size); //out string
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "RecognizeLine", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr RecognizeLine(byte[] input, int height, int width);  //out string
   [DllImport("PaddleOCRSDK.dll", EntryPoint = "RecognizeLine_image", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr RecognizeLine_image(IntPtr data, int size);  //out string
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "Dispose", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern void Dispose();
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "GetCopyrightInfo", SetLastError = true,CharSet =CharSet.Unicode)]
    public static extern IntPtr GetCopyrightInfo();
    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <param name="filepath"></param>
    [DllImport("PaddleOCRSDK.dll", EntryPoint = "LoadConfig", SetLastError = true)]
    public static extern void LoadConfig(string filepath,bool bDet,bool bRec);  //out void
        /// <summary>
        /// 检测文字区域，返回字符串格式，为4个分号为一组，比如
        /// 20,25;158,25;158,52;20,52;
        /// </summary>
        /// <param name="imageData"></param>
        /// <returns></returns>
        public static string DetectOnly(byte[] imageData)

        {

            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);
            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                IntPtr result =DetectImage(pnt, imageData.Length);
                var resultStr = Marshal.PtrToStringAnsi(result);
                Console.WriteLine("检测结果："+resultStr);
                return resultStr;


            }

            catch (Exception ex)

            {

                Console.WriteLine("[ERROR]:" + ex.Message);
                return null;

            }

            finally

            {

                // Free the unmanaged memory.

                Marshal.FreeHGlobal(pnt);

            }

        }
            public static string DetectAndRecognize(byte[] imageData)

            {

                var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
                var pnt = Marshal.AllocHGlobal(size);
                try
                {
                    // Copy the array to unmanaged memory.
                    Marshal.Copy(imageData, 0, pnt, imageData.Length);
                    IntPtr result = DetectAndRecognize_image(pnt, imageData.Length);
                    var resultStr = Marshal.PtrToStringAnsi(result);
                    return resultStr;


                }

                catch (Exception ex)

                {

                    Console.WriteLine("[ERROR]:" + ex.Message);
                    return null;

                }

                finally

                {

                    // Free the unmanaged memory.

                    Marshal.FreeHGlobal(pnt);

                }


            }

        public static string RecognizeOnly(byte[] imageData)

        {

            var size = Marshal.SizeOf(imageData[0]) * imageData.Length;
            var pnt = Marshal.AllocHGlobal(size);
            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(imageData, 0, pnt, imageData.Length);
                IntPtr result = RecognizeLine_image(pnt, imageData.Length);
                var resultStr = Marshal.PtrToStringAnsi(result);
                return resultStr;


            }

            catch (Exception ex)

            {

                Console.WriteLine("[ERROR]:" + ex.Message);
                return null;

            }

            finally

            {

                // Free the unmanaged memory.

                Marshal.FreeHGlobal(pnt);

            }


        }



    }
}
