namespace CrawlEmUp.Player
{
    using UnityEngine;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Transforms;

    public class PlayerSpawnSystem : ComponentSystem
    {
        private ComponentGroup playerSpawnGroup;

        protected override void OnCreateManager()
        {
            playerSpawnGroup = GetComponentGroup( typeof( PlayerSpawn ), typeof( Position ) );
        }

        protected override void OnUpdate()
        {
            using ( NativeArray<Entity> playerSpawns = playerSpawnGroup.ToEntityArray( Allocator.TempJob ) )
            {
                foreach( Entity playerSpawn in playerSpawns )
                {
                    GameObject playerPrefab = EntityManager.GetSharedComponentData<PlayerSpawn>( playerSpawn ).prefab;
                    Entity playerEntity = EntityManager.Instantiate( playerPrefab );
                    Position playerPosition = EntityManager.GetComponentData<Position>( playerSpawn );

                    EntityManager.SetComponentData( playerEntity, playerPosition );

                    EntityManager.DestroyEntity( playerSpawn );
                }
            }
        }
    }
}