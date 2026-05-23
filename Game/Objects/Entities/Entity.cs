using Game.Core;
using Game.Objects.Basics;
using Raylib_cs;
namespace Game.Objects.Entities;

public class GenericEntity: Rect {
    
    public delegate void Configuration( GenericEntity e );

    GameObject gameObjectID = GameObject.GenericEntity;

    private float speed = 1;

    public GenericEntity() {
        
    }

    public virtual void tick( double delta ) {
        
    }


    public virtual void render() {
        
    }



    //--------------------------------- Getters ---------------------------------\\ 
    public GameObject setGameObjectID() { return gameObjectID; }

    public GenericEntity setSpeed( float s ) { speed = s; return this; }

    //--------------------------------- Setters ---------------------------------\\ 
    public GenericEntity setGameObjectID( GameObject id ) { gameObjectID = id; return this;  }

    public float getSpeed() { return speed; }


}