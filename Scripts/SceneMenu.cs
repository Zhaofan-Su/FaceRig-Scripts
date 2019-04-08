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

public class SceneMenu : MonoBehaviour
{
    List<Sprite> backgrounds = new List<Sprite>();
    string fullPath = "Assets/Resources/Images/Backgrounds/";
    int left = 0;
    int middle = 1;
    int right = 2;
    //背景总数
    int sum;
   
    // Start is called before the first frame update
    void Start()
    {
        LoadBackgrounds();
        sum = backgrounds.Count;
        GameObject.Find("BackGround/Canvas/SceneMenu/LeftFrame/LeftPic").GetComponent<Image>().sprite = backgrounds[left];
        GameObject.Find("BackGround/Canvas/SceneMenu/MiddleFrame/MiddlePic").GetComponent<Image>().sprite = backgrounds[middle];
        GameObject.Find("BackGround/Canvas/SceneMenu/RightFrame/RightPic").GetComponent<Image>().sprite = backgrounds[right];
        GameObject.Find("BackGround/Canvas/SceneMenu").SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadBackgrounds()
    {
        //加载所有的背景
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
            backgrounds.Add(Resources.Load("Images/Backgrounds/" + names[i], typeof(Sprite)) as Sprite);
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
        GameObject.Find("BackGround/Canvas/SceneMenu/LeftFrame/LeftPic").GetComponent<Image>().sprite = backgrounds[left];
        GameObject.Find("BackGround/Canvas/SceneMenu/MiddleFrame/MiddlePic").GetComponent<Image>().sprite = backgrounds[middle];
        GameObject.Find("BackGround/Canvas/SceneMenu/RightFrame/RightPic").GetComponent<Image>().sprite = backgrounds[right];
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
        GameObject.Find("BackGround/Canvas/SceneMenu/LeftFrame/LeftPic").GetComponent<Image>().sprite = backgrounds[left];
        GameObject.Find("BackGround/Canvas/SceneMenu/MiddleFrame/MiddlePic").GetComponent<Image>().sprite = backgrounds[middle];
        GameObject.Find("BackGround/Canvas/SceneMenu/RightFrame/RightPic").GetComponent<Image>().sprite = backgrounds[right];
    }

    
    public void ChooseBackground()
    {
        //更换背景图像

        GameObject.Find("BackGround/Canvas/BackgroundImage").GetComponent<Image>().sprite = backgrounds[middle];

    }
}
