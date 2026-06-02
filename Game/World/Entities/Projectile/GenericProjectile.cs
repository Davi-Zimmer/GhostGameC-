

using Game.Core;

namespace Game.World.Entity;

public class GenericProjectile: GenericEntity {
    
    public int damage = 0; 

    public GenericProjectile( Main game ): base( game ) {
        
        Configure<GenericProjectile>( p => {
            
            p.setW( 10 ).setH( 10 )
            .setZ( 20 );

            p.setRenderable( true );

            p.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true )
            .setSolid( true );

            p.setCollisionTrigger( true );
        
        });

    }

    public override bool collisionTrigger<T>(T target) {

        if( target is GenericEntity ) {
            
            GenericEntity e = ( target as GenericEntity )!;

            e.applyLife( -getDamage() );

        }

        return true;

    }


    public override void tick( float delta ) {
    
        setXY(
            getX() + (getPhysics().getAcceleration().getX() * getSpeed()) * delta,
            getY() + (getPhysics().getAcceleration().getY() * getSpeed()) * delta
        );

    }



    //--------------------------------- Getters ---------------------------------\\ 
    public int getDamage() { return damage; }


    //--------------------------------- Setters ---------------------------------\\ 
    public GenericProjectile setDamage( int i ) { damage = i; return this; }

}