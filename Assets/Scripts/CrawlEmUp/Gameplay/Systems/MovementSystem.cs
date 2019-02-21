namespace CrawlEmUp.Gameplay
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Collections;

    using LTS_ToolKit.CharacterController;

    public class MovementSystem : JobComponentSystem
    {
        private struct ApplyMovementToVelocity : IJobProcessComponentData<Heading, MovementSpeed, Velocity>
        {
            public void Execute( [ ReadOnly ] ref Heading heading, [ ReadOnly ] ref MovementSpeed movementSpeed, ref Velocity velocity )
            {
                velocity.Value = heading.Value * movementSpeed.Value;
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            return new ApplyMovementToVelocity().Schedule( this, inputDeps );
        }
    }
}