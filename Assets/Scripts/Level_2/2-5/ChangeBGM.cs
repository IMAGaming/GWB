using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBGM : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            MusicMgr.Instance.bgmSource.clip = MusicMgr.Instance.level_Music[3];
            MusicMgr.Instance.bgmSource.Play();
            MusicMgr.Instance.bgmSource.loop = true;
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
