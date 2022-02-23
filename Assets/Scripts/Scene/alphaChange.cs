using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class alphaChange : MonoBehaviour
{
    //GameObject gam01;
    
    Color col01 = new Color(1, 1, 1, 0);
    Color col02 = new Color(1, 1, 1, 1);
    SpriteRenderer col03;

    public Animator anim_UI;
    public Animator anim_RBottle;
    //GameObject gam01;
    public Sprite item_UI;
    
    void Start()
    {
        col03 = this.GetComponent<SpriteRenderer>(); 
    }

    // Update is called once per frame
    void Update()
    {
        changeAlpha();
        Debug.Log("123");
    }

    void changeAlpha()
    {       
        //col03.color = Color.Lerp(col02, col01, Time.time);        
        StartCoroutine(changeImage()); 
    }

    //无论拾取的是酒壶还是笛子，都能够用这个代码。物品栏的物品出现时要用代码控制渐变还是用动画来控制。如果用动画来控制，那就要用到协程了
    IEnumerator changeImage()
    {
        GameObject image01;
        GameObject gam02;
        image01 = GameObject.FindWithTag("UI_Image");
        gam02 = GameObject.FindWithTag("搜寻后");

        anim_RBottle.SetTrigger("Start_GameObject");

        //先换图片，然后再播放动画吧
        //Color col = new Color(1, 1, 1, 1);
        image01.GetComponent<Image>().sprite = item_UI;
        anim_UI.SetTrigger("Start_UI");
 
        //image01.GetComponent<Image>().color = col;

        yield return new WaitForSeconds(1.3f);

        gam02.SetActive(false);

    }
}
