using UnityEngine;

public static class GameController
{
    private static int coletaveisRestantes;
    private static int pontuacao;
    private static int vidasRestantes;
    private static float tempoDecorrido;
    private static bool initialized;

    public static bool gameOver;

    // Garante que o estado estático seja resetado a cada sessão de Play no Editor.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStaticState()
    {
        initialized = false;
        gameOver = false;
        tempoDecorrido = 0f;
        pontuacao = 0;
        vidasRestantes = 3;
        coletaveisRestantes = 0;
    }

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

        coletaveisRestantes = Mathf.Max(0, coletaveisRestantes - 1);
        pontuacao += 100;
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
