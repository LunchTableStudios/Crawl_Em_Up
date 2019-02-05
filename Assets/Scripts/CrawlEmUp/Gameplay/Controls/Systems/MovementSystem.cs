namespace CrawlEmUp.Gameplay.Controls
{
    using Unity.Entities;

    using LTS_ToolKit.Controls;
    using LTS_ToolKit.CharacterController;

    public class MovementSystem : ComponentSystem
    {
        private struct MovementEntityFilter
        {
            public readonly MovementSpeed SpeedComponent;
            public readonly AxisInput InputComponent;
            public Velocity VelocityComponent;
        }

        protected override void OnUpdate()
        {
            foreach( MovementEntityFilter entity in GetEntities<MovementEntityFilter>() )
            {
                MovementSpeed speed = entity.SpeedComponent;
                AxisInput input = entity.InputComponent;
                Velocity velocity = entity.VelocityComponent;

                UnityEngine.Vector2 movementInput = new UnityEngine.Vector2( input.Values[ "HorizontalMovement" ], input.Values[ "VerticalMovement" ] ).normalized;

                if( movementInput.x != 0 )
                    velocity.Value.x = movementInput.x * speed.Value;

                if( movementInput.y != 0 )
                    velocity.Value.y = movementInput.y * speed.Value;
            }
        }
    }
}