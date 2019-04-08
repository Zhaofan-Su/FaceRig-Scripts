using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Sprites;

public class SettingsButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //设置菜单禁用
        GameObject.Find("BackGround/Canvas/SettingMenu").SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeIcon()
    {
        GameObject obj = GameObject.Find("BackGround/Canvas/Top/SettingButton");
        if (obj.tag == "off")
        {
            //设置按钮未被选中，改为选中状态
            obj.GetComponent<Image>().sprite = Resources.Load("Images/UI/setting_icon_select", typeof(Sprite)) as Sprite;
            obj.GetComponent<Image>().rectTransform.sizeDelta=new Vector2(40, 40);
            GameObject.Find("BackGround/Canvas/SettingMenu").SetActive(true);
            obj.tag = "on";
        }
        else if(obj.tag=="on")
        {
            //已被选中，改为未被选中状态
            obj.GetComponent<Image>().sprite = Resources.Load("Images/UI/setting_icon", typeof(Sprite)) as Sprite;
            obj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(30, 30);
            GameObject.Find("BackGround/Canvas/SettingMenu").SetActive(false);
            obj.tag = "off";
        }
    }
}
