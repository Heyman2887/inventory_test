using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public enum ReversalType {normalReversalType, specialReversalType};
    public ReversalType reversalType;
    public Queue<State> statesQueue = new Queue<State>();
    public Stack<State> statesStack = new Stack<State>();
    //���λ�û���Э���Ƿ�ִ�н���
    private bool isTimeReversalStart;
    //���λ�û���Э���Ƿ�ʼ��ȷ��ֻ��һ��ͬ��Э��������
    private bool isMovementCoroutineRunning;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        AddState();
    }

    public void AddState()
    {
        switch (reversalType)
        {
            case ReversalType.normalReversalType:
                if (statesQueue.Count >= 250)
                {
                    statesQueue.Dequeue();
                }
                if(!isTimeReversalStart)
                {
                    AddStateToQueue(transform.position, transform.rotation);
                }
                break;
            case ReversalType.specialReversalType:
                //TODO:����ջ���ȷ�ֹ�ڴ����
                if ((rb.velocity.magnitude > 0.001f || rb.angularVelocity.magnitude > 0.001f) && !isTimeReversalStart)
                {
                    AddStateToStack(transform.position, transform.rotation);
                }
                break;
        }
    }

    public void ReversalTime()
    {
        isTimeReversalStart = true;

        if (!isMovementCoroutineRunning)
        {
            StartCoroutine(Movement());
            statesQueue.Clear();
        }
    }

    public void AddStateToQueue(Vector3 position, Quaternion rotation)
    {
        // ��ӵ�ǰ״̬��������
        State state = new State(position, rotation);
        statesQueue.Enqueue(state);
    }

    public void AddStateToStack(Vector3 position, Quaternion rotation)
    {
        // ��ӵ�ǰ״̬��ջ��
        State state = new State(position, rotation);
        statesStack.Push(state);
    }

    IEnumerator Movement()
    {
        isMovementCoroutineRunning = true;
        
        switch (reversalType)
        {
            case ReversalType.normalReversalType:
                State[] states = statesQueue.ToArray();
                for (int i = states.Length - 1; i >= 0; i--)
                {
                    transform.position = states[i].position;
                    transform.rotation = states[i].rotation;
                    yield return new WaitForSeconds(0.02f); 

                    if (i == 0)
                        isTimeReversalStart = false;
                }
                break;
            
            case ReversalType.specialReversalType:
                while(statesStack.Count > 0)
                {
                    State state = statesStack.Pop();
                    transform.position = state.position;
                    transform.rotation = state.rotation;
                    yield return new WaitForSeconds(0.02f);
                }

                if (statesStack.Count == 0)
                {
                    isTimeReversalStart = false;
                }
                break;
        }

        isMovementCoroutineRunning = false;
    }
}
