using System;
using System.Linq;

namespace ECS
{
    public class Entity
    {
        private ECSRegister _handle;
        
        private uint _id;
        private string _name;

        private int _lastComponentID;
        private IComponent[] _components;

        public uint ID => _id;
        public string Name
        {
            get => _name;
            internal set => _name = value;
        }

        public Entity(ECSRegister handle, uint id, string name)
        {
            _handle = handle;
            _id = id;
            _name = name;
            _components = new IComponent[1024];
        }
        
        public ref IComponent AddComponent<T>() where T : IComponent, new()
        {
            if (HasComponent<T>()) throw new Exception($"{this} already has that component");
            _components[_lastComponentID] = new T();
            return ref _components[_lastComponentID++];
        }

        public bool HasComponent<T>() where T : IComponent => _components.OfType<T>().Any();

        public ref IComponent GetComponent<T>() where T : IComponent
        {        
            for (var i = 0; i < _components.Length; i++)
                if (_components[i] is T) return ref _components[i];
            throw new Exception("Entity does not contains component");
        }
    }
}