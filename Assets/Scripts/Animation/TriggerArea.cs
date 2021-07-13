using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private bool isTriggerOnce = false;

    public UnityEvent triggerEvent;
    public UnityEvent triggerExitEvent;

    private bool isTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(isTriggerOnce && isTrigger)
            {
                return;
            }
            triggerEvent?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            triggerExitEvent?.Invoke();
            isTrigger = true;
        }
    }
}
