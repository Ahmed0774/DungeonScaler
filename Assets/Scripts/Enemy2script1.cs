using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2script : MonoBehaviour
{
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

    private Vector2 bulletMoveDir;

    public float enemydmg = 10;
    public int enemymaxhealth = 100;
    public float enemyhealth = 100;
    public int attackspeed = 40;
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
            StartCoroutine(deathnow());
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

        float angleStep = (endAng - startAng) / bulletAmount;
        float angle = startAng;

        if (!canattack)
        {
            attacktime++;
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

    IEnumerator deathnow()
    {
        Quaternion quant = Quaternion.identity;
        canmove = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
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
        float angleStep = (endAng - startAng) / bulletAmount;
        float angle = startAng;

        for (int i = 0; i < bulletAmount + 1; i++)
        {
            //changed to player.transform.position instead of transform.position
            float bulDirX = player.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = player.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized;

            GameObject bul = BulletPool.BPinstance.GetBullet();

            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<Bullet1>().SetMoveDir(bulDir);

            angle += angleStep;
        }
    }
}
