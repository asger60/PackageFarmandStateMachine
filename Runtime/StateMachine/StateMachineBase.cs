using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Farmand.StateMachine
{
    public class StateMachineBase : MonoBehaviour
    {
        public List<StateBase> _states = new List<StateBase>();
        private StateBase _currentState;
        private StateBase _prevState;
        public StateBase PreviousState => _prevState;
        public float StateTime => _stateTime;
        private float _stateTime;
        public Action<StateBase> onStateEnter;

        public virtual void Awake()
        {
            foreach (var stateBase in GetComponentsInChildren<StateBase>(true))
            {
                AddState(stateBase);
                stateBase.Init(this);
                stateBase.gameObject.SetActive(false);
            }
        }

        private void AddState(StateBase state)
        {
            print("#StateMachine#adding state: " + state);
            _states.Add(state);
        }

        public void ChangeState<T>() where T : StateBase
        {
            print("#StateMachine#changing state to: " + typeof(T) +  " " + _states.Count);
            foreach (var state in _states)
            {
                print("#StateMachine#testing: " + state);
                if (state is T)
                {
                    ChangeState(state);
                    break;
                }
            }
        }

        protected virtual void ChangeState(StateBase state)
        {
            if (state == _currentState)
            {
                print("#StateMachineBase#state change ignored, already in that state");
                return;
            }
            _prevState = _currentState;
            _currentState = state;
            
            if (_prevState != null)
            {
                _prevState.OnExitState();
                _prevState.gameObject.SetActive(false);
            }

            _currentState.gameObject.SetActive(true);
            _currentState.OnEnterState();
            onStateEnter?.Invoke(_currentState);
            _stateTime = 0;

            print("#StateMachineBase#change state " + state.GetType());
        }

        public virtual void Update()
        {
            if (_currentState == null) return;
            _stateTime += Time.unscaledDeltaTime;
            _currentState.OnStateUpdate();
        }


        public StateBase GetCurrentState()
        {
            return _currentState;
        }

        public StateBase GetStateOfType<T>() where T : StateBase
        {
            return _states.OfType<T>().Select(stateBase => (stateBase as T)).FirstOrDefault();
        }
    }
}