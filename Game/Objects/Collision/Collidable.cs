using Game.Core;
using Game.Objects.Basics;

namespace Game.Objects;

public class Collidable {

    private bool solid = false;
    private bool canOverlapOthers = true;
    private bool canPushOthers = true;
    private List< GameObject > collisionExceptions = [];

    //--------------------------------- Getters ---------------------------------\\ 
    public bool getSolid() { return solid; }
    public bool getCanOverlapOthers() { return canOverlapOthers; } 
    public bool getCanPushOthers() { return canPushOthers; } 
    public List< GameObject > getExceptions() { return collisionExceptions; } 


    //--------------------------------- Setters ---------------------------------\\ 
    public Collidable setSolid( bool b ) { solid = b; return this; }
    public Collidable setCanOverlapOthers( bool b ) { canOverlapOthers = b; return this; } 
    public Collidable setCanPushOthers( bool b ) { canPushOthers = b; return this; } 


    //--------------------------------- Methods ---------------------------------\\ 
    public Collidable addException( List<GameObject> ex ) { ex.ForEach( e => collisionExceptions.Add( e ) ); return this; } 
    
    public static bool IsColliding( Rect a, Rect b,  float delta ){

        return (
            a.extractX(delta) + a.getW() > b.extractX(delta) &&
            a.extractY(delta) + a.getH() > b.extractY(delta) &&
            b.extractX(delta) + b.getW() > a.extractX(delta) &&
            b.extractY(delta) + b.getH() > a.extractY(delta)
        );

    }

    public static Overlap? GetOverlap( Rect a, Rect b ){

        float dx = (float)( ( a.getX() + a.getW() / 2f ) - ( b.getX() + b.getW() / 2f ) );
        float dy = (float)( ( a.getY() + a.getH() / 2f ) - ( b.getY() + b.getH() / 2f ) );

        float px = (float)( ( a.getW() / 2f  + b.getW() / 2f ) - Math.Abs( dx ) );
        float py = (float)( ( a.getH() / 2f  + b.getH() / 2f ) - Math.Abs( dy ) );

        if( px <= 0 || py <= 0 ) return null;
    
        Overlap t = new Overlap( 
            dx > 0 ? px : -px,
            dy > 0 ? py : -py
        );

        return t;

    }

    public struct Overlap {
        public float x = 0;
        public float y = 0;

        public Overlap( float x, float y ) {
            this.x = x;
            this.y = y;
        }
    }

}