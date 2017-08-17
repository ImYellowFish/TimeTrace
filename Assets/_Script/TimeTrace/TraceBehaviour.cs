using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace {
    public abstract class TraceBehaviour : MonoBehaviour {
        public abstract void TraceUpdate(float deltaTime);


        public virtual void Init() {

        }

        public virtual void End() {

        }

        
        
        protected void Update() {
            TraceUpdate(TimeTraceManager.deltaTime);
        }
    }
}