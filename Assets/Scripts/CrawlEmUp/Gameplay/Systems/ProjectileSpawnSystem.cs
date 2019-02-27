namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;
    using Unity.Rendering;
    using Unity.Collections;
    using Unity.Mathematics;

    using Bootstraps;

    using LTS_ToolKit.CharacterController;

    public class ProjectileSpawn : JobComponentSystem
    {
        [ BurstCompile ]
        private struct SpawnProjectileJob : IJobParallelFor
        {
            [ ReadOnly ] public EntityCommandBuffer.Concurrent CommandBuffer;
            public ComponentDataArray<Position> Positions;
            public ComponentDataArray<Rotation> Rotations;

            public float Time;

            public void Execute( int index )
            {
                float3 heading = math.up() + GetHeadingDirection( Rotations[ index ].Value.value );

                Entity projectile = CommandBuffer.CreateEntity( index );
                CommandBuffer.AddComponent( index, projectile, new Projectile{ StartTime = Time, Duration = 4f } );
                CommandBuffer.AddComponent( index, projectile, Positions[index] );
                CommandBuffer.AddComponent( index, projectile, new Velocity() );
                CommandBuffer.AddComponent( index, projectile, new MovementSpeed{ Value = 10 } );
                CommandBuffer.AddComponent( index, projectile, new Heading{ Value = new float2( heading.x, heading.y ) } );
                CommandBuffer.AddSharedComponent( index, projectile, GameplayBootstrap.ProjectileMeshComponent );
            }

            private float3 GetHeadingDirection( float4 quaternion )
            {
                float4 nq = math.normalize( quaternion );
                float3 directionVector = float3.zero;
                directionVector.x = 2.0f * ( nq.x * nq.z - nq.w * nq.y );
                directionVector.y = 2.0f * ( nq.y * nq.z - nq.w * nq.x );
                directionVector.z = 1.0f - 2.0f * ( nq.x * nq.x + nq.y * nq.y );

                return directionVector;
            }
        }

        private ComponentGroup projectileSpawnOriginGroup;
        [ Inject ] private ProjectileBarrier projectileBarrier;

        protected override void OnCreateManager()
        {
            projectileSpawnOriginGroup = GetComponentGroup(
                ComponentType.Create<Firing>(),
                ComponentType.Create<Position>(),
                ComponentType.Create<Rotation>()
            );

            projectileSpawnOriginGroup.SetFilterChanged( ComponentType.Create<Firing>() );
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            float time = UnityEngine.Time.time;

            return new SpawnProjectileJob
            {
                CommandBuffer = projectileBarrier.CreateCommandBuffer().ToConcurrent(),
                Positions = projectileSpawnOriginGroup.GetComponentDataArray<Position>(),
                Rotations = projectileSpawnOriginGroup.GetComponentDataArray<Rotation>(),
                Time = time
            }.Schedule( projectileSpawnOriginGroup.CalculateLength(), 64, inputDeps );
        }

        private class ProjectileBarrier : BarrierSystem {}
    }
}