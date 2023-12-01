using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeKing : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float mindist = 1.2f;
    [SerializeField]
    public float maxdist = 6f;
    private float distance;
    public bool canbedamaged = true;
    [SerializeField]
    Color defaultcolour;
    bool canmove = true;
    public float enemymaxhealth = 100;
    public float enemyhealth = 100;

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


    }
    private void FixedUpdate()
    {
        if (distance < maxdist && canmove && distance > mindist)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, (speed));
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

    IEnumerator dmgcooldown702ms()
    {
        canbedamaged = false;
        yield return new WaitForSeconds(0.705f);
        canbedamaged = true;
    }

    IEnumerator wait20msthendodmg(float ddmg)
    {
        yield return new WaitForSeconds(0.02f);
        enemyhealth = enemyhealth - ddmg;
        transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(200, 0, 0);
        yield return new WaitForSeconds(0.20f);
        transform.gameObject.GetComponent<SpriteRenderer>().color = defaultcolour;
    }

    IEnumerator deathin705ms()
    {
        canmove = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        //Play death anim
        yield return new WaitForSeconds(0.705f);
        Destroy(gameObject);
    }

    
}
