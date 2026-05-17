using System;
using System.Collections.Generic;
using Code.Entities;
using UnityEngine;

namespace Code.FSM
{
    public class EntityStateMachine
    {
        private string _currentStateName;
        public string CurrentStateName => _currentStateName;
        
        public EntityState CurrentState { get; private set; }

        private Dictionary<string, EntityState> _states;

        public EntityStateMachine(Entity entity, StateDataSO[] stateList)
        {
            _states = new Dictionary<string, EntityState>();
            foreach (StateDataSO state in stateList)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null : {state.className}");
                EntityState entityState = Activator.CreateInstance(type, entity, state.animationHash)
                    as EntityState;
                
                _states.Add(state.stateName, entityState);
            }
        }

        public void ChangeState(string newStateName, bool forced = false)
        {
            EntityState newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != null, $"State is null {newStateName}");
    
            if (!forced && CurrentState == newState)
                return;

            CurrentState?.Exit();
            CurrentState = newState;
            _currentStateName = newStateName;
            CurrentState.Enter();
        }

        public void UpdateStateMachine()
        {
            CurrentState?.Update();
        }

    }
}