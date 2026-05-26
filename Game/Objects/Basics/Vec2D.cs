namespace Game.Objects.Basics;

public class Vec2D : Point2D {
    
    public Vec2D() {  }

    public Vec2D multiplyX( float x ){ setX( getX() * x ); return this; }
    public Vec2D multiplyY( float y ){ setY( getY() * y ); return this; }
    public Vec2D multiply( float x, float y ){ multiplyX( x ).multiplyY(y); return this; }


    public struct SimpleVec {
        public float x { get; }
        public float y { get; }

        public SimpleVec( float x, float y ) {
            this.x = x;
            this.y = y; 
        }

    }

    public static SimpleVec NormalizeVector( float dx, float dy, float magnitude = 1  ){

        float length = (float)Math.Sqrt( dx * dx + dy * dy );

        if( length == 0 ) return new SimpleVec( 0, 0 );

        return new SimpleVec(
            (dx / length) * magnitude,
            (dy / length) * magnitude
        );

    }

}