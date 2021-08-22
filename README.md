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
# 案例一：仅做OCR检测

   Bitmap bmp = new Bitmap("D:\\1.jpg");
            Bitmap b = new Bitmap(bmp);
            bmp.Dispose();
            InferManager infer = new InferManager("config.txt",true,false);
            var result = infer.Detect("D:\\1.jpg");
            pictureBox1.Image =  infer.DrawImage(b,result);
            infer.Dispose();
