using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fracture : MonoBehaviour
{
    public GameObject fractured;
    public GameObject kaboom;
    private Vector3 dir;
    private bool broken=false;

    [SerializeField] private GameObject legendary;
    [SerializeField] private GameObject epic;

    private void Start()
    {
         dir  = new Vector3(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
    }
    private void Update()
    {
        transform.Rotate(dir*Time.deltaTime,Space.Self);

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,transform.position) > 1000)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !broken)
        {
            FractureObject();
            GameObject.Find("Cam Parent").GetComponent<PlayerControl>().enabled= false;
            GameManager.Instance.gameOver = true;
            Destroy(other.gameObject);
        }

        if (other.tag != "BrokenRocks")
        {
            Drops();
            FractureObject();
        }
    }
    public void FractureObject()
    {
        GameObject brokenState = Instantiate(fractured, transform.position, transform.rotation);//Spawn in the broken version
        GameObject blow = Instantiate(kaboom, transform.position, Quaternion.identity);
        broken = true;
        Destroy(brokenState,180f);
        Destroy(blow,5f);
        Destroy(gameObject); //Destroy the object to stop it getting in the way
    }
    void Drops()
    {
        float rng = Random.Range(0, 100);
        switch (rng)
        {
            case <= 0.000000001f:
                GameManager.Instance.buffCount += 99;// BUY A LOTTERY PLS
                break;

            case <= 2.5f:
                GameObject spawn = Instantiate(legendary, transform.position, Quaternion.identity); Destroy(spawn, 300f);
                break;

            case <= 5:
                GameObject spawn2 = Instantiate(epic, transform.position, Quaternion.identity); Destroy(spawn2, 300f);
                break;
        }
    }
}
