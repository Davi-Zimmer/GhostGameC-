using Game.Core;
using Game.Objects.Basics;
using Raylib_cs;

namespace Game.Objects;

public class WorldObject: Rect {

    GameObject gameObjectID = GameObject.None;

    private bool renderable = true;
    private Physics? physics = null;
    private Collidable collidable = new();

    protected Main game;

    public WorldObject( Main game ) {
        
        this.game = game;

    }
    
    public virtual void tick( float delta ) {
        

    }

    public virtual void render( Camera2D cam, float delta, Texture2D spriteSheet ) {

        
    }


    //--------------------------------- Getters ---------------------------------\\ 
    public GameObject getGameObjectID() { return gameObjectID; }
    public bool getRenderable() { return renderable; }
    public virtual Physics? getPhysics() { return physics; }
    public Collidable getCollidable() { return collidable; }


    //--------------------------------- Setters ---------------------------------\\ 
    public WorldObject setGameObjectID( GameObject id ) { gameObjectID = id; return this;  }
    public WorldObject setRenderable( bool b ) {  renderable = b; return this; }
    public WorldObject setPhysic( Physics p ) { physics = p; return this; }
    public WorldObject setCollidable( Collidable p ) { collidable = p; return this; }
    public WorldObject addPhysic() { physics = new Physics( this ); return this; }
    public WorldObject addCollision() { collidable = new Collidable(); return this; }

}