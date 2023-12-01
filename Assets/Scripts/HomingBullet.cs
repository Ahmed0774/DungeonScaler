using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 5;
    public float RotSpeed = 200;
    public GameObject target;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Invoke("Destroy", 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 point2Target = (Vector2)transform.position - (Vector2)target.transform.position;
        point2Target.Normalize();

        float value = Vector3.Cross(point2Target, transform.right).z;

        /*if(value>0)
        {
            rb.angularVelocity = RotSpeed;
        }
        else if (value<0)
        {
            rb.angularVelocity = -RotSpeed;
        }
        else
        {
            RotSpeed = 0;
        }*/

        rb.angularVelocity = RotSpeed * value;

        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Bullet")
        {
            Destroy(this.gameObject, 0.02f);
        }
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
