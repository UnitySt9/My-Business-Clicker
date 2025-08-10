using Code.Gameplay.Save.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Code.Gameplay.Save.Systems
{
    public class CreateSaveRequestOnFocusChangedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsPool<SaveRequestComponent> _saveRequestPool;
        private bool _previousFocusState;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _saveRequestPool = _world.GetPool<SaveRequestComponent>();
            _previousFocusState = Application.isFocused;
        }

        public void Run(IEcsSystems systems)
        {
            bool currentFocus = Application.isFocused;

            if (_previousFocusState && !currentFocus) 
                SendSaveRequest();

            _previousFocusState = currentFocus;
        }

        private void SendSaveRequest()
        {
            int entity = _world.NewEntity();
            _saveRequestPool.Add(entity);
        }
    }
}