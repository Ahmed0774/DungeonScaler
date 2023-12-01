using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    public int damage = 12;

    private void OnEnable()
    {
        Invoke("Destroy", 5f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == ("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }
    public void SetMoveDir(Vector2 dir)
    {
        moveDir = dir;
    }
    private void Destroy()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }
}
