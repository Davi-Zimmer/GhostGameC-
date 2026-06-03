using System.Numerics;
using Game.Core;
using Game.Objects;
using Game.Objects.Animation;
using Raylib_cs;

namespace Game.World.Entity;

public class Player : GenericEntity {
    Animation animation = new();

    public Player( Main game ): base( game ) {
                
        Configure<Player>( p => {
            p.setLife( 100 )
            .setGameObjectID( GameObject.Player );
            
            p.setSpeed( 200 )
            .setW( 27 )
            .setH( 36 )
            .setZ( 10 )
            .setXY( 0, 0 );

            p.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true )
            .setSolid( true );

            p.getPhysics()
            .setMass( 1000 );

        });
        
        fillSprites();

        // Console.WriteLine( getPhysics().getOrientation().getX() );
    }

    private void fillSprites() {

        animation.createAnimation( [ "up", "down", "left", "right" ] );

        animation.forSprites( 0,   0, 27, 36, 4, 5, "down"  );
        animation.forSprites( 0,  37, 27, 36, 4, 5, "left"  );
        animation.forSprites( 0,  75, 27, 36, 4, 5, "right" );
        animation.forSprites( 0, 115, 27, 36, 4, 5, "up"    );
        
    }

    private void executeKeys( float delta ) {

        int x = 0;
        int y = 0;

        if( Raylib.IsKeyDown( KeyboardKey.W ) ) {  --y; animation.changeAndPlay( "up"    ); } ;
        if( Raylib.IsKeyDown( KeyboardKey.S ) ) {  ++y; animation.changeAndPlay( "down"  ); } ;
        if( Raylib.IsKeyDown( KeyboardKey.A ) ) {  --x; animation.changeAndPlay( "left"  ); } ;
        if( Raylib.IsKeyDown( KeyboardKey.D ) ) {  ++x; animation.changeAndPlay( "right" ); } ;

        if (
            Raylib.IsKeyReleased( KeyboardKey.W ) ||
            Raylib.IsKeyReleased( KeyboardKey.S ) ||
            Raylib.IsKeyReleased( KeyboardKey.A ) ||
            Raylib.IsKeyReleased( KeyboardKey.D ) 
        ) {
            animation.stopAnimation();
        }

        if( Raylib.IsKeyPressed( KeyboardKey.F ) ) setLife( getLife() - 10 );

        if( Raylib.IsMouseButtonPressed( MouseButton.Left ) ) shot();

        getPhysics().getOrientation().setX( x ).setY( y );
    }
    
    private void shot() {
        
        Vector2 mouse = Raylib.GetScreenToWorld2D(
            Raylib.GetMousePosition(),
            game.getCamera()
        );
        
        float centerX = getMiddleX();
        float centerY = getMiddleY();

        double dirX = mouse.X - centerX;
        double dirY = mouse.Y - centerY;

        double length = Math.Sqrt(dirX * dirX + dirY * dirY);

        if(length > 0) {
            dirX /= length;
            dirY /= length;
        }

        float speed = 1000;

        GenericProjectile entity = new GenericProjectile( game ).Configure<GenericProjectile>( e => {
            e.setRenderable( true )
            .getPhysics()!
            .setFriction( 1 )
            .getAcceleration()
            .setXY( (float)dirX, (float)dirY );
        
            e.setSpeed( speed );
            e.setW( 10 ).setH( 10 );
            e.setXY( centerX, centerY );

            e.getCollidable()
            .getExceptions()
            .Add( GameObject.Player );

        });

        game.tickExecutionStack.Add( () => {
            game.addToMap( entity );
        });

    }

    public override void tick( float delta ){

        updatePosition( delta );

        executeKeys( delta );

        animation.tick();

    }

    public override void render( Camera2D cam,  float delta, Texture2D spriteSheet ) {

        Raylib.DrawRectangle( (int)extractX(delta), (int)extractY(delta), getIntW(), getIntH(), Color.Blue );
        
        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Purple );
        
        animation.render( this, spriteSheet );
    }

    //--------------------------------- Getters ---------------------------------\\ 


    //--------------------------------- Setters ---------------------------------\\ 


}