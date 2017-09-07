using UnityEngine;

namespace TimeTrace.Utility
{
    /// <summary>
    /// Manages a circular array index
    /// </summary>
    [System.Serializable]
    public class RepeatingArrayRange
    {
        /// <summary>
        /// the max length of the array
        /// </summary>
        public int maxDataCount;

        /// <summary>
        /// the current data count of the array
        /// </summary>
        public int dataCount;

        /// <summary>
        /// start index of the current range
        /// </summary>
        public int startIndex;

        /// <summary>
        /// end index of the current range
        /// </summary>
        public int endIndex;

        /// <summary>
        /// Ctor of ArrayRange. maxDataCount is the allocated length of array.
        /// </summary>
        /// <param name="maxDataCount"></param>
        public RepeatingArrayRange(int maxDataCount)
        {
            this.maxDataCount = maxDataCount;
            dataCount = 0;
            startIndex = 0;
            endIndex = -1;
        }

        /// <summary>
        /// Inscrease the range by adding endIndex by one
        /// If array is full, also increase the startIndex by one
        /// </summary>
        public void Increase()
        {
            dataCount = Mathf.Min(dataCount + 1, maxDataCount);
            endIndex = NextTerminalIndex(endIndex);

            // if buffer is full, overwrite earliest data
            if (endIndex == startIndex && IsArrayFull())
            {
                startIndex = NextTerminalIndex(startIndex);
            }
        }

        public void Decrease()
        {
            if (dataCount == 0)
                return;

            int secondLastIndex = PreviousIndex(endIndex);

            dataCount--;

            if (dataCount == 0)
            {
                endIndex = startIndex - 1;
            }
            else
            {
                endIndex = secondLastIndex;
            }
        }

        /// <summary>
        /// Get the next index in range (looping)
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public int NextIndex(int current)
        {
            // index range is [startIndex, endIndex] 
            if (startIndex <= endIndex)
            {
                return RepeatToRange(current + 1, startIndex, endIndex);
            }

            // index range is [startIndex, maxDataCount - 1][0, endIndex]
            else
            {
                if (current == maxDataCount - 1)
                {
                    return 0;
                }
                else if (current == endIndex)
                    return startIndex;
                else
                    return current + 1;
            }
        }

        /// <summary>
        /// Get the previous index in range (looping)
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public int PreviousIndex(int current)
        {
            // index range is [startIndex, endIndex] 
            if (startIndex <= endIndex)
            {
                return RepeatToRange(current - 1, startIndex, endIndex);
            }

            // index range is [startIndex, maxDataCount - 1][0, endIndex]
            else
            {
                if (current == startIndex)
                {
                    return endIndex;
                }
                else if (current == 0)
                    return maxDataCount - 1;
                else
                    return current - 1;
            }
        }

        /// <summary>
        /// Is the given index inside the array range?
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsInArrayRange(int index)
        {
            // index range is [startIndex, endIndex] 
            if (startIndex <= endIndex)
            {
                return index >= startIndex && index <= endIndex;
            }

            // index range is [startIndex, maxDataCount - 1][0, endIndex]
            else
            {
                return (index >= startIndex && index < maxDataCount) ||
                    (index >= 0 && index <= endIndex);
            }
        }

        public bool IsArrayFull()
        {
            return dataCount == maxDataCount;
        }




        #region Helper
        private int NextTerminalIndex(int current)
        {
            return (current + 1) % maxDataCount;
        }

        private int PreviousTerminalIndex(int current)
        {
            return (current - 1 + maxDataCount) % maxDataCount;
        }


        private static int RepeatToRange(int value, int min, int max)
        {
            var size = max - min + 1;
            return (value - min + size) % size + min;
        }

        #endregion
    }
}