using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TMP_Text tempoText;
    public TMP_Text pontuacaoText;
    public TMP_Text vidasText;
    public int gameplaySceneBuildIndex = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isGameplayScene = SceneManager.GetActiveScene().buildIndex == gameplaySceneBuildIndex;

        SetHudVisible(isGameplayScene);
        if (!isGameplayScene)
        {
            if (endGamePanel != null)
            {
                endGamePanel.SetActive(false);
            }
            return;
        }

        GameController.Tick(Time.deltaTime);

        float tempo = GameController.GetTempoDecorrido();
        if (tempoText != null)
        {
            tempoText.text = "Tempo: " + tempo.ToString("F1") + "s";
        }

        if (pontuacaoText != null)
        {
            pontuacaoText.text = "Pontos: " + GameController.GetPontuacao();
        }

        if (vidasText != null)
        {
            vidasText.text = "Vidas: " + GameController.GetVidasRestantes();
        }

        if (GameController.gameOver)
        {
            if (endGamePanel != null)
            {
                endGamePanel.SetActive(true);
            }
        }
        else if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    void SetHudVisible(bool visible)
    {
        if (tempoText != null)
        {
            tempoText.gameObject.SetActive(visible);
        }

        if (pontuacaoText != null)
        {
            pontuacaoText.gameObject.SetActive(visible);
        }

        if (vidasText != null)
        {
            vidasText.gameObject.SetActive(visible);
        }
    }
}
