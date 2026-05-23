using System.Diagnostics;
using System.Runtime.InteropServices.Swift;
using Game;
using Raylib_cs;

namespace Game.Rendering;

public class Window {
    
    public delegate void Loop( double delta );

    public Window( Loop loop ) {

        configuration();

        startLoop( loop );

    }

    private void configuration() {

        Raylib.InitWindow( 800, 450, "Jogo Irado" );
        Raylib.SetTargetFPS( 60 );

    }

    private void startLoop( Loop loop) {

        Stopwatch timer = new();

        timer.Start();

        double lastTime = timer.Elapsed.TotalSeconds;

        while( !Raylib.WindowShouldClose() ) {
            
            double currentTime = timer.Elapsed.TotalSeconds;

            double deltaTime = currentTime - lastTime;

            lastTime = currentTime;

            Raylib.BeginDrawing();
        
            loop( deltaTime );

            Raylib.EndDrawing();

        }

        Raylib.CloseWindow();

    }

    
}