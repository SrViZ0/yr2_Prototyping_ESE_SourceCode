using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class IconAlphaManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var color = GetComponent<MeshRenderer>().material.color;
        if (!GameManager.Instance.lazerUnlocked) 
        {
            color.a = 0;
            GetComponent<MeshRenderer>().material.color = color;
        }
        else
        {
            color.a = 1;    
            GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
