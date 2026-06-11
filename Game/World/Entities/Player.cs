using System.Numerics;
using Game.Core;
using Game.Interface;
using Game.Objects;
using Game.Objects.Animation;
using Game.Rendering;
using Raylib_cs;

namespace Game.World.Entity;

public class Player : GenericEntity {

    Animation animation = new();

    private Inventory inventory;

    public Player( Main game ): base( game ) {
                
        Configure<Player>( p => {
            p.setLife( 100 )
            .setGameObjectID( GameObject.Player );
            
            p.setSpeed( 200 )
            .setW( 27 )
            .setH( 36 )
            .setZ( 10 )
            .setXY( 500, 200 );

            p.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true )
            .setSolid( true );

            p.getPhysics()
            .setMass( 1000 );

        });
        
        inventory = new Inventory( game );

        fillSprites();

        // Console.WriteLine( getPhysics().getOrientation().getX() );
    }

    private void fillSprites() {

        animation.createAnimation( [ "up", "down", "left", "right" ] );

        animation.forSprites( Sprites.Player.Down  , 4, 5, "down"  );
        animation.forSprites( Sprites.Player.Left  , 4, 5, "left"  );
        animation.forSprites( Sprites.Player.Right , 4, 5, "right" );
        animation.forSprites( Sprites.Player.Up    , 4, 5, "up"    );
        
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

        if( Raylib.IsKeyPressed( KeyboardKey.Tab ) ) inventory.toggle();

        if( Raylib.IsMouseButtonPressed( MouseButton.Left ) )  inventory.useItem( HUDSlot.Weapon1 );
        if( Raylib.IsMouseButtonPressed( MouseButton.Right ) ) inventory.useItem( HUDSlot.Weapon2 );
        if( Raylib.IsKeyReleased( KeyboardKey.One   ) ) inventory.useItem( HUDSlot.First  );
        if( Raylib.IsKeyReleased( KeyboardKey.Two   ) ) inventory.useItem( HUDSlot.Second );
        if( Raylib.IsKeyReleased( KeyboardKey.Three ) ) inventory.useItem( HUDSlot.Third  );

        getPhysics().getOrientation().setX( x ).setY( y );
    }
    
    public override void tick( float delta ){

        updatePosition( delta );

        executeKeys( delta );

        animation.tick();

    }

    public int test = 0;
    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {

        Raylib.DrawRectangle( (int)extractX(delta), (int)extractY(delta), getIntW(), getIntH(), Color.Blue );
        
        Raylib.DrawRectangle( getIntX(), getIntY(), getIntW(), getIntH(), Color.Purple );
        
        animation.render( this, spriteSheet );

    }

    //--------------------------------- Getters ---------------------------------\\ 


    //--------------------------------- Setters ---------------------------------\\ 

    public Inventory getInvetory() { return inventory; }

    //--------------------------------- Setters ---------------------------------\\ 


}