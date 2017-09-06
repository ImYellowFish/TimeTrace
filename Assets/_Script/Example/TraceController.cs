using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimeTrace;
using System;

public class PositionTimeTraceData : TimeTraceData
{
    public Vector3 position;
    public PositionTimeTraceData(float time, int frame, Vector3 position) : base(time, frame)
    {
        this.position = position;
    }
}

public class TraceController : TraceBehaviour {
    public float speed = 10;
    public float jumpSpeed = 10;
    public float gravity = -30;

    [Header("Readonly")]
    public bool isGrounded;
    public Vector3 velocity;
    
    /// <summary>
    /// If not zero, character would try to jump this frame by this speed
    /// </summary>
    [Header("Input state")]
    public Vector3 currentFrameJumpSpeed;
    public bool bLeft;
    public bool bRight;
    public bool bUp;
    public bool bDown;

    private CharacterController cc;
    void Start() {
        cc = GetComponent<CharacterController>();
    }

    public override void RevertableUpdate(float deltaTime) {
        if (TimeTraceManager.tracing)
            return;

        if (cc.isGrounded && !isGrounded && deltaTime > 0 && cc.velocity.y < -0.01f)
            TimeTraceManager.AddTraceEvent(new LandEvent(velocity));

        isGrounded = cc.isGrounded;

        float h = 0;
        float v = 0;
        
        if (bLeft) {
            h = -1;
        } else if (bRight) {
            h = 1;
        }

        if (bUp) {
            v = 1;
        } else if (bDown) {
            v = -1;
        }

        if (isGrounded) {
            if (!Mathf.Approximately(currentFrameJumpSpeed.y, 0))
                velocity = currentFrameJumpSpeed;
            else
                velocity = new Vector3(h * speed, 0, v * speed);
        } else {
            currentFrameJumpSpeed = Vector3.zero;
        }

        velocity.y += deltaTime * gravity;

        cc.Move(velocity * deltaTime);
    }

    public override bool AutoSaveFrameData
    {
        get
        {
            return true;
        }
    }

    public override TimeTraceData GetFrameData(float time, int frame)
    {
        return new PositionTimeTraceData(time, frame, transform.position);
    }

    public override void LoadFrameData(TimeTraceData data)
    {
        var d = data as PositionTimeTraceData;
        transform.position = d.position;
    }
}
