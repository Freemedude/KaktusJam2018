using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    private Image _staminaBarImage;

    void Start()
    {
        _staminaBarImage = GetComponent<Image>();
    }

    public void OnStaminaChanged(float currentStamina)
    {
        _staminaBarImage.fillAmount = currentStamina;
    }
}
