using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceBarScript : MonoBehaviour
{
    public GameObject bonusGuage;
    public GameObject chargeGuage;
    public GameObject hpGuage;

    public TextMeshProUGUI lazerCdDisp;
    public TextMeshProUGUI missileCdDisp;

    // I like my UI scalable with screensize
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lazerCdDisp.text = (Convert.ToString(Mathf.Round(GameManager.Instance.lazerCdTime*10.0f) * 0.1)/*+"s"*/);
        missileCdDisp.text = (Convert.ToString(Mathf.Round(GameManager.Instance.missileCdTime * 10.00f) * 0.1)/* +"s"*/);

        transform.localScale = new Vector3((GameManager.Instance.lazerActiveHeat/GameManager.Instance.lazerHeat)*1f,1f,1f);

        bonusGuage.transform.localScale = new Vector3(GameManager.Instance.bonusBuffGuage / (100 * (1 + GameManager.Instance.buffCount / 10)) * 17, 0.2f, 0.2f);

        chargeGuage.transform.localScale = new Vector3((float)GameManager.Instance.shotsCount / (float)GameManager.Instance.missleLimit, 1f, 1f);

        hpGuage.transform.localScale = new Vector3(GameManager.Instance.PlayerActiveHp/GameManager.Instance.PlayerHp, 1f, 1f);
    }
}
