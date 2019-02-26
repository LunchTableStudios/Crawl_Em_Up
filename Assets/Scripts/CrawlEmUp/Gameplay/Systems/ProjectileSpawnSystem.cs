namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;
    using Unity.Rendering;
    using Unity.Collections;

    using Bootstraps;

    public class ProjectileSpawn : JobComponentSystem
    {
        private struct SpawnProjectileJob : IJobParallelFor
        {
            [ ReadOnly ] public EntityCommandBuffer.Concurrent CommandBuffer;
            public ComponentDataArray<Position> Positions;
            public ComponentDataArray<Rotation> Rotations;

            public void Execute( int index )
            {
                Entity projectile = CommandBuffer.CreateEntity( index );
                CommandBuffer.AddSharedComponent( index, projectile, GameplayBootstrap.ProjectileMeshComponent );
                CommandBuffer.AddComponent( index, projectile, Positions[index] );
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
            return new SpawnProjectileJob
            {
                CommandBuffer = projectileBarrier.CreateCommandBuffer().ToConcurrent(),
                Positions = projectileSpawnOriginGroup.GetComponentDataArray<Position>(),
                Rotations = projectileSpawnOriginGroup.GetComponentDataArray<Rotation>()
            }.Schedule( projectileSpawnOriginGroup.CalculateLength(), 64, inputDeps );
        }

        private class ProjectileBarrier : BarrierSystem {}
    }
}