using System.Numerics;
using Game.Core;
using Game.Rendering;
using Game.World.Entity;
using Raylib_cs;

namespace Game.World.Item;

using ShotData = ( float dirX, float dirY, float speed, float centerX, float centerY );

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

    private void newProjectile( ShotData data  ) {
          
          GenericProjectile entity = new GenericProjectile( game ).Configure<GenericProjectile>( e => {
            e.setRenderable( true )
            .getPhysics()!
            .setFriction( 1 )
            .getAcceleration()
            .setXY( data.dirX, data.dirY );
        
            e.setSpeed( data.speed );
            e.setW( 10 ).setH( 10 );
            e.setXY( data.centerX, data.centerY );

            e.getCollidable()
            .getExceptions()
            .Add( GameObject.Player );

        });

        game.tickExecutionStack.Add( () => {
            game.addToMap( entity );
        });

    }

    private ShotData calcShot() {

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

        ShotData data = ( (float)dirX, (float)dirY, speed, centerX, centerY ); 


        return data;

    }

    private bool canShot() {

        return game
            .getPlayer()
            .getInvetory()
            .getProjectiles( GameObject.Ectoplasma ) > 0;

    }

    private void consumeBullet() {

        game
        .getPlayer()
        .getInvetory()
        .addProjectile( GameObject.Ectoplasma, -1 );

    }

    private void shot() {

        if( !canShot() ) return;
        
        newProjectile( calcShot() );

        consumeBullet();

    }

}