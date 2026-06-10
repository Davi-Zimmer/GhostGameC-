
using Game.Core;
using Game.Objects.Animation;
using Game.Rendering;
using Raylib_cs;

namespace Game.World.Entity.Enemy;
public class Slime: GenericEntity {
    
    private Animation animation = new();

    private int coutToJump = 0; 
    private Random rand = new();
    private int minJumpCooldown = 50;

    public Slime( Main game ): base( game ) {
            
        this.game = game;

        Configure<Slime>( s => {
            s.setSpeed( 200 )
            .setW( 20 )
            .setH( 20 )
            .setZ( 5 );
            setXY( 400, 0 );

            s.getPhysics()
            .setMass( 5 )
            .setKnockback( 100 );

            s.getCollidable()
            .setCanOverlapOthers( true )
            .setCanPushOthers( true );

            s.setGameObjectID( GameObject.Slime );
            
        });

        fillSprites();

    }

    private void fillSprites() {
        
        animation.createAnimation( [ "jumpRight", "jumpLeft" ] );

        animation.forSprites( Sprites.Slime.JumpRight, 3, 5, "jumpRight",  1, 0 );
        animation.forSprites( Sprites.Slime.JumpRight, 3, 5, "jumpLeft" , -1, 0 );

    }

    private void jump() {
        
        var p = game.getPlayer();

        float dx = (p.getMiddleX() > getMiddleX() ? 1 : -1) * getSpeed();
        float dy = (p.getMiddleY() > getMiddleY() ? 1 : -1) * getSpeed();

        getPhysics().getAcceleration().apply( dx, dy );

        if( dx > 0 ) animation.changeAnimation( "jumpRight" );
        else         animation.changeAnimation( "jumpLeft" );
            
    }

    public override void tick( float delta ) {
        
        updatePosition( delta );

        coutToJump++;

        if( coutToJump > minJumpCooldown + rand.Next( 50 ) ) {

            coutToJump = 0;

            animation.playAnimation();

            jump();

        }
        
        if( animation.isRunning() ) {
            
            bool finished = animation.stepAnimation();

            animation.setAnimationRunning( !finished );

        }

    }


    public override void render( Camera2D cam, float delta, Texture2D spriteSheet ) {
        
        animation.render( this, spriteSheet );

    }


    //--------------------------------- Getters ---------------------------------\\ 
    public int getMinJumpCooldown() { return minJumpCooldown; }

    //--------------------------------- Setters ---------------------------------\\ 
    public Slime setMinJumpCooldown( int s ) { minJumpCooldown = s; return this; }

}
