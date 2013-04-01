//-----------------------------------------------
// XUI - Layer.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

// E_UiMessageType
public enum E_UiMessageType
{
    PopupConfirm = 0,
    PopupCancel,

    Count,
};

// class UiLayer
public class UiLayer : Layer
{
    // UiLayer
    public UiLayer()
        : base((int)E_Layer.UI)
    {
        //
    }

    // Startup
    public override void Startup(ContentManager content)
    {
        _UI.Startup(_G.Game, _G.GameInput);

        // load textures
        int bundleIndex = _UI.Texture.CreateBundle();

        _UI.Texture.Add(bundleIndex, "Textures\\UI_Box", "box");
        _UI.Texture.Add(bundleIndex, "Textures\\Logo", "logo");
        _UI.Texture.Add(bundleIndex, "Textures\\Logo_Word", "logo_word");
        _UI.Texture.Add(bundleIndex, "Textures\\Point_left", "point_left");
        _UI.Texture.Add(bundleIndex, "Textures\\Point_right", "point_right");
        _UI.Texture.Add(bundleIndex, "Textures\\Levelselect_Bg", "levelselect_bg");
        _UI.Texture.Add(bundleIndex, "Textures\\LevelSelect_Board", "levelselect_board");
        _UI.Texture.Add(bundleIndex, "Textures\\Tick", "tick");
        _UI.Texture.Add(bundleIndex, "Textures\\GameScene_Bg", "gamescene_bg");
        _UI.Texture.Add(bundleIndex, "Textures\\Bar", "bar");
        _UI.Texture.Add(bundleIndex, "Textures\\Option_Bg", "option_bg");
        _UI.Texture.Add(bundleIndex, "Textures\\Slider_Tail", "slider_tail");
        _UI.Texture.Add(bundleIndex, "Textures\\Slider_Pin", "slider_pin");
        _UI.Texture.Add(bundleIndex, "Textures\\MainMenu_Bg", "mainmenu_bg");
        _UI.Texture.Add(bundleIndex, "Textures\\MainMenu_List", "mainmenu_list");
        _UI.Texture.Add(bundleIndex, "Textures\\DialogBoard", "dialogboard");
        _UI.Texture.Add(bundleIndex, "Textures\\DialogYes", "dialogyes");
        _UI.Texture.Add(bundleIndex, "Textures\\DialogNo", "dialogno");
        _UI.Texture.Add(bundleIndex, "Textures\\GameScene_Exit", "gamescene_exit");
        _UI.Texture.Add(bundleIndex, "Textures\\Star_Gold", "star_gold");
        _UI.Texture.Add(bundleIndex, "Textures\\Star_Silver", "star_silver");
        _UI.Texture.Add(bundleIndex, "Textures\\Star_Bronze", "star_bronze");
        _UI.Texture.Add(bundleIndex, "Textures\\Bar_Signal", "bar_signal");
        _UI.Texture.Add(bundleIndex, "Textures\\Mastered", "mastered");
        _UI.Texture.Add(bundleIndex, "Textures\\GameScene_Board", "gamescene_board");
        _UI.Texture.Add(bundleIndex, "Textures\\AcradeEnd", "acradeend");
        _UI.Texture.Add(bundleIndex, "Textures\\publish_button", "publish_button");

        for (int i = 1; i <= 4; i++)
            for (int j = 1; j <= 9; j++)
            {
                _UI.Texture.Add(bundleIndex, "Textures\\Shadow\\" + i.ToString() + "_" + j.ToString(), "shadow" + i.ToString() + "_" + j.ToString());
            }

        // load sounds
        _G.AcradeNextShadow = _G.Game.Content.Load<SoundEffect>("Sound\\AcradeNextShadow").CreateInstance(); // 音效
        _G.ButtonPressed = _G.Game.Content.Load<SoundEffect>("Sound\\ButtonPressed").CreateInstance(); // 音效
        _G.LevelCompleted = _G.Game.Content.Load<SoundEffect>("Sound\\LevelCompleted").CreateInstance(); // 音效
        _G.Switch = _G.Game.Content.Load<SoundEffect>("Sound\\Switch").CreateInstance(); // 音效
        _G.Listdown = _G.Game.Content.Load<SoundEffect>("Sound\\Listdown").CreateInstance(); // 音效
        _G.Listup = _G.Game.Content.Load<SoundEffect>("Sound\\Listup").CreateInstance(); // 音效

        // load bg musics
        _G.MenuBg = _G.Game.Content.Load<Song>("Sound\\MenuBg"); ;
        _G.AcradeBg = _G.Game.Content.Load<Song>("Sound\\AcradeBg"); ;
        _G.ClassicBg = _G.Game.Content.Load<Song>("Sound\\ClassicBg"); ;

        // load fonts
        _UI.Font.Add("Fonts\\", "SegoeUI");

        // setup common font styles
        UI.FontStyle fontStyle = new UI.FontStyle("SegoeUI");
        fontStyle.AddRenderPass(new UI.FontStyleRenderPass());
        _UI.Store_FontStyle.Add("Default", fontStyle);

        UI.FontStyle fontStyleDS = new UI.FontStyle("SegoeUI");
        UI.FontStyleRenderPass renderPassDS = new UI.FontStyleRenderPass();
        renderPassDS.ColorOverride = Color.Black;
        renderPassDS.AlphaMult = 0.5f;
        renderPassDS.Offset = new Vector3(0.05f, -0.05f, 0.0f);
        renderPassDS.OffsetProportional = true;
        fontStyleDS.AddRenderPass(renderPassDS);
        fontStyleDS.AddRenderPass(new UI.FontStyleRenderPass());
        _UI.Store_FontStyle.Add("Default3dDS", fontStyleDS);

        // setup font icons
        _UI.Store_FontIcon.Add("A", new UI.FontIcon(_UI.Texture.Get("null"), 0.0f, 0.0f, 1.0f, 1.0f));
        _UI.Store_FontIcon.Add("B", new UI.FontIcon(_UI.Texture.Get("null"), 0.0f, 0.0f, 1.0f, 1.0f));

        // add initial screens
        _UI.Screen.AddScreen(new UI.Screen_Background());
        _UI.Screen.AddScreen(new UI.Screen_Start());
    }

    // Shutdown
    public override void Shutdown()
    {
        _UI.Shutdown();
    }




    // OnUpdate
    protected override void OnUpdate(float frameTime)
    {
#if !RELEASE
        if (_UI.DebugMenuActive)
            return;
#endif

#if !RELEASE
        _UI.Camera2D.DebugUpdate(frameTime, _UI.GameInput.GetInput(0));
        _UI.Camera3D.DebugUpdate(frameTime, _UI.GameInput.GetInput(0));
#endif

        _UI.Sprite.RenderPassTransformMatrix[0] = _UI.Camera2D.TransformMatrix; // 2d
        _UI.Sprite.RenderPassTransformMatrix[1] = _UI.Camera3D.TransformMatrix; // 3d
        _UI.Sprite.RenderPassTransformMatrix[2] = _UI.Camera2D.TransformMatrix; // 2d background

        _UI.Sprite.BeginUpdate();
        _UI.Screen.Update(frameTime);
    }

    // OnRender
    protected override void OnRender(float frameTime)
    {
        _UI.Sprite.Render(2); // 2d background
        _UI.Sprite.Render(1); // 3d
        _UI.Sprite.Render(0); // 2d
    }

    //
    public bool MM_FromStartScreen;
    public bool MM_FromLevelSelect;
    public bool SS_FromMainMenu;
    //
};
