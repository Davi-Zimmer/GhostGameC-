using System.Numerics;
using Game.Data;
using Game.Objects;
using Game.Rendering;
using Game.World.Events;
using Game.World.Persistence;
using Game.World.Tile;
using Raylib_cs;

namespace Game.Core;


public class OrganizedPeletteItem {
    public Rectangle pos;
    public SpriteFrame sprite;
    public Type classObject;
    public int spriteIndex;

    public GameObject gameObject;

    public OrganizedPeletteItem(
        Rectangle pos,
        SpriteFrame sprite,
        Type classObject,
        GameObject gameObject,
        int spriteIndex
    ) {
        this.pos         = pos;
        this.sprite      = sprite;
        this.classObject = classObject;
        this.gameObject  = gameObject;
        this.spriteIndex = spriteIndex;
    }

}

public class MapCreator {
    
    public Main game;
    public string mapfile = "map.world";
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

        List<OrganizedPeletteItem> list = [];

        int size = 50;
        int space = 10;

        for ( int index = 0; index < AllGameObjectsPalette.Items.Count; index++ ) {
            
            PaletteItem item = AllGameObjectsPalette.Items[ index ];
            
            for( int x = 0; x < item.previewSprites.Count; x++ ) {

                int posX = x     * size + space * x;
                int posY = index * size + space * index;

                list.Add( new OrganizedPeletteItem(
                    new Rectangle( posX, posY, size, size ),
                    item.previewSprites[ x ],
                    item.classObject,
                    item.gameObject,
                    x
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

        if( Raylib.IsMouseButtonPressed( MouseButton.Left  ) ) leftClick( Raylib.GetMousePosition() );
        if( Raylib.IsMouseButtonPressed( MouseButton.Middle  ) ) middleClick( Raylib.GetMousePosition() );
        if( Raylib.IsMouseButtonPressed( MouseButton.Right ) ) rightClick( Raylib.GetMousePosition() );

        if( Raylib.IsKeyPressed( KeyboardKey.F5  ) ) MapSerializer.SaveMap( map, mapfile );
        if( Raylib.IsKeyPressed( KeyboardKey.F9  ) ) overrideMap( MapSerializer.ReadMap( mapfile, game ) );
        if( Raylib.IsKeyPressed( KeyboardKey.F10 ) ) loadMap();

        if( Raylib.IsKeyPressed( KeyboardKey.R ) ) rotate( Raylib.GetMousePosition() );

    }


    private void overrideMap( List<WorldObject> newMap ) {
        newMap.ForEach( t => addToMap( t ) );
    }

    private void loadMap() {
        game.setMap( map );
        Console.WriteLine("Carregado");

    }

    private bool inside( Vector2 v, Rectangle r ) {
        return (
            v.X > r.X &&
            v.Y > r.Y &&
            v.X < r.X + r.Width &&
            v.Y < r.Y + r.Height 
        );
    }

    public Vector2 getWorldClick( Vector2 vec ) {
        
        Vector2 mouseWorld = Raylib.GetScreenToWorld2D(
            vec,
            game.getCamera()
        );

        return mouseWorld;
        
    }

    private OrganizedPeletteItem? getClickedPalleteItem( Vector2 vec ) {

        foreach( var item in organizedItems ) {

            if( inside( vec, item.pos ) ) {
                
                // Console.WriteLine( item.sprite.rect.X  + " " + item.sprite.rect.Y + " " + item.sprite.rect.Width + " " + item.sprite.rect.Height );
                
                return item;

            }
        
        }

        return null;

    }

    private WorldObject? getClickedMapItem( Vector2 vec, bool ignoreZ ) {

        foreach( var item in map ) {

            bool isInside = inside( vec, new Rectangle( item.getX(), item.getY(), item.getW(), item.getH() ) );

            if( isInside &&( z == item.getZ() && !ignoreZ ) ) {
            
                return item;

            }
        
        }

        return null;

    }

    private List<WorldObject> getClickedMapItemList( Vector2 vec, bool ignoreZ ) {

        List<WorldObject> list = new();

        foreach( var item in map ) {

            bool isInside = inside( vec, new Rectangle( item.getX(), item.getY(), item.getW(), item.getH() ) );

            if( isInside ) {

                if( !ignoreZ ) {

                    if( z == item.getZ() ) {
                        
                        list.Add( item );
                        
                        continue;

                    }

                    continue;
                }

                // Console.WriteLine( item.sprite.rect.X  + " " + item.sprite.rect.Y + " " + item.sprite.rect.Width + " " + item.sprite.rect.Height );
                
                list.Add( item );

            }
        
        }

        return list;

    }

    public void showMessage( string msg ) {
        Console.WriteLine( msg );
    }

    
    private void leftClick( Vector2 vec ) {

        if ( itemsInterface ) {

            selectedItem = getClickedPalleteItem( vec );

        } else {

            if( selectedItem == null )  return;

            WorldObject? obj = getClickedMapItem( getWorldClick( vec ), false );

            if ( obj != null ) {
                
                Console.WriteLine("Ja tem coisa ae " + obj.getGameObjectID() );
                
                return;
            
            }

            Vector2 mouseWorld = getWorldClick( vec );

            float size = game.TileSize;
                
            float x = MathF.Floor( mouseWorld.X / size ) * size;
            float y = MathF.Floor( mouseWorld.Y / size ) * size;

            if ( typeof( GenericTile ).IsAssignableTo( selectedItem.classObject ) ) {
                
                GameObject gameObject = selectedItem!.gameObject;

                GenericTile? tile = TileCreator.NewTile( gameObject, game );
                    
                if( tile == null ) return;

                tile.sprite.setSprite( selectedItem.sprite );

                tile.setSpriteIndex( selectedItem.spriteIndex )
                .setZ( z ).setXY( x, y );

                addToMap( tile );
                return;

            }

            WorldObject item = (WorldObject)Activator.CreateInstance( selectedItem!.classObject, game )!;

            item.setSpriteIndex( selectedItem.spriteIndex )
            .setZ( z ).setXY( x, y );

            addToMap( item );  

        }

    }

    private void rightClick( Vector2 vec ) {

        List<WorldObject> list = getClickedMapItemList(
            getWorldClick( vec ),
            Raylib.IsKeyDown( KeyboardKey.LeftShift )
        );

        foreach ( WorldObject item in list ) {
        
            map.Remove( item );
        
        }

    }

    private void middleClick( Vector2 vec ) {
        
        // WorldObject? target = getClickedMapItem( vec, false );

        // if( target == null ) return;

    }

    private void rotate( Vector2 vec ) {
        
        WorldObject? target = getClickedMapItem( getWorldClick( vec ), false );

        if( target is GenericTile ) {

            GenericTile tile = ( target as GenericTile )!;
            
            tile.sprite.rotationX += 90; 

        }



    }

    public void addToMap( WorldObject entity ) {

        map.Add( entity );

        map.Sort( ( a, b ) => a.getZ().CompareTo( b.getZ() ) ); // map.OrderBy( e => e.getZ() ).ToList();

    }

    public void drawInterface( ref Camera2D cam, float delta, Texture2D spriteSheet ) {

        Raylib.DrawRectangle( 0, 0, (int)game.getInnerWidth(), (int)game.getInnerHeight(), Color.Black );

        foreach( var item in organizedItems ) {
        
            Raylib.DrawRectangle( (int)item.pos.X, (int)item.pos.Y, (int)item.pos.Width, (int)item.pos.Height, Color.Blue );

            var vec = new Vector2( item.pos.Width / 2, item.pos.Height / 2 );
        
            Rectangle r = item.sprite.rect;
            
            r.Width *= item.sprite.multiplyerW;

            Raylib.DrawTexturePro( 
                spriteSheet,
                r,
                new Rectangle( 
                    item.pos.X     + item.pos.Width  / 2,
                    item.pos.Y     + item.pos.Height / 2,
                    item.pos.Width,
                    item.pos.Height
                ),
                vec,
                item.sprite.rotationX,
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

        if( selectedItem != null ) {
            
            var item = selectedItem;

            var vec = new Vector2( item.pos.Width / 2, item.pos.Height / 2 );
        
            Rectangle r = item.sprite.rect;
            r.Width *= item.sprite.multiplyerW;

            int size = 50;

            Raylib.DrawTexturePro( 
                spriteSheet,
                r,
                new Rectangle( game.getInnerWidth() - size - 10, size, size, size ),
                vec,
                item.sprite.rotationX,
                Color.White
            );

        }


    }

    private void toggleItemsInterface(){ itemsInterface = !itemsInterface; } 
    public void toggleMapCreation() { open = !open; }

}