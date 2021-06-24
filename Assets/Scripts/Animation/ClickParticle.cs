using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClickParticle : MonoBehaviour
{
    private Camera cam;
    private bool isDrag = false;
    private ParticleSystem particle;
    
    private void Start()
    {
        cam = Camera.main;
        particle = transform.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        //    transform.position = new Vector3(pos.x,pos.y,-5f);
        //    particle.Play();
        //}
        if(Input.GetMouseButtonUp(1))
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, -5f);
            particle.Play();
            MusicMgr.Instance.PlaySound(MusicMgr.Instance.clickMusic,false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, -5f);
            particle.Play();
        }

        if(Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }

        if(isDrag)
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, -5f);
        }

    }
}
