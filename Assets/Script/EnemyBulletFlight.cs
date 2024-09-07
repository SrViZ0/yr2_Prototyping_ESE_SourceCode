using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletFlight : MonoBehaviour
{
    Vector3 prevPos;
    RaycastHit hit;
    int BulletLayer;
    float bulletSpd;
    public static float bulletDmg = 0.4f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spark;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        BulletLayer = LayerMask.GetMask("Bullet");
        bulletSpd = Random.Range(500,700);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(prevPos, transform.position, out hit, BulletLayer))
        {
            if (hit.transform.tag == "Player")
            {
                GameManager.Instance.screenShake = 3;
                GameManager.Instance.shakeMagintude= 0.1f;
                Instantiate(spark, hit.transform.position+new Vector3(Random.Range(-1,1), Random.Range(-0.5f, 0.9f), Random.Range(-1, 1)), Quaternion.Euler(new Vector3(Random.Range(-180, 180), Random.Range(-180,180), Random.Range(-180, 180))), hit.transform);
                GameManager.Instance.PlayerActiveHp -= bulletDmg;
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * bulletSpd * Time.deltaTime);
        prevPos = transform.position;
    }
}
