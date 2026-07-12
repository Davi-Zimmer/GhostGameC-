using System.Numerics;
using Game.Core;
using Game.World.Entity;

namespace Game.World.Events;

public class EventEntity: GenericEntity {

    public EventName locationName = EventName.None ;
        
    public EventEntity( Main game ): base( game ) {
        
        Configure<EventEntity>( e => {
            e.setGameObjectID( GameObject.EventObject );
            e.getPhysics().setFixed( true );
            e.setW( game.TileSize ).setH( game.TileSize );
            e.setCollisionTrigger( true ); 
        });

    }

    public override bool collisionTrigger<T>( T target ) {
        
        if( target is Player ) {

            Map.ExecuteEvent( locationName, game );

        }

        return false;

    }

    public EventName getEventName(){ return locationName; }

    public EventEntity setEventName( EventName data ){ locationName = data; return this; }

}