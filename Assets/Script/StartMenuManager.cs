using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private Sprite mat1;
    [SerializeField] private Sprite mat2;
    [SerializeField] private GameObject panel;
    public GameObject[] Pages;
    public int pageNo;
    public bool guide = false;
    public bool sceneswitch;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            guide = !guide;
        }
        int i = 0;
        if (guide == true)
        {
            for (i = 0; i < Pages.Length; i++)
            {
                Pages[i].gameObject.SetActive(true);
            }

            if (Input.GetMouseButtonDown(0) && pageNo < Pages.Length-1)
            {
                Pages[pageNo].gameObject.transform.position += new Vector3(0,0,1);
                Pages[pageNo+1].gameObject.transform.position += new Vector3(0, 0, -1);
                pageNo++;
            }
            else if (Input.GetMouseButtonDown(0) && pageNo > 0)
            {
                Pages[pageNo].gameObject.transform.position += new Vector3(0, 0, 1);
                Pages[pageNo-1].gameObject.transform.position += new Vector3(0, 0, -1);
                pageNo--;
            }
        }
        else if (!guide)
        {
            for (i = 0; i < Pages.Length; i++)
            {
                Pages[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnMouseOver()
    {
        if (!guide)
        {
            panel.GetComponent<Image>().sprite = mat2;
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Build1");
            }
        }
    }
    private void OnMouseExit()
    {
        panel.GetComponent<Image>().sprite = mat1;
    }
}
