using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

namespace TimeTrace.Test
{
    
    public class TestStateMachinePause : MonoBehaviour
    {
        [Header("Config")]
        public StateTransition mode;
        public float stateTransitionDuration = 10;

        [Header("Result")]
        public float EnterTimer_A;
        public float EndTimer_A;
        public float UpdateTimer_A;
        public float UpdateTimerC;

        public enum TestState { A, B, C, D }
        public enum TestTrigger { AtoB, AtoC, BtoC, CtoA }
        public StateMachine<TestState> fsm;
        public TransitionManager<TestTrigger, TestState> trm;

        private void Start()
        {
            fsm = StateMachine<TestState>.Initialize(this, TestState.A, true);
            trm = new TransitionManager<TestTrigger, TestState>(fsm);

            trm.Configure(TestState.A).
                Permit(TestTrigger.AtoB, TestState.B, mode).
                Permit(TestTrigger.AtoC, TestState.C, mode);

            trm.Configure(TestState.B).
                Permit(TestTrigger.BtoC, TestState.C);

            trm.Configure(TestState.C).
                Permit(TestTrigger.CtoA, TestState.A);


            StateMachineDebugger.Create(gameObject, fsm, trm);
        }

        [ContextMenu("Pause")]
        public void Pause()
        {
            fsm.Pause();
        }

        [ContextMenu("Resume")]
        public void Resume()
        {
            fsm.Resume();
        }

        IEnumerator A_Enter()
        {
            EnterTimer_A = 0;
            UpdateTimer_A = 0;
            EndTimer_A = 0;

            while (EnterTimer_A < stateTransitionDuration)
            {
                EnterTimer_A += Time.deltaTime;
                yield return null;
            }
        }

        void A_Update()
        {
            UpdateTimer_A += Time.deltaTime;
        }

        IEnumerator A_Exit()
        {
            while (EndTimer_A < stateTransitionDuration)
            {
                EndTimer_A += Time.deltaTime;
                yield return null;
            }
        }

        void C_Enter()
        {
            UpdateTimerC = 0;
        }

        void C_Update()
        {
            UpdateTimerC += Time.deltaTime;
        }
    }
}