using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeTrace;

public class TimeTraceToggle : MonoBehaviour {
    public float backTrackTimeScale = -0.5f;
    
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!TimeTraceManager.tracing)
                TimeTraceManager.StartTrace(backTrackTimeScale);
            else
                TimeTraceManager.StopTrace();
        }

    }

}
