//-----------------------------------------------
// XUI - MenuNode.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;

namespace UI
{

// class WidgetMenuNode
public class WidgetMenuNode : WidgetBase
{
#if !RELEASE
	static Debug.BoolD d_RenderMenuNodes = new Debug.BoolD( "UI.RenderMenuNodes", false );
#endif

	// WidgetMenuNode
	public WidgetMenuNode()
		: this( -1, false )
	{
		//
	}

	public WidgetMenuNode( int value )
		: this( value, false )
	{
		//
	}

	public WidgetMenuNode( int value, bool locked )
		: base()
	{
		Value = value;
		Locked = locked;

	#if !RELEASE
		ColorBase = Color.White;
		ColorBase.A( 128 );

		AddTexture( "null", 0.0f, 0.0f, 1.0f, 1.0f );
	#endif
	}

	// CopyTo
	protected override void CopyTo( WidgetBase o )
	{
		base.CopyTo( o );

		WidgetMenuNode oo = (WidgetMenuNode)o;

		oo.Value = Value;
		oo.Locked = Locked;
	}

#if !RELEASE
	// OnRender
	protected override void OnRender()
	{
		if ( !d_RenderMenuNodes )
			return;

		using ( new AutoTransform( this ) )
		{

		_UI.Sprite.AddSprite( RenderPass, Layer, ref Position, ref Size, Align, ref ColorFinal, ref RenderState );
		_UI.Sprite.AddTexture( 0, Textures[ 0 ] );

		} // auto transform
	}
#endif

	//
	public int		Value;
	public bool		Locked;
	//
};

}; // namespace UI
