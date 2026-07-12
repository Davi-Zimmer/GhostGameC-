using System.Data;
using Game.Core;
using Game.Data;
using Game.Objects;
using Game.World.Entity;
using Game.World.Events;
using Game.World.Item;
using Game.World.Tile;

namespace Game.World.Persistence;

using RawEventEntityData = ( GameObject gameObject, int z, int eventName );
using RawEntityData      = ( GameObject gameObject, int z, int spriteIndex, int life );
using RawTileData        = ( GameObject gameObject, int z, int spriteIndex, int rotationX, int multiplyerW );
using RawItemData        = ( GameObject gameObject, int z, int spriteIndex, int itemDamaged );

using RawData = ( ushort x, ushort y, byte[] bytes );

public class GameDataConverter {

    public static RawData? ToBytes( WorldObject obj ) {


        if( obj is GenericTile     ) return TileToRawData        ( ( obj as GenericTile   )! );
        if( obj is GenericItem     ) return ItemToRawData        ( ( obj as GenericItem   )! );
        if( obj is EventEntity     ) return EventEntityToRawData ( ( obj as EventEntity   )! );
        if( obj is GenericEntity   ) return EntityToRawData      ( ( obj as GenericEntity )! );

        return null;
    }

    // <X> <Y> | <GameObject> <Z> <SpriteIndex> <itemDamaged>
    private static RawData ItemToRawData( GenericItem item ) {
        RawItemData raw = new(
            item.getGameObjectID(),
            (int)item.getZ(),
            item.getSpriteIndex(),
            item.getItemDamage()
        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.z,
            (byte)raw.spriteIndex,
            (byte)raw.itemDamaged
        ];

        RawData data = new(
            (ushort)item.getX(),
            (ushort)item.getY(),
            bytes
        );
        

        return data;
    }

    // <X> <Y> | <GameObject> <Z> <SpriteIndex> <Life>
    private static RawData EntityToRawData( GenericEntity entity ) {
        
        RawEntityData raw = new(
            entity.getGameObjectID(),
            (int)entity.getZ(),
            entity.getSpriteIndex(),
            entity.getLife()

        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.z,
            (byte)raw.spriteIndex,
            (byte)raw.life
        ];

        RawData data = new(
            (ushort)entity.getX(),
            (ushort)entity.getY(),
            bytes
        ); 

        return data;
    }

    // <X> <Y> | <GameObject> <Z> <SpriteIndex> <RotationX> <MultiplyerW>
    private static RawData TileToRawData( GenericTile tile ) {
         
        RawTileData raw = new(
            tile.getGameObjectID(),
            (int)tile.getZ(),
            tile.getSpriteIndex(),
            tile.sprite.rotationX / 90,
            tile.sprite.multiplyerW
        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.z,
            (byte)raw.spriteIndex,
            (byte)raw.rotationX,
            (byte)raw.multiplyerW
        ];

        RawData data = new(
            (ushort)tile.getX(),
            (ushort)tile.getY(),
            bytes
        );

        return data;
    
    }

   // <X> <Y> | <GameObject> <Z> <EventName>
    private static RawData EventEntityToRawData( EventEntity entity ) {
        
        RawEventEntityData raw = new(
            entity.getGameObjectID(),
            (int)entity.getZ(),
            (int)entity.getEventName() 
        );

        byte[] bytes = [
            Main.GameObjectToByte( raw.gameObject ),
            (byte)raw.z,
            (byte)raw.eventName
        ];

        RawData data = new(
            (ushort)entity.getX(),
            (ushort)entity.getY(),
            bytes
        );

        return data;
    
    }

