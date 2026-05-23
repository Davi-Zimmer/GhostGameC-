namespace Game.Objects.Basics;


public class Point3D : Point2D {

    public double z { get; set; } = 0;


    public Point3D(): base() {
        
    }


    public int intZ => (int) z;

    
    public Point2D apply( double x, double y, double z  ) { apply( x, y ); this.z += z; return this; }
    public Point2D applyZ( double z ) { this.z += z; return this; }

    //--------------------------------- Setters ---------------------------------\\ 
    public Point3D setZ( double z ) { this.z = z; return this; } 


} 