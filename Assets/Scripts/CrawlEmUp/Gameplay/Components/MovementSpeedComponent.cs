namespace CrawlEmUp.Gameplay
{
    using UnityEngine;
    using Unity.Entities;

    [ System.Serializable ]
    public struct MovementSpeed : IComponentData
    {
        public float Value;
    }

    public class MovementSpeedComponent : ComponentDataWrapper<MovementSpeed>
    {
        // Wrapper class
    }
}