using System;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using HandGestureRecognition.SkinDetector;
using System.Runtime.InteropServices;

namespace Video
{
    public class ProcessedEmgu
    {
        GraphicsDevice device;
        Texture2D frame;
        Capture capture;
        Image<Bgr, byte> nextFrame;
        Thread thread;
        public bool IsRunning;
        public Color[] colorData;

        Ycc YCrCb_min;
        Ycc YCrCb_max; 
        private Image<Bgr, byte> gray = null;
        IColorSkinDetector skinDetector;
        AdaptiveSkinDetector detector;
        Image<Gray, Byte> skin;

        //private HaarCascade haarCascade = new HaarCascade("haarcascade_frontalface_default.xml");

        public Texture2D Frame
        {
            get
            {
                if (frame.GraphicsDevice.Textures[0] == frame)
                    frame.GraphicsDevice.Textures[0] = null;
                frame.SetData<Color>(0, null, colorData, 0, colorData.Length);
                return frame;
            }
        }

        public ProcessedEmgu(GraphicsDevice device)
        {
            this.device = device;
            capture = new Capture(0);
            frame = new Texture2D(device, capture.Width, capture.Height);
            colorData = new Color[capture.Width * capture.Height];


            skinDetector = new YCrCbSkinDetector();
            //--------------------------------------------------------------
            detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.ERODE);
            skin = new Image<Gray, byte>(capture.Width, capture.Height);
        }

        public void Start()
        {
            ThreadStart threadStart = new ThreadStart(QueryFrame);
            thread = new Thread(threadStart);
            IsRunning = true;
            thread.Start();
        }

        public void Dispose()
        {
            IsRunning = false;
            thread.Abort();
            capture.Stop();
            capture.Dispose();
        }

        private void QueryFrame()
        {
            while (IsRunning)
            {
                nextFrame = capture.QueryFrame().Flip(FLIP.HORIZONTAL);

               
                if (nextFrame != null)
                {
                    System.Drawing.Size roisize = new System.Drawing.Size(nextFrame.Width/2, nextFrame.Height/2); //要裁剪的图片大小
                    IntPtr dst = CvInvoke.cvCreateImage(roisize, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 3);
                    
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)_G.roi_x, (int)_G.roi_y, nextFrame.Width / 2, nextFrame.Height / 2);
                    CvInvoke.cvSetImageROI(nextFrame, rect);
                    CvInvoke.cvCopy(nextFrame, dst, IntPtr.Zero);
                    Image<Bgr, Byte> temp = new Image<Bgr, Byte>(roisize);
                    CvInvoke.cvCopy(dst, temp, IntPtr.Zero);
                    temp = temp.Resize(2.0, INTER.CV_INTER_LINEAR);
                    CvInvoke.cvResetImageROI(temp);
                    /*
                    System.Drawing.Size roisize = new System.Drawing.Size(nextFrame.Width, nextFrame.Height); //要裁剪的图片大小
                    IntPtr dst = CvInvoke.cvCreateImage(roisize, Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 3);
                    CvInvoke.cvCopy(src,dst,IntPtr.Zero );
                    CvInvoke.cvCopy(nextFrame, dst, IntPtr.Zero);
                    Emgu.CV.Image<Bgr, Byte> temp2 = (Emgu.CV.Image<Bgr, Byte>)Marshal.PtrToStructure(dst, typeof(Emgu.CV.Image<Bgr, Byte>));
                    */
                    gray = temp.Convert<Bgr, Byte>();
                    CvInvoke.cvSmooth(gray, gray, SMOOTH_TYPE.CV_MEDIAN, 5, 5, 0.8, 0.8);
                    YCrCb_min = new Ycc(_G.min_y, _G.min_cr, _G.min_cb);
                    YCrCb_max = new Ycc(_G.max_y, _G.max_cr, _G.max_cb);
                 
                    Image<Gray, Byte> skin = skinDetector.DetectSkin(gray, YCrCb_min, YCrCb_max);
                    if (skin == null)
                    {
                        return;
                    }
                    /*
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(haarCascade, 1.2, 2, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new System.Drawing.Size(40, 40));
                    foreach (MCvAvgComp face in facesDetected[0])
                        nextFrame.Draw(face.rect, new Bgr(System.Drawing.Color.White), 2);
                     */
                    byte[] bgrData = skin.Bytes;
                    for (int i = 0; i < colorData.Length; i++)
                    {
                        if (bgrData[i] > 10)
                            bgrData[i] = 0;
                        else bgrData[i] = 255;

                        if (_G.isOption)
                        {
                            if (bgrData[i] == 255)
                                colorData[i] = new Color(0, 0, 0);
                            else
                                 colorData[i] = new Color(255, 255, 255);
                        }
                        else if (_G.isPlayingGame)
                        {
                            if (bgrData[i] == 255)
                                colorData[i] = new Color(0, 0, 0, 0);
                            else
                                colorData[i] = new Color(bgrData[i], bgrData[i], bgrData[i], 0.65f);
                        }
                        else
                            colorData[i] = new Color(0, 0, 0, 0);
                    }
                     
                }
            }
        }
    }
}
