using System.CodeDom.Compiler;
using System.Reflection;
using Game.Objects.Basics;
using Game.World.Entity;
using Raylib_cs;
namespace Game.Rendering;

public struct SpriteFrame {
    public Rectangle rect;
    public int multiplyerW;
    public int rotationX;

    public SpriteFrame ( int x, int y, int w, int h, int multiplyerW, int rotationX ) {
        rect = new Rectangle( x, y, w, h );
        this.multiplyerW = multiplyerW;
        this.rotationX = rotationX;
    }

}

public class PlayerSprites {
    public readonly SpriteFrame Down  = new( 0,   0, 27, 36,  1, 0 );
    public readonly SpriteFrame Left  = new( 0,  75, 27, 36, -1, 0 );
    public readonly SpriteFrame Right = new( 0,  75, 27, 36,  1, 0 );
    public readonly SpriteFrame Up    = new( 0, 115, 27, 36,  1, 0 );
}

public class SlimeSprites {
    public SpriteFrame JumpRight = new( 1, 158, 48, 42,  1, 0 );
    public SpriteFrame JumpLeft  = new( 1, 158, 48, 42, -1, 0 );

}

public class GrassSprites {
    public SpriteFrame TopLeftDirtGrass = new( 251, 1, 21, 21, 1, 0 );
    public SpriteFrame TopDirtGrass     = new( 273, 1, 21, 21, 1, 0 );
    public SpriteFrame MiddleGrass      = new( 295, 1, 21, 21, 1, 0 );

    public SpriteFrame TopLeftDirt = new( 251, 23, 21, 21, 1, 0 );
    public SpriteFrame TopDirt     = new( 273, 23, 21, 21, 1, 0 );
    public SpriteFrame MiddleDirt  = new( 295, 23, 21, 21, 1, 0 );

    public SpriteFrame TopLeftDeepDirt = new( 251, 45, 21, 21, 1, 0 );
    public SpriteFrame TopDeepDirt     = new( 273, 45, 21, 21, 1, 0 );
    public SpriteFrame MiddleGrassShadow  = new( 295, 45, 21, 21, 1, 0 );

    public SpriteFrame TopLefGrassShadow  = new( 251, 67, 21, 21, 1, 0 );
    public SpriteFrame TopGrassShadow     = new( 273, 67, 21, 21, 1, 0 );
    public SpriteFrame LeftGrassShadow    = new( 295, 65, 21, 21, 1, 0 );
    
}

public class StoneWallSprites {
    public SpriteFrame Middle = new( 184, 1, 32, 32, 1, 0 );
    
}

public class CrackedStoneWallSprites {
    public SpriteFrame Middle = new( 217, 34, 32, 32, 1, 0 );
    
}

public class PoisonSprites {
    
    public SpriteFrame icon = new( 151, 34, 32, 32, 1, 0 );
 
}

public class EctoGunSprites {
    
    public SpriteFrame icon = new( 151, 67, 32, 32, 1, 0 );
 
}



public class Sprites {

    public static readonly PlayerSprites Player = new();
    public static readonly SlimeSprites  Slime  = new();


    public static readonly GrassSprites            Grass             = new();
    public static readonly StoneWallSprites        StoneWall         = new();
    public static readonly CrackedStoneWallSprites CrackedStoneWall  = new();


    public static readonly PoisonSprites  Poison = new();
    public static readonly EctoGunSprites EctoGun = new();


    public static List<SpriteFrame> GetRects( object obj ) {

        var resultado = new List<SpriteFrame>();

        var campos = obj.GetType().GetFields(
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic
        );

        foreach( var campo in campos ) {
            
            if (campo.FieldType == typeof(SpriteFrame)) {

                resultado.Add((SpriteFrame)campo.GetValue(obj)!);

            }

        }

        return resultado;

    }

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