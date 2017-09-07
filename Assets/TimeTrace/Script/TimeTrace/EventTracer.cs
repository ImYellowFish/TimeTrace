using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace
{
    /// <summary>
    /// A component which stores and manages traceEvent. 
    /// Saved events will be automatically invoked or restored during time tracing.
    /// </summary>
    public class EventTracer : MonoBehaviour, IFrameRecordable
    {
        /// <summary>
        /// Whether the event should be invoked when added.
        /// </summary>
        public bool invokeEventWhenAdd = true;

        /// <summary>
        /// Add a new trace event
        /// If not tracing, invoke this event immediately
        /// </summary>
        /// <param name="e"></param>
        public void AddTraceEvent(TracedEvent e)
        {
            e.Init();
            if(invokeEventWhenAdd)
                e.Trace(TimeTraceManager.deltaTime);
            //Debug.Log("add event: " + e.name + ", t: " + e.time + ", ct: " + time);
            DataTracer.AddFrameData(e);
        }

        private void Update()
        {
            DataTracer.UpdateLoadSaveFrameData(this);
        }

        #region IFrameRecordable
        public FrameDataTracer eventTracerImpl = new FrameDataTracer();
        public FrameDataTracer DataTracer { get { return eventTracerImpl; } }
        public bool AutoSaveFrameData { get { return false; } }
        public void LoadFrameData(FrameData data)
        {
            var _event = data as TracedEvent;
            //Debug.Log("load event: " + _event.name + ", t: " + _event.time + ", ct: " + time);
            _event.Trace(TimeTraceManager.deltaTime);
        }

        public FrameData GetFrameData(float time, int frame)
        {
            return FrameData.Null;
        }
        #endregion
    }
}
