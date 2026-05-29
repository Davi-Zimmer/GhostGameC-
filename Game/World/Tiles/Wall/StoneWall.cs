using Game.Core;
using Game.Objects.Animation;
using Raylib_cs;

namespace Game.World.Tile;


public class StoneWall: GenericTile {
    
    public UniqueSprite sprite = new( 184, 1, 32, 32 );

    public StoneWall( Main game ): base( game ) {

        Configure<StoneWall>( g => {
            g
            .setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( 1 );

            g.setRenderable( true )
            .setGameObjectID( GameObject.StoneWall )
            .getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );

        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        sprite.render( this, spriteSheet );

    }


}