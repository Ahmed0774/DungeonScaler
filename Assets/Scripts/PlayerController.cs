using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    Vector2 moveInput;


    [SerializeField] public float baseMoveSpeed = 4f,  damage = 1f, projectileSpeed, attackSpeed = 1f, moveSpeedBuff = 1f;
    [SerializeField] public int health = 10, maxHealth = 10, level = 0, xp, xpCap, projectileCount = 1, projectileSpread = 2;

    PlayerUI playerUI;

    // Start is called before the first frame update
    void Start()
    {
        //collision ignore for specific layers
        Physics2D.IgnoreLayerCollision(9,8);
        Physics2D.IgnoreLayerCollision(9,6);
        Physics2D.IgnoreLayerCollision(9,9);

        rb =GetComponent<Rigidbody2D>();
        anim =GetComponent<Animator>();
        playerUI = GetComponent<PlayerUI>();
        playerUI.UpdateXP(xp, xpCap);
        playerUI.UpdateHealthBar(health, maxHealth);
    }

    void FixedUpdate()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb.velocity = moveInput * baseMoveSpeed * moveSpeedBuff;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Xspeed", rb.velocity.x);

        if(rb.velocity.magnitude < 0.01 && rb.velocity.magnitude >-0.01)
        {
            anim.speed = 0f;
        }
        else
        {
            anim.speed = 1f;
        }

    }

    public void TakeDamage(int damage) 
    {
        health -= damage;

        if (health <= 0) 
        {
            StartCoroutine(Endgame());
        }

        playerUI.UpdateHealthBar(health, maxHealth);
    }

    public float GetDamage() 
    {
        return damage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("XP"))
        {
            xp++;
            Destroy(collision.gameObject);
            playerUI.UpdateXP(xp, xpCap);
            if (xp >= xpCap) 
            {
                GetComponent<LevelUpPause>().LevelUp(level);
            }
        }
    }

    public void SetAttackSpeed(float value) 
    {
        attackSpeed -= value;
        Debug.Log(attackSpeed);
    }

    public float GetAttackSpeed() 
    {
        return attackSpeed;
    }

    public void SetXP(int value) 
    {
        xp = value;
        playerUI.UpdateXP(xp, xpCap);
    }

    public void SetDamage(int value) 
    {
        damage += value;
    }

    public void SetHealth(int value) 
    {
        maxHealth += value;
        health = maxHealth;
        playerUI.UpdateHealthBar(health, maxHealth);
    }

    public void SetMoveSpeedBuff(float value) 
    {
        moveSpeedBuff += value;
    }

    public int GetProjectileCount() 
    {
        return projectileCount;
    }

    public void SetProjectileCount(int value)
    {
        projectileCount += value;
    }

    public int GetProjectileSpread()
    {
        return projectileSpread;
    }

    IEnumerator Endgame()
    {
        baseMoveSpeed = 0;
        anim.SetBool("death", true);
        yield return new WaitForSeconds(0.500f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
