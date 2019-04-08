using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Sprites;
using UnityEngine.EventSystems;

public class VolumeButton : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeVolume()
    {
        //背景音乐
        GameObject audio = GameObject.Find("BackgroundMusic");
        AudioSource auso = audio.GetComponent<AudioSource>();

        //背景音乐开关按钮
        GameObject obj = GameObject.Find("BackGround/Canvas/Top/VolumeButton");
        if(obj.tag == "on")
        {
            
            obj.GetComponent<Image>().sprite = Resources.Load("Images/UI/off",typeof(Sprite)) as Sprite;
            obj.tag = "off";
            
            if (auso.isPlaying)
            {
                auso.Pause();
            }
        }
        else if(obj.tag == "off")
        {
            obj.GetComponent<Image>().sprite = Resources.Load("Images/UI/on", typeof(Sprite)) as Sprite;
            obj.tag = "on";
            if (!auso.isPlaying)
            {
                auso.Play();
            }
        }
    }
}
