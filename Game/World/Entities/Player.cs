using Game.Core;
using Game.Objects;
using Raylib_cs;

namespace Game.World.Entity;

public class Player : GenericEntity {
    
    public Player() {
                
        Configure<Player>( p => {

            p.setGameObjectID( GameObject.Player )
            .setSpeed( 100 )
            .setW( 27 * 3 )
            .setH( 36 * 3 )
            .setXY( 0, 0 );

            p.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );

            p.getPhysics()
            .setMass( 1000 );

        });
        
        // Console.WriteLine( getPhysics().getOrientation().getX() );
    }

    private void executeKeys( float delta ) {

        int x = 0;
        int y = 0;

        if( Raylib.IsKeyDown( KeyboardKey.W ) )  --y ;
        if( Raylib.IsKeyDown( KeyboardKey.S ) )  ++y ;
        if( Raylib.IsKeyDown( KeyboardKey.A ) )  --x ;
        if( Raylib.IsKeyDown( KeyboardKey.D ) )  ++x ;

        getPhysics().getOrientation().setX( x ).setY( y );
    }
    
    public override void tick( float delta ){

        updatePosition( delta );

        executeKeys( delta );

    }



    public override void render( Camera2D cam,  float delta ) {

        Raylib.DrawRectangle( (int)extractX(delta), (int)extractY(delta), getIntW(), getIntH(), Color.Blue );
        
        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Purple );
        
    }



    //--------------------------------- Getters ---------------------------------\\ 


    //--------------------------------- Setters ---------------------------------\\ 


}