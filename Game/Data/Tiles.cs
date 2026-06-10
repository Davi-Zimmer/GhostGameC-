using Game.Core;
using Game.Rendering;
using Raylib_cs;

namespace Game.Data;


public abstract class Tiles {

    public static TileDefinition MiddleGrass() {

        return new TileDefinition()
        .setGameObjectID( GameObject.MiddleGrass )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.MiddleGrass
        );

    }
        /*

    public static TileDefinition TopLeftDirtGrass() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopLeftDirtGrass )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopLeftDirtGrass
        );

    }

    public static TileDefinition TopLeftDirt() {
        return new TileDefinition()
        .setGameObjectID( GameObject.TopLeftDirt )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopLeftDirt
        );

    }

    public static TileDefinition TopDirt() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopDirt )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopDirt
        );

    }

    public static TileDefinition MiddleDirt() {

        return new TileDefinition()
        .setGameObjectID( GameObject.MiddleDirt )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.MiddleDirt
        );

    }

    public static TileDefinition TopLeftDeepDirt() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopLeftDeepDirt )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopLeftDeepDirt
        );

    }

    public static TileDefinition TopDeepDirt() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopDeepDirt )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopDeepDirt
        );

    }

    public static TileDefinition MiddleGrassShadow() {

        return new TileDefinition()
        .setGameObjectID( GameObject.MiddleGrassShadow )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.MiddleGrassShadow
        );

    }

    public static TileDefinition TopLefGrassShadow() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopLefGrassShadow )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopLefGrassShadow
        );

    }

    public static TileDefinition TopGrassShadow() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopGrassShadow )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopGrassShadow
        );

    }

    public static TileDefinition LeftGrassShadow() {

        return new TileDefinition()
        .setGameObjectID( GameObject.LeftGrassShadow )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.LeftGrassShadow
        );

    }

    public static TileDefinition TopDirtGrass() {

        return new TileDefinition()
        .setGameObjectID( GameObject.TopDirtGrass )
        .setZ( -1 )
        .setRenderable( true )
        .setSolid( false )
        .setUniqueSprite(
            Sprites.Grass.TopDirtGrass
        );

    }
    */

    public static TileDefinition StoneWall() {

        return new TileDefinition()
        .setZ( 0 )
        .setRenderable( true )
        .setSolid( true )
        .setOverlapOthers( true )
        .setPushOthers( true )
        .setUniqueSprite(
            Sprites.StoneWall.Middle
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
            Sprites.CrackedStoneWall.Middle
        )
        .setCollisionExeption([
            GameObject.Slime
        ]);


    }

}

