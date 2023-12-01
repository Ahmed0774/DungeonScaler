using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy1script : MonoBehaviour
{
    public GameObject Exp;
    GameObject player;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float mindist = 1.2f;
    [SerializeField]
    public float maxdist = 6f;
    private float distance;
    bool canattack;
    public bool canbedamaged = true;
    [SerializeField]
    float attacktime = 20;
    Color defaultcolour;
    bool canmove = true;

    public int enemydmg = 10;
    public float enemymaxhealth = 100;
    public float enemyhealth = 100;
    public float attackspeed = 40;
    Image fillimage;
    private Slider slider;
    Transform trans;
    public Vector3 offset = new Vector3();
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        fillimage = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        player = GameObject.Find("Player");
        trans = transform.GetChild(0).gameObject.transform;
        slider = trans.GetChild(0).GetComponent<Slider>();
        slider.maxValue = enemymaxhealth;
        defaultcolour = GetComponent<SpriteRenderer>().color;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.gameObject.GetComponent<Rigidbody2D>().IsSleeping())
        {
            transform.gameObject.GetComponent<Rigidbody2D>().WakeUp();
        }


        slider.transform.position = trans.position + offset;
        float fillvalue = enemyhealth / enemymaxhealth;
        slider.value = fillvalue * enemymaxhealth;

        if (enemyhealth <= 0.0f)
        {
            StartCoroutine(deathin705ms());
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;


        if (attacktime >= attackspeed)
        {
            canattack = true;
            attacktime = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!canattack)
        {
            attacktime++;
        }

        if (distance < maxdist && canmove && distance > mindist)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (speed));
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == ("Player"))
        {
            if (canattack == true)
            {
                player.GetComponent<PlayerController>().TakeDamage(enemydmg);
                StartCoroutine(sizeup());
                canattack = false;
            }
        }
    }

    public void dodmg(float dmg)
    {
        if (canbedamaged)
        {
            StartCoroutine(wait20msthendodmg(dmg));
        }
        StartCoroutine(dmgcooldown702ms());
    }

    IEnumerator deathin705ms()
    {
        Quaternion quant = Quaternion.identity;
        canmove = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.10f);
        Instantiate(Exp, transform.position, quant);
        Destroy(gameObject);
    }

    IEnumerator dmgcooldown702ms()
    {
        canbedamaged = false;
        yield return new WaitForSeconds(0.705f);
        canbedamaged = true;
    }
    IEnumerator sizeup()
    {
        gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        yield return new WaitForSeconds(0.105f);
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
    }


    IEnumerator wait20msthendodmg(float ddmg)
    {
        yield return new WaitForSeconds(0.020f);
        enemyhealth = enemyhealth - ddmg;
        transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
        yield return new WaitForSeconds(0.305f);
        transform.gameObject.GetComponent<SpriteRenderer>().color = defaultcolour;
    }
}
