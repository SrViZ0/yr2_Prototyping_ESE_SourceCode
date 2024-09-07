using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChipScriptSlot2 : MonoBehaviour
{
    private float speed;
    [SerializeField] private GameObject canvasCamera;
    [SerializeField] private RawImage Display1;
    public TextMeshProUGUI displayText;
    public bool select2 = false;

    public AudioClip thocc;
    public AudioClip clicc;

    [Header("Textures")]
    [SerializeField]private Texture[] Icons;
    public Material outline;

    void Start()
    {
        GetComponent<MeshRenderer>().materials[1].shader = null;
        speed = Random.Range(50, 50);
    }
        
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, speed) * Time.unscaledDeltaTime, Space.Self);
        displayText.transform.LookAt(-canvasCamera.transform.position);

        switch (GameManager.Instance.slot2)
        {
            case 0:
                Display1.texture = Icons[0];
                displayText.text = ("Speed+");
                break;
            case 1:
                Display1.texture = Icons[1];
                displayText.text = ("Bullet+");
                break;
            case 2:
                Display1.texture = Icons[2];
                displayText.text = ("Max HP+");
                break;
            case 3:
                Display1.texture = Icons[3];
                displayText.text = ("25% Heal");
                break;
            case 4:
                Display1.texture = Icons[4];
                displayText.text = ("50% Heal");
                break;
            case 5:
                Display1.texture = Icons[5];
                displayText.text = ("100% Heal");
                break;
            case 6:
                Display1.texture = Icons[6];
                displayText.text = ("Lazer+");
                break;
            case 7:
                Display1.texture = Icons[7];
                displayText.text = ("Magnet+");
                break;
            case 8:
                Display1.texture = Icons[8];
                displayText.text = ("Missile+");
                break;
        }
    }
    private void OnMouseEnter()
    {
        GetComponent<AudioSource>().PlayOneShot(thocc, Random.Range(0.9f, 1));
    }
    private void OnMouseOver()
    {
        GetComponent<MeshRenderer>().materials[1].shader = outline.shader;
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().PlayOneShot(clicc, 1);
            select2 = true;
        }
    }
    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().materials[1].shader = null;
    }
}
