using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularGunUsage : MonoBehaviour
{
    [SerializeField] private GameObject halo,lazer,missle;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform[] gunMuzzle;

    public KeyCode lazerOn;
    public KeyCode rocket;

    private float chargeValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        lazer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(lazerOn) && GameManager.Instance.lazerCdTime <= 0 && GameManager.Instance.lazerUnlocked && !GameManager.Instance.boosting) 
        { GameManager.Instance.lazerActive = true; }

        if (Input.GetKeyUp(lazerOn) && GameManager.Instance.lazerActive)
        {
            GameManager.Instance.lazerCd = true;
            GameManager.Instance.lazerActive = false;
        }
        Mathf.Clamp(chargeValue, 0, GameManager.Instance.missleLimit);
        if (Input.GetKeyUp(rocket))
        {
            FireMissle();
            chargeValue = 0;
            GameManager.Instance.charging = false;
        }
        if (GameManager.Instance.charging)
        {
            GameManager.Instance.netFlightSpeed = GameManager.Instance.flightSpeed * 0.35f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FireLazer();
        if (GameManager.Instance.lazerCd && GameManager.Instance.lazerCdTime <= 0 && !GameManager.Instance.lazerActive)
        {
            GameManager.Instance.lazerCd = false;
            GameManager.Instance.lazerCdTime = 1f;
        }
        else if (GameManager.Instance.lazerCdTime > 0)
        {
            GameManager.Instance.lazerCdTime -= 0.02f/GameManager.Instance.lazerDmg;
        }

        if (Input.GetKey(rocket) && GameManager.Instance.missleUnlocked && GameManager.Instance.missileCdTime <= 0)
        {
            if (chargeValue < GameManager.Instance.missleLimit)
            {
                GameManager.Instance.charging = true;
                chargeValue += 0.04f + GameManager.Instance.missleLimit / 1000;
                //Debug.Log(chargeValue);
            }

        }
        else if (GameManager.Instance.missileCdTime > 0)
        {
            GameManager.Instance.missileCdTime -= 0.02f;
        }
        GameManager.Instance.shotsCount = Mathf.RoundToInt(chargeValue);
        //Debug.Log(GameManager.Instance.shotsCount);
    }

    void FireLazer()
    {
        if (GameManager.Instance.lazerActive && GameManager.Instance.lazerActiveHeat < GameManager.Instance.lazerHeat)
        {
            lazer.transform.position = muzzle.transform.position;
            lazer.gameObject.SetActive(true);
            GameManager.Instance.lazerActiveHeat += 0.04f;
        }
        else
        {
            lazer.gameObject.SetActive(false);
            if (!GameManager.Instance.lazerActive && GameManager.Instance.lazerActiveHeat > 0)
            {
                GameManager.Instance.lazerActiveHeat -= GameManager.Instance.lazerHeat * 0.005f;
            }
            if (GameManager.Instance.lazerActiveHeat >= GameManager.Instance.lazerHeat)
            {
                GameManager.Instance.lazerCd = true;
                GameManager.Instance.lazerCdTime = 3f;
            }
        }
    }

    void FireMissle()
    {
        GameManager.Instance.shotsCount = Mathf.RoundToInt(chargeValue);
        for (int i = 0; i < GameManager.Instance.shotsCount; i++)
        {
            Instantiate(missle, gunMuzzle[UnityEngine.Random.Range(0,1)].position, Quaternion.Euler(new Vector3(UnityEngine.Random.Range(-90, 20), UnityEngine.Random.Range(90,-90), UnityEngine.Random.Range(-180, 180))));
            GameManager.Instance.missileCdTime = 30f;
        }
    }
}
