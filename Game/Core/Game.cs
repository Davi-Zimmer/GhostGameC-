using System.Numerics;
using Game.Objects;
using Game.Objects.Basics;
using Game.World.Entity;
using Raylib_cs;

namespace Game.Core;

public class Main {

    Camera2D cam = new();

    private List< GenericEntity > map = [];

    public Main() {
        
        var p = new Player();

        map.Add( p );

        configCamera( p );

        map.Add( new GenericEntity().Configure<GenericEntity>( e => e.setXY( 200, 200 ) ) );

        map.Add( new GenericEntity().Configure<GenericEntity>( e => e.setXY( 200, 400 ) ) );

    }

    public void configCamera( Player p ) {
        
        cam.Target = new Vector2( p.getX(), p.getY() ); 
    
        cam.Target   = new Vector2( p.getX() + 20.0f, p.getY() + 20.0f );
        cam.Offset   = new Vector2( Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f );
        cam.Rotation = 0.0f;
        cam.Zoom     = 1.0f;

    }

    private void collisionPush( GenericEntity e, GenericEntity other, Collidable.Overlap overlap, float delta ) {

        bool horizontal = Math.Abs( overlap.x ) < Math.Abs( overlap.y );

        if( horizontal ) {

            Collidable oColl = other.getCollidable();  
            Physics op       = other.getPhysics();  
            Physics ep       = e.getPhysics();  

            if( oColl.getCanOverlapOthers() ) e.applyX( overlap.x ); else 

            if( e.getCollidable().getCanPushOthers() ) {

                op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );

            }

            if( op.getFixed() ) return;

            op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );
            

        } else {
           

        }

   

   /*
            Collidable eColl = other.getCollidable();
            Physics ePhysic  = other    .getPhysics();
            Physics oPhysic  = e.getPhysics();

            if( eColl.getCanOverlapOthers() ) e.applyX( overlap.x ); else {
                
                if( eColl.getCanPushOthers() ) {
                    
                    oPhysic.pushX( Math.Sign( -overlap.x ), ePhysic.getMass(), ePhysic.getKnockback() );

                }
            
            }

            oPhysic.getAcceleration().multiplyX( -.5f );

            if( oPhysic.getFixed() ) return;

            ePhysic.pushX( Math.Sign( -overlap.x ), ePhysic.getMass(), ePhysic.getKnockback() );

            */
    }

    private void collision( GenericEntity e,  float delta ) {
        
        // if( e.getCollidable() == null ) return;

        foreach( var other in map) {

            if( other.getCollidable() == null ) continue;

            if( e == other ) continue;

            if ( !Collidable.IsColliding( e, other, delta ) ) continue;
        
            var overlap = Collidable.GetOverlap( e, other );
        
            if( overlap == null ) continue;

            // collision trigger

            collisionPush( e, other, overlap.Value, delta );

        }


    }

    public void update( float delta ) {

        Raylib.ClearBackground( Color.Black );
        
        // <Interface>
            
        // </Interface>

        // <Game>
            Raylib.BeginMode2D( cam );

            foreach( var t in map ) {
                
                t.tick( delta );
                
                collision( t, delta );
                
                t.render( cam, delta );

            }

            Raylib.EndMode2D();

        // </Game>

    }

    public void addToMap( GenericEntity entity ) {

        map.Add( entity );

        map = map.OrderBy( e => e.getZ() ).ToList();

    }    

}
public enum GameObject {
    GenericEntity,
    Player,
}
