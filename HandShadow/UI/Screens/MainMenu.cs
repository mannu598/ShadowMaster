//-----------------------------------------------
// XUI - MainMenu.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Net;
using System.Web;

namespace UI
{

// class Screen_MainMenu
public class Screen_MainMenu : Screen
{
    

	// Screen_MainMenu
	public Screen_MainMenu()
		: base( "MainMenu" )
	{
        WidgetGraphic bg = new WidgetGraphic();
        bg.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
        bg.AddTexture("mainmenu_bg", 0.0f, 0.0f, 1.0f, 1.0f);
        bg.ColorBase = Color.White;
        Add(bg);

        WidgetGraphic logo_word = new WidgetGraphic();
        logo_word.Size = new Vector3(561, 191, 0.0f);
        logo_word.AddTexture("logo_word", 0.0f, 0.0f, 1.0f, 1.0f);
        logo_word.Position = new Vector3(-400, 80, 0.0f);
        logo_word.ColorBase = Color.White;
        Add(logo_word);

        Timeline logo_push = new Timeline("logo_push", true, 0.0f, 1.0f, E_TimerType.Stop, E_RestType.None);
        logo_push.AddEffect(new TimelineEffect_PositionX(0.0f, 500, E_LerpType.SmootherStep));
        logo_word.AddTimeline(logo_push);

        Random ra = new Random();
        int a = ra.Next() % 2 + 1;
        int b = ra.Next() % 9 + 1;
        shadow = new WidgetGraphic();
        shadow.Size = new Vector3(285, 198, 0.0f);
        shadow.AddTexture("shadow" + a.ToString() + "_" + b.ToString(), 0, 0, 1, 1);
        shadow.ColorBase = Color.White;
        shadow.Position = new Vector3(208, 398, 0.0f);
        shadow.Alpha = 0;
        Add(shadow);
        shadowIsFadingIn = true;

        
        list = new WidgetGraphic();
        list.Size = new Vector3(548, 704, 0);
        list.Position = new Vector3(680, -800, 0);
        list.AddTexture("mainmenu_list", 0.0f, 0.0f, 1.0f, 1.0f);
        list.ColorBase = Color.White;
        Add(list);


        Timeline list_down = new Timeline("start", false, 0.0f, 1.5f, E_TimerType.Stop, E_RestType.None);
        list_down.AddEffect(new TimelineEffect_PositionY(0.0f, 800, E_LerpType.SmootherStep));
        list.AddTimeline(list_down);
        _G.Listdown.Play();

        Timeline list_up = new Timeline("start", false, 1.5f, 0.25f, E_TimerType.Stop, E_RestType.None);
        list_up.AddEffect(new TimelineEffect_PositionY(0.0f, -50, E_LerpType.SmootherStep));
        list.AddTimeline(list_up);
        

        Timeline list_bye = new Timeline("end_fade", false, 0.0f, 1.00f, E_TimerType.Stop, E_RestType.Start);
        list_bye.AddEffect(new TimelineEffect_PositionY(0.0f, -800, E_LerpType.SmootherStep));
        list.AddTimeline(list_bye);
        /*
		WidgetGraphic logo = new WidgetGraphic();
		logo.Position = new Vector3( _UI.SXM, _UI.SYM - 65.0f, 0.0f );
		logo.Size = new Vector3( _UI.SY / 3.0f, _UI.SY / 3.0f, 0.0f );
		logo.Align = E_Align.BottomCentre;
        logo.ColorBase = new SpriteColors(Color.White, Color.White, Color.White, Color.White);
        logo.AddTexture("logo", 0.0f, 0.0f, 1.0f, 1.0f);
		Add( logo );

		Timeline logoT = new Timeline( "end_fade", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.Start );
		logoT.AddEffect( new TimelineEffect_Alpha( 0.0f, -1.0f, E_LerpType.SmoothStep ) );
		logo.AddTimeline( logoT );

		Timeline logoT2 = new Timeline( "end_move", false, 0.25f, 0.25f, E_TimerType.Stop, E_RestType.None );
		logoT2.AddEffect( new TimelineEffect_PositionY( 0.0f, 65.0f, E_LerpType.SmoothStep ) );
		logo.AddTimeline( logoT2 );
        
		if ( _G.UI.MM_FromStartScreen )
		{
			logo.Position = new Vector3( _UI.SXM, _UI.SYM, 0.0f );

			Timeline logoT3 = new Timeline( "start", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.None );
			logoT3.AddEffect( new TimelineEffect_PositionY( 0.0f, -65.0f, E_LerpType.SmoothStep ) );
			logo.AddTimeline( logoT3 );
		}
		else
		if ( _G.UI.MM_FromLevelSelect )
		{
			Timeline logoT4 = new Timeline( "start", false, 0.25f, 0.25f, E_TimerType.Stop, E_RestType.None );
			logoT4.AddEffect( new TimelineEffect_Alpha( -1.0f, 0.0f, E_LerpType.SmoothStep ) );
			logo.AddTimeline( logoT4 );
		}

		_G.UI.MM_FromStartScreen = false;
		_G.UI.MM_FromLevelSelect = false;

		Logo = logo;
        */

		WidgetMenuScroll menu = new WidgetMenuScroll( E_MenuType.Vertical );
		menu.Position = new Vector3(960, 140, 0.0f );
		menu.Padding = 152.0f;
		menu.Alpha = 0.0f;
		Add( menu );

		Timeline menuT = new Timeline( "start", false, 1.25f, 2.5f, E_TimerType.Stop, E_RestType.None );
		menuT.AddEffect( new TimelineEffect_Alpha( 0.0f, 1.0f, E_LerpType.SmoothStep ) );
		menu.AddTimeline( menuT );

		Timeline menuT2 = new Timeline( "end", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.None );
		menuT2.AddEffect( new TimelineEffect_Alpha( 0.0f, -1.0f, E_LerpType.SmoothStep ) );
		menu.AddTimeline( menuT2 );

		Menu = menu;
        

		for ( int i = 0; i < Options.Length; ++i )
		{
			WidgetMenuNode node = new WidgetMenuNode( i );
			node.Parent( Menu );
			Add( node );

			Timeline nodeT = new Timeline( "selected", false, 0.0f, 0.25f, E_TimerType.Stop, E_RestType.Start );
			nodeT.AddEffect( new TimelineEffect_ScaleX( 0.0f, 0.125f, E_LerpType.SmoothStep ) );
			nodeT.AddEffect( new TimelineEffect_ScaleY( 0.0f, 0.125f, E_LerpType.SmoothStep ) );
			
			Timeline nodeT2 = new Timeline( "selected", false, 0.0f, 0.5f, E_TimerType.Bounce, E_RestType.Start );
			nodeT2.AddEffect( new TimelineEffect_Intensity( 0.0f, 0.75f, E_LerpType.SmoothStep ) );

			node.AddTimeline( nodeT );
			node.AddTimeline( nodeT2 );

			WidgetText text = new WidgetText();
			text.Size = new Vector3( 0.0f, 50.0f, 0.0f );
			text.Align = E_Align.MiddleCentre;
			text.FontStyleName = "Default";
			text.String = Options[ i ];
			text.Parent( node );
			text.ParentAttach = E_Align.MiddleCentre;
			text.ColorBase = Color.Orange;
			Add( text );

            WidgetGraphic black = new WidgetGraphic();
            black.Size = new Vector3(_UI.SX, _UI.SY, 0.0f);
            black.AddTexture("null", 0.0f, 0.0f, 1.0f, 1.0f);
            black.ColorBase = Color.Black;
            Add(black);
            Timeline blackT = new Timeline("start", true, 0.0f, 1.0f, E_TimerType.Stop, E_RestType.Start);
            blackT.AddEffect(new TimelineEffect_Alpha(0.0f, -1.0f, E_LerpType.Linear));
            black.AddTimeline(blackT);

		}
	}

