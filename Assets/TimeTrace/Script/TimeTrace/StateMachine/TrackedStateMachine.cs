﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

namespace TimeTrace.StateMachine
{
    public abstract class TrackedStateMachine<TTrigger, TState> : TraceBehaviour
        where TState : struct, IConvertible, IComparable
        where TTrigger : struct, IConvertible, IComparable
    {
        #region Interface
        
        /// <summary>
        /// Fire a trigger to invoke state transitions.
        /// Will record state change events for tracing.
        /// </summary>
        public bool Fire(TTrigger trigger)
        {
            TState state = stateMachine.State;
            bool val = transitionManager.Fire(trigger);
            if (val)
            {
                LocalEventTracer.AddTraceEvent(new StateChangeEvent<TTrigger, TState>(
                        this, state, stateMachine.State, StateTimer
                    ));
            }
            
            return val;
        }

        /// <summary>
        /// Time since current state started.
        /// This is needed to backtrack stateMachine.
        /// </summary>
        public float StateTimer { get; set; }

        /// <summary>
        /// Set stateMachine to specified state and timer
        /// Will not invoke state callbacks.
        /// </summary>
        public void SetStateTo(TState state, float timer)
        {
            stateMachine.ChangeState(state, StateTransition.Overwrite);
            StateTimer = timer;
        }

        /// <summary>
        /// Init your statemachine transitions here
        /// </summary>
        protected abstract void InitTransitions();

        /// <summary>
        /// Can one state transition to itself?
        /// </summary>
        protected abstract bool EnableTransitionToSelf { get; }

        protected void SetStateMachinePaused(bool val)
        {
            if (val)
                stateMachine.Pause();
            else
                stateMachine.Resume();
        }

        protected IEnumerator WaitForTimer(float targetTimer)
        {
            while(StateTimer < targetTimer)
            {
                yield return null;
            }
        }
        
        #endregion

        #region Implementation
        protected StateMachine<TState> stateMachine;
        protected TransitionManager<TTrigger, TState> transitionManager;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            stateMachine = StateMachine<TState>.Initialize(this, default(TState), EnableTransitionToSelf);
            transitionManager = new TransitionManager<TTrigger, TState>(stateMachine);
            transitionManager.BeforeTriggered += BeforeTransitionTriggered;
            LocalEventTracer.invokeEventWhenAdd = false;

            InitTransitions();
        }

        public override void RevertableUpdate(float deltaTime)
        {
            // Update the state timer
            StateTimer += deltaTime;

            // Pause the statemachine if tracing
            SetStateMachinePaused(TimeTraceManager.tracing);
        }

        // StateTimer is reset before a transition happens
        // we must do this before transition, or may cause error during OnEnter in the new state, if StateTimer is used
        private void BeforeTransitionTriggered(Transition<TTrigger, TState> transition)
        {
            StateTimer = 0;
        }
        #endregion
    }

    public class StateChangeEvent<TTrigger, TState> : TracedEvent
        where TState : struct, IConvertible, IComparable
        where TTrigger : struct, IConvertible, IComparable
    {
        public TrackedStateMachine<TTrigger, TState> stateMachine;
        public TState toState;
        public TState fromState;
        public float fromStateTimer;

        

        public StateChangeEvent(TrackedStateMachine<TTrigger, TState> stateMachine, 
            TState fromState, TState toState, float fromStateTimer)
        {
            this.stateMachine = stateMachine;
            this.fromState = fromState;
            this.toState = toState;
            this.fromStateTimer = fromStateTimer;
        }

        public override string EventName
        {
            get
            {
                return "State change from: " + fromState.ToString() + " to: " + toState.ToString();
            }
        }

        public override void Do(float deltaTime)
        {
            stateMachine.SetStateTo(toState, 0);
        }

        public override void Undo(float deltaTime)
        {
            stateMachine.SetStateTo(fromState, fromStateTimer);
        }


    }
}