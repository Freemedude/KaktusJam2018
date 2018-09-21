using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    private Image staminaBarImage;

    void Start()
    {
        staminaBarImage = GetComponent<Image>();
    }

    public void ChangeStamina(float currentStamina)
    {
        staminaBarImage.fillAmount = currentStamina;
    }
}
