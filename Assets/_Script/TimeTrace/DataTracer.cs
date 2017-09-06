using UnityEngine;
using System.Collections.Generic;
using TimeTrace.Utility;

namespace TimeTrace
{
    public interface IFrameRecordable
    {
        /// <summary>
        /// Whether frame data should be recorded every non-tracing Update()
        /// </summary>
        bool AutoSaveFrameData { get; }

        /// <summary>
        /// FrameDataTracer to use
        /// </summary>
        FrameDataTracer DataTracer { get; }

        /// <summary>
        /// Load from a given frame trace data
        /// </summary>
        /// <param name="data"></param>
        void LoadFrameData(TimeTraceData data);

        /// <summary>
        /// Get the frame trace data to save for current Update
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        TimeTraceData GetFrameData(float time, int frame);
    }

    /// <summary>
    /// Auto record and load data frames for IRecordable
    /// </summary>
    [System.Serializable]
    public class FrameDataTracer
    {
        public DataTimeline dataTimeline = new DataTimeline(TimeTraceManager.maxRecordFrame);
        protected bool isPreviousFrameTracing = false;
        private bool isPreviousFrameBackTracing = false;
        private int lastLoadedFrameIndex;
        private float previousUpdateTime;
        private int previousLoadedFrame;
        
        private List<TimeTraceData> framesToLoad = new List<TimeTraceData>();

        public void AddData(TimeTraceData data)
        {
            dataTimeline.Add(data);
            if (debug)
                debugger.RecordSave(data);
        }

        public void UpdateLoadSaveFrameData(IFrameRecordable tb)
        {
            float time = TimeTraceManager.time;
            float deltaTime = TimeTraceManager.deltaFrame;
            int frame = TimeTraceManager.frameCount;
            int deltaFrame = TimeTraceManager.deltaFrame;
            bool tracing = TimeTraceManager.tracing;
            bool backTracing = TimeTraceManager.backTracing;

            if (tracing)
            {
                isPreviousFrameTracing = true;
                LoadFrame(tb, time, frame, deltaFrame, backTracing);
            }
            else
            {
                if (isPreviousFrameTracing)
                {
                    DiscardLoadedFrames();
                    isPreviousFrameTracing = false;
                }
                if (tb.AutoSaveFrameData)
                {
                    SaveFrame(tb, time, frame);
                }                
            }
            isPreviousFrameBackTracing = backTracing;
            previousUpdateTime = time;
        }

        private DataTracerDebugger _debugger = new DataTracerDebugger();
        private DataTracerDebugger debugger
        {
            get
            {
                if (_debugger == null) _debugger = new DataTracerDebugger();
                return _debugger;
            }
        }
        public bool debug = false;

        protected void SaveFrame(IFrameRecordable tb, float time, int frame)
        {
            var data = tb.GetFrameData(time, frame);
            dataTimeline.Add(data);

            if(debug)
                debugger.RecordSave(data);
        }

        protected void LoadFrame(IFrameRecordable tb, float time, int frame, int deltaFrame, bool backTracing)
        {
            int index;
            // if this frame is loaded during last Update(), and the tracing direction didn't change
            // do not load this frame again
            if (frame == previousLoadedFrame && backTracing == isPreviousFrameBackTracing)
                return;

            if (dataTimeline.SearchFrame(frame, frame + deltaFrame, backTracing, out index, ref framesToLoad))
            {
                for (int i = 0; i < framesToLoad.Count; i++)
                {
                    var data = framesToLoad[i];
                    tb.LoadFrameData(data);
                    lastLoadedFrameIndex = index;
                    if (debug)
                        debugger.RecordLoad(data, previousUpdateTime, time);
                }
                previousLoadedFrame = frame;
            }

        }

        protected void DiscardLoadedFrames()
        {
            dataTimeline.RemoveFromIndexToEnd(lastLoadedFrameIndex);
            previousLoadedFrame = -1;
        }

    }
}