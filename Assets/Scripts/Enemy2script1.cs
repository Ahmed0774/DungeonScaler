using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2script : MonoBehaviour
{
    public Transform firePoint;
    public int EnemybulletForce = 1;
    public GameObject EnemyBullet;
    public GameObject Exp;
    public LayerMask raycastlayer;
    private bool withinreach = false;
    [SerializeField]
    public int shootdist = 5;
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
    private float attacktime = 20;
    Color defaultcolour;
    bool canmove = true;

    [SerializeField]
    private int bulletAmount;
    [SerializeField]

    private float startAng = 0f, endAng = 0f;
    float lookAngle;
    Vector2 playerpos;

    public float enemydmg = 10;
    public int enemymaxhealth = 100;
    public float enemyhealth = 100;
    public int attackspeed = 40;
    Image fillimage;
    private Slider slider;
    Transform trans;
    public Vector3 offset = new Vector3();
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), EnemyBullet.GetComponent<BoxCollider2D>());
        fillimage = transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        player = GameObject.Find("Player");
        trans = transform.GetChild(0).gameObject.transform;
        slider = trans.GetChild(0).GetComponent<Slider>();
        slider.maxValue = enemymaxhealth;
        defaultcolour = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        playerpos = player.transform.position;

        if (transform.gameObject.GetComponent<Rigidbody2D>().IsSleeping())
        {
            transform.gameObject.GetComponent<Rigidbody2D>().WakeUp();
        }


        slider.transform.position = trans.position + offset;
        float fillvalue = enemyhealth / enemymaxhealth;
        slider.value = fillvalue * enemymaxhealth;

        if (enemyhealth <= 0.0f)
        {
            StartCoroutine(deathin100ms());
        }

        distance = Vector3.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

    }

    private void FixedUpdate()
    {
        Vector2 lookDir = playerpos - new Vector2(firePoint.transform.position.x,firePoint.transform.position.y);
        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, lookAngle);
        firePoint.position = gameObject.transform.position;

        if (!canattack)
        {
            attacktime++;
        }

        if (attacktime >= attackspeed)
        {
            canattack = true;
            attacktime = 0;
        }

        if (distance < maxdist && canmove && distance > mindist)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (speed));
        }


        if (canattack == true && withinreach)
        {
            Invoke("Fire", 0f);
            canattack = false;
        }


        float angle = startAng;
        float bulDirX = player.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
        float bulDirY = player.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

        Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
        Vector2 bulDir = (bulMoveVector - transform.position).normalized;
        //edit rays not working below
        Debug.DrawRay(transform.position, bulDir * shootdist, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, bulDir, shootdist,raycastlayer);

        //if (hit.collider.tag == "Player")
        if (hit)
        {
            withinreach = true;
            //Debug.Log("withinreach true");
        }
        else
        {
            withinreach = false;
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

    IEnumerator deathin100ms()
    {
        Quaternion quant = Quaternion.identity;
        canmove = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.10f);
        Instantiate(Exp,transform.position,quant);
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
        yield return new WaitForSeconds(0.02f);
        enemyhealth = enemyhealth - ddmg;
        transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
        yield return new WaitForSeconds(0.20f);
        transform.gameObject.GetComponent<SpriteRenderer>().color = defaultcolour;
    }

    private void Fire()
    {
        float aimingAngle = firePoint.rotation.eulerAngles.z;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimingAngle));
        GameObject bullet = Instantiate(EnemyBullet, gameObject.transform.position, rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bullet.transform.up * EnemybulletForce, ForceMode2D.Impulse);
    }
}
