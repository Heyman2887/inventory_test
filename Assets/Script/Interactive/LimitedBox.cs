using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitedBox : MonoBehaviour, Interactable
{
    public void OnClick()
    {
        Debug.Log("´ò¿ªÁÙÊ±±¦Ïä");
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
