namespace CrawlEmUp.Gameplay
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Burst;
    using Unity.Jobs;
    using Unity.Collections;
    using Unity.Transforms;
    using Unity.Mathematics;

    public class MovementSystem : JobComponentSystem
    {
        [ BurstCompile ]
        private struct MovementJob : IJobProcessComponentData<Position, Movement, MovementSpeed>
        {
            public float deltaTime;

            public void Execute( ref Position position, [ ReadOnly ] ref Movement movement, [ ReadOnly ] ref MovementSpeed movementSpeed )
            {
                float2 normalizedMovement = math.normalize( movement.Value );
                position.Value.x = position.Value.x + ( normalizedMovement.x * movementSpeed.Value * deltaTime );
                position.Value.y = position.Value.y + ( normalizedMovement.y * movementSpeed.Value * deltaTime );
            }
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            MovementJob movementJob = new MovementJob()
            {
                deltaTime = Time.deltaTime
            };

            return movementJob.Schedule( this, inputDeps );
        }
    }
}