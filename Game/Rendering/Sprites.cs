using Raylib_cs;

namespace Game.Rendering;

public enum Variation {
    TopLeft    , Top    , TopRight,
    Left       , Middle , Right,
    BottomLeft , Bottom , BottomRight
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

}