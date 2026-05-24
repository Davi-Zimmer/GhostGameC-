using Game.Objects.Basics;

namespace Game.Objects;

public class Physics {
    private float mass = 1;
    private float friction = 1;

    private float knockback = 1;

    private bool isFixed = false; 
        
    private Vec2D acceleration = new();
    private Vec2D orientation  = new();

    private Rect parent;

    public Physics( Rect parent ) {
        
        this.parent = parent;

    }


    public void pushX( int direction, float otherMass, float knockback ) {
        
    }

    public void pushY( int direction, float otherMass, float knockback ) {
        
    }




    //--------------------------------- Getters ---------------------------------\\ 
    public float getMass() { return mass; }
    public float getFriction() { return friction; }

    public Vec2D getAcceleration() { return acceleration; }
    public Vec2D getOrientation() { return orientation; }
    public float getKnockback() { return knockback; }
    public bool getFixed() { return isFixed; }


    //--------------------------------- Setters ---------------------------------\\ 
    public Physics setMass( float m ) { mass = (float)Math.Min( m, 0.0001 ); return this; }
    public Physics setFriction( float f ) { friction = f; return this;  }
    public Physics setKnockback( float k ) { knockback = k; return this; }
    public Physics setFixed( bool b ) { isFixed = b; return this; }

}