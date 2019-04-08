using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreatureButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreatureButtonClick()
    {
        GameObject sceneButton = GameObject.Find("BackGround/Canvas/Top/SceneButton");
        GameObject creatureButton = GameObject.Find("BackGround/Canvas/Top/CreatureButton");
        if (creatureButton.tag == "off")
        {
            if (sceneButton.tag == "on")
            {
                //场景按钮被按下
                sceneButton.GetComponent<Image>().color = new Color(194f / 255, 194f / 255, 194f / 255, 0f / 255);
                sceneButton.tag = "off";
                GameObject.Find("BackGround/Canvas/SceneMenu").SetActive(false);
            }
            GameObject.Find("BackGround/Canvas/CreatureMenu").SetActive(true);
            creatureButton.GetComponent<Image>().color = new Color(194f / 255, 194f / 255, 194f / 255, 40f / 255);
            creatureButton.tag = "on";
        }
        else if (creatureButton.tag == "on")
        {
            GameObject.Find("BackGround/Canvas/CreatureMenu").SetActive(false);
            creatureButton.GetComponent<Image>().color = new Color(194f / 255, 194f / 255, 194f / 255, 0f / 255);
            creatureButton.tag = "off";
        }
    }
}
