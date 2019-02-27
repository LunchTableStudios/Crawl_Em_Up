namespace CrawlEmUp.Gameplay
{
    using Unity.Entities;

    public struct Projectile : IComponentData
    {
        public float StartTime;
        public float Duration;
    }
}