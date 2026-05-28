using Game.Core;
using Game.Objects;
using Game.Objects.Basics;
using Raylib_cs;
namespace Game.World.Entity;

public class GenericEntity: Rect {
    
    public delegate void Configuration( GenericEntity e );

    GameObject gameObjectID = GameObject.GenericEntity;

    private Physics physics;
    private Collidable collidable;

    private float speed = 1;

    public GenericEntity() {

        physics    = new Physics( this );
        collidable = new Collidable();
    
        Configure<GenericEntity>( e => {
            e
            .setW( 100 )
            .setH( 100 );

            e.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );
            
            e.getPhysics().setMass( 5 );
        });
    }

    
    public virtual void tick( float delta ) {
        
        updatePosition( delta );
        // Console.WriteLine( getPhysics().getOrientation().getX() );

        if( Raylib.IsMouseButtonDown( MouseButton.Left )  ) physics.getAcceleration().applyX( -100 );
        if( Raylib.IsMouseButtonDown( MouseButton.Right ) ) physics.getAcceleration().applyX(  100 );

    }


    public virtual void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        Raylib.DrawRectangle( (int)extractX(delta), (int)extractY(delta), getIntW(), getIntH(), Color.Blue );
        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Red );
        // Console.WriteLine( getIntX() + ' ' + getIntY() );
        
    }

    //--------------------------------- Getters ---------------------------------\\ 
    public float getSpeed() { return speed; }

    public Physics getPhysics() { return physics; }
    public Collidable getCollidable() { return collidable; }
    
    public GameObject getGameObjectID() { return gameObjectID; }


    //--------------------------------- Setters ---------------------------------\\ 
    public GenericEntity setGameObjectID( GameObject id ) { gameObjectID = id; return this;  }

    public GenericEntity setSpeed( float s ) { speed = s; return this; }

    public GenericEntity setPhysic( Physics p ) { physics = p; return this; }
    public GenericEntity setCollidable( Collidable p ) { collidable = p; return this; }

    public GenericEntity addPhysic() { physics = new Physics( this ); return this; }

    public GenericEntity addCollision() { collidable = new Collidable(); return this; }

    public override float extractX( float delta ) { 
        return getX() + (physics.getAcceleration().getX() + physics.getOrientation().getX()) * delta;
    }
    
    public override float extractY( float delta ) { 
        return getY() + (physics.getAcceleration().getY() + physics.getOrientation().getY()) * delta;
    }

    protected virtual void updatePosition( float delta ) {

        var vec = Vec2D.NormalizeVector (
            physics.getOrientation().getX(),
            physics.getOrientation().getY(),
            getSpeed()
        );

        setXY( 
            getX() + ( physics.getAcceleration().getX() + vec.x ) * delta, 
            getY() + ( physics.getAcceleration().getY() + vec.y ) * delta 
        );
        
        float friction = physics.getFriction();

        physics.getAcceleration().multiply( friction, friction );

    }


}