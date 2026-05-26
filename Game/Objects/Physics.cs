using Game.Objects.Basics;

namespace Game.Objects;

public class Physics {
    private float mass = 1;
    private float friction = .9f;

    private float knockback = 100;

    private bool isFixed = false; 
        
    private Vec2D acceleration = new();
    private Vec2D orientation  = new();

    private Rect parent;

    public Physics( Rect parent ) {
        
        this.parent = parent;

    }

    private float calcDirectionForce ( int direction, float otherMass, float knockback, float delta ) {

        float ratio = otherMass / getMass();

        if( ratio < 0.15f ) return 0;

        float force = knockback * MathF.Pow( ratio, 2f );

        return force * direction * delta;
    }

    public void pushX( int direction, float otherMass, float knockback, float delta ) {

        acceleration.applyX( calcDirectionForce( direction, otherMass, knockback, delta ) * 10 );

    }

    public void pushY( int direction, float otherMass, float knockback, float delta ) {

        acceleration.applyY( calcDirectionForce( direction, otherMass, knockback, delta ) * 10);

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