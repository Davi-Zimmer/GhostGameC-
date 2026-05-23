namespace Game.Objects.Basics;


public class Point2D {

    public double x { get; set; } = 0;
    public double y { get; set; } = 0;


    public Point2D() {

    }

    public int intX => (int) x;
    public int intY => (int) y;


    public Point2D apply( double x, double y  ) { this.x += x; this.y += y; return this; }
    public Point2D applyX( double x ) { this.x += x; return this; }
    public Point2D applyY( double y ) { this.y += y; return this; }


    //--------------------------------- Setters ---------------------------------\\ 

    public Point2D setX( double x ) { this.x = x; return this; } 
    public Point2D setY( double y ) { this.y = y; return this; } 




} 