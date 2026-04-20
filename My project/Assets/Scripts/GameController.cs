using UnityEngine;

public static class GameController
{
    private const string BestScoreKey = "best_score";
    private const string BestTimeKey = "best_time";

    private static int coletaveisRestantes;
    private static int pontuacao;
    private static int vidasRestantes;
    private static float tempoDecorrido;
    private static bool initialized;
    private static string lastGameOverReason;
    private static string playerName = "Jogador";

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
        lastGameOverReason = "";
        playerName = "Jogador";
    }

    public static void SetPlayerName(string name)
    {
        playerName = string.IsNullOrWhiteSpace(name) ? "Jogador" : name.Trim();
    }

    public static string GetPlayerName() => playerName;

    public static void Init()
    {
        coletaveisRestantes = 4;
        pontuacao = 0;
        vidasRestantes = 3;
        tempoDecorrido = 0f;
        gameOver = false;
        lastGameOverReason = "";
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
            RegisterGameOver("Voce ficou sem vidas");
        }
    }

    public static void RegisterGameOver(string reason = "Fim de jogo")
    {
        if (!initialized)
        {
            Init();
        }

        if (gameOver)
        {
            return;
        }

        gameOver = true;
        lastGameOverReason = string.IsNullOrWhiteSpace(reason) ? "Fim de jogo" : reason;
        SaveBestResults();
        RankingManager.AddEntry(playerName, pontuacao);
    }

    static void SaveBestResults()
    {
        int previousBestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        float previousBestTime = PlayerPrefs.GetFloat(BestTimeKey, 0f);

        if (pontuacao > previousBestScore)
        {
            PlayerPrefs.SetInt(BestScoreKey, pontuacao);
        }

        if (tempoDecorrido > previousBestTime)
        {
            PlayerPrefs.SetFloat(BestTimeKey, tempoDecorrido);
        }

        PlayerPrefs.Save();
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

    public static string GetLastGameOverReason()
    {
        return lastGameOverReason;
    }

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    public static float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BestTimeKey, 0f);
    }

}
