using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSwitch : MonoBehaviour
{
    public GameObject Word0;
    GameObject Word1;
    GameObject Word2;
    GameObject Word3;
    GameObject Word4;
    bool Stop;

    void Start()
    {
        Word1 = this.transform.Find("新手关文字").gameObject;
        Word2 = this.transform.Find("第一关文字").gameObject;
        Word3 = this.transform.Find("第二关文字").gameObject;
        Word4 = this.transform.Find("第三关文字").gameObject;
        
    }

    public void SwitchWord(int Number)
    {
        Stop = GameObject.Find("旋转点").GetComponent<RotateImage>().isStop;
        if (Stop == true)
        {
            switch (Number)
            {
                case 1:
                    Word0.GetComponent<SpriteRenderer>().sprite = Word1.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 2:
                    Word0.GetComponent<SpriteRenderer>().sprite = Word2.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 3:
                    Word0.GetComponent<SpriteRenderer>().sprite = Word3.GetComponent<SpriteRenderer>().sprite;
                    break;
                case 4:
                    Word0.GetComponent<SpriteRenderer>().sprite = Word4.GetComponent<SpriteRenderer>().sprite;
                    break;
            }
        }
        
    }
}
