using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGame : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameObject vp = GameObject.Find("VideoPlayer");
        vp.GetComponent<CGPlayer>().PlayCG();
    }

}
