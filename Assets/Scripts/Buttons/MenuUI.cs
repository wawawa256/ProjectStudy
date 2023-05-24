using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void UICallButtonClicked()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
