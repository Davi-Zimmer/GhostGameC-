using System.Numerics;
using Game.Objects;
using Game.Objects.Basics;
using Game.World.Entity;
using Game.World.Entity.Enemy;
using Game.World.Tile;
using Raylib_cs;

namespace Game.Core;

public class Main {

    public int TileSize = 50;

    Camera2D cam = new();

    private List< WorldObject > map = [];

    private Player player;

    private Rect? cameraTarget = null;

    public Texture2D spritesheet = new();

    private float innerWidth = 0;
    private float innerHeight = 0;

    public Main() {
    
        var p = new Player( this );

        player = p;
        cameraTarget = p;

        addToMap( p );

        configCamera( p );

        // map.Add( new GenericEntity( this ).Configure<GenericEntity>( e => e.setXY( 200, 200 ) ) );
        // map.Add( new GenericEntity( this ).Configure<GenericEntity>( e => e.setXY( 200, 400 ) ) );

        addToMap( new StoneWall( this ).Configure<StoneWall>( t => t.setXY( 500f, 200f ) ) );

        addToMap( new Slime( this ) );

        int A = 50;
        int size = 50;

        for ( int x = 0; x < A; x++ ) {

            for ( int y = 0; y < A; y++ ) {
                
                addToMap( new Grass( this ).Configure<GenericTile>( t => t.setXY( x * size, y * size ) ) );

            }

        }

    }

    public void setup() {

        innerWidth  = Raylib.GetScreenWidth();
        innerHeight = Raylib.GetScreenHeight();

        string path = Directory.GetCurrentDirectory() + "/Assets/placeholder.png";

        spritesheet = Raylib.LoadTexture( path );

    }

    public void finish() {

        Raylib.UnloadTexture( spritesheet );

    }

    public void configCamera( Player p ) {
        
        cam.Target = new Vector2( p.getX(), p.getY() ); 
    
        cam.Target   = new Vector2( p.getX() + 20.0f, p.getY() + 20.0f );
        cam.Offset   = new Vector2( innerWidth / 2f, innerHeight / 2f );
        cam.Rotation = 0.0f;
        cam.Zoom     = 1.0f;

    }

    private void collisionPush( WorldObject e, WorldObject other, Collidable.Overlap overlap, float delta ) {

        bool horizontal = Math.Abs( overlap.x ) < Math.Abs( overlap.y );

        if( horizontal ) {

            Collidable oColl = other.getCollidable();
            Physics op       = other.getPhysics()!;
            Physics ep       = e.getPhysics()!;

            if( op != null && ep != null ) {
                
                if( oColl.getCanOverlapOthers() ) e.applyX( overlap.x ); else {
                    
                    if( e.getCollidable().getCanPushOthers() ) {

                        op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );

                    }

                }

                if( op.getFixed() ) return;

                op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );
            }

            else if( oColl.getCanOverlapOthers() ) e.applyX( overlap.x );


        } else {
           
             Collidable oColl = other.getCollidable();
            Physics op        = other.getPhysics()!;
            Physics ep        = e.getPhysics()!;

            if( op != null && ep != null ) {
                
                if( oColl.getCanOverlapOthers() ) e.applyY( overlap.y ); else {
                    
                    if( e.getCollidable().getCanPushOthers() ) {

                        op.pushY( Math.Sign( -overlap.y ), ep.getMass(), ep.getKnockback(), delta );

                    }

                }

                if( op.getFixed() ) return;

                op.pushY( Math.Sign( -overlap.y ), ep.getMass(), ep.getKnockback(), delta );
            }

            else if( oColl.getCanOverlapOthers() ) e.applyY( overlap.y );

        }

    }

    private void collision( WorldObject e,  float delta ) {
        
        if( !e.getCollidable().getSolid() ) return;

        foreach( var other in map ) {

            if( !other.getCollidable().getSolid() ) continue;

            if( e == other ) continue;

            if ( !Collidable.IsColliding( e, other, delta ) ) continue;
        
            var overlap = Collidable.GetOverlap( e, other );
        
            if( overlap == null ) continue;

            // collision trigger

            collisionPush( e, other, overlap.Value, delta );

        }

    }

    private float lerp( float start, float end, float t ) {
        return start + (end - start) * t;
    }

    private double getTargetX( float x, Rect targ ) { 
        return ( x + targ.getW() / 2 ) - innerWidth / 2;
    }

    private double getTargetY( float y, Rect targ ) {
        return ( y + targ.getH() / 2 ) - innerHeight / 2;
    }

    private void cameraFollow( float delta ) {

        Rect follow = cameraTarget!;

        if( follow == null ) return;

        float xx = cameraTarget!.extractX( delta );
        float yy = cameraTarget!.extractY( delta );
        
        float x = (float)getTargetX( xx, cameraTarget );
        float y = (float)getTargetY( yy, cameraTarget );

        cam.Target.X = lerp( cam.Target.X, x, .3f );
        cam.Target.Y = lerp( cam.Target.Y, y, .3f );

    }

    private bool outsideCamera( Rect r, int margin ) {
        return (
            cam.Target.X - margin < r.getX() + r.getW() &&
            cam.Target.Y - margin < r.getY() + r.getH() &&
            cam.Target.X + margin + innerWidth  > r.getX() && 
            cam.Target.Y + margin + innerHeight > r.getY() 
        );

    }

    public void update( float delta ) {

        Raylib.ClearBackground( Color.Black );

        // <Interface>
            
        // </Interface>

        // <Game>
            Raylib.BeginMode2D( cam );

            cameraFollow( delta );
            
            foreach( var t in map ) {
                
                if( !outsideCamera( t, TileSize * 2 ) ) continue;

                t.tick( delta );
                
                collision( t, delta );
                
                if( t.getRenderable() ) t.render( cam, delta, spritesheet );

            }

            Raylib.EndMode2D();

        // </Game>

        
    }

    public void addToMap( WorldObject entity ) {

        map.Add( entity );

        map.Sort( ( a, b ) => a.getZ().CompareTo( b.getZ() ) ); // map.OrderBy( e => e.getZ() ).ToList();

    }    

    public Player getPlayer(){ return player; }

}
public enum GameObject {
    None,
    GenericEntity,
    GenericTile,
    Player,
    Slime,
    Grass,
    StoneWall

}
