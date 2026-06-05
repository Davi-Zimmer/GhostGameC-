using System.Numerics;
using Game.Data;
using Game.Objects;
using Game.Objects.Basics;
using Game.World.Entity;
using Game.World.Entity.Enemy;
using Game.World.Item;
using Game.World.Tile;
using Raylib_cs;

namespace Game.Core;

public class Main {

    public int TileSize = 50;

    Camera2D cam = new();

    private List< WorldObject > map = [];

    private Player player;

    private Rect? cameraTarget = null;

    public Texture2D spritesheet = new();

    private float innerWidth = 0;
    private float innerHeight = 0;

    public List<Action> tickExecutionStack = [];

    public MapCreator mapCreator;

    public Main() {
    
        var p = new Player( this );

        player = p;
        cameraTarget = p;

        mapCreator = new MapCreator( this );

        addToMap( p );

        configCamera( p );

        TileCreator c = new();

        createTileInMap( GameObject.StoneWall       , t => t.setXY( 500f, 200f ) );
        createTileInMap( GameObject.CrackedStoneWall, t => t.setXY( 600f, 200f ) );

        addToMap( new Slime( this ) );

        addToMap( new Poison ( this ).Configure<Poison>( i => i.setX( 200 )) );
        addToMap( new EctoGun( this ).Configure<EctoGun>( i => i.setXY( 200, 200 )) );

        int A = 50;
        int size = 50;

        for ( int x = 0; x < A; x++ ) {

            for ( int y = 0; y < A; y++ ) {
                
                createTileInMap( GameObject.Grass, t => t.setXY( x * size, y * size ) );
                
            }

        }
    
    }

    public void setup() {

        innerWidth  = Raylib.GetScreenWidth();
        innerHeight = Raylib.GetScreenHeight();

        string path = Directory.GetCurrentDirectory() + "/Assets/placeholder.png";

        spritesheet = Raylib.LoadTexture( path );

    }

    public void finish() {

        Raylib.UnloadTexture( spritesheet );

    }

    public void configCamera( Player p ) {
        
        cam.Target = new Vector2( p.getX(), p.getY() ); 
    
        cam.Target   = new Vector2( p.getX() + 20.0f, p.getY() + 20.0f );
        cam.Offset   = new Vector2( innerWidth / 2f, innerHeight / 2f );
        cam.Rotation = 0.0f;
        cam.Zoom     = 1.0f;

    }

