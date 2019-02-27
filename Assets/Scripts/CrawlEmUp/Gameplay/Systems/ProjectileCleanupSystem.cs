namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Collections;

    public class ProjectuleCleanupSystem : JobComponentSystem
    {

        [ BurstCompile ]
        private struct RemoveProjectileJob : IJobParallelFor
        {
            [ ReadOnly ] public EntityCommandBuffer.Concurrent CommandBuffer;
            public EntityArray ProjectileEntities;
            public ComponentDataArray<Projectile> Projectiles;

            public float Time;

            public void Execute( int index )
            {
                Projectile projectile = Projectiles[index];
                if( Time > projectile.StartTime + projectile.Duration )
                    CommandBuffer.DestroyEntity( index, ProjectileEntities[index] );
            }
        }

        private ComponentGroup projectileGroup;
        [ Inject ] private ProjectileCleanupBarrier projectileCleanupBarrier;

        protected override void OnCreateManager()
        {
            projectileGroup = GetComponentGroup(
                ComponentType.Create<Projectile>()
            );
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            float time = UnityEngine.Time.time;

            return new RemoveProjectileJob
            {
                CommandBuffer = projectileCleanupBarrier.CreateCommandBuffer().ToConcurrent(),
                ProjectileEntities = projectileGroup.GetEntityArray(),
                Projectiles = projectileGroup.GetComponentDataArray<Projectile>(),
                Time = time
            }.Schedule( projectileGroup.CalculateLength(), 64, inputDeps );
        }

        public class ProjectileCleanupBarrier : BarrierSystem {}
    }
}