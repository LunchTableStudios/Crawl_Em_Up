namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Collections;

    [ UpdateAfter( typeof( WeaponFiringSystem ) ) ]
    public class FiringCleanupSystem : ComponentSystem
    {
        private struct RemoveFiringJob : IJobParallelFor
        {
            public EntityArray WeaponEntities;
            public ComponentDataArray<Weapon> Weapons;
            public ComponentDataArray<Firing> Firings;

            public EntityCommandBuffer.Concurrent CommandBuffer;

            public float time;

            public void Execute( int index )
            {
                if( Weapons[index].FireRate + time > Firings[index].Time )
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

        protected override void OnUpdate()
        {
            RemoveFiringJob firingJob = new RemoveFiringJob
            {
                WeaponEntities = weaponFiringEntityFilter.Entities,
                Weapons = weaponFiringEntityFilter.WeaponComponents,
                Firings = weaponFiringEntityFilter.FiringComponents,
                CommandBuffer = firingCleanupBarrier.CreateCommandBuffer().ToConcurrent(),
                time = UnityEngine.Time.time
            };

            JobHandle handle = firingJob.Schedule( weaponFiringEntityFilter.Length, 16 );
            handle.Complete();
        }

        public class FiringCleanupBarrier : BarrierSystem {}
    }
}