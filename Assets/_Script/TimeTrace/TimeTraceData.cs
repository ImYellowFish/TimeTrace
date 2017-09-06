using UnityEngine;
using System.Collections;

namespace TimeTrace {
    [System.Serializable]
    public class TimeTraceData
    {
        public TimeTraceData(float time, int frame)
        {
            this.time = time;
            this.frame = frame;
        }

        /// <summary>
        /// Time of this data
        /// </summary>
        public float time;

        /// <summary>
        /// Frame of this data
        /// </summary>
        public int frame;

        /// <summary>
        /// Null instance for TimeTraceData
        /// </summary>
        public static TimeTraceData Null
        {
            get
            {
                if (m_null == null)
                    m_null = new TimeTraceData(-1, -1);
                return m_null;
            }
        }
        private static TimeTraceData m_null;
    }
}
