namespace Game.Objects.Basics;


public class Point3D : Point2D {

    private float z = 0;


    public Point3D(): base() {
        
    }


    public int getIntZ() { return (int) z; }
    public float getZ() { return z; } 
    
    public Point2D apply( float x, float y, float z  ) { apply( x, y ); this.z += z; return this; }
    public Point2D applyZ( float z ) { this.z += z; return this; }

    //--------------------------------- Setters ---------------------------------\\ 
    public Point3D setZ( float z ) { this.z = z; return this; } 


} 