using Game.Core;
using Game.Objects;
using Raylib_cs;

namespace Game.World.Tile;

public class GenericTile : WorldObject {
    
    public GenericTile( Main game ) : base( game ) {

        Configure< GenericTile >( t => {
            
            t.setW( 50 )
            .setH( 50 );

            t.getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );

            t.setRenderable( true )
            .setGameObjectID( GameObject.GenericTile );

        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {

        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Red );

    }
  
}