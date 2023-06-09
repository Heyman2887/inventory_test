using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float h;
    private float v;
    //��ⷶΧ
    public float detectRadius = 2f;

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

    //��Ҽ�ⷶΧ�ھ����������ײ��
    public Collider DetectItem()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            Interactable interactable = collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                float distanceToInteractable = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToInteractable < closestDistance)
                {
                    closestCollider = collider;
                    closestDistance = distanceToInteractable;
                }
            }
        }

        if (closestCollider != null)
        {
            return closestCollider;
        }
        else
        {
            return null;
        }
    }
}
