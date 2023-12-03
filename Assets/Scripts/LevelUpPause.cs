using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpPause : MonoBehaviour
{
    [SerializeField] GameObject LevelUI;
    [SerializeField] Button btn1, btn2, btn3;
    Button[] btns = new Button[3];
    PlayerController player;

    // Update is called once per frame
    void Start()
    {
        btns[0] = btn1;
        btns[1] = btn2;
        btns[2] = btn3;
        player = GetComponent<PlayerController>();
    }
    void Update()
    {

    }

    void Continue()
    {
        foreach (Button btn in btns) 
        {
            btn.onClick.RemoveAllListeners();
        }
        player.SetXP(0);
        LevelUI.SetActive(false);
        Time.timeScale = 1;
        player.GetComponent<PlayerController>().xpCap++;
    }

    public void LevelUp(int level)
    {
        Time.timeScale = 0;
        LevelUI.SetActive(true);

        foreach (Button btn in btns)
        {
            int random = Random.Range(1,6);
            Debug.Log("random btn upgrade is: " +random);

            if (random == 1)
            {
                btn.onClick.AddListener(IncreaseAttackSpeed);
                btn.GetComponentInChildren<TMP_Text>().text = "Attack Speed";
            }
            else if (random == 2)
            {
                btn.onClick.AddListener(IncreaseDamage);
                btn.GetComponentInChildren<TMP_Text>().text = "Damage";
            }
            else if (random == 3)
            {
                btn.onClick.AddListener(IncreaseHealth);
                btn.GetComponentInChildren<TMP_Text>().text = "Health";
            }
            else if (random == 4)
            {
                btn.onClick.AddListener(IncreaseMoveSpeed);
                btn.GetComponentInChildren<TMP_Text>().text = "MoveSpeed";
            }
            else if (random == 5)
            {
                btn.onClick.AddListener(IncreaseProjectiles);
                btn.GetComponentInChildren<TMP_Text>().text = "Projectiles";
            }
        }
    }

    void IncreaseAttackSpeed()
    {
        player.SetAttackSpeed(0.08f);
        Continue();
    }

    void IncreaseDamage()
    {
        player.SetDamage(1);
        Continue();
    }

    void IncreaseHealth()
    {
        player.SetHealth(5);
        Continue();
    }

    void IncreaseMoveSpeed()
    {
        player.SetMoveSpeedBuff(0.08f);
        Continue();
    }

    void IncreaseProjectiles() 
    {
        player.SetProjectileCount(1);
        Continue();
    }

}
