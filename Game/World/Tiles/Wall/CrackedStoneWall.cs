using Game.Core;
using Game.Objects.Animation;
using Raylib_cs;

namespace Game.World.Tile;


public class CrackedStoneWall: GenericTile {
    
    public UniqueSprite sprite = new( 217, 34, 32, 32 );

    public CrackedStoneWall( Main game ): base( game ) {

        Configure<CrackedStoneWall>( g => {
            g
            .setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( 7 );

            g.setRenderable( true )
            .setGameObjectID( GameObject.CrackedStoneWall )
            .getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( true )
            .setCanPushOthers( true )
            .addException([
                GameObject.Slime
            ]);


        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        sprite.render( this, spriteSheet );

    }


}