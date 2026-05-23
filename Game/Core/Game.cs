using Game;
using Game.Objects.Basics;
using Raylib_cs;

namespace Game.Core;

public class Main {
    
    private Point2D p = new Point2D();

    public Main() {


    }

    public void update( double delta ) {

        Raylib.ClearBackground( Color.Black );
        
        if( Raylib.IsKeyDown( KeyboardKey.W ) ) p.y -= 1;
        if( Raylib.IsKeyDown( KeyboardKey.A ) ) p.x -= 1;
        if( Raylib.IsKeyDown( KeyboardKey.S ) ) p.y += 1;
        if( Raylib.IsKeyDown( KeyboardKey.D ) ) p.x += 1;

        
        Raylib.DrawCircle( p.intX, p.intY, 10, Color.Red );

    }

    public void test() {
        
    }

}

