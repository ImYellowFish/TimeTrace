using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace {
    public class TimeTraceManager : MonoBehaviour {
        public static TimeTraceManager Instance {
            get {
                if (m_instance == null) {
                    m_instance = Create();
                }
                return m_instance;
            }
        }

        public static EventTracer eventTracer {
            get { return Instance._eventTracer; }
        }


        public float fixedDeltaTime = 0.02f;
        public static float time { get { return Instance.m_time; } }
        public static float timeScale { get { return Instance.m_timeScale; } }
        //public static float deltaTime { get { return Instance.m_timeScale * Time.deltaTime; } }
        public static float deltaTime { get { return Instance.m_timeScale * Instance.fixedDeltaTime; } }
        public static bool tracing { get { return Instance.m_tracing; } }
        
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
        /// </summary>
        /// <param name="e"></param>
        public static void AddTraceEvent(TraceEvent e) {
            e.Init();
            e.Trace(deltaTime);
            eventTracer.Add(e);
        }

        public float m_time = 0;
        public float m_timeScale = 1;
        public float m_startTimeOfTracing = 0;
        public bool m_tracing = false;
        public EventTracer m_eventTracer;

        


        private static TimeTraceManager m_instance;
        private static TimeTraceManager Create() {
            var go = new GameObject("TimeTraceManager");
            return go.AddComponent<TimeTraceManager>();
        }


        public void StartTraceByTimeScale(float timeScale) {
            m_startTimeOfTracing = time;
            m_timeScale = timeScale;
            eventTracer.SetTraceTime(time);
            m_tracing = true;
        }

        public void ChangeTraceTimeScale(float timeScale) {
            m_timeScale = timeScale;
        }

        public void StopTraceByTimeScale() {
            m_timeScale = 1;
            eventTracer.DiscardFutureEvents();
            m_tracing = false;

            m_time = Mathf.Clamp(m_time, 0, m_startTimeOfTracing);
        }
        
        public EventTracer _eventTracer {
            get {
                if (m_eventTracer == null)
                    m_eventTracer = gameObject.AddComponent<EventTracer>();
                return m_eventTracer;
            }
        }


        void Awake() {
            //if (m_instance != this)
            //    m_instance = this;
        }


        void Update() {
            m_time += deltaTime;

            if (tracing) {
                // Stops backtracking when time goes to zero
                // stops fronttracking when time goes to start of tracing
                if (time <= 0 && timeScale < 0)
                    m_timeScale = 0;
                if (time >= m_startTimeOfTracing && timeScale > 0)
                    m_timeScale = 0;

                eventTracer.Trace(deltaTime);

            }
        }
    }
}