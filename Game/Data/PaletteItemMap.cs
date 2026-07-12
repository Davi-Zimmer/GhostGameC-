using Game.Core;
using Game.Rendering;
using Game.World.Entity;
using Game.World.Entity.Enemy;
using Game.World.Events;
using Game.World.Item;
using Game.World.Tile;

namespace Game.Data;


public class PaletteItem {
    public GameObject gameObject = GameObject.None;
    public Type classObject;
    public List<SpriteFrame> previewSprites;

    public PaletteItem( GameObject o, Type classObj, List<SpriteFrame> preview ) {
        gameObject     = o;
        classObject    = classObj;
        previewSprites = preview;
    }

}


public class AllGameObjectsPalette {
    
    public static List<PaletteItem> Items = new() {

        new PaletteItem ( GameObject.Player           , typeof( Player )      , Sprites.GetRects( Sprites.Player ) ),
        new PaletteItem ( GameObject.Slime            , typeof( Slime )       , Sprites.GetRects( Sprites.Slime ) ),
        new PaletteItem ( GameObject.Grass            , typeof( GenericTile ) , Sprites.GetRects( Sprites.Grass ) ),
        new PaletteItem ( GameObject.StoneWall        , typeof( GenericTile ) , Sprites.GetRects( Sprites.StoneWall ) ),
        new PaletteItem ( GameObject.CrackedStoneWall , typeof( GenericTile ) , Sprites.GetRects( Sprites.CrackedStoneWall ) ),
        new PaletteItem ( GameObject.Poison           , typeof( Poison )      , Sprites.GetRects( Sprites.Poison ) ),
        new PaletteItem ( GameObject.EctoGun          , typeof( EctoGun )     , Sprites.GetRects( Sprites.EctoGun ) ),
        new PaletteItem ( GameObject.EventObject      , typeof( EventEntity ) , Sprites.GetRects( Sprites.Others ) )
    };

    public static PaletteItem? FindByGameObject( GameObject gameObject ) {
        
        foreach ( var item in Items ) {
        
            if( item.gameObject == gameObject ) return item; 
        
        }

        return null;
    }

}