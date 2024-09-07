using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonColle : MonoBehaviour
{
    Vector3 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(dir * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.bonusBuffGuage += 10 + ((100 * (1 + GameManager.Instance.buffCount / 10)) * 0.01f);
            Destroy(gameObject);
        }
    }
}
