using Raylib_cs;
using static Raylib_cs.Raylib;

namespace ECS
{
    public class ECSRegister
    {
        private uint _lastID; 
        
        private readonly Entity[] _entities;

        public ECSRegister()
        {
            _lastID = 0;
            _entities = new Entity[1025];
        }

        public ref Entity CreateEntity(string name)
        {
            var entity = new Entity(this, _lastID, name);
            entity.AddComponent<TransformComponent>();
            _entities[(int) _lastID] = entity;
            return ref _entities[(int)_lastID++];
        }

        public void DeleteEntity(Entity entity)
        {
            for (var i = 0; i < _entities.Length; i++)
            {

            }
        }

        public void UpdateEntities(float dt)
        {
            foreach (var entity in _entities)
            {
                if (entity == null) continue;
                if (!entity.HasComponent<ScriptComponent>()) continue;
                if (entity.GetComponent<ScriptComponent>() is not ScriptComponent scriptComponent) continue;
                scriptComponent.Script?.OnUpdate(dt);
            }
        }
        public void DrawEntities()
        {
            foreach (var entity in _entities)
            {
                if (entity == null) continue;
                if (!entity.HasComponent<ScriptComponent>()) continue;
                if (entity.GetComponent<ScriptComponent>() is not ScriptComponent scriptComponent) continue;
                scriptComponent.Script?.OnDraw();
            }
        }
    }
}