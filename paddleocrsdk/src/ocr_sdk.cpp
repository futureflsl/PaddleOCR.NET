
#include "glog/logging.h"
#include "omp.h"
#include "opencv2/core.hpp"
#include "opencv2/imgcodecs.hpp"
#include "opencv2/imgproc.hpp"
#include <chrono>
#include <iomanip>
#include <iostream>
#include <ostream>
#include <vector>

#include <string>
#include <locale>
#include <codecvt>

#include <cstring>
#include <fstream>
#include <numeric>
#include <include/config.h>
#include <include/ocr_det.h>
#include <include/ocr_rec.h>

using namespace std;
using namespace cv;
using namespace PaddleOCR;
#define WINAPI __declspec(dllexport)

Classifier* cls = nullptr;
CRNNRecognizer* rec = nullptr;
DBDetector* det = nullptr;
bool useDet=true;
bool useRec=true;
bool useCls=false;


std::string wstr2utf8str(const std::wstring& str)
{
    static std::wstring_convert<std::codecvt_utf8<wchar_t> > strCnv;
    return strCnv.to_bytes(str);
}


std::wstring utf8str2wstr(const std::string& str)
{
    static std::wstring_convert< std::codecvt_utf8<wchar_t> > strCnv;
    return strCnv.from_bytes(str);
}


std::string wstr2str(const std::wstring& str, const std::string& locale)
{
    typedef std::codecvt_byname<wchar_t, char, std::mbstate_t> F;
    static std::wstring_convert<F> strCnv(new F(locale));
    return strCnv.to_bytes(str);
}


std::wstring str2wstr(const std::string& str, const std::string& locale)
{
    typedef std::codecvt_byname<wchar_t, char, std::mbstate_t> F;
    static std::wstring_convert<F> strCnv(new F(locale));
    return strCnv.from_bytes(str);
}



extern "C" WINAPI void  LoadConfig(const char* filepath,const bool bDet,const bool  bRec);
WINAPI void  LoadConfig(const char* filepath, const bool bDet, const bool  bRec)
{
    OCRConfig config(filepath);
    config.PrintConfigInfo();

    if (config.use_angle_cls == true) {
        useCls=config.use_angle_cls;
        cls = new Classifier(config.cls_model_dir, config.use_gpu, config.gpu_id,
            config.gpu_mem, config.cpu_math_library_num_threads,
            config.use_mkldnn, config.cls_thresh,
            config.use_tensorrt, config.use_fp16);
    }
    useDet=bDet;
    if(bDet == true)
    {
    det =new DBDetector(config.det_model_dir, config.use_gpu, config.gpu_id,
        config.gpu_mem, config.cpu_math_library_num_threads,
        config.use_mkldnn, config.max_side_len, config.det_db_thresh,
        config.det_db_box_thresh, config.det_db_unclip_ratio,
        config.use_polygon_score, config.visualize,
        config.use_tensorrt, config.use_fp16);
    }
    useRec=bRec;
    if(bRec==true)
    {
    rec = new CRNNRecognizer(config.rec_model_dir, config.use_gpu, config.gpu_id,
        config.gpu_mem, config.cpu_math_library_num_threads,
        config.use_mkldnn, config.char_list_file,
        config.use_tensorrt, config.use_fp16);
    }

}


extern "C" WINAPI void Detect(char* input, int width, int height, std::string& detresult); //__declspec(dllexport)
WINAPI void Detect(char* input, int width, int height,std::string& detresult)
{
    cv::Mat srcimg(height, width, CV_8UC3, input);
    det->Run(srcimg, detresult);
}

extern "C" WINAPI char* Detect_image(const char *data, const size_t data_length); //__declspec(dllexport)
WINAPI char* Detect_image(const char *data, const size_t data_length)
{
    std::vector<char> vdata(data, data + data_length);
    cv::Mat srcimg = imdecode(cv::Mat(vdata), 1);
    std::string detresult;
    det->Run(srcimg, detresult);
    auto output = (char*)detresult.data();
    return output;
}
extern "C" WINAPI char* RecognizeLine(char* input, int width, int height); //__declspec(dllexport)
WINAPI char* RecognizeLine(char* input, int width, int height)
{
    cv::Mat srcimg(height, width, CV_8UC3, input);
    std::string recresult;
    rec->Run(recresult, srcimg, cls);
    auto output = (char*)recresult.data();
    return output;
}
extern "C" WINAPI char* RecognizeLine_image(const char *data, const size_t data_length); //__declspec(dllexport)
WINAPI char* RecognizeLine_image(const char *data, const size_t data_length)
{
    std::vector<char> vdata(data, data + data_length);
    cv::Mat srcimg = imdecode(cv::Mat(vdata), 1);
    std::string recresult;
    rec->Run(recresult, srcimg, cls);
    auto output = (char*)recresult.data();
    return output;
}
extern "C" WINAPI char* DetectAndRecognize(char* input, int width, int height); //__declspec(dllexport)
WINAPI char* DetectAndRecognize(char* input, int width, int height)
{
    cv::Mat srcimg(height, width, CV_8UC3, input);
    std::string recresult;
    std::vector<std::vector<std::vector<int>>> boxes;
    det->Run(srcimg, boxes);
    rec->Run(recresult,boxes, srcimg, cls);
    auto output = (char*)recresult.data();
    return output;
}
extern "C" WINAPI char* DetectAndRecognize_image(const char* data, const size_t data_length); //__declspec(dllexport)
WINAPI char* DetectAndRecognize_image(const char *data, const size_t data_length)
{
    std::vector<char> vdata(data, data + data_length);
    cv::Mat srcimg = imdecode(cv::Mat(vdata), 1);
    std::string recresult;
    std::vector<std::vector<std::vector<int>>> boxes;
    det->Run(srcimg, boxes);
    rec->Run(recresult,boxes, srcimg, cls);
    auto output = (char*)recresult.data();
    return output;
}
extern "C" WINAPI void Dispose(); //__declspec(dllexport)
WINAPI void Dispose()
{
if(useDet==true)
{
    delete det;
}
if(useRec==true)
{
    delete rec;
}
if(useCls==true)
{
delete cls;
}

}

char info[]= "version:1.0.0\r\nwriter:FIRC\r\nemail:1623863129@qq.com\r\ndate:2021-8-21\r\nCopyright 2021 FIRC. All rights reserved.";
extern "C" WINAPI char* GetCopyrightInfo(); //__declspec(dllexport)
WINAPI char* GetCopyrightInfo()
{
    return info;
}


