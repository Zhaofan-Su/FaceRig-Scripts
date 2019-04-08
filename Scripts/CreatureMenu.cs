using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Sprites;
using UnityEngine.EventSystems;

public class CreatureMenu : MonoBehaviour
{
    List<Sprite> creatures = new List<Sprite>();
    string fullPath = "Assets/Resources/Images/Creatures/";
    int left = 0;
    int middle = 1;
    int right = 2;
    //背景总数
    int sum;

    // Start is called before the first frame update
    void Start()
    {
        LoadCreatures();
        sum = creatures.Count;
        //GameObject.Find("BackGround/Canvas/CreatureMenu/LeftFrame/LeftPic").GetComponent<Image>().sprite = creatures[left];
        //GameObject.Find("BackGround/Canvas/CreatureMenu/MiddleFrame/MiddlePic").GetComponent<Image>().sprite = creatures[middle];
        //GameObject.Find("BackGround/Canvas/CreatureMenu/RightFrame/RightPic").GetComponent<Image>().sprite = creatures[right];
        GameObject.Find("BackGround/Canvas/CreatureMenu").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadCreatures()
    {
        //加载所有的角色
        List<string> names = new List<string>();
        if (Directory.Exists(fullPath))
        {
            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                {
                    names.Add(files[i].Name.Substring(0, files[i].Name.IndexOf(".")));
                }
            }
        }
        for (int i = 0; i < names.Count; i++)
        {
            creatures.Add(Resources.Load("Images/Creatures/" + names[i], typeof(Sprite)) as Sprite);
        }
    }

    public void RightButtonClick()
    {
        right = middle;
        middle = left;
        left = left - 1;
        if (left < 0)
        {
            left = sum - 1;
        }
        //重新渲染图片
        GameObject.Find("BackGround/Canvas/CreatureMenu/LeftFrame/LeftPic").GetComponent<Image>().sprite = creatures[left];
        GameObject.Find("BackGround/Canvas/CreatureMenu/MiddleFrame/MiddlePic").GetComponent<Image>().sprite = creatures[middle];
        GameObject.Find("BackGround/Canvas/CreatureMenu/RightFrame/RightPic").GetComponent<Image>().sprite = creatures[right];
    }
    public void LeftButtonClick()
    {
        left = middle;
        middle = right;
        right = right + 1;
        if (right >= sum)
        {
            right = 0;
        }
        //重新渲染图片
        GameObject.Find("BackGround/Canvas/CreatureMenu/LeftFrame/LeftPic").GetComponent<Image>().sprite = creatures[left];
        GameObject.Find("BackGround/Canvas/CreatureMenu/MiddleFrame/MiddlePic").GetComponent<Image>().sprite = creatures[middle];
        GameObject.Find("BackGround/Canvas/CreatureMenu/RightFrame/RightPic").GetComponent<Image>().sprite = creatures[right];
    }
}
