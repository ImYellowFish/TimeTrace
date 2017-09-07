using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGameManager : MonoBehaviour {
    public static SimpleGameManager Instance;
    void Awake() {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;

        player = FindObjectOfType<TraceController>();
    }

    public TraceController player;
}
