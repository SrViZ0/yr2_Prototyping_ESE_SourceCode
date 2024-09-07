using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Lazer : MonoBehaviour
{
    private GameObject hitTarget;
    [SerializeField] private GameObject beam;
    [SerializeField] private GameObject modGun;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LazerAim();
    }
    void LazerAim()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float lowestDist = 0;

        for (i = 0; i < enemies.Length; i++)
        {
            if (i > 0 && lowestDist >= Vector3.Distance(enemies[i].transform.position, transform.position))
            {
                lowestDist = Vector3.Distance(enemies[i].transform.position, transform.position);
                hitTarget = enemies[i];
            }
            else if (i <= 0)
            {
                lowestDist = Vector3.Distance(enemies[i].transform.position, transform.position);
                hitTarget = enemies[i];
            }
            //Debug.Log(Vector3.Distance(enemies[i].transform.position, transform.position));
            transform.LookAt(hitTarget.transform.position);
            transform.localScale = new Vector3(1f,1f,Vector3.Distance(hitTarget.transform.position,transform.position));
        }
    }
}
