using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeTrace;

public class InputController : MonoBehaviour {
    public float backTrackTimeScale = 0.5f;
    public InputEventDirectionalFactory directionalInputFactory;

    void Start() {
        directionalInputFactory = new InputEventDirectionalFactory();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T)) {
            if (!TimeTraceManager.tracing)
                TimeTraceManager.StartTrace(backTrackTimeScale);
            else
                TimeTraceManager.StopTrace();
        }

        if (!TimeTraceManager.tracing) {
            directionalInputFactory.CheckForInputEvent(OnDirectionalInput);

            if (Input.GetKeyDown(KeyCode.Space)) {
                TimeTraceManager.AddTraceEvent(new JumpEvent());
            }
        }
	}

    private void OnDirectionalInput(InputEventDirectional input) {
        TimeTraceManager.AddTraceEvent(input);
    }
}
