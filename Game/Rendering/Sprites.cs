using Game.World.Entity;
using Raylib_cs;

namespace Game.Rendering;



public class PlayerSprites {
    public Rectangle Down  => new Rectangle( 0,   0, 27, 36 );
    public Rectangle Left  => new Rectangle( 0,  37, 27, 36 );
    public Rectangle Right => new Rectangle( 0,  75, 27, 36 );
    public Rectangle Up    => new Rectangle( 0, 115, 27, 36 );
}

public class SlimeSprites {
    public Rectangle JumpRight => new Rectangle( 1,  158, 48, 42 );

}

public class GrassSprites {
    public Rectangle Middle => new Rectangle( 151, 1, 32, 32 );
    
}


public class StoneWallSprites {
    public Rectangle Middle => new Rectangle( 184, 1, 32, 32 );
    
}

public class CrackedStoneWallSprites {
    public Rectangle Middle => new Rectangle( 151, 1, 32, 32 );
    
}

public abstract class Sprites {

    public static readonly PlayerSprites Player = new();
    public static readonly SlimeSprites  Slime  = new();


    public static readonly GrassSprites            Grass             = new();
    public static readonly StoneWallSprites        StoneWall         = new();
    public static readonly CrackedStoneWallSprites CrackedStoneWall  = new();


}
















/*

public enum Variation {
    TopLeft    , Top    , TopRight,
    Left       , Middle , Right,
    BottomLeft , Bottom , BottomRight
}


public enum PlayerVariation {
    Down, Left, Right, Up
}
public abstract class Sprites {
    public delegate Rectangle GetRectCallback();

    public static Rectangle GetRectangle( Dictionary< Variation, GetRectCallback > list, Variation? variation ) {
        
        if( variation == null ) return list[ Variation.Middle ]();

        Variation v = variation!.Value;

        if( !list.ContainsKey( v ) ) return list[ Variation.Middle ]();       

        return list[ v ]();

    }

    public static Rectangle Grass( Variation? variation ) {
        
        Dictionary< Variation, GetRectCallback > list = new() {

            { Variation.Middle, () =>  new Rectangle( 151, 1, 32, 32 ) }

        };

        return GetRectangle( list, variation );
            
    }

    public static Rectangle StoneWall( Variation? variation ) {
        
        Dictionary< Variation, GetRectCallback > list = new() {
        
            { Variation.Middle, () =>  new Rectangle( 184, 1, 32, 32 ) }
            
        };

        return GetRectangle( list, variation );

    }

    public static Rectangle CrackedStoneWall( Variation? variation ) {
        Dictionary< Variation, GetRectCallback > list = new() {
        
            { Variation.Middle, () =>  new Rectangle( 217, 34, 32, 32 ) }
            
        };

        return GetRectangle( list, variation );
    }

    public static Rectangle Player() {

        Dictionary< Variation, GetRectCallback > list = new() {
        
            { PlayerVariation.Down  , () =>  new Rectangle( 0,   0, 27, 36 ) }
            { PlayerVariation.Left  , () =>  new Rectangle( 0,  37, 27, 36 ) }
            { PlayerVariation.Right , () =>  new Rectangle( 0,  75, 27, 36 ) }
            { PlayerVariation.Up    , () =>  new Rectangle( 0, 115, 27, 36 ) }
            
        };

        return GetRectangle( list, variation ); 
    } 

}
*/