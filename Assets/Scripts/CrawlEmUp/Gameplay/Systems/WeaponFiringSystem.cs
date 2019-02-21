namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Jobs;
    using Unity.Collections;

    using Rewired;

    public class WeaponFiringSystem : JobComponentSystem
    {
        private ComponentGroup weaponGroup;

        private struct FireWeaponJob : IJobParallelFor
        {
            [ ReadOnly ] public NativeArray<Entity> weaponEntities;
            public EntityCommandBuffer.Concurrent CommandBuffer;

            public void Execute( int index )
            {
                CommandBuffer.AddComponent<Firing>( index, weaponEntities[ index ], new Firing() );
            }
        }

        protected override void OnCreateManager()
        {
            weaponGroup = GetComponentGroup(
                typeof( Weapon ),
                ComponentType.Subtractive<Firing>()
            );
        }

        protected override JobHandle OnUpdate( JobHandle inputDeps )
        {
            using( NativeArray<Entity> weapons = weaponGroup.ToEntityArray( Allocator.TempJob ) )
            {
                Player rewiredPlayer = ReInput.players.GetSystemPlayer();
                
                if( rewiredPlayer.GetButton( "Shoot" ) )
                {    
                    return new FireWeaponJob{

                        weaponEntities = weapons,
                        CommandBuffer = new EntityCommandBuffer.Concurrent()

                    }.Schedule( weapons.Length, 64, inputDeps );
                }

                return inputDeps;
            }
        }
    }
}