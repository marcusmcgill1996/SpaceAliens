using UnityEngine;
using TMPro;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;

    private TMP_Text healthText;

    private void Start()
    {
        healthText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (playerHealth == null) return;
        healthText.text = "HP: " + playerHealth.CurrentHealth + " / " + playerHealth.MaxHealth;
    }
}