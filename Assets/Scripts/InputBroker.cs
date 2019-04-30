using UnityEngine;
using System.Collections;

// Helper class to enable random input.
public class InputBroker {
    private float currentHorizontal = 0;
    private float currentVertical = 0;
    public float GetAxis(string axisName) {
        if (axisName == "Horizontal") {
            return currentHorizontal;
        } else if (axisName == "Vertical") {
            return currentVertical;
        }
        return 0.0f;
    }
    public bool HorizontalPressed() {
        return currentHorizontal != 0;
    }
    public bool VerticalPressed() {
        return currentVertical != 0;
    }
    public void setHorizontal(float value) {
        currentHorizontal = value;
    }
    public void setVertical(float value) {
        currentVertical = value;
    }
}