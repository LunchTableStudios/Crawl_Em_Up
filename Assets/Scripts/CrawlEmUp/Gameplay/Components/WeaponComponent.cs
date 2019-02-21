namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;

    [ System.Serializable ]
    public struct Weapon : IComponentData
    {
        public float FireRate;
    }

    public class WeaponComponent : ComponentDataWrapper<Weapon>
    {
        // Wrapper class
    }
}