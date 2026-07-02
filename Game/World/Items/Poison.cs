using Game.Core;
using Game.Objects;
using Game.Objects.Animation;
using Game.Rendering;
using Game.World.Entity;
using Raylib_cs;

namespace Game.World.Item;

public class Poison: GenericItem {
    
    public Poison( Main game ): base( game ) {

        Configure<Poison>( g => {
            g
            .setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( 9 );

            g.setRenderable( true )
            .setGameObjectID( GameObject.Poison )
            .getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( false )
            .setCanPushOthers( false );

            g.setCollisionTrigger( true );

            g.loadSpriteFrame( Sprites.Poison.icon );

        });

    }

    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        sprite.render( this, spriteSheet );

    }

    public override void use() {
        
        Player p = game.getPlayer();

        p.applyLife( 25 );
        
        p.getInvetory().dropItem( GameObject.Poison );
        
    }   

    public override bool collisionTrigger<WorldObject>( WorldObject target ) {

        if( target.getGameObjectID() == GameObject.Player ) {
            
            Player targ = ( target as Player )!;

            targ.getInvetory().pickItem( this );

            return true;

        }

        return false;

    }

}