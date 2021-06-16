using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSwitch : MonoBehaviour
{
    
    void SwitchImage1Order()
    {
        GameObject Image1;
        GameObject Image2;
        Image1 = GameObject.FindGameObjectWithTag("前");
        Image2 = GameObject.FindGameObjectWithTag("后");
        Image1.GetComponent<SpriteRenderer>().sortingOrder = 2;
        Image2.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    void SwitchImage2Order()
    {
        GameObject Image1;
        GameObject Image2;
        Image1 = GameObject.FindGameObjectWithTag("前");
        Image2 = GameObject.FindGameObjectWithTag("后");
        Image1.GetComponent<SpriteRenderer>().sortingOrder = 3;
        Image2.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
