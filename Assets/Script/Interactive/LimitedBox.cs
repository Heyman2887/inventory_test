using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedBox : MonoBehaviour, Interactable
{
    public void OnClick()
    {
        Debug.Log("����ʱ����");
        Destroy(gameObject);
    }
    public void OnInteract()
    {
        
    }
    public void OnHoverStart()
    {
        
    }
    public void OnHoverEnd()
    {

    }
}
