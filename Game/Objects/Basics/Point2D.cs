namespace Game.Objects.Basics;


public class Point2D {

    public float x { get; set; } = 0;
    public float y { get; set; } = 0;


    public Point2D() {

    }

    public T Configure<T>(Action<T> cfg) where T : Point2D {
        cfg((T)this);
        return (T)this;
    }


    public Point2D apply( float x, float y  ) { this.x += x; this.y += y; return this; }
    public Point2D applyX( float x ) { this.x += x; return this; }
    public Point2D applyY( float y ) { this.y += y; return this; }


    //--------------------------------- Getters ---------------------------------\\ 
    public float getX() { return x; } 
    public float getY() { return y; }


    public int getIntX() { return (int) x; } 
    public int getIntY() { return (int) y; }

    public virtual float extractX() { return x; }
    public virtual float extractY() { return y; }

    //--------------------------------- Setters ---------------------------------\\ 

    public Point2D setX( float x ) { this.x = x; return this; } 
    public Point2D setY( float y ) { this.y = y; return this; } 




} 