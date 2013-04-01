using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UI;
using Microsoft.Xna.Framework;
using Debug;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Xna.Framework.Input;
using System.Net;
using System.IO;

namespace UI
{
    public class ShareScene : Screen
    {
        public ShareScene()
            : base("ShareScene")
        {
            WidgetGraphic bg = new WidgetGraphic();
            bg.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
            bg.AddTexture("acradeend", 0.0f, 0.0f, 1.0f, 1.0f);
            bg.ColorBase = Color.White;
            Add(bg);


            publish_button = new WidgetGraphic();
            publish_button.Size = new Vector3(142, 32, 0.0f);
            publish_button.AddTexture("publish_button", 0.0f, 0.0f, 1.0f, 1.0f);
            publish_button.Position = new Vector3(200, 200, 0);
            publish_button.ColorBase = Color.White;
            //publish_button.FlagSet(E_WidgetFlag.ProcessMouse);
            //publish_button += ((sender) => { if(_G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left))   Console.WriteLine("ssssss"); });
            //publish_button.Mouse_OnEnter += ((sender) => { Console.WriteLine("ssssss"); });
            //publish_button.Mouse_OnEnter += new Mouse_OnEnter(this.publish_button_Mouse_OnEnter);
            Add(publish_button);

            WidgetGraphic exitBtn = new WidgetGraphic();
            exitBtn.Size = new Vector3(80, 80, 0.0f);
            exitBtn.Position = new Vector3(60, 600, 5.0f);
            exitBtn.AddTexture("gamescene_exit", 0.0f, 0.0f, 1.0f, 1.0f);
            exitBtn.ColorBase = Color.White;
            Add(exitBtn);

            wid_score = new WidgetText();
            wid_score.Position = new Vector3(280, 420, 0.0f);
            wid_score.Size = new Vector3(0.0f, 180.0f, 0.0f);
            wid_score.Align = E_Align.MiddleLeft;
            wid_score.FontStyle = _UI.Store_FontStyle.Get("Default3dDS").Copy();
            wid_score.FontStyle.TrackingPercentage = 0.1875f;
            wid_score.ColorBase = Color.Orange;
            wid_score.AddFontEffect(new FontEffect_ColorLerp(0.03125f, 1.5f, 3.0f, Color.White, E_LerpType.BounceOnceSmooth));
            wid_score.AddFontEffect(new FontEffect_Scale(0.03125f, 0.75f, 3.0f, 1.0f, 1.1f, 1.0f, 1.4f, E_LerpType.Sin));
            wid_score.String = _G.Score.ToString();
            Add(wid_score);

            wid_textbox = new WidgetText();
            wid_textbox.Position = new Vector3(300, 300, 0.0f);
            wid_textbox.Size = new Vector3(1000.0f, 100.0f, 0.0f);
            wid_textbox.Align = E_Align.None;
            wid_textbox.FontStyle = _UI.Store_FontStyle.Get("Default3dDS").Copy();
            wid_textbox.FontStyle.TrackingPercentage = 0.1875f;
            wid_textbox.ColorBase = Color.Orange;
            wid_textbox.AddFontEffect(new FontEffect_ColorLerp(0.03125f, 1.5f, 3.0f, Color.White, E_LerpType.BounceOnceSmooth));
            wid_textbox.AddFontEffect(new FontEffect_Scale(0.03125f, 0.75f, 3.0f, 1.0f, 1.1f, 1.0f, 1.4f, E_LerpType.Sin));
            wid_textbox.String = Weibo_name;
            Add(wid_textbox);
            //_G.KeyDown += new KeyEventHandler(KeyDown);
            //weibo();
        }


        protected override void OnProcessInput(Input input)
        {
            Keys ked = new Keys();
            var ke = Keyboard.GetState(PlayerIndex.One);   
            //if(_G.GameInput.Keyboard.KeyboardState.IsKeyDown() )
            var keya = ke.GetPressedKeys();
            if (keya.Length != 0)
            {
                _keyDown = keya[0].ToString();
                ked = keya[0];

            }
            if (_G.GameInput.Keyboard.KeyboardState.IsKeyUp(ked) && _keyDown != null)
            {
                wid_textbox.String += _keyDown;
                _keyDown = null;    
            }
            
            //Console.WriteLine(_G.GameInput.Mouse.XY());
            if (input.ButtonJustPressed((int)E_UiButton.B) || _G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Right)
                 || (_G.GameInput.Mouse.X() > 72 && _G.GameInput.Mouse.X() < 146 && _G.GameInput.Mouse.Y() > 602 && _G.GameInput.Mouse.Y() < 670
                && _G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left)))
            {
                _G.isShare = false;
                _G.photo.Clear();
                _UI.Screen.SetNextScreen(new Screen_MainMenu());
                SetScreenTimers(0.0f, 0.5f);
                base.End();
            }
        }

        protected override void OnUpdate(float frameTime)
        {
            base.OnUpdate(frameTime);
        }

        public void weibo()
        {

            var oauth = new NetDimension.Weibo.OAuth("1727300261", "17d31a78da307172e2786faf42300b70", "http://DiseaseCourse.com");
            //bool result = oauth.ClientLogin("8785169@163.com", "zhengyu19910808");
            //if (result) //返回true授权成功
            //{
            //    Console.WriteLine(oauth.AccessToken); //还是来打印下AccessToken看看与前面方式获取的是不是一样的
            //    var Sina = new NetDimension.Weibo.Client(oauth);
            //    var uid = Sina.API.Dynamic.Statuses.BilateralTimeline(null);
            //}

            var authUrl = oauth.GetAuthorizeURL(); //VS2008需要指定全部4个参数，这里是VS2010等支持“可选参数”的开发环境的写法
            /* 第二步访问这个地址。
             * Console用Process.Start()方法，Web用Response.Redirect()方法。
             * 方法很多，自行选择。
             * 本例采用控制台的形式来演示代码。*/
            var da = GetModel(authUrl);
            int a = 3;
            Console.WriteLine(a);
            string fds = da.ToString();
            //Console.Write("填写浏览器地址中的Code参数：");
            var code = Console.ReadLine();
            var accessToken = oauth.GetAccessTokenByAuthorizationCode(code);
            //打印AccessToken
            Console.WriteLine(accessToken.Token);
            //或者
            Console.WriteLine(oauth.AccessToken);
            if (!string.IsNullOrEmpty(accessToken.Token))
            {
                var Sina = new NetDimension.Weibo.Client(oauth);
                //var uid = Sina.API.Account.GetUID(); //调用API中获取UID的方法
                //Console.WriteLine(uid);
            }
        }

        private string GetModel(string strUrl)
        {
            string strRet = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Timeout = 2000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                System.IO.Stream resStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.Default;
                StreamReader readStream = new StreamReader(resStream, encode);

                Char[] read = new Char[256];
                int count = readStream.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strRet = strRet + str;
                    count = readStream.Read(read, 0, 256);
                }

                resStream.Close();
            }
            catch (Exception e)
            {
                strRet = "";
            }
            return strRet;
        }  

        private WidgetText wid_score;
        private WidgetGraphic publish_button;
        string Weibo_name = "";
        private String Weibo_password = "";
        private WidgetText wid_textbox;

        private string _keyDown, _keyUp;
        Keys i;
        //private Mouse_OnEnter publish_button_Mouse_OnEnter;
    }
}
