using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TMP_Text tempoText;
    public TMP_Text pontuacaoText;
    public TMP_Text vidasText;
    public TMP_Text gameOverTitleText;
    public TMP_Text gameOverStatsText;
    public TMP_Text gameOverRecordsText;
    public TMP_Text gameOverActionsHintText;
    public Image gameOverPanelImage;
    public Sprite gameOverPanelSprite;

    public int gameplaySceneBuildIndex = 1;
    public int menuSceneBuildIndex = 0;
    public bool pauseTimeOnGameOver = true;
    public KeyCode restartKey = KeyCode.R;
    public KeyCode menuKey = KeyCode.Escape;

    private bool gameOverUiShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanelImage != null && gameOverPanelSprite != null)
        {
            gameOverPanelImage.sprite = gameOverPanelSprite;
        }

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
            gameOverUiShown = false;
            Time.timeScale = 1f;

            if (endGamePanel != null)
            {
                endGamePanel.SetActive(false);
            }
            return;
        }

        if (!GameController.gameOver)
        {
            GameController.Tick(Time.deltaTime);
        }

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
            if (!gameOverUiShown)
            {
                ShowGameOverUI();
            }

            if (endGamePanel != null)
            {
                endGamePanel.SetActive(true);
            }

            if (Input.GetKeyDown(restartKey))
            {
                RestartGameplay();
            }

            if (Input.GetKeyDown(menuKey))
            {
                GoToMenu();
            }
        }
        else if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
            gameOverUiShown = false;
        }
    }

    void ShowGameOverUI()
    {
        gameOverUiShown = true;

        if (pauseTimeOnGameOver)
        {
            Time.timeScale = 0f;
        }

        if (gameOverTitleText != null)
        {
            string reason = GameController.GetLastGameOverReason();
            gameOverTitleText.text = string.IsNullOrWhiteSpace(reason) ? "Game Over" : reason;
        }

        if (gameOverStatsText != null)
        {
            gameOverStatsText.text = "Pontos: " + GameController.GetPontuacao() + "\nTempo: " + GameController.GetTempoDecorrido().ToString("F1") + "s";
        }

        if (gameOverRecordsText != null)
        {
            gameOverRecordsText.text = "Recordes\nPontos: " + GameController.GetBestScore() + "\nTempo: " + GameController.GetBestTime().ToString("F1") + "s";
        }

        if (gameOverActionsHintText != null)
        {
            gameOverActionsHintText.text = "Pressione " + restartKey + " para reiniciar\nPressione " + menuKey + " para menu";
        }
    }

    public void RestartGameplay()
    {
        Time.timeScale = 1f;
        GameController.Init();
        SceneManager.LoadScene(gameplaySceneBuildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneBuildIndex);
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
