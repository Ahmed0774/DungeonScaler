using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Image healthBar, XPBar;

    public void UpdateHealthBar(int health, int maxHealth) 
    {
        healthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void UpdateXP(int XP, int XPCap) 
    {
        XPBar.fillAmount = (float)XP / (float)XPCap;
        Debug.Log("Pickup XP");
    }

    public void UpdateStats() 
    { 
        
    }
}
