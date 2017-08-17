using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeTrace;
using System;

public class JumpEvent : InputEvent<Vector3> {
    TraceController player;
    public JumpEvent() : base(Vector3.zero, Vector3.zero) {
        player = SimpleGameManager.Instance.player;
        doValue = player.velocity;
        doValue.y = player.jumpSpeed;
    }

    public override Vector3 InputValue {
        get {
            return player.currentFrameJumpSpeed;
        }

        set {
            player.currentFrameJumpSpeed = value;
        }
    }

    public override string EventName {
        get {
            return "Jump";
        }
    }
}

public class LandEvent : InputEvent<Vector3> {
    TraceController player;
    public LandEvent(Vector3 velocityBeforeLand) : base(Vector3.zero, velocityBeforeLand) {
        player = SimpleGameManager.Instance.player;
    }

    public override Vector3 InputValue {
        get {
            return player.currentFrameJumpSpeed;
        }

        set {
            player.currentFrameJumpSpeed = value;
        }
    }

    public override string EventName {
        get {
            return "Land: " + undoValue;
        }
    }
}
