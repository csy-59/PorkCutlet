using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool Q { get; private set; }
    public bool W { get; private set; }
    public bool E { get; private set; }
    public bool R { get; private set; }

    public bool Click { get; private set; }

    private void Update()
    {
        Q = W = E = R = false;

        Q = Input.GetKeyDown(KeyCode.Q);
        W = Input.GetKeyDown(KeyCode.W);
        E = Input.GetKeyDown(KeyCode.E);
        R = Input.GetKeyDown(KeyCode.R);

        Click = Input.GetMouseButtonDown(0);
    }
}
