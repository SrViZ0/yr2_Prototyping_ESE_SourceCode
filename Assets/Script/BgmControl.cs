using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.buffSelect || GameManager.Instance.guide)
        {
            GetComponent<AudioSource>().volume = 0.5f;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0.8f;
        }

    }
}
