using System.Numerics;
using Game.Objects;
using Game.World.Entity;
using Raylib_cs;

namespace Game.Core;

public class Main {

    Camera2D cam = new();

    private List< GenericEntity > map = [];

    public Main() {
        
        var p = new Player().Configure<Player>( p => {
            p.addCollision()
            .addPhysic()
            .setSpeed( 200 )
            .setX( Raylib.GetScreenWidth () )
            .setY( Raylib.GetScreenHeight() );

            p.getCollidable()!
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );
        });

        map.Add( p );

        configCamera( p );

        map.Add( new GenericEntity().Configure<GenericEntity>( e => {
            e.addCollision()
            .addPhysic()
            .setW( 100 )
            .setH( 100 )
            .setX( 300 )
            .setY( 150 );

            p.getCollidable()!.setCanOverlapOthers( true )
            .setCanPushOthers( true );
        }));

    }

    public void configCamera( Player p ) {
        
        cam.Target = new Vector2( p.getX(), p.getY() ); 
    
        cam.Target   = new Vector2( p.getX() + 20.0f, p.getY() + 20.0f );
        cam.Offset   = new Vector2( Raylib.GetScreenWidth() / 2f, Raylib.GetScreenHeight() / 2f );
        cam.Rotation = 0.0f;
        cam.Zoom     = 1.0f;

    }

    private void collisionPush( GenericEntity e, GenericEntity other, Collidable.Overlap overlap ) {
        
        bool horizontal = Math.Abs( overlap.x ) < Math.Abs( overlap.y );

        if( horizontal ) {

            Collidable coll = e.getCollidable()!;

            Physics? ePhysic = e.getPhysics();

            if ( coll.getCanOverlapOthers() ) other.applyX( -overlap.x  );

            else if( coll.getCanPushOthers() ){
                
                if ( ePhysic != null ) return; 

                other.getPhysics()!.pushX( Math.Sign( overlap.x ), ePhysic!.getMass(), ePhysic!.getKnockback() );

            }

            Physics? oPhysic = other.getPhysics();

            if( oPhysic == null ) return;

            oPhysic!.getAcceleration().multiplyX( -.5f );

            if( ePhysic!.getFixed() ) return;

            ePhysic.pushX( Math.Sign( overlap.x ), oPhysic!.getMass(), oPhysic!.getKnockback() );


        } else {
            
            Collidable coll = e.getCollidable()!;

            Physics? ePhysic = e.getPhysics();

            if ( coll.getCanOverlapOthers() ) other.applyY( -overlap.y  );

            else if( coll.getCanPushOthers() ){
                
                if ( ePhysic != null ) return; 

                other.getPhysics()!.pushY( Math.Sign( overlap.y ), ePhysic!.getMass(), ePhysic!.getKnockback() );

            }

            Physics? oPhysic = other.getPhysics();

            if( oPhysic == null ) return;

            oPhysic!.getAcceleration().multiplyY( -.5f );

            if( ePhysic!.getFixed() ) return;

            ePhysic.pushY( Math.Sign( overlap.x ), oPhysic!.getMass(), oPhysic!.getKnockback() );


        }



    }

    private void collision( GenericEntity e ) {
        
        if( e.getCollidable() == null ) return;

        foreach( var other in map) {

            if( other.getCollidable() == null ) continue;

            if( e == other ) continue;

            if ( !Collidable.IsColliding( e, other ) ) continue;

            var overlap = Collidable.GetOverlap( e, other );
        
            if( overlap == null ) continue;

            // collision trigger

            collisionPush( e, other, overlap.Value );

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
                
                collision( t );
                
                t.render( cam );

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
