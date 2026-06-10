using Game.Core;
using Game.Objects;
using Game.Objects.Animation;
using Raylib_cs;

namespace Game.World.Tile;

public class GenericTile : WorldObject {
    
    public UniqueSprite sprite = new( 184, 1, 32, 32, 1, 0 );
    
    public GenericTile( Main game ) : base( game ) {

        Configure< GenericTile >( t => {
            
            t.setW( game.TileSize )
            .setH( game.TileSize );

            t.getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );

            t.setRenderable( true )
            .setGameObjectID( GameObject.GenericTile );

        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {

        // Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Red );
        sprite.render( this, spriteSheet );

    }
  
}