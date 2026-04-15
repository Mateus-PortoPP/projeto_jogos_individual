using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlatformCycler : MonoBehaviour
{
    public GameObject[] platforms;
    public string autoFindTag = "platform";
    public string alternateAutoFindTag = "plataform";
    public bool autoFindByTagIfEmpty = true;
    public float switchInterval = 4f;
    public float warningDuration = 0.5f;
    public int warningBlinkCount = 3;
    public float warningAlpha = 0.25f;

    private float timer;
    private int lastPlatformIndex = -1;
    private int currentDisabledIndex = -1;
    private bool warningTriggered = false;

    void Start()
    {
        EnsurePlatforms();
    }

    void Update()
    {
        EnsurePlatforms();

        if (platforms == null || platforms.Length == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        float warningStartTime = Mathf.Max(0f, switchInterval - warningDuration);
        if (!warningTriggered && timer >= warningStartTime)
        {
            warningTriggered = true;
            int warningIndex = GetRandomPlatformIndex();
            if (warningIndex >= 0)
            {
                TriggerWarning(platforms[warningIndex]);
                lastPlatformIndex = warningIndex;
            }
        }

        if (timer >= switchInterval)
        {
            timer = 0f;
            warningTriggered = false;
            ToggleNextPlatform();
        }
    }

    void EnsurePlatforms()
    {
        if (!autoFindByTagIfEmpty)
        {
            return;
        }

        if (platforms != null && platforms.Length > 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(autoFindTag))
        {
            autoFindTag = "platform";
        }

        GameObject[] found = Array.Empty<GameObject>();
        try
        {
            found = GameObject.FindGameObjectsWithTag(autoFindTag);
        }
        catch (UnityException)
        {
            // Tag não existe no projeto, segue para fallback por nome.
            found = Array.Empty<GameObject>();
        }

        if (found.Length == 0 && !string.IsNullOrWhiteSpace(alternateAutoFindTag))
        {
            try
            {
                found = GameObject.FindGameObjectsWithTag(alternateAutoFindTag);
            }
            catch (UnityException)
            {
                found = Array.Empty<GameObject>();
            }
        }

        List<GameObject> filtered = new List<GameObject>();
        foreach (GameObject go in found)
        {
            if (go == null || go == gameObject)
            {
                continue;
            }
            filtered.Add(go);
        }

        // Fallback: se ninguém tem tag, usa objetos com nome "Square" (plataformas da fase atual).
        if (filtered.Count == 0)
        {
            foreach (SpriteRenderer renderer in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
            {
                if (renderer == null || renderer.gameObject == gameObject)
                {
                    continue;
                }

                string n = renderer.gameObject.name;
                if (n.StartsWith("Square"))
                {
                    filtered.Add(renderer.gameObject);
                }
            }
        }

        platforms = filtered.ToArray();
    }

    void TriggerWarning(GameObject platform)
    {
        if (platform == null)
        {
            return;
        }

        SpriteRenderer renderer = platform.GetComponent<SpriteRenderer>();
        if (renderer != null && renderer.enabled)
        {
            StartCoroutine(BlinkRenderer(renderer));
        }
    }

    IEnumerator BlinkRenderer(SpriteRenderer renderer)
    {
        Color originalColor = renderer.color;
        Color warningColor = originalColor;
        warningColor.a = warningAlpha;

        float blinkStep = warningDuration / Mathf.Max(1, warningBlinkCount * 2);
        for (int i = 0; i < warningBlinkCount; i++)
        {
            renderer.color = warningColor;
            yield return new WaitForSeconds(blinkStep);
            renderer.color = originalColor;
            yield return new WaitForSeconds(blinkStep);
        }

        renderer.color = originalColor;
    }

    void ToggleNextPlatform()
    {
        // Reativa a plataforma que estava desativada no ciclo anterior
        if (currentDisabledIndex >= 0 && currentDisabledIndex < platforms.Length
            && platforms[currentDisabledIndex] != null)
        {
            platforms[currentDisabledIndex].SetActive(true);
        }

        // Desativa a plataforma selecionada no aviso (ou uma aleatória)
        int indexToDisable = lastPlatformIndex >= 0 ? lastPlatformIndex : GetRandomPlatformIndex();
        lastPlatformIndex = -1;

        if (indexToDisable >= 0 && indexToDisable < platforms.Length
            && platforms[indexToDisable] != null)
        {
            platforms[indexToDisable].SetActive(false);
            currentDisabledIndex = indexToDisable;
        }
        else
        {
            currentDisabledIndex = -1;
        }
    }

    int GetRandomPlatformIndex()
    {
        if (platforms == null || platforms.Length == 0)
        {
            return -1;
        }

        if (platforms.Length == 1)
        {
            return 0;
        }

        // Evita repetir a plataforma que acabou de ser desativada
        int avoidIndex = currentDisabledIndex >= 0 ? currentDisabledIndex : lastPlatformIndex;

        int tries = 0;
        int index = UnityEngine.Random.Range(0, platforms.Length);
        while (index == avoidIndex && tries < 8)
        {
            index = UnityEngine.Random.Range(0, platforms.Length);
            tries++;
        }

        return index;
    }

    void TogglePlatform(GameObject platform)
    {
        platform.SetActive(!platform.activeSelf);
    }
}
