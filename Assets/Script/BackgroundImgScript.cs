using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImgScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI badgeText;
    public RawImage BadgeSlot;
    public Texture[] badge;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameOver)
        {
            var color = GetComponent<RawImage>().color;
            color.a += Time.unscaledDeltaTime * 0.5f;
            GetComponent<RawImage>().color = color;
        }
        else
        {
            var color = GetComponent<RawImage>().color;
            color.a = 0;
            GetComponent<RawImage>().color = color;
        }
        scoreText.text = ("Score \n " + GameManager.Instance.finalScore);
        switch (GameManager.Instance.finalScore) 
        {
            case >= 150000:BadgeSlot.texture = badge[5]; badgeText.text = ("Impossible!!!\n Performance Rank: ???"); break;
            case >= 25000: BadgeSlot.texture = badge[4]; badgeText.text = ("Fantastic!!\n Performance Rank: Master"); break;
            case >= 7500: BadgeSlot.texture = badge[3];  badgeText.text = ("Impressive!\n Performance Rank: Expert"); break;
            case >= 3000: BadgeSlot.texture = badge[2];  badgeText.text = ("Almost There!\n Performance Rank: Elite"); break;
            case >= 1000: BadgeSlot.texture = badge[1];  badgeText.text = ("Getting There!\n Performance Rank: Veteran"); break;
            default: BadgeSlot.texture = badge[0];       badgeText.text = ("Nice Try!\n Performance Rank: Rookie"); break;
        }

    }
}
