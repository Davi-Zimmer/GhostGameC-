using System.Data;
using System.Numerics;
using System.Xml.Linq;
using Game.Data;
using Game.Objects;
using Game.Objects.Basics;
using Game.Rendering;
using Game.World.Entity;
using Game.World.Entity.Enemy;
using Game.World.Item;
using Game.World.Tile;
using Raylib_cs;

namespace Game.Core;


using RawTileData   = ( GameObject gameObject, int x, int y, int z, int spriteIndex, int rotationX, int multiplyerW );
using RawEntityData = ( GameObject gameObject, int x, int y, int z, int spriteIndex, int life );
using RawItemData   = ( GameObject gameObject, int x, int y, int z, int spriteIndex, int itemDamaged );


public class GameDataConverter {

    public static byte[] ToBytes( WorldObject obj ) {

        if( obj is GenericTile   ) return TileToBytes   ( ( obj as GenericTile   )! );
        if( obj is GenericItem   ) return ItemToBytes   ( ( obj as GenericItem   )! );
        if( obj is GenericEntity ) return EntityToBytes ( ( obj as GenericEntity )! );

        return [];
    }

    // <GameObject> <X> <Y> <Z> <SpriteIndex> <itemDamaged>
    private static byte[] ItemToBytes( GenericItem item ) {
        RawItemData raw = new(
            item.getGameObjectID(),
            (int)item.getX(),
            (int)item.getY(),
            (int)item.getZ(),
            item.getSpriteIndex(),
            item.getItemDamage()

        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.x,
            (byte)raw.y,
            (byte)raw.z,
            (byte)raw.spriteIndex,
            (byte)raw.itemDamaged
        ]; 

        return bytes;
    }

    // <GameObject> <X> <Y> <Y> <SpriteIndex> <Life>
    private static byte[] EntityToBytes( GenericEntity entity ) {
        
        RawEntityData raw = new(
            entity.getGameObjectID(),
            (int)entity.getX(),
            (int)entity.getY(),
            (int)entity.getZ(),
            entity.getSpriteIndex(),

            entity.getLife()

        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.x,
            (byte)raw.y,
            (byte)raw.z,
            (byte)raw.spriteIndex,
            (byte)raw.life
        ]; 

        return bytes;
    }

    // <GameObject> <X> <Y> <Z> <SpriteIndex> <RotationX> <MultiplyerW>
    private static byte[] TileToBytes( GenericTile tile ) {
         
        RawTileData raw = new(
            tile.getGameObjectID(),
            (int)tile.getX(),
            (int)tile.getY(),
            (int)tile.getZ(),
            tile.getSpriteIndex(),
            tile.sprite.rotationX / 90,
            tile.sprite.multiplyerW
        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.x,
            (byte)raw.y,
            (byte)raw.z,
            (byte)raw.spriteIndex,
            (byte)raw.rotationX,
            (byte)raw.multiplyerW
        ];

        return bytes;
    
    }

    public static WorldObject? BytesToWorldObject( byte[] bytes, Main game ) {
        
        GameObject gameObject = Main.ByteToGameObject( bytes[0] );

        PaletteItem? item = AllGameObjectsPalette.FindByGameObject( gameObject );

        if( item == null ) return null; 
        
        int x = bytes[ 1 ];
        int y = bytes[ 2 ];
        int z = bytes[ 3 ];

        if ( typeof( GenericTile ).IsAssignableTo( item.classObject ) ) {

            int spriteIndex = bytes[ 4 ];
            int rotationX   = bytes[ 5 ];
            int multiplyerW = bytes[ 6 ];
        
            GenericTile? tile = TileCreator.NewTile( gameObject, game );
                
            if( tile == null ) return null;

            tile.sprite.setSprite( item.previewSprites[ spriteIndex ] );

            tile.sprite.rotationX   = rotationX * 90;
            tile.sprite.multiplyerW = multiplyerW;

            Console.WriteLine( rotationX );

            tile.setZ( z ).setXY( x, y );

            tile.setSpriteIndex( spriteIndex );

            return tile;

        }

        if( typeof( GenericItem ).IsAssignableTo( item.classObject )) {
            
            int spriteIndex = bytes[ 4 ];
            byte itemDamaged = bytes[ 5 ];
            
            GenericItem i = (GenericItem)Activator.CreateInstance( item.classObject, game )!;
            i.setSpriteIndex( spriteIndex );
            i.setZ( z ).setXY( x, y );
            i.setItemDamage( itemDamaged );
            return i;
            
        }

        if( typeof( GenericEntity ).IsAssignableTo( item.classObject )) {
            
            int spriteIndex = bytes[ 4 ];
            int life = bytes[ 5 ];
            
            GenericEntity e = (GenericEntity)Activator.CreateInstance( item.classObject, game )!;
            e.setSpriteIndex( spriteIndex );
            e.setZ( z ).setXY( x, y );
            if( life != -1 ) e.setLife( life ); 

            return e;
            
        }

        WorldObject obj = (WorldObject)Activator.CreateInstance( item.classObject, game )!;

        obj.setZ( z ).setXY( x, y );

        return obj;

    }  

}

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

        if( Raylib.IsKeyPressed( KeyboardKey.F5 ) )  saveMap();
        if( Raylib.IsKeyPressed( KeyboardKey.F12 ) ) loadMap();
        

        if( Raylib.IsKeyPressed( KeyboardKey.R ) ) rotate( Raylib.GetMousePosition() );

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

    public void saveMap() {

        List<byte[]> byteMap = [];

        foreach( var obj in map ) {

            var t = GameDataConverter.ToBytes( obj );

            byteMap.Add( t );
            
        }

        try {
            var stream = File.Create( mapfile );

            foreach ( var b in byteMap ) {
                stream.WriteByte( (byte)b.Length );
                stream.Write( b );
            }

            stream.Close();
            
        } catch( Exception e ) {
            Console.WriteLine( e.ToString() );
        }

  
    }

    public void loadMap() {
        
        var stream = File.OpenRead( mapfile );

        while(stream.Position < stream.Length)
        {
            int size = stream.ReadByte();

            byte[] tile = new byte[size];

            stream.ReadExactly(tile);

            WorldObject? obj = GameDataConverter.BytesToWorldObject( tile, game );

            if( obj != null  ) {
                // Console.WriteLine( "Loaded: " + obj.getGameObjectID() );
                addToMap( obj );

            } else {
                Console.WriteLine( "--Fail--" );
            }

        }

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