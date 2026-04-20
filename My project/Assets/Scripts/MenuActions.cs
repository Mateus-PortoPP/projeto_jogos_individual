using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuActions : MonoBehaviour
{
    public int gameplaySceneBuildIndex = 1;
    public string gameplaySceneName = "";
    public TMP_Text bestScoreText;
    public TMP_Text bestTimeText;
    public RectTransform titleRect;
    public float titleFloatAmplitude = 8f;
    public float titleFloatSpeed = 1.5f;
    public Image menuBackgroundImage;
    public Sprite menuBackgroundSprite;
    public TMP_InputField nameInputField;

    private Vector2 initialTitlePosition;

    void Start()
    {
        Time.timeScale = 1f;

        if (titleRect != null)
        {
            initialTitlePosition = titleRect.anchoredPosition;
        }

        if (menuBackgroundImage != null && menuBackgroundSprite != null)
        {
            menuBackgroundImage.sprite = menuBackgroundSprite;
        }

        RefreshBestResultsTexts();
    }

    void Update()
    {
        if (titleRect != null)
        {
            float yOffset = Mathf.Sin(Time.unscaledTime * titleFloatSpeed) * titleFloatAmplitude;
            titleRect.anchoredPosition = initialTitlePosition + new Vector2(0f, yOffset);
        }
    }

    public void IniciarJogo()
    {
        Time.timeScale = 1f;
        if (nameInputField != null)
            GameController.SetPlayerName(nameInputField.text);
        GameController.Init();
        if (!string.IsNullOrWhiteSpace(gameplaySceneName))
        {
            SceneManager.LoadScene(gameplaySceneName);
            return;
        }

        SceneManager.LoadScene(gameplaySceneBuildIndex);
    }

    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SairJogo()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RefreshBestResultsTexts()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "Recorde de Pontos: " + GameController.GetBestScore();
        }

        if (bestTimeText != null)
        {
            bestTimeText.text = "Recorde de Tempo: " + GameController.GetBestTime().ToString("F1") + "s";
        }
    }
}