    private void collisionPush( WorldObject e, WorldObject other, Collidable.Overlap overlap, float delta ) {

        bool horizontal = Math.Abs( overlap.x ) < Math.Abs( overlap.y );

        if( horizontal ) {

            Collidable oColl = other.getCollidable();
            Physics op       = other.getPhysics()!;
            Physics ep       = e.getPhysics()!;

            if( op != null && ep != null ) {
                
                if( oColl.getCanOverlapOthers() ) e.applyX( overlap.x ); else {
                    
                    if( e.getCollidable().getCanPushOthers() ) {

                        op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );

                    }

                }

                if( op.getFixed() ) return;

                op.pushX( Math.Sign( -overlap.x ), ep.getMass(), ep.getKnockback(), delta );
            }

            else if( oColl.getCanOverlapOthers() ) e.applyX( overlap.x );


        } else {
           
             Collidable oColl = other.getCollidable();
            Physics op        = other.getPhysics()!;
            Physics ep        = e.getPhysics()!;

            if( op != null && ep != null ) {
                
                if( oColl.getCanOverlapOthers() ) e.applyY( overlap.y ); else {
                    
                    if( e.getCollidable().getCanPushOthers() ) {

                        op.pushY( Math.Sign( -overlap.y ), ep.getMass(), ep.getKnockback(), delta );

                    }

                }

                if( op.getFixed() ) return;

                op.pushY( Math.Sign( -overlap.y ), ep.getMass(), ep.getKnockback(), delta );
            }

            else if( oColl.getCanOverlapOthers() ) e.applyY( overlap.y );

        }

    }

    private void executeCollisionTrigger( WorldObject e, WorldObject other ) {

        bool remove = other.collisionTrigger( e );

        if( !remove ) return;

        tickExecutionStack.Add( () => {
            
            map.Remove( other );

        });

    }

    private void executeStack()  {
        
        if( tickExecutionStack.Count > 0 ){

            foreach ( var func in tickExecutionStack ) {
                
                func();

            }

            tickExecutionStack = [];

        }

    }

    private void collision( WorldObject e,  float delta ) {
        
        if( !e.getCollidable().getSolid() ) return;

        foreach( var other in map ) {

            if( !other.getCollidable().getSolid() ) continue;

            if( other.getCollidable().getExceptions().Contains( e.getGameObjectID() ) ) continue;
            if( e.getCollidable().getExceptions().Contains( other.getGameObjectID() ) ) continue;

            if( e == other ) continue;

            if ( !Collidable.IsColliding( e, other, delta ) ) continue;
        
            var overlap = Collidable.GetOverlap( e, other );
        
            if( overlap == null ) continue;

            // collision trigger

            if( other.getCollisionTrigger() ) executeCollisionTrigger( e, other );

            collisionPush( e, other, overlap.Value, delta );

        }

    }

    private float lerp( float start, float end, float t ) {
        return start + (end - start) * t;
    }

    private double getTargetX( float x, Rect targ ) { 
        return ( x + targ.getW() / 2 ) - innerWidth / 2;
    }

    private double getTargetY( float y, Rect targ ) {
        return ( y + targ.getH() / 2 ) - innerHeight / 2;
    }

    private void cameraFollow( float delta ) {

        Rect follow = cameraTarget!;

        if( follow == null ) return;

        float xx = cameraTarget!.extractX( delta );
        float yy = cameraTarget!.extractY( delta );
        
        float x = (float)getTargetX( xx, cameraTarget );
        float y = (float)getTargetY( yy, cameraTarget );

        cam.Target.X = lerp( cam.Target.X, x, .3f );
        cam.Target.Y = lerp( cam.Target.Y, y, .3f );

    }

    public bool outsideCamera( Rect r, int margin ) {
        return (
            cam.Target.X - margin < r.getX() + r.getW() &&
            cam.Target.Y - margin < r.getY() + r.getH() &&
            cam.Target.X + margin + innerWidth  > r.getX() && 
            cam.Target.Y + margin + innerHeight > r.getY() 
        );

    }

    private void renderInterface() {

        {
            int width = 100;

            int x = 10;
            int y = 10;

            int lifePercent = player.getLife() * width / 100;

            Raylib.DrawRectangle( x, y, lifePercent, 30, Color.Green );
            Raylib.DrawRectangle( lifePercent + x, y, width-lifePercent, 30, Color.Red );
        }

        {
            int sizeX = 20;
            int sizeY = 30;

            int border = 10;
            int gap = 5;

            for ( int x = 0; x < 3; x++ ) {

                float y = innerHeight - 10;
                Raylib.DrawRectangle( border + x * sizeX + gap * x, (int)y - sizeY - border, sizeX, sizeY, new Color( 255, 0, 100, .2f ) );

            }

        }

    }

    public void update( float delta ) {
        
        if( mapCreator.open ) {

            mapCreator.update( ref cam, delta, spritesheet );

            return;
        }

        if( Raylib.IsKeyPressed( KeyboardKey.F1 ) ) mapCreator.toggleMapCreation();

        executeStack();

        if( player.getInvetory().open ) {
            
            player.getInvetory().tick( delta );
            
            player.getInvetory().render( cam, delta, spritesheet );

            return;
        }

        Raylib.ClearBackground( Color.Black );

        // <Game>
            Raylib.BeginMode2D( cam );

            cameraFollow( delta );
            
            foreach( var t in map ) {
                
                if( !outsideCamera( t, TileSize * 2 ) ) continue;

                t.tick( delta );
                
                collision( t, delta );
                
                if( t.getRenderable() ) t.render( cam, delta, spritesheet );

            }

            Raylib.EndMode2D();

        // </Game>

        // <Interface>

            renderInterface();

        // </Interface>

        
    }

    public void addToMap( WorldObject entity ) {

        map.Add( entity );

        map.Sort( ( a, b ) => a.getZ().CompareTo( b.getZ() ) ); // map.OrderBy( e => e.getZ() ).ToList();

    }    

    public delegate void Callback( GenericTile t );
    public void createTileInMap( GameObject gameObject, Callback func ) {
        
        GenericTile? tile = TileCreator.NewTile( gameObject, this );

        if( tile == null) return; 
            
        func( tile! );

        addToMap( tile );

    }

    public Player getPlayer(){ return player; }
    public float getInnerWidth() { return innerWidth; }
    public float getInnerHeight() { return innerHeight; }

    public Camera2D getCamera() { return cam; }

}

public enum GameObject {
    None,
    GenericEntity, GenericItem, GenericTile,
    Player, Slime,
    Poison, EctoGun,

    Grass, StoneWall, CrackedStoneWall,

}
