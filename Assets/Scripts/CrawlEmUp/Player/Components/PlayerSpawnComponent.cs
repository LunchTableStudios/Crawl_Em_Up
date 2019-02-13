namespace CrawlEmUp.Player
{
    using UnityEngine;
    using Unity.Entities;

    [ System.Serializable ]
    public struct PlayerSpawn : ISharedComponentData
    {
        public GameObject prefab;
    }

    public class PlayerSpawnComponent : SharedComponentDataWrapper<PlayerSpawn>
    {
        // Wrapper class
    }

}