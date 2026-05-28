using Game.Rendering;
using Game.Core;
using Raylib_cs;

namespace Game;

public class Program {

    Window window;
    Main game;

    public Program() {
        
        game = new Main();

        window = new Window( update, end, loadTextures );

    }

    public void loadTextures() {
        game.setup();
        
    }

    public void update( float delta ) {
        
        // Raylib.ClearBackground( Color.Black );

        // Raylib.DrawText( "Funcionou!", 300, 200, 30, Color.White );


        game.update( delta );

    }

    public void end() {
        game.finish();
    }
      

}
