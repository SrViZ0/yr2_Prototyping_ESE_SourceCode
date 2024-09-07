using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            GameManager.Instance.disabled = true;
            GameManager.Instance.netFlightSpeed = 3f;
        }

    }
}
