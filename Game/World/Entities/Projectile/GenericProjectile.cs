

using Game.Core;

namespace Game.World.Entity;

public class GenericProjectile: GenericEntity {
    
    public GenericProjectile( Main game ): base( game ) {
        
        Configure<GenericProjectile>( p => {
            
            p.setW( 10 ).setH( 10 )
            .setZ( 20 );

            p.setRenderable( true );

            p.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true )
            .setSolid( true );

        });

    }

    public override void tick( float delta ) {
    
        setXY(
            getX() + (getPhysics().getAcceleration().getX() * getSpeed()) * delta,
            getY() + (getPhysics().getAcceleration().getY() * getSpeed()) * delta
        );

    }

}