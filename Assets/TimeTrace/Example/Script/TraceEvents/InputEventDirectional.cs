using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeTrace;
using System;

public abstract class InputEventDirectional : InputEvent<bool> {
    public InputEventDirectional(bool doValue) : base(doValue, doValue) {
        // save the value before invoke as undo value
        undoValue = !doValue;
    }

    public override string EventName {
        get {
            return "Directional input";
        }
    }
    
    public abstract KeyCode Key { get; }

    public abstract InputEventDirectional Clone();
}

public class InputEventDirectionalFactory {
    public InputEventDirectional[] inputs;

    public InputEventDirectionalFactory() {
        inputs = new InputEventDirectional[] {
            new InputEventLeft(true),
            new InputEventLeft(false),
            new InputEventRight(true),
            new InputEventRight(false),
            new InputEventUp(true),
            new InputEventUp(false),
            new InputEventDown(true),
            new InputEventDown(false),
        };
    }

    public void CheckForInputEvent(Action<InputEventDirectional> callback) {
        foreach(var p in inputs) {
            if (p.doValue) {
                if (Input.GetKeyDown(p.Key))
                    callback(p.Clone());
            }else {
                if (Input.GetKeyUp(p.Key))
                    callback(p.Clone());
            }
        }
    }
}



public class InputEventLeft : InputEventDirectional {
    public InputEventLeft(bool doValue) : base(doValue) {

    }

    public override string EventName {
        get {
            return "Left: " + doValue;
        }
    }

    public override KeyCode Key {
        get {
            return KeyCode.A;
        }
    }

    public override bool InputValue {
        get {
            return SimpleGameManager.Instance.player.bLeft;
        }

        set {
            SimpleGameManager.Instance.player.bLeft = value;
        }
    }

    public override InputEventDirectional Clone() {
        return new InputEventLeft(doValue);
    }
}

public class InputEventRight : InputEventDirectional {
    public InputEventRight(bool doValue) : base(doValue) {

    }

    public override string EventName {
        get {
            return "Right: " + doValue;
        }
    }

    public override KeyCode Key {
        get {
            return KeyCode.D;
        }
    }


    public override bool InputValue {
        get {
            return SimpleGameManager.Instance.player.bRight;
        }

        set {
            SimpleGameManager.Instance.player.bRight = value;
        }
    }

    public override InputEventDirectional Clone() {
        return new InputEventRight(doValue);
    }
}

public class InputEventUp : InputEventDirectional {
    public InputEventUp(bool doValue) : base(doValue) {

    }

    public override string EventName {
        get {
            return "Up: " + doValue;
        }
    }

    public override KeyCode Key {
        get {
            return KeyCode.W;
        }
    }


    public override bool InputValue {
        get {
            return SimpleGameManager.Instance.player.bUp;
        }

        set {
            SimpleGameManager.Instance.player.bUp = value;
        }
    }

    public override InputEventDirectional Clone() {
        return new InputEventUp(doValue);
    }
}

public class InputEventDown : InputEventDirectional {
    public InputEventDown(bool doValue) : base(doValue) {

    }

    public override string EventName {
        get {
            return "Down: " + doValue;
        }
    }

    public override KeyCode Key {
        get {
            return KeyCode.S;
        }
    }


    public override bool InputValue {
        get {
            return SimpleGameManager.Instance.player.bDown;
        }

        set {
            SimpleGameManager.Instance.player.bDown = value;
        }
    }

    public override InputEventDirectional Clone() {
        return new InputEventDown(doValue);
    }
}