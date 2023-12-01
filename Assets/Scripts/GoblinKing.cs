using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinKing : MonoBehaviour
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

    IEnumerator deathin705ms()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        canmove = false;
        transform.GetComponent<SpriteRenderer>().enabled = false;
        //Play death anim
        yield return new WaitForSeconds(0.705f);
        Destroy(gameObject);
    }
}
