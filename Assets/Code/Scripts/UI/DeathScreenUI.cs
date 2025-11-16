using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DeathScreenUI : MonoBehaviour
{
    [Header("Referencia")]
    [SerializeField] GameObject panel;

    [Header("UI")]
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] RectTransform rect;

    [Header("Botones")]
    [SerializeField] Button retryButton;
    [SerializeField] Button menuButton;

    [Header("Animación")]
    [SerializeField] float animDuration = 0.5f;
    [SerializeField] Vector2 shownPosition = Vector2.zero;
    [SerializeField] Vector2 hiddenOffset = new Vector2(0f, 300f);

    bool isShowing;

    void Awake()
    {
        if (!panel) panel = gameObject;

        if (!canvasGroup) canvasGroup = panel.GetComponent<CanvasGroup>();
        if (!rect) rect = panel.GetComponent<RectTransform>();

        panel.SetActive(false);
        if (canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (rect)
        {
            rect.anchoredPosition = shownPosition + hiddenOffset;
        }

        if (retryButton) retryButton.onClick.AddListener(Reintentar);
        if (menuButton) menuButton.onClick.AddListener(VolverAlMenu);
    }

    public void MostrarPantallaMuerte()
    {
        if (isShowing) return;

        isShowing = true;
        panel.SetActive(true);

        Time.timeScale = 0f;

        StartCoroutine(AnimarEntrada());
    }

    IEnumerator AnimarEntrada()
    {
        if (!canvasGroup || !rect)
            yield break;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Vector2 startPos = shownPosition + hiddenOffset;
        Vector2 endPos = shownPosition;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / animDuration;
            float eased = EaseOutCubic(Mathf.Clamp01(t));

            canvasGroup.alpha = eased;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, eased);

            yield return null;
        }

        canvasGroup.alpha = 1f;
        rect.anchoredPosition = endPos;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }

    private void Reintentar()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    private void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
