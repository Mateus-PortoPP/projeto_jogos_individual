using UnityEngine;

public static class GameController
{
    private static int coletaveisRestantes;
    private static int pontuacao;
    private static int vidasRestantes;
    private static float tempoDecorrido;
    private static bool initialized;

    public static bool gameOver;

    public static void Init()
    {
        coletaveisRestantes = 4;
        pontuacao = 0;
        vidasRestantes = 3;
        tempoDecorrido = 0f;
        gameOver = false;
        initialized = true;
    }

    public static void Tick(float deltaTime)
    {
        if (!initialized)
        {
            Init();
        }

        if (!gameOver)
        {
            tempoDecorrido += deltaTime;
        }
    }

    public static void Collect()
    {
        if (!initialized)
        {
            Init();
        }

        coletaveisRestantes--;
        pontuacao += 100;

        if (coletaveisRestantes <= 0)
        {
            gameOver = true;
        }
    }

    public static void PlayerHit()
    {
        if (!initialized)
        {
            Init();
        }

        if (gameOver)
        {
            return;
        }

        vidasRestantes--;
        if (vidasRestantes <= 0)
        {
            gameOver = true;
        }
    }

    public static int GetPontuacao()
    {
        return pontuacao;
    }

    public static float GetTempoDecorrido()
    {
        return tempoDecorrido;
    }

    public static int GetVidasRestantes()
    {
        return vidasRestantes;
    }

}
