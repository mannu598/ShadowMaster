//-----------------------------------------------
// XUI - Screen.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UI
{

// class Screen
public abstract class Screen
{
	// Screen
	public Screen()
		: this( "<no name>" )
	{
		//
	}

	public Screen( string name )
	{
		Name = name;
		AllowInput = true;
		DoUpdate = true;
		DoRender = true;
		Widgets = new List< WidgetBase >();
	}



	// Init
	public void Init()
	{
		SetScreenTimers( 0.0f, 0.0f ); // default

		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].Init();

		OnInit();

		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].PostInit();

		OnPostInit();
	}

	// OnInit
	protected virtual void OnInit()
	{
		//
	}

	// OnPostInit
	protected virtual void OnPostInit()
	{
		//
	}

	// StartLoop
	public bool StartLoop( float frameTime )
	{
		if ( !DoUpdate )
			return false;

		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].Update( frameTime );

		OnStartLoop( frameTime );

		return true;
	}

	// OnStartLoop
	protected virtual void OnStartLoop( float frameTime )
	{
		//
	}

	// ProcessInput
	public void ProcessInput( Input input )
	{
		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].ProcessInput( input );

		OnProcessInput( input );
	}

	// OnProcessInput
	protected virtual void OnProcessInput( Input input )
	{
		//
	}

	// ProcessMessage
	public void ProcessMessage( ref ScreenMessage message )
	{
		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].ProcessMessage( ref message );

		OnProcessMessage( ref message );
	}

	// OnProcessMessage
	protected virtual void OnProcessMessage( ref ScreenMessage message )
	{
		//
	}

	// Update
	public bool Update( float frameTime )
	{
		if ( !DoUpdate )
			return false;

		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].Update( frameTime );

		OnUpdate( frameTime );

		return true;
	}

	// OnUpdate
	protected virtual void OnUpdate( float frameTime )
	{
		//
	}

	// EndLoop
	public bool EndLoop( float frameTime )
	{
		if ( !DoUpdate )
			return false;

		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].Update( frameTime );

		OnEndLoop( frameTime );

		return true;
	}

	// OnEndLoop
	protected virtual void OnEndLoop( float frameTime )
	{
		//
	}

	// End
	public void End()
	{
		OnEnd();
	}

	// OnEnd
	protected virtual void OnEnd()
	{
		//
	}

	// Render
	public bool Render()
	{
		if ( !DoRender )
			return false;

		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].Render();

		OnRender();

		return true;
	}

	// OnRender
	protected virtual void OnRender()
	{
		//
	}

	// SetScreenTimers
	protected void SetScreenTimers( float startTime, float endTime )
	{
		_UI.Screen.SetScreenTimers( startTime, endTime );
	}

	// Add
	public void Add( WidgetBase widget )
	{
		Widgets.Add( widget );
	}

	// TimelineActive
	public void TimelineActive( string name, bool value, bool children )
	{
		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].TimelineActive( name, value, children );
	}

	// TimelineReset
	public void TimelineReset( string name, bool value, float time, bool children )
	{
		for ( int i = 0; i < Widgets.Count; ++i )
			Widgets[ i ].TimelineReset( name, value, time, children );
	}

	//
	public  string					Name;
	public  bool					AllowInput;
	public  bool					DoUpdate;
	public  bool					DoRender;
	private List< WidgetBase >		Widgets;
	//
};

}; // namespace UI
