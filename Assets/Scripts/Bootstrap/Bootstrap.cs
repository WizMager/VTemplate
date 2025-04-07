using System.Collections.Generic;
using System.Linq;
using Bootstrap.Interfaces;
using UnityEngine;
using VContainer.Unity;

namespace Bootstrap
{
    public class Bootstrap : IInitializable, IStartable, ITickable, IFixedTickable, ILateTickable
    {
        private readonly List<IInitialize> _initializes = new ();
        private readonly List<IStart> _starts = new ();
        private readonly List<IUpdate> _updates = new();
        private readonly List<IFixedUpdate> _fixedUpdate = new();
        private readonly List<ILateUpdate> _lateUpdate = new();

        public Bootstrap(IEnumerable<IController> controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller is IInitialize initialize)
                {
                    _initializes.Add(initialize);
                }
                
                if (controller is IStart start)
                {
                    _starts.Add(start);
                }
                
                if (controller is IUpdate update)
                {
                    _updates.Add(update);
                }
                
                if (controller is IFixedUpdate fixedUpdate)
                {
                    _fixedUpdate.Add(fixedUpdate);
                }
                
                if (controller is ILateUpdate lateUpdatable)
                {
                    _lateUpdate.Add(lateUpdatable);
                }
            }
        }
        
        public void Initialize()
        {
            
        }

        public void Start()
        {
            
        }

        public void Tick()
        {
            foreach (var update in _updates)
            {
                update.Update();
            }
        }

        public void FixedTick()
        {
            
        }

        public void LateTick()
        {
            
        }
    }
}