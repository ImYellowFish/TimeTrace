using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using TimeTrace.StateMachine;
using System;

namespace TimeTrace.Test
{
    public enum TestTraceState { A, B, C, D }
    public enum TestTraceTrigger { AtoA, AtoB, AtoC, BtoC, CtoA }
    
    public class TestStateMachineTrace : TrackedStateMachine<TestTraceTrigger, TestTraceState>
    {

        [Header("Config")]
        public StateTransition mode;
        public float stateTransitionDuration = 10;
        public TestTraceTrigger testTrigger;

        [Header("Result")]
        public float EnterTimer_A;
        public float EndTimer_A;
        public float UpdateTimer_A;
        public float UpdateTimerC;
        
        protected override void InitTransitions()
        {
            transitionManager.Configure(TestTraceState.A).
                Permit(TestTraceTrigger.AtoA, TestTraceState.A, mode).
                Permit(TestTraceTrigger.AtoB, TestTraceState.B, mode).
                Permit(TestTraceTrigger.AtoC, TestTraceState.C, mode);

            transitionManager.Configure(TestTraceState.B).
                Permit(TestTraceTrigger.BtoC, TestTraceState.C);

            transitionManager.Configure(TestTraceState.C).
                Permit(TestTraceTrigger.CtoA, TestTraceState.A);

            StateMachineDebugger.Create(gameObject, stateMachine, transitionManager);
        }

        protected override bool EnableTransitionToSelf
        {
            get
            {
                return true;
            }
        }

        public override void RevertableUpdate(float deltaTime) {
            base.RevertableUpdate(deltaTime);
            if (Input.GetKeyDown(KeyCode.R)) {
                FireTestTrigger();
            }
        }

        
        [ContextMenu("FireTestTrigger")]
        public void FireTestTrigger()
        {
            Fire(testTrigger);
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            stateMachine.Pause();
        }

        [ContextMenu("Resume")]
        public void Resume()
        {
            stateMachine.Resume();
        }

        IEnumerator A_Enter()
        {
            EnterTimer_A = 0;
            UpdateTimer_A = 0;
            EndTimer_A = 0;

            while (EnterTimer_A < stateTransitionDuration)
            {
                EnterTimer_A = StateTimer;
                yield return null;
            }
        }

        void A_Update()
        {
            UpdateTimer_A = StateTimer - EnterTimer_A;
        }

        IEnumerator A_Exit()
        {
            while (EndTimer_A < stateTransitionDuration)
            {
                EndTimer_A = StateTimer - EnterTimer_A - UpdateTimer_A;
                yield return null;
            }
        }

        void C_Enter()
        {
            UpdateTimerC = 0;
        }

        void C_Update()
        {
            UpdateTimerC = StateTimer;
        }
    }
}