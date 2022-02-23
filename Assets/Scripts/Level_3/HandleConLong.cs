using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleConLong : HandleCon
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isClick == false)
        {
            if (collision.gameObject.name == "111")
            {
                PuzzleCon.instance.Bottom[0] = true;

            }

            if (collision.gameObject.name == "222")
            {
                PuzzleCon.instance.Bottom[1] = true;

            }

            if (collision.gameObject.name == "333")
            {
                PuzzleCon.instance.Bottom[2] = true;

            }
            if (collision.gameObject.name == "444")
            {
                PuzzleCon.instance.Bottom[3] = true;

            }
            if (collision.gameObject.name == "555")
            {
                PuzzleCon.instance.Bottom[4] = true;

            }
            if (collision.gameObject.name == "666")
            {
                PuzzleCon.instance.Bottom[5] = true;

            }

        }

    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        for (int i = 0; i < 6; i++)
        {
            PuzzleCon.instance.Bottom[i] = false;
        }

    }
}
