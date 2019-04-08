using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneButtonClick()
    {
        GameObject sceneButton = GameObject.Find("BackGround/Canvas/Top/SceneButton");
        GameObject creatureButton = GameObject.Find("BackGround/Canvas/Top/CreatureButton");
        if (sceneButton.tag == "off")
        {
            if (creatureButton.tag == "on")
            {
                //角色按钮被按下
                creatureButton.GetComponent<Image>().color = new Color(194f / 255, 194f / 255, 194f / 255, 0f / 255);
                creatureButton.tag = "off";
                GameObject.Find("BackGround/Canvas/CreatureMenu").SetActive(false);
            }
            GameObject.Find("BackGround/Canvas/SceneMenu").SetActive(true);
            GameObject.Find("BackGround/Canvas/SceneMenu").SetActive(true);
            sceneButton.GetComponent<Image>().color = new Color(194f / 255, 194f / 255, 194f / 255, 40f / 255);
            sceneButton.tag = "on";
        }
        else if (sceneButton.tag == "on")
        {
            GameObject.Find("BackGround/Canvas/SceneMenu").SetActive(false);
            sceneButton.GetComponent<Image>().color = new Color(194f / 255, 194f / 255, 194f / 255, 0f / 255);
            sceneButton.tag = "off";
        }
    }
}
