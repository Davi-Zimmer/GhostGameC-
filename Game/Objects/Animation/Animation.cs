namespace Game.Objects.Animation;

using System.Numerics;
using Game.Objects.Basics;
using Game.Rendering;
using Raylib_cs;
using SpriteType = ( int x, int y, int w, int h, int frames, int multplyerW, int rotationX );

public class Animation {
    
    private int frame = 0;
    private int animationDelay = 0;
    private bool animationRuning = false;

    private string animationName = ""; 


    private Dictionary< string, List< SpriteType > > sprites = [];

    public void createAnimation( List<string> names ) {
        
        if( names.Count == 0 ) return;
        
        foreach( string name in names ) {
            
            sprites.Add( name, [] );
            
        }

        animationName = names[ 0 ];

    }


    public void forSprites( SpriteFrame s, int frames, int frameDelay, string animationName ) {
        
        sprites[ animationName ] = [];

        Rectangle r = s.rect;

        for( int i = 0; i < frames; i++ ){

            sprites[ animationName ].Add((
                (int)r.X + i * (int)r.Width + i * 2,
                (int)r.Y, (int)r.Width, (int)r.Height, frameDelay,
                s.multiplyerW,
                s.rotationX
            ));

        }

    }


    public bool nextFrame() {
        
        frame++;
        
        if( frame > sprites[ animationName ].Count - 1 ) {
            frame = 0;

            return true;    
        }

        return false;

    }

    public bool stepAnimation() {
        
        animationDelay++;

        if( animationDelay > sprites[ animationName ][ frame ].frames ) {
            
            animationDelay = 0;

            return nextFrame();

        }

        return false;

    }

    public Animation changeAnimation( string name ) { animationName = name; return this; }

    public Animation playAnimation() { animationRuning = true; return this; }
    public Animation stopAnimation() { animationRuning = false; return this; }
    public Animation setAnimationRunning( bool b ) { animationRuning = b; return this; }
    
    public bool isRunning() { return animationRuning; }
    public SpriteType getFrameCoords() { return sprites.ContainsKey( animationName ) ? sprites[ animationName ][ frame ] : ( 0, 0, 0, 0, 0, 0, 0); }
    public Rectangle getFrameRectangle() {

        var s = getFrameCoords();

        return new Rectangle( s.x, s.y, s.w * s.multplyerW, s.h );

    }
    public int getFrame() { return frame; }

    public Animation changeAndPlay( string name ) { return changeAnimation( name ).playAnimation(); }

    public void tick() {

        if( isRunning() ) stepAnimation();
        
    }

    public void render( WorldObject r, Texture2D spriteSheet ) {

        Rectangle rect = getFrameRectangle();

        var vec = new Vector2( r.getW() / 2, r. getH() / 2 );
    
        Raylib.DrawTexturePro( 
            spriteSheet,
            rect,
            new Rectangle( r.getMiddleX(), r.getMiddleY(), r.getW(), r.getH() ),
            vec,
            r.getRotationX(),
            Color.White
        );

    }

}