namespace CrawlEmUp.Gameplay
{
    using UnityEngine;
    using Unity.Entities;
    using Unity.Mathematics;

    [ System.Serializable ]
    public struct Movement : IComponentData
    {
        public float2 Value;
    }

    public class MovementComponent : ComponentDataWrapper<Movement>
    {
        // Wrapper class
    }
}