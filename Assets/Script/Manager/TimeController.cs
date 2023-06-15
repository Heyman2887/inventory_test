using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public Queue<State> statesQueue = new Queue<State>();
    public static event Action<GameObject> isUsePocketWatch;

    void FixedUpdate()
    {
        if (statesQueue.Count >= 250)
        {
            statesQueue.Dequeue();
        }
        AddState(transform.position, transform.rotation);
        if(isUsePocketWatch != null)
        {
            StartCoroutine(Movement());
            isUsePocketWatch = null;
        }
    }

    public void AddState(Vector3 position, Quaternion rotation)
    {
        // 添加当前状态到队列中
        State state = new State(position, rotation);
        statesQueue.Enqueue(state);
    }

    IEnumerator Movement()
    {
        State[] states = statesQueue.ToArray();
        for (int i = states.Length - 1; i >= 0; i--)
        {
            transform.position = states[i].position;
            transform.rotation = states[i].rotation;
            yield return new WaitForSeconds(0.02f); 
        }
    }
}
