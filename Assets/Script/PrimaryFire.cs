using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryFire : MonoBehaviour
{
    Vector3 prevPos;
    RaycastHit hit;
    int BulletLayer;
    PlayerControl resetAim;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Playercam");
        //resetAim = player.GetComponent<PlayerControl>();
        BulletLayer = LayerMask.GetMask("Bullet");
    }

    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(prevPos, transform.position, out hit, BulletLayer))
        {
            if (hit.transform.tag== "Enemy")
            {
                //if (GameManager.Instance.lockOnTarget)
                //{ resetAim.LockOn();}

                EnemyAi health = hit.transform.gameObject.GetComponent<EnemyAi>();
                health.hp -= GameManager.Instance.bulletDamage;
                //Destroy(hit.transform.gameObject);
                Destroy(gameObject);
            }
            if (hit.transform.tag == "Enemy02")
            {
                //if (GameManager.Instance.lockOnTarget)
                //{ resetAim.LockOn();}

                EliteEnemyMovement health = hit.transform.gameObject.GetComponent<EliteEnemyMovement>();
                health.hp -= GameManager.Instance.bulletDamage;
                //Destroy(hit.transform.gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * GameManager.Instance.bulletSpeed * Time.deltaTime);
        prevPos = transform.position;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Enemy") 
    //    {
    //        Destroy(gameObject);
    //    }

    //}
}
