using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Referencia de Vida del Jugador")]
    [SerializeField] Health health;

    [Header("UI")]
    [SerializeField] Image barraVida;
    [SerializeField] float smoothSpeed = 8f;

    float currentFill;

    void Start()
    {
        if (barraVida != null)
            currentFill = barraVida.fillAmount;
    }

    private void Update()
    {
        if (!health || !barraVida) return;

        float target = (float)health.GetCurrentHealth() / health.GetMaxHealth();

        currentFill = Mathf.Lerp(currentFill, target, Time.deltaTime * smoothSpeed);

        barraVida.fillAmount = currentFill;
    }
}

