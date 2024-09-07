using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    GameObject target;
    public float rtDamp;
    public float speed = 240f;
    private GameObject[] enemies;
    [SerializeField] private GameObject kaboom;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = enemies[Random.Range(0, enemies.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //Debug.Log("Target" + target);
        if (target == GameObject.Find("Core"))
        {
            target = enemies[Random.Range(0, enemies.Length)];
        }
        if (target.gameObject == null)
        {
            Instantiate(kaboom, transform.position, Quaternion.Euler(new Vector3(Random.Range(90, -90), Random.Range(90, -90), Random.Range(90, -90))));
            Destroy(gameObject,1f);
        }
        else if ( target.gameObject != null)
        {
            Vector3 pos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(pos);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rtDamp * Time.deltaTime);
            //transform.LookAt(target.transform.position);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, target.transform.position) < 100f)
        {
            rtDamp = 100 - Vector3.Distance(transform.position, target.transform.position);
        }
        else
        {
            rtDamp = 0.7f;
        }

    }

    private void FixedUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Rocks") 
        { 
            Instantiate(kaboom,transform.position,Quaternion.Euler(new Vector3(Random.Range(90, -90), Random.Range(90, -90), Random.Range(90, -90))));
            Destroy(gameObject);
        }
    }
}
