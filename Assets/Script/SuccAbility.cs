using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SuccAbility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.magnetUnlocked) { Succ(); }
    }

    void Succ()
    {
        GetComponent<SphereCollider>().radius = GameManager.Instance.magnetRange;
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Drops" && GameManager.Instance.magnetUnlocked)
        {
            Debug.Log("In Range");
            other.gameObject.transform.LookAt(transform.position);
            other.gameObject.transform.Translate(Vector3.forward*150f*Time.deltaTime);
        }
    }
}
