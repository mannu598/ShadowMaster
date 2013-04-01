using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Emgu.CV;
using DirectShowLib;

//-----------------------------------------------
// XUI - Globals.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

// class _G
public static class _G
{
    public static Game1 Game;
    public static GameInput GameInput;
    public static UiLayer UI;

    public static bool isOption;
    // CV part
    public static int max_y;
    public static int max_cr;
    public static int max_cb;
    public static int min_y;
    public static int min_cr;
    public static int min_cb;

    public static double roi_x;
    public static double roi_y;

    // game logic part
    public static bool isPlayingGame;
    public static int gameLevel;
    public static int gameNum;
    public static bool isShare;

    // sounds
    public static SoundEffectInstance AcradeNextShadow;
    public static SoundEffectInstance ButtonPressed;
    public static SoundEffectInstance LevelCompleted;
    public static SoundEffectInstance Switch;
    public static SoundEffectInstance Listdown;
    public static SoundEffectInstance Listup;

    // bg musics
    public static Song MenuBg;
    public static Song AcradeBg;
    public static Song ClassicBg;

    public static int camIndex;
    public static List<KeyValuePair<int, string>> ListCamerasData;

    public static void getCamInfo()
    {
        ListCamerasData = new List<KeyValuePair<int, string>>();

        //-> Find systems cameras with DirectShow.Net dll 
        DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

        int _DeviceIndex = 0;
        foreach (DirectShowLib.DsDevice _Camera in _SystemCamereas)
        {
            ListCamerasData.Add(new KeyValuePair<int, string>(_DeviceIndex, _Camera.Name));
            _DeviceIndex++;
        }

        camIndex = ListCamerasData.Count - 1 ;
    }

    // 触控变量
    public static int Pre_Mouse_X;
    public static int Current_Mouse_X;

    // Mouse Click
    public static bool Mouse_Click()
    {
        if (_G.Pre_Mouse_X == _G.Current_Mouse_X && _G.Pre_Mouse_X != 0 && _G.Current_Mouse_X != 0)
        {
            _G.Pre_Mouse_X = 0;
            _G.Current_Mouse_X = 0;
            return true;
        }
        else
            return false;
    }

    // 游戏截图
    public static List<Texture2D> photo = new List<Texture2D>();

    // 街机评分
    public static int Score;

};
