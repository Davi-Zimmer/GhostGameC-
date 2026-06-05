namespace Game.Core;


public class MapCreator {
    
    public Main game;
    
    public bool open = false;

    public MapCreator( Main game ) {
        
        this.game = game;

    }


    public void update() {
        
    }



    public void toggle() { open = !open; }

}