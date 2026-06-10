using System.Numerics;
using Game.Objects;
using Game.Rendering;
using Game.World.Entity;
using Game.World.Entity.Enemy;
using Game.World.Item;
using Game.World.Tile;
using Raylib_cs;

namespace Game.Core;


public class PaletteItem {
    public GameObject gameObject = GameObject.None;
    public Type classObject;
    public List<Rectangle> previewSprites;

    public PaletteItem( GameObject o, Type classObj, List<Rectangle> preview ) {
        gameObject     = o;
        classObject    = classObj;
        previewSprites = preview;
    }

}

public class OrganizedPeletteItem {
    public Rectangle pos;
    public Rectangle sprite;
    public Type classObject;
    public GameObject gameObject;

    public OrganizedPeletteItem(
        Rectangle pos,
        Rectangle sprite,
        Type classObject,
        GameObject gameObject
    ) {
        this.pos         = pos;
        this.sprite      = sprite;
        this.classObject = classObject;
        this.gameObject  = gameObject;
    }


}


public class MapCreator {
    
    public Main game;
    
    public bool open = false;
    public bool itemsInterface = false;

    private List<WorldObject> map = [];

    private List<OrganizedPeletteItem> organizedItems;

    private OrganizedPeletteItem? selectedItem = null; 

    private int z = 0;

    public MapCreator( Main game ) {

        this.game = game;

        organizedItems = organizeItems();

    }

    private List<OrganizedPeletteItem> organizeItems() {

        List<PaletteItem> items = new() {

            new PaletteItem (
                GameObject.Player,
                typeof( Player ),
                Sprites.GetRects( Sprites.Player )
            ),

            new PaletteItem (
                GameObject.Slime,
                typeof( Slime ),
                Sprites.GetRects( Sprites.Slime )
            ),

            new PaletteItem (
                GameObject.Grass,
                typeof( GenericTile ),
                Sprites.GetRects( Sprites.Grass )
            ),

            new PaletteItem (
                GameObject.StoneWall,
                typeof( GenericTile ),
                Sprites.GetRects( Sprites.StoneWall )
            ),

            new PaletteItem (
                GameObject.CrackedStoneWall,
                typeof( GenericTile ),
                Sprites.GetRects( Sprites.CrackedStoneWall )
            ),

            new PaletteItem (
                GameObject.Poison,
                typeof( Poison ),
                Sprites.GetRects( Sprites.Poison )
            ),

            new PaletteItem (
                GameObject.EctoGun,
                typeof( EctoGun ),
                Sprites.GetRects( Sprites.EctoGun )
            ),
            
        };
        
        List<OrganizedPeletteItem> list = [];

        int size = 50;
        int space = 10;

        for ( int index = 0; index < items.Count; index++ ) {
            
            PaletteItem item = items[ index ];
            
            for( int x = 0; x < item.previewSprites.Count; x++ ) {

                int posX = x     * size + space * x;
                int posY = index * size + space * index;

                list.Add( new OrganizedPeletteItem(
                    new Rectangle( posX, posY, size, size ),
                    item.previewSprites[ x ],
                    item.classObject,
                    item.gameObject
                ));

            }

        }

        return list;
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

        if( Raylib.IsMouseButtonPressed( MouseButton.Left ) ) leftClick( Raylib.GetMousePosition() );


    }

    private bool inside( Vector2 v, Rectangle r ) {
        return (
            v.X > r.X &&
            v.Y > r.Y &&
            v.X < r.X + r.Width &&
            v.Y < r.Y + r.Height 
        );
    }

    private void leftClick( Vector2 vec ) {

        foreach( var item in organizedItems ) {

            if( inside( vec, item.pos ) ) {
                
                selectedItem = item;
                
                if( selectedItem != null ) Console.WriteLine( selectedItem.gameObject );

                return;

            }
        
        }


    }

    public void drawInterface( ref Camera2D cam, float delta, Texture2D spriteSheet ) {

        Raylib.DrawRectangle( 0, 0, (int)game.getInnerWidth(), (int)game.getInnerHeight(), Color.Black );

        foreach( var item in organizedItems ) {
        
            Raylib.DrawRectangle( (int)item.pos.X, (int)item.pos.Y, (int)item.pos.Width, (int)item.pos.Height, Color.Blue );

            var vec = new Vector2( 0, 0 );
        
            Raylib.DrawTexturePro( 
                spriteSheet,
                item.sprite,
                item.pos,
                vec,
                0,
                Color.White
            );

        }

    }

    public void update( ref Camera2D cam, float delta, Texture2D spriteSheet ) {

        events( delta, ref cam );

        if( itemsInterface ) {

            drawInterface( ref cam, delta, spriteSheet );

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