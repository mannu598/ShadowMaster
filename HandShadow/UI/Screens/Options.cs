//-----------------------------------------------
// XUI - Options.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Video;
using System;

namespace UI
{

// class Screen_Options
public class Screen_Options : Screen
{
	// Screen_Options
	public Screen_Options()
		: base( "Options" )
	{
        WidgetGraphic bg = new WidgetGraphic();
        bg.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
        bg.AddTexture("option_bg", 0.0f, 0.0f, 1.0f, 1.0f);
        bg.ColorBase = Color.White;
        Add(bg);


		WidgetMenuScroll menu = new WidgetMenuScroll( E_MenuType.Vertical );
		menu.Position = new Vector3(170, 205.0f, 0.0f );
		menu.Padding = 153.0f;
		menu.Alpha = 0.0f;
        OptionMenu = menu;

		Timeline menuT = new Timeline( "start", false, 0.25f, 0.25f, E_TimerType.Stop, E_RestType.None );
		menuT.AddEffect( new TimelineEffect_Alpha( 0.0f, 1.0f, E_LerpType.SmoothStep ) );
		menu.AddTimeline( menuT );

		Timeline menuT2 = new Timeline( "end", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.None );
		menuT2.AddEffect( new TimelineEffect_Alpha( 0.0f, -1.0f, E_LerpType.SmoothStep ) );
		menu.AddTimeline( menuT2 );

		Add( menu );

		// music volume
		WidgetMenuNode node0 = new WidgetMenuNode( 0 );
		Add( node0 );
		node0.Parent( menu );

		Timeline nodeT = new Timeline( "selected", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.Start );
		nodeT.AddEffect( new TimelineEffect_ScaleX( 0.0f, 0.125f, E_LerpType.SmoothStep ) );
		nodeT.AddEffect( new TimelineEffect_ScaleY( 0.0f, 0.125f, E_LerpType.SmoothStep ) );
			
		Timeline nodeT2 = new Timeline( "selected", false, 0.0f, 0.5f, E_TimerType.Bounce, E_RestType.Start );
		nodeT2.AddEffect( new TimelineEffect_Intensity( 0.0f, 0.75f, E_LerpType.SmoothStep ) );

		node0.AddTimeline( nodeT );
		node0.AddTimeline( nodeT2 );

		WidgetText text0 = new WidgetText();
		text0.Size = new Vector3( 0.0f, 35.0f, 0.0f );
		text0.FontStyleName = "Default";
		text0.String = "MUSIC VOLUME";
		text0.Align = E_Align.MiddleLeft;
		text0.Parent( node0 );
		text0.ParentAttach = E_Align.MiddleLeft;
		text0.ColorBase = Color.Orange;
		Add( text0 );

		WidgetMenuSwitch menuSwitch0 = new WidgetMenuSwitch( E_MenuType.Horizontal );
		menuSwitch0.Position = new Vector3( 80.0f, 0.0f, 0.0f );
		menuSwitch0.Parent( node0 );
		menuSwitch0.ParentAttach = E_Align.MiddleLeft;
		menuSwitch0.DeactivateArrows = false;
		Add( menuSwitch0 );

		Timeline timelineArrow_Selected = new Timeline( "selected", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.Start );
		timelineArrow_Selected.AddEffect( new TimelineEffect_Alpha( 0.0f, 1.0f, E_LerpType.SmoothStep ) );

		Timeline timelineArrow_Nudge = new Timeline( "nudge", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.None );
		timelineArrow_Nudge.AddEffect( new TimelineEffect_ScaleX( 0.0f, 0.25f, E_LerpType.BounceOnceSmooth ) );
		timelineArrow_Nudge.AddEffect( new TimelineEffect_ScaleY( 0.0f, 0.25f, E_LerpType.BounceOnceSmooth ) );

		WidgetGraphic arrow = new WidgetGraphic();
		arrow.Size = new Vector3( 0.0f, 0.0f, 0.0f );
		arrow.ColorBase = Color.Orange;
		arrow.Alpha = 0.0f;
		arrow.Align = E_Align.MiddleCentre;
		arrow.RenderState.Effect = (int)E_Effect.IntensityAsAlpha_PMA;
		arrow.FlagClear( E_WidgetFlag.InheritIntensity );
		arrow.AddTimeline( timelineArrow_Selected.Copy() );
		arrow.AddTimeline( timelineArrow_Nudge.Copy() );
		arrow.ParentAttach = E_Align.MiddleCentre;
		
		WidgetGraphic arrowLeft = (WidgetGraphic)arrow.Copy();
		arrowLeft.Name = "arrow_decrease";
		arrowLeft.Position = new Vector3( 40.0f, 80.0f, 0.0f );
		arrowLeft.Rotation.Z = 180.0f;
		arrowLeft.AddTexture( "null", 0.5f, 0.0f, 0.5f, 1.0f );
		arrowLeft.Parent( menuSwitch0 );
		Add( arrowLeft );

		WidgetGraphic arrowRight = (WidgetGraphic)arrow.Copy();
		arrowRight.Name = "arrow_increase";
		arrowRight.Position = new Vector3( 110.0f, 80.0f, 0.0f );
		arrowRight.AddTexture( "null", 0.5f, 0.0f, 0.5f, 1.0f );
		arrowRight.Parent( menuSwitch0 );
		Add( arrowRight );

		menuSwitch0.ArrowDecrease = arrowLeft;
		menuSwitch0.ArrowIncrease = arrowRight;

		for ( int i = 0; i < 101; ++i )
		{
			WidgetMenuNode node = new WidgetMenuNode( i );
			node.Parent( menuSwitch0 );
			Add( node );

			WidgetText text = new WidgetText();
            text.Position = new Vector3(75.0f, 80.0f, 0.0f);
			text.Size = new Vector3( 0.0f, 30.0f, 0.0f );
			//text.String = "" + i + "%";
			text.FontStyleName = "Default";
			text.Align = E_Align.MiddleCentre;
			text.ColorBase = Color.Orange;
			text.Parent( node );
			text.ParentAttach = E_Align.MiddleCentre;
			//Add( text );
		}

		// sfx volume
 		WidgetMenuNode node1 = (WidgetMenuNode)node0.Copy();
 		node1.Value = 1;
 		Add( node1 );
 		node1.Parent( menu );
 
 		WidgetText text1 = (WidgetText)text0.Copy();
 		text1.String = "SFX VOLUME";
 		text1.Parent( node1 );
 		Add( text1 );
 
 		WidgetMenuSwitch menuSwitch1 = (WidgetMenuSwitch)menuSwitch0.CopyAndAdd( this );
 		menuSwitch1.ArrowDecrease = menuSwitch1.FindChild( "arrow_decrease" );
 		menuSwitch1.ArrowIncrease = menuSwitch1.FindChild( "arrow_increase" );
 		menuSwitch1.Parent( node1 );

		
        // luminance
        WidgetMenuNode node2 = (WidgetMenuNode)node0.Copy();
        node2.Value = 2;
        Add(node2);
        node2.Parent(menu);

        WidgetText text2 = (WidgetText)text0.Copy();
        text2.String = "LUMINANCE";
        text2.Parent(node2);
        Add(text2);

        WidgetMenuSwitch menuSwitch2 = (WidgetMenuSwitch)menuSwitch0.CopyAndAdd(this);
        menuSwitch2.ArrowDecrease = menuSwitch2.FindChild("arrow_decrease");
        menuSwitch2.ArrowIncrease = menuSwitch2.FindChild("arrow_increase");
        menuSwitch2.Parent(node2);

        MenuSwitch0 = menuSwitch0;
        MenuSwitch1 = menuSwitch1;
        MenuSwitch2 = menuSwitch2;
        //----------------------------------

        Pin0 = new WidgetGraphic();
        Pin0.Size = new Vector3(25, 30, 1.0f);
        Pin0.Position = new Vector3(160, 240, 1.0f);
        Pin0.AddTexture("slider_pin", 0.0f, 0.0f, 1.0f, 1.0f);
        Pin0.ColorBase = Color.White;
        Add(Pin0);

        Pin1 = (WidgetGraphic)Pin0.Copy();
        Pin1.Position = new Vector3(160, 392, 1.0f);
        Add(Pin1);

        Pin2 = (WidgetGraphic)Pin0.Copy();
        Pin2.Position = new Vector3(160, 540, 1.0f);
        Add(Pin2);

        WidgetGraphic exitBtn = new WidgetGraphic();
        exitBtn.Size = new Vector3(80, 80, 0.0f);
        exitBtn.Position = new Vector3(60, 600, 5.0f);
        exitBtn.AddTexture("gamescene_exit", 0.0f, 0.0f, 1.0f, 1.0f);
        exitBtn.ColorBase = Color.White;
        Add(exitBtn);

        webcamText = new WidgetText();
        webcamText.Position = new Vector3(980, 50, 0.0f);
        webcamText.Size = new Vector3(0.0f, 30.0f, 0.0f);
        webcamText.Align = E_Align.MiddleCentre;
        webcamText.FontStyle = _UI.Store_FontStyle.Get("Default").Copy();
        webcamText.FontStyle.TrackingPercentage = 0.1875f;
        webcamText.String = "";
        webcamText.ColorBase = Color.Orange;
        Add(webcamText);
	}

