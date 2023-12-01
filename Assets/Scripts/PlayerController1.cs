using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController1 : MonoBehaviour
{

    Rigidbody2D rb;
    Animator anim;
    public Slider playerslider;
    public float maxhp =200;
    private float fillvalue;
    public float currenthp;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim =GetComponent<Animator>();
        playerslider.maxValue = maxhp;
    }

    void FixedUpdate()
    {
        float speed = 4f;
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"))*speed;
    }

    // Update is called once per frame
    void Update()
    {
        fillvalue = currenthp / maxhp;
        playerslider.value = fillvalue*maxhp;
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

    public void dodmgtoplayer(float dmg)
    {
        currenthp = currenthp - dmg;
        StartCoroutine(damagecolor());
    }

    IEnumerator damagecolor()
    {
        yield return new WaitForSeconds(0.17f);
        transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
        yield return new WaitForSeconds(0.305f);
        transform.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
