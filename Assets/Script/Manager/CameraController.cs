using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour 
{
    // 跟随目标
    public Transform target;
    // 相机距离和高度偏移量
    public Vector3 offset = new Vector3(0.0f, 15.0f, -10.0f);
    // 平滑过渡时间
    private float smoothSpeed = 10.0f;

    void FixedUpdate() 
    {
        // 计算相机的目标位置和旋转角度
        Vector3 targetPos = target.position + offset;

    	// 使相机平滑地过渡到目标位置和角度
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }
}


