using System.Numerics;
using System.Runtime.CompilerServices;
using Game.Data;
using Game.Objects;
using Game.Objects.Basics;
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
    public bool eventInterface = false;

    public bool itemsInterface = false;

    public EventName selectedEvent = EventName.None;

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

    public void userEvents( float delta, ref Camera2D cam ) {

        float speed = 200 * delta;

        if( Raylib.IsKeyDown( KeyboardKey.W ) ) cam.Target.Y -= speed; else 
        if( Raylib.IsKeyDown( KeyboardKey.S ) ) cam.Target.Y += speed;
        if( Raylib.IsKeyDown( KeyboardKey.A ) ) cam.Target.X -= speed; else
        if( Raylib.IsKeyDown( KeyboardKey.D ) ) cam.Target.X += speed;


        if( Raylib.IsKeyPressed( KeyboardKey.E ) )   toggleEventsInterface(); 
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

    private EventName getClickedEventName( Vector2 vec ) {

        int offset = 20;
        int y   = 0;
        int gap = 5;
        int x   = (int)game.getInnerWidth() - 190; 
        int width  = 150;
        int height = 30;

        foreach( var item in Map.AllEvents ) {
            int yy = y * gap + y * height + offset;

            if ( inside( vec, new Rectangle( x, yy, width, height ) ) ) {
                
                return item.Key;

            }

            y++;
        }

        return EventName.None;

    }

    private WorldObject? ob = null ;

    private List<WorldObject> getClickedMapItemList( Vector2 vec, bool ignoreZ ) {

        List<WorldObject> list = new();

        foreach( var item in map ) {

            bool isInside = inside( vec, new Rectangle( item.getX(), item.getY(), item.getW(), item.getH() ) );

            if( isInside ) {

                if( !ignoreZ ) {

                    if( z == item.getZ() ) {
                        
                        list.Add( item );
                        ob = item;
                        
                        continue;

                    }

                    continue;
                }

                // Console.WriteLine( item.sprite.rect.X  + " " + item.sprite.rect.Y + " " + item.sprite.rect.Width + " " + item.sprite.rect.Height );
                
                list.Add( item );

                ob = item;

            }
        
        }


        return list;

    }

    public void showMessage( string msg ) {
        Console.WriteLine( msg );
    }
    

    public void placeItem( float x, float y ) {

        var obj = getClickedMapItem( new Vector2( x + 10 , y + 10  ), false );

        if( obj != null ) {

            Console.WriteLine("Ja tem coisa ae " + obj.getGameObjectID() );
            
            return;
        }

        if ( typeof( GenericTile ).IsAssignableTo( selectedItem!.classObject ) ) {
                
            GameObject gameObject = selectedItem!.gameObject;

            GenericTile? tile = TileCreator.NewTile( gameObject, game );
                
            if( tile == null ) return;

            tile.sprite.setSprite( selectedItem.sprite );

            tile.setSpriteIndex( selectedItem.spriteIndex )
            .setZ( z ).setXY( x, y );

            addToMap( tile );
            return;

        }

        if( typeof( EventEntity ).IsAssignableTo( selectedItem.classObject ) ){
            
            EventEntity? evEntity = (EventEntity)Activator.CreateInstance( selectedItem!.classObject, game )!;

            evEntity.setEventName( selectedEvent );

            evEntity.setZ( z ).setXY( x, y );

            addToMap( evEntity );  

            return;

        }

        WorldObject item = (WorldObject)Activator.CreateInstance( selectedItem!.classObject, game )!;

        item.setSpriteIndex( selectedItem.spriteIndex )
        .setZ( z ).setXY( x, y );

        addToMap( item );  
    }

    private void leftClick( Vector2 vec ) {

        if ( eventInterface && vec.X > game.getInnerWidth() - 200 ) {

            selectedEvent = getClickedEventName( vec );

            return;
        }

        if ( itemsInterface ) {

            selectedItem = getClickedPalleteItem( vec );

        } else {

            if( selectedItem == null )  return;

            Vector2 mouseWorld = getWorldClick( vec );

            float size = game.TileSize;
                
            float x = MathF.Floor( mouseWorld.X / size ) * size;
            float y = MathF.Floor( mouseWorld.Y / size ) * size;

           placeItem( x, y );

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

    public void drawEventInterface( ref Camera2D cam, float delta ) {
        
        int size = 200;

        Raylib.DrawRectangle( 
            (int)game.getInnerWidth() - size,
            0,
            size,
            (int)game.getInnerHeight(),
            Color.DarkBlue
        );
        
        int offset = 20;
        int y   = 0;
        int gap = 5;
        int x   = (int)game.getInnerWidth() - 190; 
        int width  = 150;
        int height = 30;

        foreach( var item in Map.AllEvents ) {
            
            int yy = y * gap + y * height + offset;

            Raylib.DrawRectangle(
                x,
                yy,
                width,
                height,
                Color.Black
            );

            Raylib.DrawText( 
                item.Key.ToString(),
                x,
                yy,
                20,
                Color.White
            );
            
            y++;
        }

            
        Raylib.DrawText( selectedEvent.ToString(), x + 50, 5, 15, Color.Red );


    }

    public void update( ref Camera2D cam, float delta, Texture2D spriteSheet ) {

        userEvents( delta, ref cam );

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

        int tileSize = game.TileSize;
        Vector2 mouseScreen1 = Raylib.GetMousePosition();
        Vector2 mouseWorld2  = Raylib.GetScreenToWorld2D( mouseScreen1, cam );

        int tileX = (int)MathF.Floor( mouseWorld2.X / tileSize );
        int tileY = (int)MathF.Floor( mouseWorld2.Y / tileSize );

        Raylib.DrawRectangle(
            tileX * tileSize,
            tileY * tileSize,
            tileSize,
            tileSize,
            Raylib.ColorAlpha( Color.White, .2f )
        );

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

        if( eventInterface ) {
            
            drawEventInterface( ref cam, delta );

        }
        
        Raylib.DrawText( $"X: {tileX} Y: {tileY}", 10, 50, 20, Color.Pink );

        HandleFillTool( tileX, tileY );

    }

    private void toggleEventsInterface(){ eventInterface = !eventInterface; } 
    private void toggleItemsInterface(){ itemsInterface = !itemsInterface; } 
    public void toggleMapCreation() { open = !open; }

    private bool fillMode = false;
    private int startX;
    private int startY;
    private bool eraseMode = false;

    void HandleFillTool( int tileX, int tileY ) {
        bool ctrl = Raylib.IsKeyDown( KeyboardKey.LeftControl );

        if (!ctrl) return;

        bool leftClick  = Raylib.IsMouseButtonPressed(MouseButton.Left);
        bool rightClick = Raylib.IsMouseButtonPressed(MouseButton.Right);

        if (!leftClick && !rightClick) return;

        if (!fillMode) {

            fillMode = true;
            startX = tileX;
            startY = tileY;

            eraseMode = rightClick;
        } else {
            fillMode = false;

            int minX = Math.Min(startX, tileX);
            int maxX = Math.Max(startX, tileX);

            int minY = Math.Min(startY, tileY);
            int maxY = Math.Max(startY, tileY);

            for (int y = minY; y <= maxY; y++) {
                
                for (int x = minX; x <= maxX; x++) {
                        
                    if (eraseMode) {
                        
                        

                    } else {
  
                        placeItem( x * game.TileSize, y * game.TileSize );

                    }
                }
            }
        }


        Console.WriteLine( map.Count );
    }
}