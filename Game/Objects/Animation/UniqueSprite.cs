namespace Game.Objects.Animation;

using System.Numerics;
using Game.Objects.Basics;
using Raylib_cs;
public class UniqueSprite {
    
    public Rectangle rect { get; set; }


    public UniqueSprite( float x, float y, float w, float h ) {
        
        rect = new Rectangle( x, y, w, h );

    }

    public void render( Rect r, Texture2D spriteSheet ) {
        
        var vec = new Vector2( 0, 0 );
    
        Raylib.DrawTexturePro( 
            spriteSheet,
            rect,
            new Rectangle( r.getX(), r.getY(), r.getW(), r.getH() ),
            vec,
            0,
            Color.White
        );

    }

}