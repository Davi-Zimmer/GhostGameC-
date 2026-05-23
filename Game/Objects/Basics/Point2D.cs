namespace Game.Objects.Basics;


public class Point2D {

    public double x { get; set; } = 0;
    public double y { get; set; } = 0;


    public Point2D() {

    }

    public T Configure<T>(Action<T> cfg) where T : Point2D {
        cfg((T)this);
        return (T)this;
    }


    public Point2D apply( double x, double y  ) { this.x += x; this.y += y; return this; }
    public Point2D applyX( double x ) { this.x += x; return this; }
    public Point2D applyY( double y ) { this.y += y; return this; }


    //--------------------------------- Getters ---------------------------------\\ 
    public double getX() { return x; } 
    public double getY() { return y; }


    public int getIntX() { return (int) x; } 

    public int getIntY() { return (int) y; }

    //--------------------------------- Setters ---------------------------------\\ 

    public Point2D setX( double x ) { this.x = x; return this; } 
    public Point2D setY( double y ) { this.y = y; return this; } 




} 