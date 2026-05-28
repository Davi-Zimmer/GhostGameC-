

using System.Drawing;

namespace Game.Objects.Basics;

public class Rect : Point3D {
    
    private float w { get; set; } = 0;
    private float h { get; set; } = 0;

    public Rect(): base() {

    }


    //--------------------------------- Getters ---------------------------------\\ 
    public float getW() { return w; } 
    public float getH() { return h; } 

    public int getIntW() { return (int) w; } 
    public int getIntH() { return (int) h; } 

    public float getMiddleX() { return getX() + getW() / 2; }
    public float getMiddleY() { return getY() + getH() / 2; }

    //--------------------------------- Setters ---------------------------------\\ 

    public Rect setW( float w ) { this.w = w; return this; }
    public Rect setH( float h ) { this.h = h; return this; }


}
