//-----------------------------------------------
// XUI - Start.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using Video;
using HandShadow;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace UI
{

    // class Screen_Start
    public class Screen_AcradeGameScene : Screen
    {
        // Screen_Start
        public Screen_AcradeGameScene()
            : base("AcradeGameScene")
        {
            WidgetGraphic bg = new WidgetGraphic();
            bg.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
            bg.AddTexture("gamescene_bg", 0.0f, 0.0f, 1.0f, 1.0f);
            bg.ColorBase = Color.White;
            Add(bg);

            bar = new WidgetGraphic();
            bar.Size = new Vector3(0, 22, 1.0f);
            bar.Align = E_Align.MiddleLeft;
            bar.Position = new Vector3(245, 33, 0.0f);
            bar.AddTexture("bar", 0.0f, 0.0f, 1.0f, 1.0f);
            bar.ColorBase = Color.Red;
            Add(bar);

            WidgetGraphic board = new WidgetGraphic();
            board.Size = new Vector3(380, 144, 0.0f);
            board.AddTexture("gamescene_board", 0.0f, 0.0f, 1.0f, 1.0f);
            board.ColorBase = Color.White;
            board.Parent(bar);
            board.Position = new Vector3(580, 500, 5.0f);
            Add(board);

            wid_score = new WidgetText();
            wid_score.Position = new Vector3(1100, 560, 0.0f);
            wid_score.Size = new Vector3(0.0f, 50.0f, 0.0f);
            wid_score.Align = E_Align.MiddleLeft;
            wid_score.FontStyle = _UI.Store_FontStyle.Get("Default").Copy();
            wid_score.FontStyle.TrackingPercentage = 0.1875f;
            wid_score.ColorBase = Color.Orange;
            wid_score.String = "0";
            Add(wid_score);

            wid_timeleft = new WidgetText();
            wid_timeleft.Position = new Vector3(980, 610, 0.0f);
            wid_timeleft.Size = new Vector3(0.0f, 50.0f, 0.0f);
            wid_timeleft.Align = E_Align.MiddleLeft;
            wid_timeleft.FontStyle = _UI.Store_FontStyle.Get("Default").Copy();
            wid_timeleft.FontStyle.TrackingPercentage = 0.1875f;
            wid_timeleft.ColorBase = Color.Orange;
            wid_timeleft.String = "90";
            wid_timeleft.AddFontEffect(new FontEffect_ColorLerp(0.03125f, 1.5f, 1.0f, Color.White, E_LerpType.Sin));
            wid_timeleft.AddFontEffect(new FontEffect_Scale(0.03125f, 0.75f, 1.0f, 1.0f, 1.1f, 1.0f, 1.1f, E_LerpType.Sin));
            Add(wid_timeleft);

            WidgetGraphic exitBtn = new WidgetGraphic();
            exitBtn.Size = new Vector3(80, 80, 0.0f);
            exitBtn.Position = new Vector3(60, 600, 5.0f);
            exitBtn.AddTexture("gamescene_exit", 0.0f, 0.0f, 1.0f, 1.0f);
            exitBtn.ColorBase = Color.White;
            Add(exitBtn);

            WidgetGraphic signal = new WidgetGraphic();
            signal.Size = new Vector3(44, 29, 0.0f);
            signal.AddTexture("bar_signal", 0.0f, 0.0f, 1.0f, 1.0f);
            signal.ColorBase = Color.White;
            signal.Parent(bar);
            signal.Position = new Vector3((float)(800 * 0.85), -1.0f, 5.0f);
            Add(signal);

            WidgetGraphic star = new WidgetGraphic();
            star.Size = new Vector3(25, 25, 0.0f);
            star.ColorBase = Color.White;
            star.Position = new Vector3(1, -3, 0);
            star.Align = E_Align.MiddleCentre;
            star.ParentAttach = E_Align.MiddleCentre;
            star.Parent(signal);
            star.AddTexture("star_gold", 0.0f, 0.0f, 1.0f, 1.0f);
            Add(star);

            countdown = new WidgetText();
            countdown.Position = new Vector3(_UI.SXM, _UI.SYM - 100.0f, 0.0f);
            countdown.Size = new Vector3(0.0f, 150.0f, 0.0f);
            countdown.Align = E_Align.MiddleCentre;
            countdown.FontStyle = _UI.Store_FontStyle.Get("Default").Copy();
            countdown.FontStyle.TrackingPercentage = 0.1875f;
            countdown.ColorBase = Color.Orange;
            countdown.String = "READY";
            countdown.AddFontEffect(new FontEffect_ColorLerp(0.03125f, 1.5f, 1.0f, Color.Red, E_LerpType.Sin));
            countdown.AddFontEffect(new FontEffect_Scale(0.03125f, 0.75f, 1.0f, 1.0f, 1.1f, 1.0f, 1.1f, E_LerpType.Sin));
            Add(countdown);
        }

        protected void BeginGame()
        {

            Random ra = new Random();
            _G.Game.gesture = new CompareGesture(_G.Game.GraphicsDevice, "Content\\Textures\\Shadow\\" + ((int)(ra.Next() % 2 + 1)).ToString() + "_" + ((int)(ra.Next() % 9 + 1)).ToString() + ".png");  //  读取hand.png这个图像作为识别目标

            _G.Game.orginVideo = new OriginEmgu(_G.Game.GraphicsDevice);    //  初始化摄像头捕捉到的原始视频
            _G.Game.processedVideo = new ProcessedEmgu(_G.Game.GraphicsDevice);

            _G.isPlayingGame = true;
            _G.Game.orginVideo.Start();
            _G.Game.processedVideo.Start();
            getTransparentRatio();
        }


        protected override void OnPostInit()
        {
            MediaPlayer.Play(_G.AcradeBg);
            MediaPlayer.IsRepeating = true;
            base.OnPostInit();
        }
        protected override void OnEnd()
        {
            MediaPlayer.Stop();
            _G.isPlayingGame = false;
            base.OnEnd();
            _G.Game.orginVideo.Dispose();
            _G.Game.processedVideo.Dispose();
        }

        // OnProcessInput
        protected override void OnProcessInput(Input input)
        {
            //Console.WriteLine(_G.GameInput.Mouse.X());
            if (input.ButtonJustPressed((int)E_UiButton.B)
                || (_G.GameInput.Mouse.X() > 72 && _G.GameInput.Mouse.X() < 146 && _G.GameInput.Mouse.Y() > 602 && _G.GameInput.Mouse.Y() < 670
                && _G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left)))
            {
                SetScreenTimers(0.0f, 0.5f);
                _G.isPlayingGame = false;
                _UI.Screen.AddScreen(new Screen_Popup(E_PopupType.ExitFromGameScene));
            }
        }

        protected override void OnUpdate(float frameTime)
        {
            if (!isCountedDown)
            {
                divider--;
                if (divider == 0)
                {
                    divider = 60;
                    if (countdown.String.Equals("READY"))
                    {
                        countdown.String = "3";
                    }
                    else if (countdown.String.Equals("3"))
                    {
                        countdown.String = "2";
                    }
                    else if (countdown.String.Equals("2"))
                    {
                        countdown.String = "1";
                    }
                    else if (countdown.String.Equals("1"))
                    {
                        countdown.String = "GO!";
                    }
                    else if (countdown.String.Equals("GO!"))
                    {
                        divider = 0;
                        wid_score.String = score.ToString();
                        wid_timeleft.String = (timeleft / 2).ToString();
                        countdown.String = "";

                        isCountedDown = true;
                        BeginGame();
                    }
                    else if (countdown.String.Equals(""))
                    {
                        divider = 120;
                        countdown.String = "Time's up!";
                        _G.LevelCompleted.Play();
                        _G.isPlayingGame = false;
                    }
                    else
                    {
                        _G.isShare = true;
                        //_UI.Screen.AddScreen(new Screen_Popup(E_PopupType.AcradeOver));
                        _G.Score = score;
                        _UI.Screen.SetNextScreen(new ShareScene());
                        base.End();
                    }
                }
                return;
            }

            if ((float)result / 100 < bar.Size.X / 805)
            {
                bar.Size = new Vector3(bar.Size.X - 2, 22, 1.0f);

            }
            else if ((float)result / 100 > bar.Size.X / 805)
            {
                bar.Size = new Vector3(bar.Size.X + 2, 22, 1.0f);
            }

            bar.ColorBase.R((byte)(255 * Math.Sqrt(1 - bar.Size.X / 805)));
            bar.ColorBase.G((byte)(255 * Math.Pow((bar.Size.X / 805), 2)));
            bar.FlagSet(E_WidgetFlag.DirtyColor);

            divider = (divider + 1) % 31;

            if (divider == 30 && _G.isPlayingGame)
            {
                timeleft--;
                wid_timeleft.String = (timeleft / 2).ToString();
                if ((timeleft / 2) % 2 == 0 && isDone)
                {
                    Take_Photo();
                    if (_G.photo.Count % 7 == 0)
                        isDone = false;
                }
                if (timeleft == 0)
                {
                    divider = 10;
                    isCountedDown = false;
                }

                result = (int)calculateSimilarity();

                if ((bar.Size.X / 805) > 0.85)
                {
                    isDone = true;
                    bar.Size.X = 0;
                    //_G.isPlayingGame = false;
                    //_UI.Screen.AddScreen(new Screen_Popup(E_PopupType.NextLevel));
                    //SetScreenTimers(0.0f, 0.5f);
                    //base.End();
                    _G.AcradeNextShadow.Play();
                    score++;
                    wid_score.String = score.ToString();

                    Random ra = new Random();
                    _G.Game.gesture = new CompareGesture(_G.Game.GraphicsDevice, "Content\\Textures\\Shadow\\" + 1.ToString() + "_" + ((int)(ra.Next() % 9 + 1)).ToString() + ".png");  //  读取hand.png这个图像作为识别目标
                    getTransparentRatio();
                }
            }
            base.OnUpdate(frameTime);
        }

        public void Take_Photo()
        {
            Texture2D newt = new Texture2D(_G.Game.GraphicsDevice, _G.Game.orginVideo.Photoframe.Width, _G.Game.orginVideo.Photoframe.Height);
            newt.SetData<Color>(0, null, _G.Game.orginVideo.PhotoFream(), 0, (int)(640 * 480));
            _G.photo.Add(newt);

        }

        protected override void OnProcessMessage(ref ScreenMessage message)
        {
            E_UiMessageType type = (E_UiMessageType)message.Type;

            if (type == E_UiMessageType.PopupConfirm)
            {
                switch ((E_PopupType)message.Data)
                {
                    case E_PopupType.AcradeOver:
                        _G.gameLevel = -1;
                        _G.gameNum = -1;
                        _UI.Screen.SetNextScreen(new Screen_Loading()); break;
                    case E_PopupType.ExitFromGameScene:
                        _UI.Screen.SetNextScreen(new Screen_MainMenu());
                        break;
                }
            }
            else if (type == E_UiMessageType.PopupCancel)
            {
                switch ((E_PopupType)message.Data)
                {
                    case E_PopupType.AcradeOver:
                        _UI.Screen.SetNextScreen(new Screen_MainMenu()); break;
                    case E_PopupType.ExitFromGameScene:
                        _G.isPlayingGame = true;
                        _G.Game.processedVideo.Start();
                        break;
                }
            }
        }

        void getTransparentRatio()
        {
            int transparentNum = 0;
            for (int i = 0; i < _G.Game.gesture.colorData.Length; i++)
            {
                if (_G.Game.gesture.colorData[i].Equals(Color.Transparent))
                    transparentNum++;
            }
            transparentRatio = (float)transparentNum / (float)(640.0 * 480.0);
        }

        //  一个非常naive的匹配度计算方法，点点相比较。
        private double calculateSimilarity()
        {
            int rightNum = 0;
            for (int i = 0; i < _G.Game.gesture.colorData.Length; i++)
            {
                if (_G.Game.gesture.colorData[i].Equals(_G.Game.processedVideo.colorData[i]))
                {
                    rightNum++;
                }
            }
            double result = (double)rightNum / (640.0 * 480.0);

            result -= 0.6;
            result *= 4;

            result = (result - transparentRatio) / (1 - transparentRatio);

            //System.Console.WriteLine(result);

            result *= 100;

            if (result < 0)
            {
                result = 0;
            }
            if (result > 100)
            {
                result = 100;
            }

            return result;
        }

        private WidgetGraphic bar;
        private WidgetText wid_score;
        private WidgetText wid_timeleft;
        private WidgetText countdown;
        private bool isCountedDown = false;
        private bool isDone = true;
        private int divider = 60;
        private int result = 0;
        private int score = 0;
        private int timeleft = 6;

        private float transparentRatio = 0;
    };

}; // namespace UI
