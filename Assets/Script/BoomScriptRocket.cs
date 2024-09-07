using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomScriptRocket : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.shakeMagintude = 1;
        GameManager.Instance.screenShake = 5;
        Invoke("DisableHitbox",0.1f);
        Destroy(gameObject,5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyAi health = other.transform.gameObject.GetComponent<EnemyAi>();
            health.hp -= GameManager.Instance.buffCount+7;
        }
    }

    void DisableHitbox()
    {
        GetComponent<Collider>().enabled = false;
    }
}
