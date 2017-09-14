using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace {
    /// <summary>
    /// The main manager for TimeTrace functionality.
    /// Maintains a custom time and frame, and offers a time backtracking method.
    /// </summary>
    public class TimeTraceManager : MonoBehaviour {
        public static TimeTraceManager Instance {
            get {
                if (m_instance == null) {
                    Create();
                }
                return m_instance;
            }
        }
        
        public static float time { get { return Instance.m_time; } }
        public static float timeScale { get { return Instance.m_timeScale; } }
        public static float deltaTime { get { return Instance.m_timeScale * Time.deltaTime; } }
        public static bool tracing { get { return Instance.m_tracing; } }
        public static bool backTracing { get { return Instance.m_backTracing; } }
        public static int frameCount { get { return Mathf.FloorToInt(Instance.m_frame); } }
        public static int deltaFrame { get { return frameCount - Instance.m_previousFrameCount; } }
        public static int maxRecordFrame { get { return 600; } }
        public static EventTracer eventTracer { get { return Instance.m_eventTracer; } }

        /// <summary>
        /// Start a new trace, by timeScale
        /// </summary>
        /// <param name="timeScale"></param>
        public static void StartTrace(float timeScale) {
            Instance.StartTraceByTimeScale(timeScale);
        }

        /// <summary>
        /// Change the timeScale during a trace
        /// </summary>
        /// <param name="timeScale"></param>
        public static void ChangeTrace(float timeScale) {
            Instance.ChangeTraceTimeScale(timeScale);
        }

        /// <summary>
        /// End the current trace
        /// </summary>
        public static void StopTrace() {
            Instance.StopTraceByTimeScale();
        }
        
        /// <summary>
        /// Add a new trace event
        /// If not tracing, invoke this event immediately
        /// </summary>
        /// <param name="e"></param>
        public static void AddTraceEvent(TracedEvent e) {
            eventTracer.AddTraceEvent(e);
        }

        private static TimeTraceManager m_instance;
        public static TimeTraceManager Create()
        {
            var go = new GameObject("TimeTraceManager");
            m_instance = go.AddComponent<TimeTraceManager>();
            return m_instance;
        }

        public float m_time = 0;
        public float m_timeScale = 1;
        public float m_startTimeOfTracing = 0;
        public bool m_tracing = false;
        public float m_frame = 0;
        public int m_previousFrameCount = 0;
        public EventTracer m_eventTracer;
        public bool m_backTracing { get { return m_tracing && m_timeScale < 0; } }
        
        public void StartTraceByTimeScale(float timeScale) {
            m_startTimeOfTracing = time;
            m_timeScale = timeScale;
            m_tracing = true;
        }

        public void ChangeTraceTimeScale(float timeScale) {
            m_timeScale = timeScale;
        }

        public void StopTraceByTimeScale() {
            m_timeScale = 1;
            m_tracing = false;

            m_time = Mathf.Clamp(m_time, 0, m_startTimeOfTracing);
        }

        private void Awake()
        {
            if(m_eventTracer == null)
                m_eventTracer = gameObject.AddComponent<EventTracer>();
        }

        private void Update()
        {
            m_previousFrameCount = frameCount;
            m_time = Mathf.Max(0, m_time + deltaTime);
            m_frame = Mathf.Max(0, m_frame + timeScale);
            
            if (tracing)
            {
                // Stops backtracking when time goes to zero
                // stops fronttracking when time goes to start of tracing
                if (time <= 0 && timeScale < 0)
                    m_timeScale = 0;
                if (time >= m_startTimeOfTracing && timeScale > 0)
                    m_timeScale = 0;
            }           
        }
    }
}