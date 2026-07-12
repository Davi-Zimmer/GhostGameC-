using Game.Core;
using Game.Rendering;
using Game.World.Entity;

namespace Game.World.Item;

public class Ectoplasma: GenericItem {
    
    public Ectoplasma( Main game ) : base( game ) {
        
        Configure< Ectoplasma >( e => {
           e.setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( 9 );

            e.setRenderable( true );
            e.setGameObjectID( GameObject.Ectoplasma )
            .getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( false )
            .setCanPushOthers( false );

            e.setCollisionTrigger( true );
            e.loadSpriteFrame( Sprites.Others.None );

        });

    }

    public override bool collisionTrigger<WorldObject>( WorldObject target ) {

        if( target is Player ) {

            Player p = ( target as Player )!;

            Random rdn = new();

            int quantity = rdn.Next( 1, 7 );

            p.getInvetory().addProjectile( getGameObjectID(), quantity );

            return true;

        }

        return false;

    }


}