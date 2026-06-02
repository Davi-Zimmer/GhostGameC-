using Game.Core;
using Game.Objects;
using Game.Objects.Animation;
using Raylib_cs;

namespace Game.Item;


public class GenericItem: WorldObject {
    
    public UniqueSprite sprite = new( 184, 34, 32, 32 );

    public GenericItem( Main game ): base( game ) {

        Configure<GenericItem>( g => {
            g
            .setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( 9 );

            g.setRenderable( true )
            .setGameObjectID( GameObject.GenericItem )
            .getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( false )
            .setCanPushOthers( false );

            g.setCollisionTrigger( true );

        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        sprite.render( this, spriteSheet );

    }

    public override bool collisionTrigger<WorldObject>( WorldObject target ) {

        if( target.getGameObjectID() == GameObject.Player ) {
            
            Console.WriteLine("Pegou");

            return true;

        }

        return false;

    }

}