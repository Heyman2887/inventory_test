using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour 
{
    // ����Ŀ��
    public Transform target;
    // �������͸߶�ƫ����
    public Vector3 offset = new Vector3(0.0f, 15.0f, -10.0f);
    // ƽ������ʱ��
    private float smoothSpeed = 10.0f;

    void FixedUpdate() 
    {
        // ���������Ŀ��λ�ú���ת�Ƕ�
        Vector3 targetPos = target.position + offset;

    	// ʹ���ƽ���ع��ɵ�Ŀ��λ�úͽǶ�
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }
}


