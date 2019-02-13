namespace CrawlEmUp.Player
{
    using UnityEngine;
    using Unity.Entities;

    [ System.Serializable ]
    public struct Player : IComponentData
    {
        public int Id;
    }

    public class PlayerComponent : ComponentDataWrapper<Player>
    {
        // Wrapper class
    }

}