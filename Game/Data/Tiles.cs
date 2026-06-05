using Game.Core;
using Game.Rendering;
using Raylib_cs;

namespace Game.Data;


public abstract class Tiles {

    public static TileDefinition Grass() {

        return new TileDefinition()
        .setGameObjectID( GameObject.Grass )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass( null )
        );

    }

    public static TileDefinition StoneWall() {

        return new TileDefinition()
        .setZ( 0 )
        .setRenderable( true )
        .setSolid( true )
        .setOverlapOthers( true )
        .setPushOthers( true )
        .setUniqueSprite(
            Sprites.StoneWall( null )
        )
        .setGameObjectID( GameObject.StoneWall );
    
    }

    public static TileDefinition CrackedStoneWall() {

        return new TileDefinition()
        .setGameObjectID( GameObject.CrackedStoneWall )
        .setZ( 7 )
        .setRenderable( true )
        .setSolid( true )
        .setOverlapOthers( true )
        .setPushOthers( true )
        .setUniqueSprite(
            Sprites.CrackedStoneWall( null )
        )
        .setCollisionExeption([
            GameObject.Slime
        ]);


    }

}

