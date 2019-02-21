namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct MovementSpeed : IComponentData
    {
        [ UnityEngine.SerializeField ] public float Value;
    }

    public class MovementSpeedComponent : ComponentDataWrapper<MovementSpeed>
    {
        // Wrapper class
    }
}