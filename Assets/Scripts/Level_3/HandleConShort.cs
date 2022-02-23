using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleConShort : HandleCon
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isClick == false)
        {
            if (collision.gameObject.name == "1")
            {
                PuzzleCon.instance.Top[0] = true;

            }

            if (collision.gameObject.name == "2")
            {
                PuzzleCon.instance.Top[1] = true;

            }

            if (collision.gameObject.name == "3")
            {
                PuzzleCon.instance.Top[2] = true;

            }
            if (collision.gameObject.name == "4")
            {
                PuzzleCon.instance.Top[3] = true;

            }
            if (collision.gameObject.name == "5")
            {
                PuzzleCon.instance.Top[4] = true;

            }
            if (collision.gameObject.name == "6")
            {
                PuzzleCon.instance.Top[5] = true;

            }

        }

    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        for (int i = 0; i < 6; i++)
        {
            PuzzleCon.instance.Top[i] = false;
        }

    }
}
