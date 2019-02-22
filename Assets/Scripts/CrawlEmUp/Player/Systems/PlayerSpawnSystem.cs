namespace CrawlEmUp.Player
{
    using UnityEngine;
    using Unity.Collections;
    using Unity.Entities;
    using Unity.Transforms;

    public class PlayerSpawnSystem : ComponentSystem
    {
        private ComponentGroup playerSpawnGroup;
        private ComponentGroup playerGroup;

        protected override void OnCreateManager()
        {
            playerSpawnGroup = GetComponentGroup( typeof( PlayerSpawn ), typeof( Position ) );
            playerGroup = GetComponentGroup( typeof( Player ) );
        }

        protected override void OnUpdate()
        {
            using( NativeArray<Entity> playerSpawns = playerSpawnGroup.ToEntityArray( Allocator.TempJob ) )
            {
                int playerCount = 0;
                foreach( Entity playerSpawn in playerSpawns )
                {
                    GameObject playerPrefab = EntityManager.GetSharedComponentData<PlayerSpawn>( playerSpawn ).prefab;

                    Entity playerEntity = EntityManager.Instantiate( playerPrefab );
                    Player player = EntityManager.GetComponentData<Player>( playerEntity );
                    Position playerPosition = EntityManager.GetComponentData<Position>( playerSpawn );

                    player.Id = playerCount;

                    EntityManager.SetComponentData( playerEntity, player );
                    EntityManager.SetComponentData( playerEntity, playerPosition );
                    
                    EntityManager.DestroyEntity( playerSpawn );

                    playerCount++;
                }
            }
        }
    }
}