namespace CrawlEmUp.Input
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Mathematics;

    using Gameplay;

    using Rewired;

    public class KeyboardMovementSystem : JobComponentSystem
    {
        private struct CaptureKeyboardInputJob : IJobProcessComponentData<KeyboardMovement, Heading>
        {
            public float2 input;

            public void Execute( [ ReadOnly ] ref KeyboardMovement keyboardMovement, ref Heading heading )
            {
                heading.Value = input;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            Player rewiredPlayer = ReInput.players.GetSystemPlayer();
            
            Vector2 input = new Vector2( rewiredPlayer.GetAxis( "HorizontalMovement" ), rewiredPlayer.GetAxis( "VerticalMovement" ) ).normalized;

            return new CaptureKeyboardInputJob()
            {
                input = new float2( input.x, input.y )
            }.Schedule( this, inputDeps );
        }
    }
}