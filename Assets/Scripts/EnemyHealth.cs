using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [Range(0,100)]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {   
            Die();
        }
    }

    public void Die()
    {
       
    }

    private void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;

        healthBarFill.fillAmount = healthPercent;

        healthText.text = currentHealth.ToString();
    }
}
