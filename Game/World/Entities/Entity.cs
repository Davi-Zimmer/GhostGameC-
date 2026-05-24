using Game.Core;
using Game.Objects;
using Game.Objects.Basics;
using Raylib_cs;
namespace Game.World.Entity;

public class GenericEntity: Rect {
    
    public delegate void Configuration( GenericEntity e );

    GameObject gameObjectID = GameObject.GenericEntity;

    private Physics? physics;
    private Collidable? collidable;

    private float speed = 1;

    public GenericEntity() {

    }

    public virtual void tick( float delta ) {
        
    }


    public virtual void render( Camera2D cam ) {
        
        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Red );


    }

    //--------------------------------- Getters ---------------------------------\\ 
    public float getSpeed() { return speed; }

    public Physics? getPhysics() { return physics; }
    public Collidable? getCollidable() { return collidable; }
    


    //--------------------------------- Setters ---------------------------------\\ 
    public GenericEntity setGameObjectID( GameObject id ) { gameObjectID = id; return this;  }

    public GenericEntity setSpeed( float s ) { speed = s; return this; }

    public GameObject setGameObjectID() { return gameObjectID; }

    public GenericEntity setPhysic( Physics? p ) { physics = p; return this; }
    public GenericEntity setCollidable( Collidable? p ) { collidable = p; return this; }

    public GenericEntity addPhysic() { physics = new Physics( this ); return this; }

    public GenericEntity addCollision() { collidable = new Collidable(); return this; }

}