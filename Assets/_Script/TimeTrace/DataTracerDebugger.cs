using UnityEngine;
using System.Collections.Generic;

namespace TimeTrace.Utility
{
    public class DataTracerDebugger
    {
        public List<TimeTraceData> records = new List<TimeTraceData>();
        // public List<TimeTraceData> loads = new List<TimeTraceData>();
        public int previousIndex = -1;

        public void RecordSave(TimeTraceData data)
        {
            records.Add(data);
            previousIndex = records.Count - 1;
        }


        public void RecordLoad(TimeTraceData data, float timeMin, float timeMax)
        {
            int i = records.IndexOf(data);
            if (i != previousIndex)
            {
                Debug.LogError("Missing frame: " + data.time);
                Debug.Log("timeMin: " + timeMin + ", timeMax: " + timeMax);
            }
            else
            {
                try
                {
                    records.RemoveAt(previousIndex);
                }
                catch
                {
                    Debug.LogError(records.Count + ", " + previousIndex);
                }
                previousIndex--;
            }
        }
    }
}