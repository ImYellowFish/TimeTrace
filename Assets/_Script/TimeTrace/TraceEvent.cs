using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace {
    [System.Serializable]
    public class TraceEvent {
        /// <summary>
        /// name of this event
        /// </summary>
        public string name;

        /// <summary>
        /// Time that this event happens
        /// </summary>
        public float time;
        
        /// <summary>
        /// Trigger if timeScale > 0
        /// Undo if timeScale < 0
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Trace(float deltaTime) {
            if(deltaTime > 0) {
                Do(deltaTime);
            }else {
                Undo(deltaTime);
            }
        }
        
        public void Init() {
            name = EventName;
            time = TimeTraceManager.time;
        }

        /// <summary>
        /// let this event take place
        /// </summary>
        public virtual void Do(float deltaTime) {
        }

        /// <summary>
        /// undo this event
        /// </summary>
        public virtual void Undo(float deltaTime) {
        }


        public virtual string EventName {
            get { return "Undefined"; }
        }
        

    }
}