    public static WorldObject? rawDataToWorldObject( RawData data, Main game ) {

        int x = data.x;
        int y = data.y;

        GameObject gameObject = Main.ByteToGameObject( data.bytes[0] );

        PaletteItem? item = AllGameObjectsPalette.FindByGameObject( gameObject );
        
        if( item == null ) return null;

        int z = data.bytes[ 1 ];

        if ( typeof( GenericTile ).IsAssignableTo( item.classObject ) ) {
            int spriteIndex = data.bytes[ 2 ];
            int rotationX   = data.bytes[ 3 ];
            int multiplyerW = data.bytes[ 4 ];
        
            GenericTile? tile = TileCreator.NewTile( gameObject, game );
                
            if( tile == null ) return null;

            tile.sprite.setSprite( item.previewSprites[ spriteIndex ] );

            tile.sprite.rotationX   = rotationX * 90;
            tile.sprite.multiplyerW = multiplyerW;

            tile.setZ( z ).setXY( x, y );

            tile.setSpriteIndex( spriteIndex );

            return tile;
        }

        if( typeof( GenericItem ).IsAssignableTo( item.classObject )) {
            
            int spriteIndex  = data.bytes[ 2 ];
            byte itemDamaged = data.bytes[ 3 ];
            
            GenericItem i = (GenericItem)Activator.CreateInstance( item.classObject, game )!;
            i.setSpriteIndex( spriteIndex );
            i.setZ( z ).setXY( x, y );
            i.setItemDamage( itemDamaged );
            return i;
            
        }

        if( typeof( EventEntity ).IsAssignableTo( item.classObject )) {
            
            int eventName = data.bytes[ 2 ];
            
            EventEntity e = (EventEntity)Activator.CreateInstance( item.classObject, game )!;
            // e.setGameObjectID( gameObject );
            
            e.setEventName( (EventName)eventName );
            
            e.setZ( z ).setXY( x, y );

            return e;
            
        }

        if( typeof( GenericEntity ).IsAssignableTo( item.classObject )) {
            
            int spriteIndex = data.bytes[ 2 ];
            int life        = data.bytes[ 3 ];
            
            GenericEntity e = (GenericEntity)Activator.CreateInstance( item.classObject, game )!;
            e.setSpriteIndex( spriteIndex );
            e.setZ( z ).setXY( x, y );
            if( life != -1 ) e.setLife( life ); 

            return e;
            
        }

        // Console.WriteLine( gameObject + "RANDOM OBJECTAAAAAAAAAAA");

        WorldObject obj = (WorldObject)Activator.CreateInstance( item.classObject, game )!;

        obj.setZ( z ).setXY( x, y );

        return obj;

    }  

}

public class MapSerializer {
    private static void ShowMessage( string msg ) {
        Console.WriteLine( msg );
    }

    public static void SaveMap( List<WorldObject> map, string mapFile ) {

        List<RawData> byteMap = [];

        foreach( var obj in map ) {

            var t = GameDataConverter.ToBytes( obj );

            if( t != null ) byteMap.Add( t.Value );
            
        }

        try {
            var stream = File.Create( mapFile );
            var writer = new BinaryWriter( stream );

            foreach ( var raw in byteMap ) {

                writer.Write( raw.x );
                writer.Write( raw.y );

                writer.Write( (byte)raw.bytes.Length );
                writer.Write( raw.bytes );
            }

            stream.Close();
            
            ShowMessage("Map saved");

        } catch( Exception e ) {
            Console.WriteLine( e );
        }

  
    }

    public static List<WorldObject> ReadMap( string mapfile, Main game ) {
        
        List<WorldObject> map = new();

        try {
            var stream = File.OpenRead( mapfile );
            var reader = new BinaryReader( stream );

            int count = 0;

            while( reader.BaseStream.Position < reader.BaseStream.Length ) {
                
                ushort x = reader.ReadUInt16();
                ushort y = reader.ReadUInt16();

                byte lenght = reader.ReadByte();

                byte[] bytes = reader.ReadBytes( lenght );

                RawData raw = new( x, y, bytes );

                WorldObject? obj = GameDataConverter.rawDataToWorldObject( raw, game );

                if( obj == null ) {
                    
                    ShowMessage( "--Fail To Read The Map--" );
                    ShowMessage( "F(): GameDataConverter.rawDataToWorldObject returned null\n Check if item exists in Main.AllGameObjects" );
                    stream.Close();

                    return [];

                }

                count++;

                map.Add( obj );
                
            }

            stream.Close();

            ShowMessage( count + " Items loaded" );

        } catch ( Exception ex ) {
            Console.WriteLine( ex );
        }

        return map;

    }

}