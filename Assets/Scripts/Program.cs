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

    public bool preventRevive = false;

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
        if (health < 0)
            { health = 0; }
        if (shield > 100)
        {
            shield = 100;
        }
        if (shield < 0)
            { shield = 0; }
        if (level > 99)
        {
            level = 99;
        }
        if (lives > 99)
        {
            lives = 99;
        }
    }

    public static void RunAllUnitTests()
    {
        TestForNegativeNumbers();
        TestTakeDamage_ShieldOnly();
        TestTakeDamage_ShieldAndHealth();
        TestTakeDamage_HealthOnly();
        TestTakeDamage_HealthToZero();
        TestTakeDamage_ShieldAndHealthToZero();
        TestTakeDamage_NegativeDamage();

        TestHeal_NormalHealing();
        TestHeal_AtMaxHealth();
        TestHeal_NegativeHealing();

        TestRegenerateShield_Normal();
        TestRegenerateShield_AtMax();
        TestRegenerateShield_Negative();

        TestRevive();
        TestResetGame();

        TestIncreaseXP_Normal();
        TestIncreaseXP_LevelUpTo99();
    }
    public static void TestForNegativeNumbers()
    {
        var healthSystem = new HealthSystem();
        healthSystem.TakeDamage(-10);

        UnityEngine.Debug.Assert(healthSystem.health == 100);
    }

    public static void TestTakeDamage_ShieldOnly()
    {
        var healthSystem = new HealthSystem();
        healthSystem.TakeDamage(30);
        UnityEngine.Debug.Assert(healthSystem.shield == 70);
        UnityEngine.Debug.Assert(healthSystem.health == 100);
    }

    public static void TestTakeDamage_ShieldAndHealth()
    {
        var healthSystem = new HealthSystem();
        healthSystem.TakeDamage(130);
        healthSystem.Heal(30); 
        UnityEngine.Debug.Assert(healthSystem.shield == 0, $"Expected 0 shield, got {healthSystem.shield}");
        UnityEngine.Debug.Assert(healthSystem.health == 70, $"Expected 70 health, got {healthSystem.health}");
    }

    public static void TestTakeDamage_HealthOnly()
    {
        var healthSystem = new HealthSystem();
        healthSystem.shield = 0;
        healthSystem.lives = 100;
        healthSystem.TakeDamage(50);
        healthSystem.Heal(50);
        UnityEngine.Debug.Assert(healthSystem.health == 50, $"Expected 50 health, got {healthSystem.health}");
    }

    public static void TestTakeDamage_HealthToZero()
    {
        var healthSystem = new HealthSystem();
        healthSystem.preventRevive = true; 
        healthSystem.shield = 0; 
        healthSystem.TakeDamage(200);
        UnityEngine.Debug.Assert(healthSystem.health == 0, $"Expected 0 health, got {healthSystem.health}");
        UnityEngine.Debug.Assert(healthSystem.lives == 3, $"Expected 3 lives, got {healthSystem.lives}"); 
    }

    public static void TestTakeDamage_ShieldAndHealthToZero()
    {
        var healthSystem = new HealthSystem();
        healthSystem.preventRevive = true;
        healthSystem.TakeDamage(200);
        UnityEngine.Debug.Assert(healthSystem.health == 0, $"Expected 0 health, got {healthSystem.health}");
        UnityEngine.Debug.Assert(healthSystem.shield == 0, $"Expected 0 shield, got {healthSystem.shield}");
    }

    public static void TestTakeDamage_NegativeDamage()
    {
        var healthSystem = new HealthSystem();
        healthSystem.TakeDamage(-10);
        UnityEngine.Debug.Assert(healthSystem.health == 100);
        UnityEngine.Debug.Assert(healthSystem.shield == 100);
    }

    public static void TestHeal_NormalHealing()
    {
        var healthSystem = new HealthSystem();
        healthSystem.shield = 0;
        healthSystem.TakeDamage(30); 
        healthSystem.Heal(20 + 30);  
        UnityEngine.Debug.Assert(healthSystem.health == 90, $"Expected 90 health, got {healthSystem.health}");
    }

    public static void TestHeal_AtMaxHealth()
    {
        var healthSystem = new HealthSystem();
        healthSystem.Heal(20);
        UnityEngine.Debug.Assert(healthSystem.health == 100);
    }

    public static void TestHeal_NegativeHealing()
    {
        var healthSystem = new HealthSystem();
        healthSystem.Heal(-10);
        UnityEngine.Debug.Assert(healthSystem.health == 100);
    }

    public static void TestRegenerateShield_Normal()
    {
        var healthSystem = new HealthSystem();
        healthSystem.RegenerateShield(20);
        UnityEngine.Debug.Assert(healthSystem.shield == 100); 
    }

    public static void TestRegenerateShield_AtMax()
    {
        var healthSystem = new HealthSystem();
        healthSystem.shield = 50;
        healthSystem.RegenerateShield(100);
        UnityEngine.Debug.Assert(healthSystem.shield == 100);
    }

    public static void TestRegenerateShield_Negative()
    {
        var healthSystem = new HealthSystem();
        healthSystem.RegenerateShield(-10);
        UnityEngine.Debug.Assert(healthSystem.shield == 100);
    }

    public static void TestRevive()
    {
        var healthSystem = new HealthSystem();
        healthSystem.TakeDamage(300); 
        UnityEngine.Debug.Assert(healthSystem.health == 100);
        UnityEngine.Debug.Assert(healthSystem.shield == 100);
        UnityEngine.Debug.Assert(healthSystem.lives == 2); 
    }

    public static void TestResetGame()
    {
        var healthSystem = new HealthSystem();
        healthSystem.TakeDamage(300);
        healthSystem.ResetGame();
        UnityEngine.Debug.Assert(healthSystem.health == 100);
        UnityEngine.Debug.Assert(healthSystem.shield == 100);
        UnityEngine.Debug.Assert(healthSystem.lives == 3);
    }

    public static void TestIncreaseXP_Normal()
    {
        var healthSystem = new HealthSystem();
        healthSystem.IncreaseXP(50);
        UnityEngine.Debug.Assert(healthSystem.xp == 50);
    }

    public static void TestIncreaseXP_LevelUpTo99()
    {
        var healthSystem = new HealthSystem();
        healthSystem.level = 97;
        healthSystem.IncreaseXP(200);
        UnityEngine.Debug.Assert(healthSystem.level == 99);
        UnityEngine.Debug.Assert(healthSystem.xp == 0);
    }
}