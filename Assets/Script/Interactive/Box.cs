using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, Interactable
{
    public void OnClick()
    {
        Debug.Log("´ò¿ª±¦Ïä");
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