	// OnInit
	protected override void OnInit()
	{

        _G.Game.orginVideo = new OriginEmgu(_G.Game.GraphicsDevice);    //  初始化摄像头捕捉到的原始视频
        _G.Game.processedVideo = new ProcessedEmgu(_G.Game.GraphicsDevice);

        _G.isOption = true;
        _G.Game.orginVideo.Start();
        _G.Game.processedVideo.Start();
		SetScreenTimers( 0.25f, 0.25f );
	}

	// OnPostInit
	protected override void OnPostInit()
	{
        lastTimeMenuValue = OptionMenu.GetByValue();
		MenuSwitch0.SetByValue( (int)(MediaPlayer.Volume * 100) );
        MenuSwitch1.SetByValue( (int)(SoundEffect.MasterVolume * 100));
        MenuSwitch2.SetByValue( _G.min_cr - 78 );
        webcamText.String = _G.ListCamerasData[_G.camIndex].Value + "(click to switch)";
	}

    protected override void OnEnd()
    {
        _G.isOption = false;
        base.OnEnd();
        _G.Game.orginVideo.Dispose();
        _G.Game.processedVideo.Dispose();
    }

    protected override void OnUpdate(float frameTime)
    {
        Console.WriteLine(_G.GameInput.Mouse.XY());
        if (lastTimeMenuValue != OptionMenu.GetByValue())
        {
            _G.Switch.Play();
            lastTimeMenuValue = OptionMenu.GetByValue();
        }

        previous = current;
        current = Keyboard.GetState();

        //add roi x
        if (current.IsKeyDown(Keys.NumPad6) && previous.IsKeyDown(Keys.NumPad6))
        {
            _G.roi_x += _G.roi_x < 640.0 / 2 ? 0.5 : 0.0;
        }

        //sub roi x
        if (current.IsKeyDown(Keys.NumPad4) && previous.IsKeyDown(Keys.NumPad4))
        {
            _G.roi_x -= _G.roi_x > 0 ? 0.5 : 0.0;
        }
        //add roi y
        if (current.IsKeyDown(Keys.NumPad2) && previous.IsKeyDown(Keys.NumPad2))
        {
            _G.roi_y += _G.roi_y < 480.0 / 2 ? 0.5 : 0.0;
        }

        //sub roi y
        if (current.IsKeyDown(Keys.NumPad8) && previous.IsKeyDown(Keys.NumPad8))
        {
            _G.roi_y -= _G.roi_x > 0 ? 0.5 : 0.0;
        }

        if (_G.roi_x < 0 || _G.roi_x > 640)
        {
            _G.roi_x = _G.roi_x < 0 ? 0 : 640;
        }
        if (_G.roi_y < 0 || _G.roi_y > 480)
        {
            _G.roi_y = _G.roi_y < 0 ? 0 : 480;
        }
        //-------------------------------------------------

        Pin0.Position = new Vector3(160 + MenuSwitch0.GetByValue() * 360 / 100, 240, 1.0f);
        Pin1.Position = new Vector3(160 + MenuSwitch1.GetByValue() * 360 / 100, 392, 1.0f);
        Pin2.Position = new Vector3(160 + MenuSwitch2.GetByValue() * 360 / 100, 540, 1.0f);

        base.OnUpdate(frameTime);
    }

