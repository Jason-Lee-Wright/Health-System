using System;
using Unity.VisualScripting;
using UnityEngine;
public class HealthSystem
{
    // Variables
    public int health;
    public string healthStatus;
    public int shield;
    public int lives;
    

    // Optional XP system variables
    public int xp;
    public int level;

    private int maxlevel = 99;

    public HealthSystem()
    {
        ResetGame();
        UpdateHealthStatus();
    }


    public string ShowHUD()
    {
        // Implement HUD display
        return $" Lives:{lives} HP:{health} SHP:{shield} \n Level:{level} Exp:{xp} \n {healthStatus}";
    }

    public void TakeDamage(int damage)
    { 

            if (damage < 0)
            {
                return;
            }

            if (shield > 0)
            {
                int shieldDamage = Math.Min(damage, shield);
                shield -= shieldDamage;
                damage -= shieldDamage;
            }

            if (damage > 0)
            {
                health -= damage;
                if (health < 0)
                {
                    health = 0;
                }
            }

            if (health <= 0)
            {
                Revive();
            }

            else
            {
                 health = health - damage;
            }

             UpdateHealthStatus();

    }

    public void Heal(int hp)
    {
        if ( health < 100)
        {
            health += hp;
        }
        if ( health > 100)
        {
            health = 100;
        }
    }

    public void RegenerateShield(int hp)
    {
        if (shield < 100)
        {
            shield += hp;
        }
        if (shield > 100)
        {
            shield = 100;
        }
    }

    public void Revive()
    {
        if (health == 0 && lives > 0)
        {
            lives--;
            health = 100;
            shield = 100;
        }
        else if (health == 0 && lives <= 0)
        {
            ResetGame();
        }
    }

    public void ResetGame()
    {
        // Reset all variables to default values
        health = 100;
        shield = 100;
        lives = 3;
        xp = 0;
        level = 1;
    }

    // Optional XP system methods
    public void IncreaseXP(int exp)
    {
        if (level != maxlevel)
        {
            xp += exp;

            if (xp >= 100)
            {
                level++;
            }

            xp = xp % 100;
        }

    }

    private void UpdateHealthStatus()
    {
        if (health <= 10)
        {
            healthStatus = "Chuckles you're in danger!";
        }
        else if (health <= 50)
        {
            healthStatus = "Half health";
        }
        else if (health <= 75)
        {
            healthStatus = "Walk it off";
        }
        else if (health <= 90)
        {
            healthStatus = "Too slow";
        }
        else
        {
            healthStatus = "Full Health";
        }
    }

    public void limits()
    {
        if (health > 100)
        {
            health = 100;
        }
        if (shield > 100)
        {
            shield = 100;
        }
        if (level > 99)
        {
            level = 99;
        }
        if (lives > 99)
        {
            lives = 99;
        }
    }
}