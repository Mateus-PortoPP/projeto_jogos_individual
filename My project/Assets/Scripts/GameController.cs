using UnityEngine;

public static class GameController
{
    private static int coletaveisColetados;
    private static bool initialized;

    public static bool gameOver;

    public static void Init()
    {
        coletaveisColetados = 4;
        gameOver = false;
        initialized = true;
    }

    public static void Collect()
    {
        if (!initialized)
        {
            Init();
        }

        coletaveisColetados--;
        if (coletaveisColetados <= 0)
        {
            gameOver = true;
        }
    }

}