	// OnProcessInput
	protected override void OnProcessInput( Input input )
    {
        // 鼠标触控
        if (_G.GameInput.Mouse.ButtonJustPressed((int)E_MouseButton.Left))
            _G.Pre_Mouse_X = _G.GameInput.Mouse.X();
        if (_G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left))
            _G.Current_Mouse_X = _G.GameInput.Mouse.X();

        if ((_G.GameInput.Mouse.X() > 72 && _G.GameInput.Mouse.X() < 146 && _G.GameInput.Mouse.Y() > 602 && _G.GameInput.Mouse.Y() < 670
        && _G.GameInput.Mouse.ButtonJustPressed((int)E_MouseButton.Left)))
        {
            _UI.Screen.SetNextScreen(new Screen_MainMenu());
            return;
        }

        // 确定修改option的项目
        if (_G.GameInput.Mouse.X() > OptionLow && _G.GameInput.Mouse.X() < OptionTop
    && _G.GameInput.Mouse.Y() > Pin0.Position.Y - 100 && _G.GameInput.Mouse.Y() < Pin0.Position.Y + 100)
            OptionMenu.SetByValue(0);
        if (_G.GameInput.Mouse.X() > OptionLow - 100 && _G.GameInput.Mouse.X() < OptionTop
    && _G.GameInput.Mouse.Y() > Pin1.Position.Y - 100 && _G.GameInput.Mouse.Y() < Pin1.Position.Y + 100)
            OptionMenu.SetByValue(1);
        if (_G.GameInput.Mouse.X() > OptionLow - 100 && _G.GameInput.Mouse.X() < OptionTop
    && _G.GameInput.Mouse.Y() > Pin2.Position.Y - 100 && _G.GameInput.Mouse.Y() < Pin2.Position.Y + 100)
            OptionMenu.SetByValue(2);

