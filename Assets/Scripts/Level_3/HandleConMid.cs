using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleConMid : HandleCon
{
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (isClick == false)
        {
            if (collision.gameObject.name == "11")
            {
                PuzzleCon.instance.Mid[0] = true;

            }

            if (collision.gameObject.name == "22")
            {
                PuzzleCon.instance.Mid[1] = true;

            }

            if (collision.gameObject.name == "33")
            {
                PuzzleCon.instance.Mid[2] = true;

            }
            if (collision.gameObject.name == "44")
            {
                PuzzleCon.instance.Mid[3] = true;


            }
            if (collision.gameObject.name == "55")
            {
                PuzzleCon.instance.Mid[4] = true;

            }
            if (collision.gameObject.name == "66")
            {
                PuzzleCon.instance.Mid[5] = true;

            }

        }

    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        for (int i = 0; i < 6; i++)
        {
            PuzzleCon.instance.Mid[i] = false;
        }

    }
}
