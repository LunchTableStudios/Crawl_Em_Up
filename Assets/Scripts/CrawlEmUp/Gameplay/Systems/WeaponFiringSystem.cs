namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Collections;
    using Unity.Burst;

    using Rewired;

    public class WeaponFiringSystem : JobComponentSystem
    {
        private ComponentGroup weaponGroup;

        [ BurstCompile ]
        private struct FireWeaponJob : IJobParallelFor
        {
            public EntityArray WeaponEntities;
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public float time;

            public void Execute( int index )
            {
                CommandBuffer.AddComponent<Firing>( index, WeaponEntities[ index ], new Firing{ Time = time } );
            }
        }

        private struct WeaponEntityFilter
        {
            public readonly int Length;
            public EntityArray Entities;
            public ComponentDataArray<Weapon> WeaponComponents;
            public SubtractiveComponent<Firing> FiringComponents;
        }

        [ Inject ] private WeaponEntityFilter weaponEntityFilter;
        [ Inject ] private FiringBarrier firingBarrier;

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            float time = UnityEngine.Time.time;
            Player rewiredPlayer = ReInput.players.GetSystemPlayer();

            if( rewiredPlayer.GetButton( "Shoot" ) )
            {
                return new FireWeaponJob
                {
                    WeaponEntities = weaponEntityFilter.Entities,
                    CommandBuffer = firingBarrier.CreateCommandBuffer().ToConcurrent(),
                    time = time
                }.Schedule( weaponEntityFilter.Length, 64, inputDeps );
            }

            return inputDeps;
        }

        public class FiringBarrier : BarrierSystem {}
    }
}