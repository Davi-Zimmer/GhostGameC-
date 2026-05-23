using Raylib_cs;

namespace Game.Objects.Entities;

public class Player : GenericEntity {
    
    public Player() {
        
    }


    private void executeKeys( double delta ) {
        
        if( Raylib.IsKeyDown( KeyboardKey.W ) ) applyY( -getSpeed() * delta );
        if( Raylib.IsKeyDown( KeyboardKey.S ) ) applyY(  getSpeed() * delta );
        if( Raylib.IsKeyDown( KeyboardKey.A ) ) applyX( -getSpeed() * delta );
        if( Raylib.IsKeyDown( KeyboardKey.D ) ) applyX(  getSpeed() * delta );

    }
    
    public override void tick( double delta ){
        
        executeKeys( delta );

    }

    public override void render() {
    
        Raylib.DrawCircle( getIntX(), getIntY(), getIntW(), Color.Violet );

    }



    //--------------------------------- Getters ---------------------------------\\ 


    //--------------------------------- Setters ---------------------------------\\ 


}