using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeTrace.Utility;

namespace TimeTrace {
    public abstract class TraceBehaviour : MonoBehaviour, IFrameRecordable{
        /// <summary>
        /// This trace update will be called during every update
        /// When backtracing, this trace update will be called too
        /// with a nagative delta time.
        /// 
        /// The implementation should be applicable for both front tracing and back tracing
        /// </summary>
        /// <param name="deltaTime"></param>
        public abstract void RevertableUpdate(float deltaTime);

        /// <summary>
        /// Get the current frame data for synchronization
        /// Called when not tracing;
        /// </summary>
        /// <returns></returns>
        public virtual FrameData GetFrameData(float time, int frame)
        {
            return FrameData.Null;
        }

        /// <summary>
        /// Load from the given frame data
        /// Called during time tracing
        /// </summary>
        /// <param name="data"></param>
        public virtual void LoadFrameData(FrameData data)
        {

        }



        
        protected void Update() {
            RevertableUpdate(TimeTraceManager.deltaTime);
            DataTracer.UpdateLoadSaveFrameData(this);
        }


        public FrameDataTracer dataTracer = new FrameDataTracer();
        public FrameDataTracer DataTracer { get { return dataTracer; } }      
        public virtual bool AutoSaveFrameData { get { return true; } }
    }
}