	// OnInit
	protected override void OnInit()
	{
        //weibo();
		SetScreenTimers( 0.25f, 0.25f );
	}

    protected override void OnPostInit()
    {
        lastTimeMenuValue = Menu.GetByValue();
        if (MediaPlayer.State == MediaState.Stopped)
        {
            MediaPlayer.Play(_G.MenuBg);
            MediaPlayer.IsRepeating = true;
        }
        base.OnPostInit();
    }

	// OnProcessInput
	protected override void OnProcessInput( Input input )
	{

        // 鼠标控制
        if (_G.GameInput.Mouse.X() > 700 && _G.GameInput.Mouse.X() < 1209)
        {
            if (_G.GameInput.Mouse.Y() > 95 && _G.GameInput.Mouse.Y() < 190)
            {
                Menu.SetByValue(0);
            }
            else if (_G.GameInput.Mouse.Y() > 249 && _G.GameInput.Mouse.Y() < 335)
            {
                Menu.SetByValue(1);
            }
            else if (_G.GameInput.Mouse.Y() > 403 && _G.GameInput.Mouse.Y() < 492)
            {
                Menu.SetByValue(2);
            }
            else if (_G.GameInput.Mouse.Y() > 552 && _G.GameInput.Mouse.Y() < 648)
            {
                Menu.SetByValue(3);
            }

            if (_G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Left))
            {
                if (Menu.GetByValue() == 0)
                {
                    list.TimelineActive("end_fade", true, false);
                    //Logo.TimelineActive("end_fade", true, false);

                    _UI.Screen.SetNextScreen(new Screen_LevelSelect());
                }
                else
                    if (Menu.GetByValue() == 1)
                    {
                        _G.gameLevel = -1;
                        _G.gameNum = -1;
                        list.TimelineActive("end_fade", true, false);
                        // Logo.TimelineActive("end_fade", true, false);
                        _UI.Screen.SetNextScreen(new Screen_Loading());
                    }
                    else
                        if (Menu.GetByValue() == 2)
                        {
                            list.TimelineActive("end_fade", true, false);
                            _UI.Screen.SetNextScreen(new Screen_Options());
                        }
                        else
                            if (Menu.GetByValue() == 3)
                            {
                                _UI.Screen.AddScreen(new Screen_Popup(E_PopupType.Quit));
                            }
            }
        }

