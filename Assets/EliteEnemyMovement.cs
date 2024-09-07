using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] float rtDamp = 0.5f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject EBulletPrefab;
    [SerializeField] private GameObject GunMuzzle;
    private float cdTime = 5f;
    private float cdTime02 = 0.3f;
    public float hp = 10f;
    Vector3 newPlayerpos;
    private bool hit = false;
    private float hitIcd = 0f;
    PlayerControl resetAim;
    [SerializeField] private GameObject legendary;
    [SerializeField] private GameObject epic;
    [SerializeField] private GameObject rare;
    [SerializeField] private GameObject common;

    [SerializeField] private GameObject spark;

    [SerializeField] private float netSpeed;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Playercam");
        resetAim = player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            GameManager.Instance.enemyKilled++;
            if (GameManager.Instance.lockOnTarget)
            { resetAim.LockOn(); }
            LootDrop();
            Destroy(gameObject);
        }
        Turn();
        Flight();
        newPlayerpos = player.transform.position /*+ new Vector3(0, -1f, 0)*/;
        GunMuzzle.transform.LookAt(newPlayerpos);
    }
    private void FixedUpdate()
    {
        if (cdTime <= 0)
        {
            rtDamp = Random.Range(0.5f, 3f);
            int newCD = Random.Range(5, 30);
            cdTime = newCD;
        }
        else
        {
            cdTime -= 0.02f;
        }
        if (cdTime02 <= 0)
        {
            GameObject enemyFire = Instantiate(EBulletPrefab, GunMuzzle.transform.position, GunMuzzle.transform.rotation);
            cdTime02 = 50f;
        }
        else
        {
            cdTime02 -= 0.02f;
        }
        if (hit && hitIcd <= 0)
        {
            hp -= GameManager.Instance.lazerDmg;
            GameObject hitFeeback = Instantiate(spark, transform.position, spark.transform.rotation, transform);
            Destroy(hitFeeback, 1f);
            hitIcd = 0.09f;
            Debug.Log(hit);
        }
        else
        {
            hitIcd -= 0.02f;
            hit = false;
        }
        if (hit)
        {
            speed = 8f;
        }
        else if (speed < 40)
        {
            speed += 0.02f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet02")
        {
            hit = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bullet02")
        {
            hit = true;
        }
    }
    void Flight()
    {
        if (Vector3.Distance(player.transform.position, transform.position) > 300)
        {
            netSpeed = Vector3.Distance(player.transform.position, transform.position) * speed * 0.01f + speed;
            transform.position += transform.forward * netSpeed * Time.deltaTime;
        }
    }
    void Turn()
    {
        Vector3 pos = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rtDamp/2 * Time.deltaTime);
    }

    void LootDrop()
    {
        float rng = Random.Range(0, 100);
        switch (rng)
        {
            case <= 0.000000001f:
                GameManager.Instance.buffCount += 99;// BUY A LOTTERY PLS
                break;

            case <= 5:
                GameObject spawn = Instantiate(legendary, transform.position, Quaternion.identity); Destroy(spawn, 300f);
                break;

            case <= 10:
                GameObject spawn2 = Instantiate(epic, transform.position, Quaternion.identity); Destroy(spawn2, 300f);
                break;

            case <= 40:
                GameObject spawn3 = Instantiate(rare, transform.position, Quaternion.identity); Destroy(spawn3, 300f);
                break;
            case <= 100:
                GameObject spawn4 = Instantiate(common, transform.position, Quaternion.identity); Destroy(spawn4, 300f);
                break;
        }
    }
}
