using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazerCollision : MonoBehaviour
{
    [SerializeField] private GameObject beigboom;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Instantiate(beigboom.gameObject, other.gameObject.transform.position, Quaternion.identity);
            GameManager.Instance.PlayerActiveHp -= 3f;
        }
    }
}
