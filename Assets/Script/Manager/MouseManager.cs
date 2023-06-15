using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseManager : MonoBehaviour
{
    static MouseManager instance;
    public static event Action<GameObject> OnClickMouse;
    RaycastHit hitInfo;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Update()
    {
        ObjectExist();
    }

    public void ObjectExist()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if(OnClickMouse != null)
            {
                if(hitInfo.collider.GetComponent<TimeController>() != null && Input.GetMouseButtonDown(0))
                {
                    TimeController.isUsePocketWatch += OnClickObject;
                    OnClickMouse = null;
                }
            }
        }
    }

    public void OnClickObject(GameObject gameObject)
    {

    }
}
