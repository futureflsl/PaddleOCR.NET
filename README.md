# PaddleOCR.NET
# 简介
## 开发环境
- windows10 x64
- VS2019专业版
- paddle_inference==2.1.1 cpu_avx_mkl
- PaddleOCR-release-2.2
- cmake==3.17.2
- NET Framework4.5

# 使用教程

- 1、下载paadleor推理引擎：https://paddle-wheel.bj.bcebos.com/2.1.1/win-infer/mkl/cpu/paddle_inference.zip，并解压所有的DLL文件到自己运行目录

- 2、引用PaddleOCR.NET到自己项目中，编写代码
- 
# 案例一：仅做OCR检测，支持byte[],图片路径,和Bitmap，如果使用opencvsharp也可以扩展

   Bitmap bmp = new Bitmap("D:\\1.jpg");  
   Bitmap b = new Bitmap(bmp);  
   bmp.Dispose();  
   InferManager infer = new InferManager("config.txt",true,false);  
   var result = infer.Detect("D:\\1.jpg");  
   pictureBox1.Image =  infer.DrawImage(b,result);  
   infer.Dispose();
   
# 案例二：仅做OCR识别，单文本图片识别
   InferManager infer = new InferManager("config.txt", false, true);  
   Bitmap bmp = new Bitmap("D:\\line.jpg");  
   var result = infer.RecognizeOnly(bmp);  
   infer.Dispose();  
   MessageBox.Show(result.Text+"|"+result.Score);
  
 # 案例三：对图片所有文本检测ocr检测和识别，并返回json数据格式
     InferManager infer = new InferManager("config.txt", true, true);  
     var result = infer.DetectAndRecognize("D:\\22.jpg");  
     Console.WriteLine(result);  
     infer.Dispose();
     
## 注意：所有代码在VS2019 x64 release测试通过，识别和检测加入20%概率的不检测提示，个人或者商业授权请关注微信公众号未来自主研究中心获取


