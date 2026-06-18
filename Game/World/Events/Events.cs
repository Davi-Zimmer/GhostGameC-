using System.Numerics;
using Game.Core;
using Game.Objects;
using Game.World.Persistence;

namespace Game.World.Events;

public struct Location {
    public Level level;
    public Vector2 playerPos;

    public Location( Level l, Vector2 v ) {
        level = l;
        playerPos = v;
    }

};

public enum EventName {
    Spawn,
    None,
    Test

}

public enum Level {
    None,
    Spawn,
    Test
}

public class Map {

    public static void Change( Level level, int x, int y, Main game ) {
        
        string path = level.ToString() + ".world";

        List<WorldObject> map = MapSerializer.ReadMap( path, game );

        game.changeMap( map );

        game.getPlayer().setXY( x, y );

    }

    public delegate void EventFunc( Main game );

    public static Dictionary< EventName, EventFunc > AllEvents = new() {
        [ EventName.None  ] = ( Main game ) => { Console.WriteLine("DISPAROU"); },
        [ EventName.Spawn ] = ( Main game ) => Change( Level.Spawn , 0, 100 , game ),
        [ EventName.Test  ] = ( Main game ) => Change( Level.Test  , 0, 100 , game ),
    };

    public static void ExecuteEvent( EventName eventName, Main game ) {
        
        EventFunc f = AllEvents[ eventName ];

        try {
            
            f( game );

        } catch ( Exception ex) {
            
            Console.WriteLine( ex );

        }

    }

}


/*
    Northwest  North   Northeast
    West       Center  East
    Southwest  South   Southeast


    Noroeste    Norte     Nordeste 
    Oeste       Center    Leste
    Sudoeste    Sul       Sudeste


    preciso carregar o mapa e botar o player no local correto

*/