using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float h;
    private float v;

    private void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }
    private void FixedUpdate()
    {
        Vector3 dir = new Vector3(h, 0, v);
        if (dir != Vector3.zero)
        {
            transform.Translate(dir * 0.2f);
        }
    }
}
