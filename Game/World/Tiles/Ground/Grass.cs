using Game.Core;
using Game.Objects.Animation;
using Raylib_cs;

namespace Game.World.Tile;


public class Grass: GenericTile {
    
    public UniqueSprite sprite = new( 151, 1, 32, 32 );

    public Grass( Main game ): base( game ) {

        Configure<Grass>( g => {
            g
            .setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( -1 );

            g.setRenderable( true )
            .setGameObjectID( GameObject.Grass )
            .getCollidable().setSolid( false )
            .setCanOverlapOthers( false )
            .setCanPushOthers( false );

        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        sprite.render( this, spriteSheet );

    }


}