﻿
using System;
using UnityEngine;

namespace Farmand.StateMachine
{
    public abstract class StateBase : MonoBehaviour
    {
        private StateMachineBase _stateMachineBase;

        public float StateTime => _stateMachineBase.StateTime;
        public void Init(StateMachineBase stateMachineBase)
        {
            _stateMachineBase = stateMachineBase;
            OnAdd();
        }
        public virtual void OnAdd()
        {
        }



        public abstract void OnExitState();
        public abstract void OnEnterState();
        public abstract void OnStateUpdate();
    }
}
