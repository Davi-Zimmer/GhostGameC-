namespace Game.Objects.Basics;

public class Vec2D : Point2D {
    
    public Vec2D() {  }

    public Vec2D multiplyX( float x ){ setX( getX() * x ); return this; }
    public Vec2D multiplyY( float y ){ setY( getY() * y ); return this; }
    public Vec2D multiply( float x, float y ){ multiplyX( x ).multiplyY(y); return this; }

}