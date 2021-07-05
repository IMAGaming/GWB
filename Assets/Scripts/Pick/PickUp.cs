using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    
    //public GameObject image01;
    //public GameObject Button01; //传送门旁边的按钮  
    public bool IsPick;
    
    void Start()
    {
        IsPick = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        //GameObject gam01;

        /*if (collision.tag == "搜寻物")
        {
            Debug.Log("捡到了");

            collision.gameObject.SetActive(false);
            Color col = new Color(1, 1, 1, 1); //rgb都为1即完全显示那个图片的颜色
            image01.GetComponent<Image>().color = col; //不可以直接改color里面的某个变量值，要通过赋值的方式整体一起改
            image01.GetComponent<Image>().sprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;

            IsPick = true;
        }*/

        if(collision.tag == "搜寻前")
        {
            
            Vector3 vec01 = new Vector3(1, 1, 1);
            Vector3 vec02 = new Vector3(2, 2, 1);

            collision.gameObject.GetComponent<Animator>().SetTrigger("alphaChange");
            IsPick = true;

        }
        
        //与传送门交互。当人物捡到东西然后触碰传送门，将会出现一个按钮
        /*if (collision.tag == "传送门")
        {
            if(IsPick == true)
            {
                //GameObject Button01;
                //Button01 = GameObject.FindGameObjectWithTag("clickButton");
                Button01.SetActive(true);
            }
                                 
        }     */  
    }

    //当人物触发按钮后，离开传送门，按钮将会自动消失
   /* private void OnTriggerExit2D(Collider2D collision)
    {
        Button01.SetActive(false);
    }*/

}
