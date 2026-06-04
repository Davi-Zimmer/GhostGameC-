using Game.Objects;
using Game.Rendering;
using Raylib_cs;

namespace Game.Core;


public class MapCreator {
    
    public Main game;
    
    public bool open = false;
    public bool itemsInterface = false;

    private List< WorldObject > map = [];


    private int z = 0;

    public MapCreator( Main game ) {
        this.game = game;
    }

    public void events( float delta, ref Camera2D cam ) {

        float speed = 200 * delta;

        if( Raylib.IsKeyDown( KeyboardKey.W ) ) cam.Offset.Y += speed; else 
        if( Raylib.IsKeyDown( KeyboardKey.S ) ) cam.Offset.Y -= speed;
        if( Raylib.IsKeyDown( KeyboardKey.A ) ) cam.Offset.X += speed; else
        if( Raylib.IsKeyDown( KeyboardKey.D ) ) cam.Offset.X -= speed;


        if( Raylib.IsKeyPressed( KeyboardKey.Tab ) ) toggleItemsInterface(); 
        if( Raylib.IsKeyPressed( KeyboardKey.F1  ) ) toggleMapCreation();


        // Math.Min( Math.Max( Raylib.GetMouseWheelMove(), -1), 1 );

        if( Raylib.IsKeyPressed( KeyboardKey.Down ) ) z--; else
        if( Raylib.IsKeyPressed( KeyboardKey.Up   ) ) z++;


    }

    public void update( ref Camera2D cam, float delta, Texture2D spriteSheet ) {

        events( delta, ref cam );

        if( itemsInterface ) {

            Raylib.DrawRectangle( 0, 0, (int)game.getInnerWidth(), (int)game.getInnerHeight(), Color.Red );
            

            return;
        }

        Raylib.ClearBackground( Color.Black );


        Raylib.BeginMode2D( cam );

        Raylib.DrawRectangle( 0, 0, 50, 50, Color.Red );

            foreach( var t in map ) {
                
                if( !game.outsideCamera( t, game.TileSize * 2 ) ) continue;
    
                t.render( cam, delta, spriteSheet );

            }


        Raylib.EndMode2D();

        Raylib.DrawText( "z: " + z,  10, 10, 20, Color.White );


    }



    private void toggleItemsInterface(){ itemsInterface = !itemsInterface; } 
    public void toggleMapCreation() { open = !open; }

}