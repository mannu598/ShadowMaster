//-----------------------------------------------
// XUI - Game1.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Video;
using HandShadow;
using System.IO;

// E_Layer
public enum E_Layer
{
    UI = 0,

    // etc ...

    Count,
};

// class Game1
public class Game1 : Game
{
    // Game1
    public Game1()
        : base()
    {
        Graphics = new GraphicsDeviceManager(this);

        Graphics.PreferredBackBufferWidth = 1280;
        Graphics.PreferredBackBufferHeight = 720;
        //this.Graphics.IsFullScreen = true; // 需要全屏则注释上面两条，留下这一段代码
        Graphics.PreferMultiSampling = true;

#if PROFILE
		IsFixedTimeStep = false;
		Graphics.SynchronizeWithVerticalRetrace = false;
#endif

        Content.RootDirectory = "Content";
    }

    // Initialize
    protected override void Initialize()
    {
        _G.Game = this;
        //this.IsMouseVisible = true;
        // add core components
        Components.Add(new GamerServicesComponent(this));

        // add layers
        UiLayer = new UiLayer();
        _G.UI = UiLayer;

        // add other components
        _G.GameInput = new GameInput((int)E_GameButton.Count, (int)E_GameAxis.Count);
        GameControls.Setup(); // initialise mappings

        _G.isOption = false;
        // -------------初始化setting的颜色参数，调整它可以识别不同肤色（如黑人肤色）-------------
        _G.max_y = 255;
        _G.max_cr = 170;
        _G.max_cb = 135;
        _G.min_y = 0;
        _G.min_cr = 128;
        _G.min_cb = 80;
        _G.roi_x = 320;
        _G.roi_y = 0;
        // ----------------------------------------------------------------------------------------

        base.Initialize();
    }

    // LoadContent
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        Guide.NotificationPosition = NotificationPosition.BottomRight;

        // startup ui
        UiLayer.Startup(Content);

        // setup debug menu
#if !RELEASE
        _UI.SetupDebugMenu(null);
#endif
        cursorTex = Content.Load<Texture2D>("Textures\\Cursor");

        base.LoadContent();
    }

    // UnloadContent
    protected override void UnloadContent()
    {
        // shutdown ui
        UiLayer.Shutdown();

        base.UnloadContent();
    }

    // Update
    protected override void Update(GameTime gameTime)
    {
        IsRunningSlowly = gameTime.IsRunningSlowly;

        float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // update input
        _G.GameInput.Update(frameTime);

#if !RELEASE
        Input input = _G.GameInput.GetInput(0);

        //if (input.ButtonJustPressed((int)E_UiButton.Quit))
         //   this.Exit();
#endif

#if !RELEASE
        // update debug menu
        _UI.DebugMenuActive = _UI.DebugMenu.Update(frameTime);
#endif

        // TODO - other stuff here ...
        cursorPos = new Vector2(_G.GameInput.Mouse.X(), _G.GameInput.Mouse.Y());
        // update ui
        UiLayer.Update(frameTime);

        base.Update(gameTime);
    }

    // Draw
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // TODO - other stuff here ...

        // render ui
        UiLayer.Render(frameTime);

#if !RELEASE
        // render debug menu
        _UI.DebugMenu.Render();
#endif

        spriteBatch.Begin();
        spriteBatch.Draw(cursorTex, cursorPos, Color.White);
        if (_G.isOption)
        {
            spriteBatch.Draw(orginVideo.Frame, new Rectangle(825, 115, 335, 250), Color.White);   //  原摄像头画面（左上角）
            spriteBatch.Draw(processedVideo.Frame, new Rectangle(825, 407, 335, 250), Color.White);  //  切割后画面（右上角）
        }
        else if (_G.isPlayingGame)
        {
            spriteBatch.Draw(orginVideo.Frame, new Rectangle((int)_UI.SXM - 80, (int)_UI.SYM + 165, 200, 150), Color.White);   //  原摄像头画面（左上角）
            spriteBatch.Draw(processedVideo.Frame, new Rectangle(700, 140, 400, 300), Color.White);  //  切割后画面（右上角）
            spriteBatch.Draw(gesture.Frame, new Rectangle(200, 140, 400, 300), Color.White); //  目标手势图像
        }
        else if (_G.isShare)
        {
            delay++;
            if (delay % 20 == 0)
            {
                photo_conut = (photo_conut + 1) % _G.photo.Count;
            }
            //bianyi += 20;
            spriteBatch.Draw(_G.photo[photo_conut], new Rectangle(670, 243, 452, 343), Color.White);   //  原摄像头画面（左上角）

        }
        spriteBatch.End();

        base.Draw(gameTime);
    }

    //
    private GraphicsDeviceManager Graphics;
    SpriteBatch spriteBatch;

    private Texture2D cursorTex;
    private Vector2 cursorPos;

    public OriginEmgu orginVideo;                          //  摄像头捕捉的原始画面
    public ProcessedEmgu processedVideo;
    public CompareGesture gesture;

    private UiLayer UiLayer;

    private int photo_conut = 0;
    public bool IsRunningSlowly;
    public int delay = 0;
    public int photodelay = 0;
    public int bianyi = 20;
    //
};
