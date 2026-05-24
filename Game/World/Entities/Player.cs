using Game.Core;
using Raylib_cs;

namespace Game.World.Entity;

public class Player : GenericEntity {
    
    public Player() {
        
        setGameObjectID( GameObject.Player );

        setSpeed( 100 )
        .setW( 100 )
        .setH( 100 );

    }

    private void executeKeys( float delta ) {
        
        if( Raylib.IsKeyDown( KeyboardKey.W ) ) applyY( -getSpeed() * delta ); else 
        if( Raylib.IsKeyDown( KeyboardKey.S ) ) applyY(  getSpeed() * delta );
        if( Raylib.IsKeyDown( KeyboardKey.A ) ) applyX( -getSpeed() * delta ); else
        if( Raylib.IsKeyDown( KeyboardKey.D ) ) applyX(  getSpeed() * delta );

    }
    
    public override void tick( float delta ){
        
        executeKeys( delta );

    }



    public override void render( Camera2D cam ) {
    
        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Purple );

    }



    //--------------------------------- Getters ---------------------------------\\ 


    //--------------------------------- Setters ---------------------------------\\ 


}