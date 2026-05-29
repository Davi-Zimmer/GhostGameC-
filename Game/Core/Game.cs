using System.Numerics;
using Game.Objects;
using Game.Objects.Basics;
using Game.World.Entity;
using Game.World.Entity.Enemy;
using Raylib_cs;

namespace Game.Core;

public class Main {

    Camera2D cam = new();

    private List< WorldObject > map = [];

    private Player player;

    private Rect? cameraTarget = null;

    public Texture2D spritesheet = new();

    public Main() {
        
        var p = new Player( this );

        player = p;
        cameraTarget = p;

        map.Add( p );

        configCamera( p );

        // map.Add( new GenericEntity( this ).Configure<GenericEntity>( e => e.setXY( 200, 200 ) ) );
        // map.Add( new GenericEntity( this ).Configure<GenericEntity>( e => e.setXY( 200, 400 ) ) );

        map.Add( new Slime( this ) );
        
    }

    public void setup() {

        string path = Directory.GetCurrentDirectory() + "/Assets/placeholder.png";

        spritesheet = Raylib.LoadTexture( path );

    }

    public void finish() {

        Raylib.UnloadTexture( spritesheet );

    }

    public void configCamera( Player p ) {
        
        cam.Target = new Vector2( p.getX(), p.getY() ); 
    
        cam.Target   = new Vector2( p.getX() + 20.0f, p.getY() + 20.0f );
        cam.Offset   = new Vector2( Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f );
        cam.Rotation = 0.0f;
        cam.Zoom     = 1.0f;

    }

    private void collisionPush( WorldObject e, WorldObject other, Collidable.Overlap overlap, float delta ) {

        bool horizontal = Math.Abs( overlap.x ) < Math.Abs( overlap.y );

        if( horizontal ) {

            Collidable oColl = other.getCollidable();
            Physics op       = other.getPhysics()!;    if( op == null ) return;
            Physics ep       = e.getPhysics()!;        if( ep == null ) return;


            if( oColl.getCanOverlapOthers() ) e.applyX( overlap.x ); else {
                
                if( e.getCollidable().getCanPushOthers() ) {

                    op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );

                }

            }

            if( op.getFixed() ) return;

            op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );
            

        } else {
           
            Collidable oColl = other.getCollidable()!; if( oColl == null ) return;
            Physics op       = other.getPhysics()!;    if( op == null ) return;
            Physics ep       = e.getPhysics()!;        if( ep == null ) return;

            if( oColl.getCanOverlapOthers() ) e.applyY( overlap.y ); else {
                
                if( e.getCollidable().getCanPushOthers() ) {

                    op.pushY( Math.Sign( -overlap.y ), ep.getMass(), ep.getKnockback(), delta );

                }

            }
            if( op.getFixed() ) return;

            op.pushY( Math.Sign( -overlap.y ), ep.getMass(), ep.getKnockback(), delta );

        }

    }

    private void collision( WorldObject e,  float delta ) {
        
        if( !e.getCollidable()!.getSolid() ) return;

    
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
        return ( x + targ.getW() / 2 ) - Raylib.GetScreenWidth () / 2;
    }

    private double getTargetY( float y, Rect targ ) {
        return ( y + targ.getH() / 2 ) - Raylib.GetScreenHeight() / 2;
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

    public void update( float delta ) {

        Raylib.ClearBackground( Color.Black );

        // <Interface>
            
        // </Interface>

        // <Game>
            Raylib.BeginMode2D( cam );

            cameraFollow( delta );
            
            foreach( var t in map ) {
                
                t.tick( delta );
                
                collision( t, delta );
                
                if( t.getRenderable() ) t.render( cam, delta, spritesheet );

            }

            Raylib.EndMode2D();

        // </Game>

        
    }

    public void addToMap( GenericEntity entity ) {

        map.Add( entity );

        map = map.OrderBy( e => e.getZ() ).ToList();

    }    

    public Player getPlayer(){ return player; }

}
public enum GameObject {
    None,
    GenericEntity,
    GenericTile,
    Player,
    Slime
}
