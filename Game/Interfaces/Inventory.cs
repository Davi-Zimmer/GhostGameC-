using System.Numerics;
using Game.Core;
using Game.Data;
using Game.Objects.Basics;
using Game.Rendering;
using Game.World.Item;
using Raylib_cs;


namespace Game.Interface;

using InventoryObject = ( bool hasItem, SpriteFrame sprite, Rectangle rect );

public enum HUDSlot {
    Second,
    Third,
    First,
    Weapon1,
    Weapon2
}


public class Inventory {

    public bool open = false;
    // public List< GenericItem > items = new();
    public int selectedItem = 0;
    public Main game;

    private byte[] itemDamaged = new byte[64];
    private ulong items;

    List<GameObject> gameObjectItemIDs = [
        GameObject.EctoGun,
        GameObject.Poison
    ];

    private Dictionary< HUDSlot, GenericItem?> hudItems = new() {
        [ HUDSlot.Weapon1 ] = null,
        [ HUDSlot.Weapon2 ] = null,
        [ HUDSlot.First   ] = null,
        [ HUDSlot.Second  ] = null,
        [ HUDSlot.Third   ] = null
    };

    private List<InventoryObject> allItems = new();
    
    public Inventory( Main game ) {

        this.game = game;
        
    }

    public void toggle() {
        
        open = !open;

        if(open) constructItems(); else destructItems();

    }

    public void tick( float delta ) {
        
        if( Raylib.IsKeyPressed( KeyboardKey.Tab ) ) toggle();

        if( Raylib.IsKeyPressed( KeyboardKey.A ) ) previousItem();
        if( Raylib.IsKeyPressed( KeyboardKey.D ) ) nextItem();

    }

    private void nextItem() {
        selectedItem = Math.Min( selectedItem + 1, allItems.Count -1  );
    }

    private void previousItem() {
        selectedItem = Math.Max( selectedItem - 1, 0 );
    }

    public void constructItems() {
        
        int x      = 0;
        int margin = 50;
        int left   = 50;

        foreach( var id in gameObjectItemIDs ) {
            
            foreach ( var obj in AllGameObjectsPalette.items ) {

                if( obj.gameObject == id ) {

                    SpriteFrame s = obj.previewSprites[0];

                    allItems.Add( new InventoryObject() {
                        hasItem = hasItem( obj.gameObject ),
                        sprite = s,
                        rect = new Rectangle( 
                            left + x + margin * x + s.rect.Width  / 2,
                            100  + s.rect.Height / 2,
                            50,
                            50
                        )
                    });

                    x++;           
                
                }

            }

        }

    }

    public void destructItems() {
        
        allItems.Clear();

    }

    public void render( Camera2D cam,  float delta, Texture2D spriteSheet ) {
        
        int   border  = 50;
        float width   = game.getInnerWidth()  - border * 2;
        float height  = game.getInnerHeight() - border * 2;

        Raylib.DrawRectangle( border, border, (int) width, (int) height, Color.Black );

        foreach( var item in allItems ) {

            Rectangle rect = allItems[ selectedItem ].rect;
            
            SpriteFrame s = item.sprite;

            var vec = new Vector2( s.rect.Width / 2, s.rect.Height / 2 );

            Raylib.DrawRectangle( (int)(rect.X - vec.X), (int)(rect.Y - vec.Y), (int)rect.Width, (int)rect.Height, Color.Blue );

            Rectangle r = s.rect;
            r.Width *= s.multiplyerW;

            float alpha = item.hasItem ? 1 : .3f;

            Color color = new Color( 255, 255, 255, alpha );

            Raylib.DrawTexturePro( 
                spriteSheet,
                r,
                item.rect,
                vec,
                s.rotationX,
                color
            );

        }


    }

    public int gameObjetToItemID( GameObject gameObject ) {
        return gameObjectItemIDs.IndexOf( gameObject );
    }

    public bool hasItem( GameObject gameObject ) {

        int itemId = gameObjetToItemID( gameObject );
        
        if( itemId == -1 ) return false;

        return ( items & ( 1UL << itemId )) != 0;

    }

    public void damageItem( GameObject gameObject ) {
        
        int itemId = gameObjetToItemID( gameObject );

        byte newDamage = itemDamaged[ itemId ]++;

        setItemDamage( gameObject, newDamage );

    }

    public void selectItem( GameObject gameObject, HUDSlot slot ) {

        if( !hasItem( gameObject ) ) return;
        
        hudItems[ slot ] = getItem( gameObject );

    }

    public void pickItem( GenericItem item ) {
        
        GameObject obj = item.getGameObjectID();

        setItem( obj, true );
        
        setItemDamage( obj, item.getItemDamage() );

    }

    public GenericItem? dropItem( GameObject gameObject ) {
        
        GenericItem? item = getItem( gameObject );
        
        if( item != null) {
            
            byte? damage = getItemDamage( gameObject );

            if( damage == null ) return item;

            item.setItemDamage( damage.Value );

        }

        return item;

    }

    public void useItem( HUDSlot slot ) {
        
        hudItems[ slot ]?.use();
        
    }

    //--------------------------------- Getters ---------------------------------\\ 
    public GenericItem? getSelectedItem( HUDSlot slot ) { return hudItems[ slot ]; }
    
    public byte? getItemDamage( GameObject gameObject ) {
        
        int itemId = gameObjetToItemID( gameObject );

        if( itemId == -1 ) return null;

        return itemDamaged[ itemId ];

    }
    
    public GenericItem? getItem( GameObject gameObject ) {

        if( !hasItem( gameObject ) ) return null;

        foreach ( var obj in AllGameObjectsPalette.items ) {
            
            if( obj.gameObject == gameObject ) {

                GenericItem item = (GenericItem)Activator.CreateInstance( obj.classObject, game )!;

                return item;

            }

        }
        
        return null;

    }
    
    //--------------------------------- Setters ---------------------------------\\ 
    public Inventory setSelectedItem( int i ) { selectedItem = i; return this; }
       
    private void setItem( GameObject gameObject, bool value ) {

        int itemId = gameObjetToItemID( gameObject );

        if( itemId == -1 ) return;

        if( value ) items |= 1UL << itemId;
        else items &= ~( 1UL << itemId );

    }

     public void setItemDamage( GameObject gameObject, byte damage ) {
        
        int itemId = gameObjetToItemID( gameObject );
        
        itemDamaged[ itemId ] = damage;

    }

}