using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float damage;
    void Start()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Contact enemy");          //have to add many else if's for different enemies with getcomponenet<Scriptname>
            
            if(collision.gameObject.GetComponent<Enemy2script>().enemyhealth >= 1f)
            {
                collision.gameObject.GetComponent<Enemy2script>().dodmg(GameObject.Find("Player").GetComponent<PlayerController>().GetDamage());
                Destroy(gameObject);
            }
        }
        else if(collision.gameObject.CompareTag("SlimeKing"))
        {
            if (collision.gameObject.GetComponent<SlimeKing>().enemyhealth >= 1f)
            {
                collision.gameObject.GetComponent<SlimeKing>().dodmg(GameObject.Find("Player").GetComponent<PlayerController>().GetDamage());
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Slime"))
        {
            if (collision.gameObject.GetComponent<Enemy1script>().enemyhealth >= 1f)
            {
                collision.gameObject.GetComponent<Enemy1script>().dodmg(GameObject.Find("Player").GetComponent<PlayerController>().GetDamage());
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float dam)
    {
        damage = dam;
    }
}
