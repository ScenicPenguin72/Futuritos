using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardOnlyInputModule : StandaloneInputModule
{
    public override void Process()
    {
        // Only process keyboard/controller input
        if (!IsMouseInputActive())
        {
            base.Process();
        }
    }

    private bool IsMouseInputActive()
    {
        return Input.mousePresent &&
               (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) ||
                Input.GetMouseButton(1) || Input.GetMouseButtonDown(1) ||
                Input.GetMouseButton(2) || Input.GetMouseButtonDown(2) ||
                Input.mouseScrollDelta.sqrMagnitude > 0);
    }

    // Disable pointer data from mouse completely
    protected override MouseState GetMousePointerEventData()
    {
        // Return a dummy state with no interaction
        return new MouseState();
    }
}
