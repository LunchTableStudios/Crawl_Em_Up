namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Collections;

    using Rewired;

    public class WeaponFiringSystem : ComponentSystem
    {
        private ComponentGroup weaponGroup;

        private struct FireWeaponJob : IJobParallelFor
        {
            [ ReadOnly ] public NativeArray<Entity> weaponEntities;
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public float time;

            public void Execute( int index )
            {
                CommandBuffer.AddComponent<Firing>( index, weaponEntities[ index ], new Firing{ Time = time } );
            }
        }

        [ Inject ] private FiringBarrier firingBarrier;

        protected override void OnCreateManager()
        {
            // weaponGroup = GetComponentGroup(
            //     typeof( Weapon ), 
            //     ComponentType.Subtractive<Firing>()
            // );
            weaponGroup = GetComponentGroup( new EntityArchetypeQuery(){
                All = new ComponentType[] {
                    ComponentType.Create<Weapon>()
                },
                None = new ComponentType[] {
                    ComponentType.Create<Firing>()
                }
            } );
        }

        protected override void OnUpdate()
        {
            using( NativeArray<Entity> weapons = weaponGroup.ToEntityArray( Allocator.TempJob ) )
            {
                Player rewiredPlayer = ReInput.players.GetSystemPlayer();
                if( rewiredPlayer.GetButton( "Shoot" ) )
                {
                    FireWeaponJob fireWeaponJob = new FireWeaponJob
                    {
                        weaponEntities = weapons,
                        CommandBuffer = firingBarrier.CreateCommandBuffer().ToConcurrent(),
                        time = UnityEngine.Time.time
                    };

                    JobHandle jobHandle = fireWeaponJob.Schedule( weapons.Length, 16 );    
                    jobHandle.Complete();    
                }
            }
        }

        public class FiringBarrier : BarrierSystem {}
    }
}