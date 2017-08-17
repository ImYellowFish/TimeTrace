using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeTrace {
    public class EventTracer : MonoBehaviour {
        public List<TraceEvent> events = new List<TraceEvent>();
        public float currentTime;

        /// <summary>
        /// The index of the first event before or at the tracing time
        /// </summary>
        public int currentIndex;

        public void Add(TraceEvent e) {
            Debug.Log("add: " + e.name + ", time: " + e.time);
            events.Insert(events.Count, e);
        }

        public void SetTraceTime(float time) {
            currentTime = time;
            currentIndex = ComputeCurrentIndexFromTime(time);
        }
        
        public void Trace(float deltaTime) {
            if(deltaTime <= 0) {
                TraceBack(deltaTime);
            }else {
                TraceForward(deltaTime);
            }
        }

        public void DiscardFutureEvents() {
            var removeCount = events.Count - currentIndex - 1;
            if(removeCount > 0)
                events.RemoveRange(currentIndex + 1, removeCount);
        }

        private void TraceForward(float deltaTime) {
            currentTime += deltaTime;

            while (currentIndex + 1 >= 0 &&
                    currentIndex + 1 < events.Count &&
                    events[currentIndex + 1].time < currentTime) {

                var currentEvent = events[currentIndex + 1];
                currentEvent.Undo(deltaTime);
                currentIndex++;
            }
        }

        private void TraceBack(float deltaTime) {
            currentTime += deltaTime;
            
            while (currentIndex >= 0 && 
                    currentIndex < events.Count && 
                    events[currentIndex].time >= currentTime) {

                var currentEvent = events[currentIndex];
                currentEvent.Undo(deltaTime);
                currentIndex--;
            }
        }



        /// <summary>
        /// Give current tracing time, compute the index of the event just before the time
        /// 
        /// returns -1 if no event is found
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private int ComputeCurrentIndexFromTime(float time) {
            int i;
            for(i = 0; i < events.Count; i++) {
                if(events[i].time > time) {
                    break;
                }
            }
            return i - 1;
        }
    }
}