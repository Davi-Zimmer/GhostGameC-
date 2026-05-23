using Game;
using Game.Objects.Basics;
using Game.Objects.Entities;
using Raylib_cs;

namespace Game.Core;

public class Main {
    
    private List< GenericEntity > map = [];

    public Main() {

        map.Add(
            new Player().Configure<Player>( e => {
                e.setW( 100 );
            })
        );

    }

    public void update( double delta ) {

        Raylib.ClearBackground( Color.Black );
        
        foreach( var t in map ) {
            
            t.tick( delta );
            t.render();

        }


        // Raylib.DrawCircle( p.intX, p.intY, 10, Color.Red );

    }

    
    //--------------------------------- Enums ---------------------------------\\ 


}
public enum GameObject {
    GenericEntity
}
