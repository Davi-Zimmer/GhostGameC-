using Game.Core;
using Game.World.Item;
using Raylib_cs;

namespace Game.Interface;

public class Inventory {

    public bool open = false;

    public List< GenericItem > items = new();
    
    public int selectedItem = 0;
    public Main game;

    public Inventory( Main game ) {

        this.game = game;

    }

    public void toggle() { open = !open; }

    public void tick( float delta ) {
        
        if( Raylib.IsKeyPressed( KeyboardKey.Tab ) ) toggle();


    }


    public void render( Camera2D cam,  float delta, Texture2D spriteSheet  ) {
        
        int   border  = 50;
        float width   = game.getInnerWidth()  - border * 2;
        float height  = game.getInnerHeight() - border * 2;

        Raylib.DrawRectangle( border, border, (int) width, (int) height, Color.Black );

    }



    //--------------------------------- Getters ---------------------------------\\ 
    public int getSelectedItem() { return selectedItem; }


    //--------------------------------- Setters ---------------------------------\\ 
    public Inventory setSelectedItem( int i ) { selectedItem = i; return this; }



    public Inventory addItem( GenericItem item ){ items.Add( item ); return this; }

    public Inventory useSelectedItem() {
        
        if( items.Count == 0 ) return this;

        items[ selectedItem ].use();

        return this;
    }

}