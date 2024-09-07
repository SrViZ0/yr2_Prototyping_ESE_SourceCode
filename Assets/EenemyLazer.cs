using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EenemyLazer : MonoBehaviour
{
    [SerializeField] private GameObject beam;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private float cd = 2f;
    [SerializeField] private float fireCd = 30f;
    private bool fired;
    // Start is called before the first frame update
    void Start()
    {
        beam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fired)
        {
            if (cd <= 0f)
            {
                beam.SetActive(false);
                fired = false;
                cd = 2f;
            }
            cd -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.X) ) 
        {
            fired = true;
            beam.SetActive(true);
            transform.position = muzzle.transform.position;
            transform.LookAt(GameObject.FindGameObjectWithTag("Playercam").transform.position);
            transform.localScale = new Vector3(1f, 1f, Vector3.Distance(GameObject.FindGameObjectWithTag("Playercam").transform.position, transform.position)+50);
            fireCd = 30f;
        }
        else
        {
            fireCd -= Time.deltaTime;
        }
    }
}