        // 拖动鼠标修改参数
        if (_G.GameInput.Mouse.ButtonDown((int)E_MouseButton.Left) &&
            _G.GameInput.Mouse.X() > OptionLow &&
            _G.GameInput.Mouse.X() < OptionTop && 
            _G.GameInput.Mouse.Y() > Pin0.Position.Y - 100 && 
            _G.GameInput.Mouse.Y() < Pin2.Position.Y + 100)
        {
            int setvalue = (int)(((float)(_G.GameInput.Mouse.X() - OptionLow) / (float)(OptionTop - OptionLow)) * 100);
            if (setvalue > 100)
                setvalue = 100;
            if (setvalue < 0)
                setvalue = 0;
            if (OptionMenu.GetByValue() == 0)
                MenuSwitch0.SetByValue(setvalue);
            if (OptionMenu.GetByValue() == 1)
                MenuSwitch1.SetByValue(setvalue);
            if (OptionMenu.GetByValue() == 2)
                MenuSwitch2.SetByValue(setvalue);
            RenewData();
        }
        // 切换camera
        if (_G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left) &&
            _G.GameInput.Mouse.X() > 750 &&
            _G.GameInput.Mouse.X() < 1192 &&
            _G.GameInput.Mouse.Y() > 5 &&
            _G.GameInput.Mouse.Y() < 80)
        {
            SwitchCamera();
        }

        if (input.ButtonJustPressed((int)E_UiButton.B) || _G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Right))
			_UI.Screen.SetNextScreen( new Screen_MainMenu() );
        else if (input.ButtonJustPressed((int)E_UiButton.Left) || input.ButtonJustPressed((int)E_UiButton.Right))
        {
            RenewData();
        }
	}

    private void SwitchCamera()
    {
        _G.camIndex = (_G.camIndex + 1) % _G.ListCamerasData.Count;
        webcamText.String = _G.ListCamerasData[_G.camIndex].Value + "(click to switch)";
        _G.isOption = false;
        _G.Game.orginVideo.Dispose();
        _G.Game.processedVideo.Dispose();

        _G.Game.orginVideo = new OriginEmgu(_G.Game.GraphicsDevice);
        _G.Game.processedVideo = new ProcessedEmgu(_G.Game.GraphicsDevice);
        _G.Game.orginVideo.Start();
        _G.Game.processedVideo.Start();
        _G.isOption = true;
    }

    private void RenewData()
    {
        MediaPlayer.Volume = (float)MenuSwitch0.GetByValue() / (float)100.0;
        SoundEffect.MasterVolume = (float)MenuSwitch1.GetByValue() / (float)100.0;
        _G.min_cr = 78 + MenuSwitch2.GetByValue();
    }

	//
    private WidgetMenuScroll        OptionMenu;
	private WidgetMenuSwitch		MenuSwitch0;
	private WidgetMenuSwitch		MenuSwitch1;
    private WidgetMenuSwitch        MenuSwitch2;

    private WidgetGraphic Pin0, Pin1, Pin2;

    private int lastTimeMenuValue;

    private KeyboardState           current, previous;            // 键盘状态，用于检测按键的触发
    
    // 鼠标拖动辅助坐标
    private int OptionTop = 535;
    private int OptionLow = 168;
    private WidgetText webcamText;
};

}; // namespace UI
