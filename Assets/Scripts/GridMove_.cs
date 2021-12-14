using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove_ : MonoBehaviour
{
    private Tank tank;
    private float move = 0.05f;
    private int face = -1;
    private float y;
    private float x;

    void Awake()
    {
        tank = GetComponent<Tank>();
    }

    void Update()
    {
        if (tank.dirHeld != face)
        {
            Vector3 rPos = tank.pos;
            y = Mathf.Round(rPos.y);
            x = Mathf.Round(rPos.x);

            float delta = 0;

            if (face == 1 || face == 3)
            {
                // Движение по горизонтали, выравнивание по оси y
                delta = y - rPos.y;
            }
            else
            {
                //// Движение по вертикали, выравнивание по оси x
                delta = x - rPos.x;
            }

            if (delta > 0.05 || delta < -0.05)
            {
                float mov = move;
                if (delta < 0) mov = -move;

                if (face == 1 || face == 3)
                {
                    // Движение по горизонтали, выравнивание по оси y
                    rPos.y += mov;
                    delta += mov;
                }
                else if (face == 0 || face == 2)
                {
                    // Движение по вертикали, выравнивание по оси x
                    rPos.x += mov;
                    delta += mov;
                }

                tank.pos = rPos;
            }
            else
            {
                face = tank.dirHeld;
            }
        }
    }
}
