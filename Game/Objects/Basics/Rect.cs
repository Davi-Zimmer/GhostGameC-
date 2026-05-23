

namespace Game.Objects.Basics;

public class Rect : Point3D {
    
    private float w { get; set; } = 0;
    private float h { get; set; } = 0;
    
    public Rect(): base() {
        
    }



    //--------------------------------- Setters ---------------------------------\\ 

    public Rect setW( float w ) { this.w = w; return this; }
    public Rect setH( float h ) { this.h = h; return this; }


}
