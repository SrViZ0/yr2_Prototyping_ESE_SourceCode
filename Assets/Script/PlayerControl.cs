using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    [Header("Freecam Variables")]
    [SerializeField] private Camera playercam;
    [SerializeField] private GameObject rotationPoint;
    private Vector3 previousPosition;
    private float zoom = -6f;
    [SerializeField] private GameObject camPoint;//Reset Position
    public GameObject spaceship;//Object with movement script

    [Header("Misc.")]
    public bool enableLook = true;
    public bool enableFlight = true;
    public bool enableCamReset = true;
    public bool enableBoost = true;
    [SerializeField] private bool enableCamLock;
    [SerializeField] private float lockOnRange = 500f;
    [SerializeField] private GameObject core;

    [Header("Player variables")]
    [SerializeField] private float horiz = 5f;
    [SerializeField] private float horizAmount = 240f;
    private GameObject target;
    public GameObject gun1;
    public GameObject gun2;
    public GameObject bulletPrefab;
    public GameObject muzzle1;
    public GameObject muzzle2;
    public GameObject modGun;
    private float cd;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (enableLook) MouseLook();
        if (enableFlight) Flight();
        if (enableCamReset) ResetCam();
        if (enableBoost) Boost();
        if (Input.GetMouseButtonDown(2)){LockOn();}
        if (GameManager.Instance.lockOnTarget && lockOnRange >= Vector3.Distance(target.transform.position, transform.position))
        {
            playercam.transform.LookAt(target.transform.position);
            playercam.transform.position = rotationPoint.transform.position;
            playercam.transform.Translate(new Vector3(0, +2, zoom));
        }
        else if (GameManager.Instance.lockOnTarget && lockOnRange < Vector3.Distance(target.transform.position, transform.position))
        {
            GameManager.Instance.lockOnTarget = false;
        }
        if (Input.GetKeyDown(KeyCode.CapsLock)) 
        {
            enableCamLock = !enableCamLock;
        }
        Aim();
        ModularGun();
        CameraShake();
    }

    void FixedUpdate()
    {
        Slow();
    }

    void MouseLook()
    {   
        float zoomAmt = 100f;
        zoom = Mathf.Clamp(zoom, -12f, -6f);
        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = playercam.ScreenToViewportPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(1))
        {

            Vector3 direction = previousPosition - playercam.ScreenToViewportPoint(Input.mousePosition);

            playercam.transform.position = rotationPoint.transform.position;

            playercam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
            playercam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, relativeTo: Space.World);
            playercam.transform.Translate(new Vector3(0, +2, zoom));

            previousPosition = playercam.ScreenToViewportPoint(Input.mousePosition);

            if (Input.mouseScrollDelta.y > 0)
            {
                zoom -= zoomAmt * Time.deltaTime;

            }
            if (Input.mouseScrollDelta.y < 0)
            {            
                zoom += zoomAmt * Time.deltaTime;
            }
        }
    }

    private void Flight()
    {
        transform.position = spaceship.transform.position;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        horiz += horizontalInput * horizAmount * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(Vector3.up * horiz);
    }

