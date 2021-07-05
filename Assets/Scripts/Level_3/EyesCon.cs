using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesCon : MonoBehaviour
{
    
    public Animator Anim1;
    public Animator Anim2;
    public Animator Anim3;

    private void Start()
    {
        StartCoroutine(Blink());
    }
    IEnumerator Blink()
    {
        Anim1.gameObject.SetActive(true);
        Anim1.SetTrigger("Blink1");
        yield return new WaitForSeconds(3f);
        Anim1.gameObject.SetActive(false);

        Anim2.gameObject.SetActive(true);
        Anim2.SetTrigger("Blink2");
        yield return new WaitForSeconds(3f);
        Anim2.gameObject.SetActive(false);

        Anim3.gameObject.SetActive(true);
        Anim3.SetTrigger("Blink3");
        yield return new WaitForSeconds(3f);
        Anim3.gameObject.SetActive(false);

        StartCoroutine(Blink());
    }

}
