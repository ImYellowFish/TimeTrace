using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneReadme : MonoBehaviour {
    public string readmeText;

    private void OnGUI()
    {
        GUILayout.Label(readmeText);
    }
}
