using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace {
    [System.Serializable]
    public class TraceEvent : TimeTraceData {
        /// <summary>
        /// name of this event
        /// </summary>
        public string name;

        public TraceEvent() : base(TimeTraceManager.time, TimeTraceManager.frameCount)
        {

        }
        
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
        
        /// <summary>
        /// Init event time with current time
        /// Mark the name with EventName
        /// </summary>
        public void Init() {
            name = EventName;
            time = TimeTraceManager.time;
            frame = TimeTraceManager.frameCount;
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