        // 键盘控制

		if ( input.ButtonJustPressed( (int)E_UiButton.A ) )
		{
            _G.Listup.Play();
			if ( Menu.GetByValue() == 0 )
			{
                list.TimelineActive("end_fade", true, false);
               //Logo.TimelineActive("end_fade", true, false);
                
                _UI.Screen.SetNextScreen(new Screen_LevelSelect());
			}
			else
			if ( Menu.GetByValue() == 1 )
            {
                _G.gameLevel = -1;
                _G.gameNum = -1;
                list.TimelineActive("end_fade", true, false);
               // Logo.TimelineActive("end_fade", true, false);
                _UI.Screen.SetNextScreen(new Screen_Loading());
			}
			else
			if ( Menu.GetByValue() == 2 )
			{
                list.TimelineActive("end_fade", true, false);
				_UI.Screen.SetNextScreen( new Screen_Options() );
			}
			else
			if ( Menu.GetByValue() == 3 )
			{
				_UI.Screen.AddScreen( new Screen_Popup( E_PopupType.Quit ) );
			}
		}
		else
            if (input.ButtonJustPressed((int)E_UiButton.B) || _G.GameInput.Mouse.ButtonJustReleased((int)E_MouseButton.Right))
		{
            MediaPlayer.Stop();
			SetScreenTimers( 0.0f, 0.5f );
            list.TimelineActive("end_fade", true, false);
			//Logo.TimelineActive( "end_move", true, false );
			_G.UI.SS_FromMainMenu = true;
			
			_UI.Screen.SetNextScreen( new Screen_Start() );
		}
	}

    protected override void OnUpdate(float frameTime)
    {
        if (lastTimeMenuValue != Menu.GetByValue())
        {
            _G.Switch.Play();
            lastTimeMenuValue = Menu.GetByValue();
        }
        if (shadowIsFadingIn)
        {
            if (shadow.Alpha < 0.6f)
            {
                shadow.Alpha += 0.005f;
            }
            else
                shadowIsFadingIn = false;
        }
        else
        {
            if (shadow.Alpha > 0)
            {
                shadow.Alpha -= 0.005f;
            }
            else
            {
                Random ra = new Random();
                int a = ra.Next() % 2 + 1;
                int b = ra.Next() % 9 + 1;

                shadow.ChangeTexture(0, "shadow" + a.ToString() + "_" + b.ToString(), 0, 0, 1, 1);
                shadowIsFadingIn = true;
            }
        }
        base.OnUpdate(frameTime);
    }

	// OnProcessMessage
	protected override void OnProcessMessage( ref ScreenMessage message )
	{
		E_UiMessageType type = (E_UiMessageType)message.Type;

		if ( type == E_UiMessageType.PopupConfirm )
		{
			switch ( (E_PopupType)message.Data )
			{
				case E_PopupType.NewGame:	/* new game logic here */ 		break;
				case E_PopupType.Quit:		_UI.Game.Exit();				break;
			}
		}
	}

    public void weibo()
    {
        var oauth = new NetDimension.Weibo.OAuth("1727300261", "17d31a78da307172e2786faf42300b70", "http://DiseaseCourse.com");
        var authUrl = oauth.GetAuthorizeURL();
        System.Diagnostics.Process.Start(authUrl);
        var code = "52fa8383f9d13d64f31ad97b7b796b13";
        var accessToken = oauth.GetAccessTokenByAuthorizationCode(code);
    }
    

	//
    private static string[]         TextureNames = { "point_right", "point_left", "point_right", "point_left" };
	private static string[]			Options = { "CLASSIC GAME", "ACRADE MODE", "OPTIONS", "QUIT" };

	private WidgetMenuScroll		Menu;
    private WidgetGraphic list;
    private WidgetGraphic shadow;
    private bool shadowIsFadingIn;
    private int lastTimeMenuValue;
	//
};

}; // namespace UI
