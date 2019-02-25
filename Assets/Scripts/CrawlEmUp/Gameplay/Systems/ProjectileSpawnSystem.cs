namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Burst;
    using Unity.Transforms;

    public class ProjectileSpawn : JobComponentSystem
    {
        private struct SpawnProjectileJob : IJobProcessComponentData<Firing, Position, Rotation>
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute( ref Firing firing, ref Position position, ref Rotation rotation )
            {
                
            }
        }

        private ComponentGroup projectileSpawnOriginGroup;
        [ Inject ] private ProjectileBarrier projectileBarrier;

        protected override void OnCreateManager()
        {
            projectileSpawnOriginGroup = GetComponentGroup(
                typeof( Firing ),
                typeof( Position ),
                typeof( Rotation )
            );
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            return new SpawnProjectileJob
            {
                CommandBuffer = projectileBarrier.CreateCommandBuffer().ToConcurrent()
            }.Schedule( this, inputDeps );
        }

        private class ProjectileBarrier : BarrierSystem {}
    }
}