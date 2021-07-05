using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D),typeof(Rigidbody2D))]
public class AudioCollider : MonoBehaviour
{
    public Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AudioTrigger"))
        {
            AudioClip clip = collision.GetComponent<AudioTrigger>().targetClip;
            MusicMgr.Instance.PlaySound(clip, false);
        }
    }
}
