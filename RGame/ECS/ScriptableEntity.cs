using ECS;

namespace RGame.ECS
{
    public abstract class ScriptableEntity
    {
        protected Entity Entity;

        protected ScriptableEntity(Entity entity)
        {
            Entity = entity;
        }

        public abstract void OnInit();
        public abstract void OnUpdate(float dt);
        public abstract void OnDraw();
        public abstract void OnDestroy();
    }
}
