using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    private void Update()
    {
        transform.Translate(Vector3.right * 0.01f);
    }
}
