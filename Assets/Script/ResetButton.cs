using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Material resetBG;
    public Color resetBGCol;
    // Start is called before the first frame update
    void Start()
    {
        resetBGCol=resetBG.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        var color = resetBG.color;
        color.a = 1;
        resetBG.color = color;
        if (Input.GetMouseButtonDown(0)) { SceneManager.LoadScene("StartMenu"); resetBG.color = resetBGCol; }
    }
    private void OnMouseExit()
    {
        resetBG.color = resetBGCol;
    }

}
