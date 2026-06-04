using Game.Core;
using Game.World.Tile;
using Raylib_cs;

namespace Game.Data;

public class TileDefinition {
    public float x = 0;
    public float y = 0;
    public float z = 0;
    public float w = 0;
    public float h = 0;
    public float knockback = 1;
    public float friction = .9f;
    public float mass = 1;

    public bool physic = false;
    public bool renderable = false;
    public bool solid = false;
    public bool overlapOthers = false;
    public bool pushOthers = false;
    public bool collisionTrigger = false;

    public GameObject gameObjectID = GameObject.None;

    public List< GameObject > collisionExeption = [];

    public Rectangle uniqueSprite;


    public TileDefinition setX( float f ){ x = f; return this; }
    public TileDefinition setY( float f ){ y = f; return this; }
    public TileDefinition setZ( float f ){ z = f; return this; }
    public TileDefinition setW( float f ){ w = f; return this; }
    public TileDefinition setH( float f ){ h = f; return this; }    
    public TileDefinition setKnockback ( float f ){ knockback = f; return this; }
    public TileDefinition setFriction  ( float f ) { friction = f; return this; }
    public TileDefinition setMass      ( float f ){ mass = f; return this; }


    public TileDefinition setRenderable      ( bool b ){ renderable = b; return this; }
    public TileDefinition setSolid           ( bool b ){ solid = b; return this; }
    public TileDefinition setOverlapOthers   ( bool b ){ overlapOthers = b; return this; }
    public TileDefinition setPushOthers      ( bool b ){ pushOthers = b; return this; }
    public TileDefinition setCollisionTrigger( bool b ){ collisionTrigger = b; return this; }
    public TileDefinition setPhysic          ( bool b ){ physic = b; return this; }


    public TileDefinition setGameObjectID( GameObject o ) { gameObjectID = o; return this; }
    public TileDefinition setCollisionExeption( List<GameObject> l ) { collisionExeption = l; return this; }
    public TileDefinition setUniqueSprite( Rectangle s ) { uniqueSprite = s; return this; }

}

public class TileCreator {

    private static GenericTile LoadTile( TileDefinition def, Main game ) {
        
        GenericTile tile = new( game );

        tile.setGameObjectID( def.gameObjectID );

        tile.setZ( def.z )
            .setX( def.x )
            .setY( def.y );

        if( def.w == 0 ) def.w = game.TileSize; 
        if( def.h == 0 ) def.h = game.TileSize; 

        tile.setW( def.w )
            .setH( def.h );
        
        tile.getCollidable()
            .setCanOverlapOthers( def.overlapOthers )
            .setCanPushOthers( def.pushOthers )
            .setSolid( def.solid )
            .getExceptions()
            .AddRange(
                def.collisionExeption
            );

        tile.setRenderable( def.renderable );


        if( def.physic ) {
            
            tile.addPhysic()
                .getPhysics()!
                .setMass( def.mass )
                .setKnockback( def.knockback )
                .setFriction( def.friction );

        }

        Rectangle r = def.uniqueSprite;

        tile.sprite = new( r.X, r.Y, r.Width, r.Height );

        return tile;

    }
    
    public static TileDefinition? Find( GameObject o ) {

        switch( o ) {
            case GameObject.Grass            : return Tiles.Grass();
            case GameObject.StoneWall        : return Tiles.StoneWall();
            case GameObject.CrackedStoneWall : return Tiles.CrackedStoneWall();
        }
    
        return null;

    }

    public static GenericTile? NewTile( GameObject id, Main game ) {
        
        var t = Find( id );

        if( t == null ) return null;

        return LoadTile( t, game );

    }

}
