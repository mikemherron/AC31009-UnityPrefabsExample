using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController playerController;
    public Slider slider;
    
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = playerController.maxHealth;
        slider.value = playerController.health;
        playerController.onPlayerHealthUpdated.AddListener(UpdateHealth);
    }

    void UpdateHealth(int health)
    {
        slider.value = health;
    }
}
