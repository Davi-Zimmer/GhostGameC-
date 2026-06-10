namespace Game.Objects.Animation;

using System.Numerics;
using Game.Objects.Basics;
using Game.World.Tile;
using Raylib_cs;
public class UniqueSprite {
    
    public Rectangle rect { get; set; }
    public int rotationX;
    public int multiplyerW;
    public UniqueSprite( float x, float y, float w, float h, int multiplyerW, int rotationX ) {
        
        rect = new Rectangle( x, y, w, h );

        this.multiplyerW = multiplyerW;
        this.rotationX = rotationX;

    }

    public void render( WorldObject r, Texture2D spriteSheet ) {
        
        var vec = new Vector2( rect.Width / 2, rect.Height / 2 );
    
        Raylib.DrawTexturePro( 
            spriteSheet,
            rect,
            new Rectangle( r.getX() + rect.Width / 2, r.getY() + rect.Height / 2, r.getW() * multiplyerW, r.getH() ),
            vec,
            rotationX,
            Color.White
        );

    }

}