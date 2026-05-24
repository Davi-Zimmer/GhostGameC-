using Game.Core;
using Game.Objects.Basics;

namespace Game.Objects;

public class Collidable {

    public List< GameObject > except = [];
    private bool solid = false;
    private bool canOverlapOthers = true;
    private bool canPushOthers = true;

    //--------------------------------- Getters ---------------------------------\\ 
    public bool getSolid() { return solid; }
    public List< GameObject > getExceptions() { return except; }
    public bool getCanOverlapOthers() { return canOverlapOthers; } 
    public bool getCanPushOthers() { return canPushOthers; } 


    //--------------------------------- Setters ---------------------------------\\ 
    public Collidable setSolid( bool b ) { solid = b; return this; }
    public Collidable setCanOverlapOthers( bool b ) { canOverlapOthers = b; return this; } 
    public Collidable setCanPushOthers( bool b ) { canPushOthers = b; return this; } 







    //--------------------------------- Methods ---------------------------------\\ 
    public static bool IsColliding( Rect a, Rect b ){

        return (
            a.extractX() + a.getW() > b.extractX() &&
            a.extractY() + a.getH() > b.extractY() &&
            b.extractX() + b.getW() > a.extractX() &&
            b.extractY() + b.getH() > a.extractY()
        );

    }

    public static Overlap? GetOverlap( Rect a, Rect b ){

        float dx = (float)(( a.extractX() + a.getW() / 2 ) - ( b.extractX() + b.getW() / 2 ));
        float dy = (float)(( a.extractY() + a.getH() / 2 ) - ( b.extractY() + b.getH() / 2 ));

        float px = (float)(( a.getW() / 2  + b.getW() / 2 ) - Math.Abs( dx ));
        float py = (float)(( a.getH() / 2  + b.getH() / 2 ) - Math.Abs( dy ));

        if( px <= 0 || py <= 0 ) return null;

        Overlap t = new Overlap( 
            dx > 0 ? px : -px,
            dy > 0 ? py : -py
        );

        return t;

    }



    public struct Overlap {
        public float x;
        public float y;

        public Overlap( float x, float y ) {
            this.x = x;
            this.y = y;
        }
    }

}