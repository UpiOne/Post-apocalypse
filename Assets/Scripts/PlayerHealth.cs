using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [Range(0,100)]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject UIGameOver;
    [Range(0,100)] public int armor = 0;
    public static PlayerHealth Instance;

    public Image healthBarFill;
    public TextMeshProUGUI healthText;

    private int currentHealth;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - armor, 0);

        currentHealth -= finalDamage;

        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar();
    }

    private void Die()
    {
        UIGameOver.SetActive(true);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / (float)maxHealth;
        healthBarFill.fillAmount = healthPercent;
        healthText.text = currentHealth.ToString();
    }
}
