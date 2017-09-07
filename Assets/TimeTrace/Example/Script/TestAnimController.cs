using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeTrace;

[System.Serializable]
public class TestAnimTestInput
{
    public KeyCode key;
    public string anim;
}

[RequireComponent(typeof(AnimationTracer))]
public class TestAnimController : MonoBehaviour {
    AnimationTracer at;
    public List<TestAnimTestInput> testInputs;
    
	// Use this for initialization
	void Start () {
        at = GetComponent<AnimationTracer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (TimeTraceManager.tracing)
            return;

		for(int i = 0; i < testInputs.Count; i++)
        {
            var inputConfig = testInputs[i];
            if (Input.GetKeyDown(inputConfig.key))
                at.Play(inputConfig.anim);
        }
	}
    
}
