using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public Vector3 position;
    public Quaternion rotation;

    public State(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}