private void ResetCam()
    {
        if (Input.GetKey(KeyCode.Y) || enableCamLock)
        {
            playercam.transform.rotation = camPoint.transform.rotation;

            playercam.transform.position = rotationPoint.transform.position;
            playercam.transform.Translate(new Vector3(0, +2 , zoom));
        }

    }

    private void Boost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !GameManager.Instance.charging && !GameManager.Instance.lazerActive ) 
        {
            GameManager.Instance.netFlightSpeed = GameManager.Instance.flightSpeed * 3;
            GameManager.Instance.boosting = true ;
        }
        else if (!Input.GetKey(KeyCode.LeftControl)&& GameManager.Instance.netFlightSpeed > GameManager.Instance.flightSpeed && !GameManager.Instance.charging)
        {
            GameManager.Instance.netFlightSpeed = GameManager.Instance.flightSpeed;
            GameManager.Instance.boosting = false;
        }
    }
    public void LockOn()
    {
        GameObject [] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float lowestDist = 0;

        for (int i = 0; i < enemies.Length; i++)
        {
            if (i > 0 && lowestDist >= Vector3.Distance(enemies[i].transform.position, transform.position))
            {
                lowestDist = Vector3.Distance(enemies[i].transform.position, transform.position);
                target = enemies[i];
            }       
            else if (i <= 0)
            {
                lowestDist = Vector3.Distance(enemies[i].transform.position, transform.position);
                target = enemies[i];
            }
            //Debug.Log(Vector3.Distance(enemies[i].transform.position, transform.position));
        }
        GameManager.Instance.lockOnTarget = (!GameManager.Instance.lockOnTarget);
    }
    private void Aim()
    {
        if (Input.GetMouseButton(0)) 
        {
            if (cd <= 0 && !GameManager.Instance.disabled)
            {
                GameObject projectile = Instantiate(bulletPrefab, muzzle1.transform.position, gun1.transform.rotation);
                GameObject projectile2 = Instantiate(bulletPrefab, muzzle2.transform.position, gun2.transform.rotation);
                Destroy(projectile, 5f);
                Destroy(projectile2, 5f);
                cd = 0.15f;
            }
            else 
            { 
                cd-=Time.deltaTime;
            }

        }
        if (GameManager.Instance.lockOnTarget)
        {
            gun1.transform.LookAt(target.transform.position + new Vector3(0, 0, 0));
            gun2.transform.LookAt(target.transform.position + new Vector3(0, 0, 0));
        }
        else 
        {
            gun1.transform.rotation = playercam.transform.rotation;
            gun2.transform.rotation = playercam.transform.rotation;
        }
    }

    private void Slow()
    {
        if (Input.GetKey(KeyCode.LeftControl) && GameManager.Instance.flightSpeed*0.1 <= GameManager.Instance.netFlightSpeed)
        {
            GameManager.Instance.netFlightSpeed -= GameManager.Instance.flightSpeed * 0.004f;
        }
        else if (GameManager.Instance.netFlightSpeed < GameManager.Instance.flightSpeed)
        {
            GameManager.Instance.netFlightSpeed += GameManager.Instance.flightSpeed * 0.004f;//abt 5 sec
        }

    }

    void ModularGun()
    {
        if (GameManager.Instance.lockOnTarget)
        {
            modGun.transform.LookAt(target.transform.position + new Vector3(0, 0, 0));
        }
        else
        {
            modGun.transform.rotation = playercam.transform.rotation;
        }
    }

    void CameraShake()
    {
        Vector3 startpos = new Vector3 (0,0,-10);
        if (GameManager.Instance.screenShake > 0)
        {
            GameObject.Find("CanvasCam").transform.position = new Vector3(Random.Range(-GameManager.Instance.shakeMagintude, GameManager.Instance.shakeMagintude), Random.Range(-GameManager.Instance.shakeMagintude, GameManager.Instance.shakeMagintude),-10);

            playercam.transform.position = rotationPoint.transform.position;

            Vector3 shake = new Vector3(Random.Range(-GameManager.Instance.shakeMagintude, GameManager.Instance.shakeMagintude), Random.Range(-GameManager.Instance.shakeMagintude, GameManager.Instance.shakeMagintude), 0);

            playercam.transform.position += shake;

            playercam.transform.Translate(new Vector3(0, +2, zoom));

            GameManager.Instance.screenShake--;
        }
        else
        {
            GameObject.Find("CanvasCam").transform.position = startpos;
        }

    }

    //void Warp()//scrapped

    //{
    //    if (Input.GetKeyDown(KeyCode.Q)) 
    //    {
    //        spaceship.transform.position = core.transform.position - new Vector3(0,0,600);
    //        spaceship.transform.LookAt(core.transform.position);
    //    }
    //}
}
// Iv'e reiterated this shit like 5 times, idek what im coin anymore, if it works, it works(it dosent) (4/7)
//shouldve just used cinemachine smh (25/6)