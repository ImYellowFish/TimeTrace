using UnityEngine;
using System.Collections;

namespace TimeTrace {
    [System.Serializable]
    public class FrameData
    {
        public FrameData(float time, int frame)
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
        public static FrameData Null
        {
            get
            {
                if (m_null == null)
                    m_null = new FrameData(-1, -1);
                return m_null;
            }
        }
        private static FrameData m_null;
    }
}
