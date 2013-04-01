//-----------------------------------------------
// XUI - Start.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using Video;
using HandShadow;
using Microsoft.Xna.Framework.Media;
using System;

namespace UI
{

    // class Screen_Start
    public class Screen_GameScene : Screen
    {
        // Screen_Start
        public Screen_GameScene()
            : base("GameScene")
        {
            difficulty = SavingDataHandler.isLevelCompleted[_G.gameLevel - 1, _G.gameNum - 1];

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

            WidgetGraphic black = new WidgetGraphic();
            black.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
            black.AddTexture("null", 0.0f, 0.0f, 1.0f, 1.0f);
            black.ColorBase = Color.Black;
            Add(black);
            Timeline blackT = new Timeline("start", true, 0.0f, 1.5f, E_TimerType.Stop, E_RestType.Start);
            blackT.AddEffect(new TimelineEffect_Alpha(0.0f, -1.0f, E_LerpType.Linear));
            black.AddTimeline(blackT);

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
            Add(signal);

            star = new WidgetGraphic();
            star.Size = new Vector3(15, 15, 0.0f);
            star.ColorBase = Color.White;
            star.Position = new Vector3(-10, 2, 0);
            star.Align = E_Align.MiddleCentre;
            star.ParentAttach = E_Align.MiddleCentre;
            star.Parent(signal);

            if (difficulty == 0)
            {
                signal.Position = new Vector3((float)(800 * 0.85), -1.0f, 5.0f);
                star.AddTexture("star_bronze", 0.0f, 0.0f, 1.0f, 1.0f);
            }
            else if (difficulty == 1)
            {
                signal.Position = new Vector3((float)(800 * 0.90), -1.0f, 5.0f);
                star.AddTexture("star_silver", 0.0f, 0.0f, 1.0f, 1.0f);
            }
            else //2 or 3
            {
                signal.Position = new Vector3((float)(800 * 0.95), -1.0f, 5.0f);
                star.AddTexture("star_gold", 0.0f, 0.0f, 1.0f, 1.0f);
            }
            Add(star);

            Timeline star_move = new Timeline("star_move", false, 0.0f, 1.50f, E_TimerType.Stop, E_RestType.Start);
            star_move.AddEffect(new TimelineEffect_PositionX(12.0f, 405 - signal.Position.X, E_LerpType.SmootherStep));
            star_move.AddEffect(new TimelineEffect_PositionY(-1.0f, 300, E_LerpType.SmootherStep));
            star_move.AddEffect(new TimelineEffect_SizeX(15, 350, E_LerpType.SmoothStep));
            star_move.AddEffect(new TimelineEffect_SizeY(15, 350, E_LerpType.SmoothStep));
            star.AddTimeline(star_move);
        }

        protected override void OnInit()
        {
            _G.Game.gesture = new CompareGesture(_G.Game.GraphicsDevice, "Content\\Textures\\Shadow\\" + _G.gameLevel + "_" + _G.gameNum + ".png");  //  读取hand.png这个图像作为识别目标

            _G.Game.orginVideo = new OriginEmgu(_G.Game.GraphicsDevice);    //  初始化摄像头捕捉到的原始视频
            _G.Game.processedVideo = new ProcessedEmgu(_G.Game.GraphicsDevice);

            _G.isPlayingGame = true;
            _G.Game.orginVideo.Start();
            _G.Game.processedVideo.Start();

            base.OnInit();
        }

        protected override void OnPostInit()
        {
            MediaPlayer.Play(_G.ClassicBg);
            MediaPlayer.IsRepeating = true;
            _G.isPlayingGame = true;
            getTransparentRatio();
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

            if (input.ButtonJustPressed((int)E_UiButton.B)
                || (_G.GameInput.Mouse.X() > 72 && _G.GameInput.Mouse.X() < 146 && _G.GameInput.Mouse.Y() > 602 && _G.GameInput.Mouse.Y() < 670
                && _G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left))
                )
            {
                SetScreenTimers(0.0f, 0.5f);
                _G.isPlayingGame = false;
                _UI.Screen.AddScreen(new Screen_Popup(E_PopupType.ExitFromGameScene));
            }
        }

        protected override void OnUpdate(float frameTime)
        {
            if (gamePass)
            {
                gamePassTimeCounter--;
                if (gamePassTimeCounter == 0)
                {
                    _UI.Screen.AddScreen(new Screen_Popup(E_PopupType.NextLevel));
                    base.End();
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

            divider++;
            if (divider == 30 && _G.isPlayingGame)
            {
                result = (int)calculateSimilarity();
                divider = 0;

                if ((bar.Size.X / 805) > (difficulty == 0 ? 0.85 : (difficulty == 1 ? 0.90 : 0.95)))
                {
                    _G.LevelCompleted.Play();
                    if (SavingDataHandler.isLevelCompleted[_G.gameLevel - 1, _G.gameNum - 1] != 3)
                    {
                        SavingDataHandler.isLevelCompleted[_G.gameLevel - 1, _G.gameNum - 1]++;
                        SavingDataHandler.writeFile();
                    }
                    _G.isPlayingGame = false;

                    star.TimelineActive("star_move", true, false);
                    gamePass = true;
                    SetScreenTimers(0.0f, 0.5f);
                }
            }
            base.OnUpdate(frameTime);
        }

        protected override void OnProcessMessage(ref ScreenMessage message)
        {
            E_UiMessageType type = (E_UiMessageType)message.Type;

            if (type == E_UiMessageType.PopupConfirm)
            {
                switch ((E_PopupType)message.Data)
                {
                    case E_PopupType.NextLevel:
                        _G.gameLevel = _G.gameNum == 9 ? (_G.gameLevel + 1) : _G.gameLevel;
                        _G.gameNum = _G.gameNum == 9 ? 1 : (_G.gameNum + 1);
                        _UI.Screen.SetNextScreen(new Screen_Loading()); break;
                    case E_PopupType.ExitFromGameScene:
                        _UI.Screen.SetNextScreen(new Screen_LevelSelect());
                        break;
                }
            }
            else if (type == E_UiMessageType.PopupCancel)
            {
                switch ((E_PopupType)message.Data)
                {
                    case E_PopupType.NextLevel:
                        _UI.Screen.SetNextScreen(new Screen_LevelSelect()); break;
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


            result = (result - transparentRatio) / (1 - transparentRatio);

            //System.Console.WriteLine(result);

            if (result < 0)
            {
                result = 0;
            }

            result = Math.Pow(result, 0.25);

            result *= 110;
            if (result > 100)
            {
                result = 100;
            }

            return result;
        }

        private WidgetGraphic bar;
        private int divider = 0;
        private int result = 0;
        private int difficulty = 0;

        private float transparentRatio = 0;

        private WidgetGraphic star;

        private bool gamePass = false;
        private int gamePassTimeCounter = 60;
    };

}; // namespace UI
