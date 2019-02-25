namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Collections;
    using Unity.Burst;

    [ UpdateAfter( typeof( WeaponFiringSystem ) ) ]
    public class FiringCleanupSystem : JobComponentSystem
    {
        [ BurstCompile ]
        private struct RemoveFiringJob : IJobParallelFor
        {
            public EntityArray WeaponEntities;
            public ComponentDataArray<Weapon> Weapons;
            public ComponentDataArray<Firing> Firings;

            public EntityCommandBuffer.Concurrent CommandBuffer;

            public float time;

            public void Execute( int index )
            {
                if( time > Firings[index].Time + Weapons[index].FireRate  )
                {
                    CommandBuffer.RemoveComponent<Firing>( index, WeaponEntities[index] );
                }
            }
        }

        private struct WeaponFiringEntityFilter
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<Weapon> WeaponComponents;
            public ComponentDataArray<Firing> FiringComponents;
        }

        private ComponentGroup weaponGroup;

        [ Inject ] private WeaponFiringEntityFilter weaponFiringEntityFilter;
        [ Inject ] private FiringCleanupBarrier firingCleanupBarrier;

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            float time = UnityEngine.Time.time;
            
            return new RemoveFiringJob
            {
                WeaponEntities = weaponFiringEntityFilter.Entities,
                Weapons = weaponFiringEntityFilter.WeaponComponents,
                Firings = weaponFiringEntityFilter.FiringComponents,
                CommandBuffer = firingCleanupBarrier.CreateCommandBuffer().ToConcurrent(),
                time = time
            }.Schedule( weaponFiringEntityFilter.Length, 16, inputDeps );
        }

        public class FiringCleanupBarrier : BarrierSystem {}
    }
}