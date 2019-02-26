namespace CrawlEmUp.Bootstraps
{
    using UnityEngine;
    using Unity.Rendering;

    public class GameplayBootstrap : MonoBehaviour
    {
        public static RenderMesh ProjectileMeshComponent;
        [ SerializeField ] private RenderMesh projectileMeshComponent;

        void Awake()
        {
            ProjectileMeshComponent = projectileMeshComponent;
        }
    }
}