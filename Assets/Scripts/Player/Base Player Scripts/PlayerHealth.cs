using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Image healthBar = null;

    [Header("Health Variables")]
    [SerializeField] float currentMaxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float timeBeforeStartHealth;
    float healTimer;
    [SerializeField] float healthOverTime;
    [SerializeField] float healthOverTimeMultiplyer;

    private void Start()
    {
        currentHealth = currentMaxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        HealthOverTime();
    }

    private void HealthOverTime()
    {
        if (currentHealth < currentMaxHealth)
        {
            if (healTimer > 0)
            {
                healTimer -= Time.deltaTime;
            }
            else
            {
                currentHealth += healthOverTime * healthOverTimeMultiplyer * Time.deltaTime;
                UpdateHealthBar();
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / currentMaxHealth;
    }

    #region Public Functions
    public void GetDamage(float amount)
    {
        currentHealth -= amount;

        UpdateHealthBar();

        healTimer = timeBeforeStartHealth;
    }
    #endregion

    #region Getters
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return currentMaxHealth;
    }
    #endregion

    #region Setters
    public void SetCurrentHealth(float amount)
    {
        currentHealth = amount;
    }

    public void SetMaxHealth(float amount)
    {
        currentMaxHealth = amount;
    }
    #endregion
}
