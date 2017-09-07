using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TimeTrace;
using System;

public abstract class InputEvent<TValue> : TracedEvent {
    public TValue doValue;
    public TValue undoValue;
    public abstract TValue InputValue { get; set; }
    
    public InputEvent(TValue doValue, TValue undoValue) {
        this.doValue = doValue;
        this.undoValue = undoValue;
    }

    public override void Do(float deltaTime) {
        InputValue = doValue;
    }

    public override void Undo(float deltaTime) {
        InputValue = undoValue;
    }
}

