using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace
{
    /// <summary>
    /// Event which describes animation state change
    /// </summary>
    public class AnimChangeEvent : TracedEvent
    {
        public AnimationTracer animTracer;
        public int fromHash;
        public int toHash;
        public float fromNormalizedTime;

        public AnimChangeEvent(AnimationTracer animTracer, 
            int fromHash, int toHash, float fromNormalizedTime)
        {
            this.animTracer = animTracer;
            this.fromHash = fromHash;
            this.toHash = toHash;
            this.fromNormalizedTime = fromNormalizedTime;
        }

        public override string EventName
        {
            get
            {
                return "Anim change from: " + fromHash + " to: " + toHash;
            }
        }

        public override void Do(float deltaTime)
        {
            animTracer.at.Play(toHash);
        }

        public override void Undo(float deltaTime)
        {
            animTracer.at.Play(fromHash, 0, fromNormalizedTime);
        }
    }

    [RequireComponent(typeof(Animator))]
    public class AnimationTracer : TraceBehaviour
    {
        public Animator at { get; set; }
        void Awake()
        {
            at = GetComponent<Animator>();
        }

        public override void RevertableUpdate(float deltaTime)
        {
            at.SetFloat("Speed", TimeTraceManager.timeScale);
        }

        public void Play(string state)
        {           
            var stateInfo = at.GetCurrentAnimatorStateInfo(0);
            int from = stateInfo.shortNameHash;
            int to = Animator.StringToHash(state);

            if (!at.HasState(0, to))
            {
                Debug.LogWarning("Playing a missing animation state: " + state);
            }

            TimeTraceManager.AddTraceEvent(new AnimChangeEvent(this, from, to, stateInfo.normalizedTime));
        }
        
    }
}