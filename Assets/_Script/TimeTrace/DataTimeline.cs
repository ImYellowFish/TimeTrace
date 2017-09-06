using UnityEngine;
using System.Collections.Generic;
using System;

namespace TimeTrace.Utility
{
    public abstract class IDataTimeline
    {
        public abstract void Add(TimeTraceData data);
        public abstract void RemoveLast();
        public abstract bool SearchTime(float timeMin, float timeMax, bool backTracing, out int index, ref List<TimeTraceData> data);
        public abstract bool SearchFrame(int frameMin, int frameMax, bool backTracing, out int index, ref List<TimeTraceData> data);
        public abstract void RemoveFromIndexToEnd(int index);
    }

    [System.Serializable]
    public class DataTimeline : IDataTimeline
    {
        public TimeTraceData[] dataArray;
        public RepeatingArrayRange arrayRange;
        public int searchIndex;

        public int StartIndex { get { return arrayRange.startIndex; } }
        public int EndIndex { get { return arrayRange.endIndex; } }
        public int DataCount { get { return arrayRange.dataCount; } }

        public DataTimeline(int maxRecords)
        {
            dataArray = new TimeTraceData[maxRecords];
            arrayRange = new RepeatingArrayRange(maxRecords);
        }

        // TODO: optimize allocation here
        public override void Add(TimeTraceData data)
        {
            arrayRange.Increase();
            dataArray[EndIndex] = data;

            // reset search index
            searchIndex = EndIndex;
        }

        
        public override void RemoveLast()
        {
            if (DataCount == 0)
                return;

            dataArray[EndIndex].time = -1;
            arrayRange.Decrease();
            
            // reset search index
            searchIndex = EndIndex;
        }

        /// <summary>
        /// Look for the frame in range (frameA, frameB] or (frameB, frameA].
        /// Optimized for chrono order search
        /// </summary>
        public override bool SearchFrame(int frameA, int frameB, bool backTracing, out int index, ref List<TimeTraceData> searchResults)
        {
            searchFrameMin = Mathf.Min(frameA, frameB);
            searchFrameMax = Mathf.Max(frameA, frameB);
            return SearchInternal(IsInSearchFrameRange, backTracing, out index, ref searchResults);
        }

        /// <summary>
        /// Look for the time in range (timeA, timeB] or (timeB, timeA] depending on which time is larger.
        /// Optimized for chrono order search
        /// </summary>
        public override bool SearchTime(float timeA, float timeB, bool backTracing, out int index, ref List<TimeTraceData> searchResults)
        {
            searchTimeMin = Mathf.Min(timeA, timeB);
            searchTimeMax = Mathf.Max(timeA, timeB);
            
            return SearchInternal(IsInSearchTimeRange, backTracing, out index, ref searchResults);
        }
        
        /// <summary>
        /// Look for the trace data according to Func<bool, TimeTraceData> IsInRange.
        /// Optimized for chrono order search
        /// </summary>
        public bool SearchInternal(Func<TimeTraceData, bool> IsInRange, bool backTracking, out int index, ref List<TimeTraceData> searchResults)
        {
            bool found = false;

            if (searchResults == null)
                searchResults = new List<TimeTraceData>();

            index = -1;
            searchResults.Clear();

            if (DataCount == 0)
                return false;

            // da.Length - 1 is the number of elements we need to check
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (IsInRange.Invoke(dataArray[searchIndex]))
                {
                    index = searchIndex;
                    searchResults.Add(dataArray[searchIndex]);
                    found = true;
                }
                // If previously found some results, but current frame is not in range
                // then stop searching
                else if (found)
                {
                    return found;
                }

                if (backTracking)
                    searchIndex = arrayRange.PreviousIndex(searchIndex);
                else
                {
                    searchIndex = arrayRange.NextIndex(searchIndex);
                }
            }

            return found;
        }

        public override void RemoveFromIndexToEnd(int index)
        {
            if (!arrayRange.IsInArrayRange(index))
            {
                throw new ArgumentException("Index is not in range: " + index + 
                    ", start = " + StartIndex + ", end = " + EndIndex);
            }
            while (DataCount > 0)
            {
                RemoveLast();
                
                if (EndIndex == index)
                {
                    RemoveLast();
                    break;
                }
            }
        }

        #region Helper
        private float searchTimeMin;
        private float searchTimeMax;
        private float searchFrameMin;
        private float searchFrameMax;

        private bool IsInSearchTimeRange(TimeTraceData data)
        {
            return data.time > searchTimeMin && data.time <= searchTimeMax;
        }

        private bool IsInSearchFrameRange(TimeTraceData data)
        {
            return data.frame > searchFrameMin && data.frame <= searchFrameMax;
        }
        #endregion


        #region Test
        //[ContextMenu("Add")]
        //public void TestAdd()
        //{
        //    var data = new TimeTraceData(TimeTraceManager.time + 100);
        //    Add(data);
        //}

        //[ContextMenu("Remove")]
        //public void TestRemove()
        //{
        //    RemoveLast();
        //}

        //[Header("Test")]
        //public Vector2 testSearchTimeRange;

        //[ContextMenu("Search")]
        //public void TestSearch()
        //{
        //    int index;
        //    T data;
        //    bool r = Search(testSearchTimeRange.x, testSearchTimeRange.y, out index, out data);
        //    Debug.Log("r: " + r.ToString() + ", index = " + index);
        //}

        //public int testRemoveSinceIndex;

        //[ContextMenu("RemoveSince")]
        //public void TestRemoveSince()
        //{
        //    RemoveFromIndexToEnd(testRemoveSinceIndex);
        //}
        #endregion
    }
}