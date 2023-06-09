using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    public void OnClick()
    {
        Debug.Log("´ò¿ªÃÅ");
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
