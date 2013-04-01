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
    public class OriginEmgu
    {
        GraphicsDevice device;
        Texture2D frame;
        Texture2D photoframe;
        Capture capture;
        Image<Bgr, byte> nextFrame;
        Image<Bgr, byte> photoFrame;
        Thread thread;
        public bool IsRunning;
        public Color[] colorData;
        public Color[] photo_colorData;

        private Image<Gray, byte> gray = null;
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

        public Texture2D Photoframe
        {
            get
            {
                if (photoframe.GraphicsDevice.Textures[0] == photoframe)
                    photoframe.GraphicsDevice.Textures[0] = null;
                photoframe.SetData<Color>(0, null, photo_colorData, 0, photo_colorData.Length);
                return photoframe;
            }
        }

        public OriginEmgu(GraphicsDevice device)
        {
            this.device = device;
            capture = new Capture(0);
            frame = new Texture2D(device, capture.Width, capture.Height);
            photoframe = new Texture2D(device, capture.Width, capture.Height);
            colorData = new Color[capture.Width * capture.Height];
            photo_colorData = new Color[capture.Width * capture.Height];
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
                    gray = nextFrame.Convert<Gray, Byte>();
                    /*
                    MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(haarCascade, 1.2, 2, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new System.Drawing.Size(40, 40));
                    foreach (MCvAvgComp face in facesDetected[0])
                        nextFrame.Draw(face.rect, new Bgr(System.Drawing.Color.White), 2);
                     */

                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle((int)_G.roi_x, (int)_G.roi_y, nextFrame.Width / 2, nextFrame.Height / 2);
                    nextFrame.Draw(rect, new Bgr(System.Drawing.Color.White), 2);
                    byte[] bgrData = nextFrame.Bytes;
                    for (int i = 0; i < colorData.Length; i++)
                        colorData[i] = new Color(bgrData[3 * i + 2], bgrData[3 * i + 1], bgrData[3 * i]);

                }
            }
        }

        public Color[] PhotoFream()
        {
            while (IsRunning)
            {
                photoFrame = capture.QueryFrame().Flip(FLIP.HORIZONTAL);
                if (photoFrame != null)
                {
                    gray = photoFrame.Convert<Gray, Byte>();

                    byte[] bgrData = photoFrame.Bytes;
                    for (int i = 0; i < photo_colorData.Length; i++)
                        photo_colorData[i] = new Color(bgrData[3 * i + 2], bgrData[3 * i + 1], bgrData[3 * i]);

                }
                return photo_colorData;
            }
            return null;
        }

    }
}
