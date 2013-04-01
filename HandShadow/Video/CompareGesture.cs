using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Video
{
    public class CompareGesture
    {
        GraphicsDevice device;
        public Texture2D compareGesture;
        Image<Bgr, byte> frame;
        public Color[] colorData;

        public CompareGesture(GraphicsDevice device, string gestureFileName)
        {
            this.device = device;
            colorData = new Color[640 * 480];
            frame = new Image<Bgr, byte>(640, 480);
            compareGesture  = new Texture2D(device, 640, 480);
            IntPtr img = CvInvoke.cvLoadImage(gestureFileName, Emgu.CV.CvEnum.LOAD_IMAGE_TYPE.CV_LOAD_IMAGE_COLOR);
            CvInvoke.cvCopy(img, frame, IntPtr.Zero);
            Image<Gray, byte> gray = frame.Convert<Gray, Byte>();

            byte[] bgrData = gray.Bytes;
            for (int i = 0; i < colorData.Length; i++)
            {
                if (bgrData[i] > 10)
                    bgrData[i] = 255;
                else bgrData[i] = 0;

                if (bgrData[i] == 255)
                    colorData[i] = new Color(0, 0, 0, 0);
                else
                    colorData[i] = new Color(bgrData[i], bgrData[i], bgrData[i], 0.65f);
            }
                     
        }

        public Texture2D Frame
        {
            get
            {
                compareGesture.SetData<Color>(0, null, colorData, 0, colorData.Length);
                return compareGesture;
            }
        }
    }
}
