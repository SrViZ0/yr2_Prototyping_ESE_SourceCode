using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Variables")]
    public float flightSpeed = 80f;
    public float netFlightSpeed;
    public bool lockOnTarget;
    public float PlayerHp = 10;
    public float PlayerActiveHp;
    public float bulletSpeed = 400f;
    public float bulletDamage = 2f;
    public bool modularGunEquip = false;
    public float shakeMagintude = 0.5f;
    public int screenShake;
    public bool boosting = false;
    public bool disabled = false;
    public float disabledCdTime = 3f;


    [Header("Spec abilities Variables")]

    public bool lazerUnlocked = false; // lazer section
    public float lazerHeat = 5f;
    public float lazerActiveHeat = 0f;
    public float lazerDmg = 0.5f;
    public float lazerCdTime = 0f;
    public bool lazerCd;
    public bool lazerActive = false;


    public bool magnetUnlocked = false; // magnet section
    public float magnetRange = 5f;

    public bool missleUnlocked = false;
    public int missleLimit = 1;
    public bool charging;
    public int shotsCount;
    public float missileCdTime;


    [Header("Stage Variables")]
    public GameObject enemyPrefab, enemy2Prefab;
    public GameObject[] rockPrefab;
    public float waveTimer = 30f; // time for a wave
    public float waveCd = 120f; // time for next wave
    public int waveCount = 0;
    public float gameTimer;
    public int enemyCount;
    public int rockCount;
    public bool gameOver = false;
    public int finalScore;
    public int enemyKilled;

    [Header("GameState")]
    public bool buffSelect = false;
    public bool guide = false;
    public GameObject endScene;


    [Header("Buffs Variables")]
    public int buffAvailable = 0;
    public int buffCount = 0;
    private int buff1, buff2, buff3;
    public int slot1, slot2, slot3;
    public float bonusBuffGuage = 0;

    [Header("Ui Elements")]
    public GameObject[] Pages;
    public GameObject chipSlot1, chipSlot2, chipSlot3;
    public int pageNo;
    public AudioClip clicc;


    //I could add a list to organise but.. ehh
    void Start()
    {
        Instance= this;
        PlayerActiveHp = PlayerHp;
    }

    void Update()
    {
        if (bonusBuffGuage >= 100 * (1+buffCount/10 ))
        {
            buffAvailable++;
            bonusBuffGuage -= 100 * (1 + buffCount / 10);
        }
        //slowgame
        if (buffSelect || guide || gameOver)
        {
            PauseGame();
            GameObject.FindGameObjectWithTag("Playercam").GetComponent<PlayerControl>().enabled = false;        
        }
        else if (!buffSelect || !guide || !gameOver)
        {
            ResumeGame();
            GameObject.FindGameObjectWithTag("Playercam").GetComponent<PlayerControl>().enabled = true;
        }
        if (buffAvailable > 0) 
        { 
            buffSelect= true;
        }
        else
        {
            buffSelect= false;
        }

        var input = Input.inputString; //Selection Mechanism
        if (buffSelect && !gameOver) 
        {
            chipSlot1.gameObject.SetActive(true);
            chipSlot2.gameObject.SetActive(true);
            chipSlot3.gameObject.SetActive(true);

            BuffRolls();//Make RNG

            // Other Input Option
            if (chipSlot1.GetComponent<ChipScriptSlot1>().select1 == true)
            {
                input = "1";
            }
            if (chipSlot2.GetComponent<ChipScriptSlot2>().select2 == true)
            {
                input = "2";
            }
            if (chipSlot3.GetComponent<ChipScripSlot3>().select3 == true)
            {
                input = "3";
            }

            switch (input) // braindamage aka Assigning buffs.
            {
                case "1":
                    Buff_Slot_1_Apply();
                    chipSlot1.GetComponent<ChipScriptSlot1>().select1 = false;

             Reset: buff1 = 0; //Common return point
                    buff2 = 0;
                    buff3 = 0;
                    buffAvailable--;
                    buffCount++;

                    break;

                case "2":
                    Buff_Slot_2_Apply();
                    chipSlot2.GetComponent<ChipScriptSlot2>().select2 = false;
                    goto Reset;  

                case "3":
                    Buff_Slot_3_Apply();
                    chipSlot3.GetComponent<ChipScripSlot3>().select3 = false;
                    goto Reset;
            }
        }
        else //Deactivate Selection
        {
            chipSlot1.gameObject.SetActive(false);
            chipSlot2.gameObject.SetActive(false);
            chipSlot3.gameObject.SetActive(false);
        }


        if (PlayerActiveHp >= PlayerHp) 
        {
            PlayerActiveHp = PlayerHp;
        }
        if (PlayerActiveHp <= 0) 
        {
            //Destroy(GameObject.FindGameObjectWithTag("Player"));
            GameObject player = GameObject.FindGameObjectWithTag("Player"); player.SetActive(false);
            gameOver= true;
        }

        if (lazerActiveHeat < 0) 
        { 
            lazerActiveHeat = 0;
        }

        //Guide menu toggle
        if (Input.GetKeyUp(KeyCode.Escape) && !buffSelect)
        {
            GetComponent<AudioSource>().PlayOneShot(clicc, 1);
            guide = !guide;
        }
        if (buffSelect) { guide = false; }
        GuideMenu();
        if (!gameOver)
        {
            gameTimer += Time.unscaledDeltaTime;
        }

        if (PlayerHp <= 0 || gameOver == true)
        {
            finalScore = Mathf.RoundToInt((float)buffCount * (float)enemyKilled * (float)waveCount - gameTimer*(waveCount/20));
            if (finalScore < 0) 
            { 
                finalScore = 0;
            }
            endScene.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        if (enemyCount == 0 && waveTimer >= 30)
        {
            waveTimer = 30f;
        }
        if (waveTimer <= 0/* && enemyCount == 0*/)
        {
            //Buff handling, i wanna die while coding this, Pray to god pls dont have bugs
            //Updt: thank god it had no bugs
            buffAvailable++;

            waveCount++;
            SpawnEnemy();
            waveTimer = waveCd + waveCount * 2;
        }
        else
        { waveTimer -= 0.02f; }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length-1;

        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Rocks");
        rockCount = rocks.Length;

        if (rockCount < 300 ) 
        {
            Instantiate(rockPrefab[Random.Range(0,15)],GameObject.FindGameObjectWithTag("MainCamera").transform.position + new Vector3(Random.Range(1000,-100), Random.Range(1000, -1000), Random.Range(1000, -1000)),Quaternion.identity,transform);
        }

        //Debug.Log("enemyCount:" + enemyCount);
        //for (int i = 0; i < enemies.Length; i++)
        //{

        //}
        if (disabled)
        {
            if (disabledCdTime <= 0)
            {
                disabled = false;
                disabledCdTime = 3f;
            }
            disabledCdTime -= 0.02f;
        }
    }
    void SpawnEnemy()
    {
        if (waveCount >= 15)
        {
            Instantiate(enemy2Prefab, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)), Quaternion.Euler(new Vector3(Random.Range(-180, -45), Random.Range(-180, -45), Random.Range(-180, -45))));
        }
        for (int i = 0; i< waveCount+(waveCount/3); i++) 
        {
            Instantiate(enemyPrefab, GameObject.FindGameObjectWithTag("Player").transform.position +new Vector3(Random.Range(-1000,1000), Random.Range(-1000, 1000), Random.Range(-1000, 1000)), Quaternion.Euler(new Vector3(Random.Range(-180, -45), Random.Range(-180, -45), Random.Range(-180, -45))));
        }  
    }

    void BuffRolls() //Rng Machine
    {
        while (buff1 == buff2 || buff3 == buff1 || buff3 == buff2) //check for dupe buffs
        {
            //Debug.Log("rollling");

            //Equal R8 for all buffs
            buff1 = Random.Range(0, 9);
            buff2 = Random.Range(0, 9);
            buff3 = Random.Range(0, 9);
        }
        slot1 = buff1;
        slot2 = buff2;
        slot3 = buff3;


        //Debug.Log("Slot1: " + slot1);
        //Debug.Log("Slot2: " + slot2);
        //Debug.Log("Slot3: " + slot3);
    }

    void Buff_Slot_1_Apply()
    {
            switch (slot1)
            // Prolly could made this into a method that returned buff index & accesed that common data through dictionary or whatver to make the code less eye pain
            // but ehh It works bug free :).
            // Note: adjust the case value(float) to change rate for each drop
            {
                case 0:
                    flightSpeed += 5;
                    break;
                case 1:
                    bulletSpeed += 50f;
                bulletDamage += 0.5f;
                break;
                case 2:
                    PlayerHp += 5;
                    PlayerActiveHp += 5;
                break;
                case 3:
                    PlayerActiveHp += PlayerHp * 0.25f;
                break;
                case 4:
                    PlayerActiveHp += PlayerHp*0.5f;
                break;
                case 5:
                    PlayerActiveHp += PlayerHp;            
                break;
                case 6:
                    modularGunEquip = true;
                    if (lazerUnlocked) 
                    {
                    lazerHeat += 0.5f;
                    lazerDmg += 0.2f;
                    }
                    lazerUnlocked = true;
                break;
                case 7:
                    if (magnetUnlocked) 
                    {
                        magnetRange += 10;
                    }
                    magnetUnlocked = true;
                break;
                case 8:
                    missleUnlocked= true;
                    if (missleUnlocked) 
                    {
                    missleLimit += 1;
                    }
                break;
            }
    }

    void Buff_Slot_2_Apply()
    {
        switch (slot2)
        {
            case 0:
                flightSpeed += 5;
                break;
            case 1:
                bulletSpeed += 50f;
                bulletDamage += 0.7f;
                break;
            case 2:
                PlayerHp += 5;
                PlayerActiveHp += 5;
                break;
            case 3:
                PlayerActiveHp += PlayerHp * 0.25f;
                break;
            case 4:
                PlayerActiveHp += PlayerHp * 0.5f;
                break;
            case 5:
                PlayerActiveHp += PlayerHp;
                break;
            case 6:
                modularGunEquip = true;
                if (lazerUnlocked)
                {
                    lazerHeat += 0.5f;
                    lazerDmg += 0.2f;
                }
                lazerUnlocked = true;
                break;
            case 7:
                if (magnetUnlocked)
                {
                    magnetRange += 10;
                }
                magnetUnlocked = true;
                break;
            case 8:
                missleUnlocked = true;
                if (missleUnlocked)
                {
                    missleLimit += 1;
                }
                break;
        }
    }
    void Buff_Slot_3_Apply()
    {
        switch (slot3)
        {
            case 0:
                flightSpeed += 5;
                break;
            case 1:
                bulletSpeed += 50f;
                bulletDamage += 0.5f;
                break;
            case 2:
                PlayerHp += 5;
                PlayerActiveHp += 5;
                break;
            case 3:
                PlayerActiveHp += PlayerHp * 0.25f;
                break;
            case 4:
                PlayerActiveHp += PlayerHp * 0.5f;
                break;
            case 5:
                PlayerActiveHp += PlayerHp;
                break;
            case 6:
                modularGunEquip = true;
                if (lazerUnlocked)
                {
                    lazerHeat += 0.5f;
                    lazerDmg += 0.2f;
                }
                lazerUnlocked = true;
                break;
            case 7:
                if (magnetUnlocked)
                {
                    magnetRange += 10;
                }
                magnetUnlocked = true;
                break;
            case 8:
                missleUnlocked = true;
                if (missleUnlocked)
                {
                    missleLimit += 1;
                }
                break;
        }
    }

    public void PauseGame()
    {
            Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
            Time.timeScale = 1f;    
    }
    
    public void GuideMenu()
    {
        if (guide)
        {
            GameObject.FindGameObjectWithTag("Playercam").GetComponent<PlayerControl>().enabled = false;
            for (int i = 0; i < Pages.Length; i++) 
            {
                Pages[i].gameObject.SetActive(true);
            }

            //if (Input.GetMouseButtonDown(0) && pageNo < Pages.Length - 1) //Old Code For Reference
            //{
            //    pageNo++;
            //    Pages[1].gameObject.transform.position += new Vector3(0, 0, -1);
            //}
            //if (Input.GetMouseButtonDown(1) && pageNo != 0)
            //{
            //    pageNo--;
            //    Pages[1].gameObject.transform.position += new Vector3(0, 0, +1);
            //}

            if (Input.GetMouseButtonDown(0) && pageNo < Pages.Length - 1)
            {
                Pages[pageNo].gameObject.transform.position += new Vector3(0, 0, 1);
                Pages[pageNo + 1].gameObject.transform.position += new Vector3(0, 0, -1);
                pageNo++;
            }
            else if (Input.GetMouseButtonDown(0) && pageNo > 0)
            {
                Pages[pageNo].gameObject.transform.position += new Vector3(0, 0, 1);
                Pages[pageNo - 1].gameObject.transform.position += new Vector3(0, 0, -1);
                pageNo--;
            }
        }
        else if (!guide)
        {
            GameObject.FindGameObjectWithTag("Playercam").GetComponent<PlayerControl>().enabled = true;
            for (int i = 0; i < Pages.Length; i++)
            {
                Pages[i].gameObject.SetActive(false);
            }
           
        }
    }
}
// I did not plan for this to be this long (30/7)
//Damn this keeps getting longer(3/8)
