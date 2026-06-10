using System.Numerics;
using Game.Core;
using Game.Rendering;
using Game.World.Entity;
using Raylib_cs;

namespace Game.World.Item;

public class EctoGun: GenericItem {

    public EctoGun( Main game ): base( game ) {
        
        Configure<EctoGun>( g => {
            g
            .setW( game.TileSize )
            .setH( game.TileSize )
            .setZ( 9 );

            g.setRenderable( true )
            .setGameObjectID( GameObject.EctoGun )
            .getCollidable()
            .setSolid( true )
            .setCanOverlapOthers( false )
            .setCanPushOthers( false );

            g.setCollisionTrigger( true );

            g.loadSpriteFrame( Sprites.EctoGun.icon );

        });


    }


    public override void use() {

        shot();

    }


    private void shot() {
        
        Vector2 mouse = Raylib.GetScreenToWorld2D(
            Raylib.GetMousePosition(),
            game.getCamera()
        );
        
        float centerX = game.getPlayer().getMiddleX();
        float centerY = game.getPlayer().getMiddleY();

        double dirX = mouse.X - centerX;
        double dirY = mouse.Y - centerY;

        double length = Math.Sqrt(dirX * dirX + dirY * dirY);

        if(length > 0) {
            dirX /= length;
            dirY /= length;
        }

        float speed = 1000;

        GenericProjectile entity = new GenericProjectile( game ).Configure<GenericProjectile>( e => {
            e.setRenderable( true )
            .getPhysics()!
            .setFriction( 1 )
            .getAcceleration()
            .setXY( (float)dirX, (float)dirY );
        
            e.setSpeed( speed );
            e.setW( 10 ).setH( 10 );
            e.setXY( centerX, centerY );

            e.getCollidable()
            .getExceptions()
            .Add( GameObject.Player );

        });

        game.tickExecutionStack.Add( () => {
            game.addToMap( entity );
        });

    }

}