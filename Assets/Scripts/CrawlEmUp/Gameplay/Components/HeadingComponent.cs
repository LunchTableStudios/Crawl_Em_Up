namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;
    using Unity.Mathematics;

    [ System.Serializable ]
    public struct Heading : IComponentData
    {
        [ UnityEngine.SerializeField ] public float2 Value;
    }

    public class HeadingComponent : ComponentDataWrapper<Heading>
    {
        // Wrapper class
    }
}