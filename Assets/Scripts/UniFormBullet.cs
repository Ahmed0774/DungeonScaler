using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniFormBullet : MonoBehaviour
{
    [SerializeField]
    public int bulletAmount;
    [SerializeField]
    private float startAng = 0f, endAng = 0f;
    [SerializeField]
    public int shootdist = 5;
    bool withinreach = false;
    GameObject player;
    public LayerMask raycastlayer;
    bool canattack = false;
    [SerializeField]
    public float attacktime = 20f;
    private float attackspeed;

    private Vector2 bulletMoveDir;


    void Start()
    {
        player = GameObject.Find("Player");
        //InvokeRepeating("Fire", 0f, 5f);
    }

    private void Update()
    {
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

        float bulDirX = player.transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
        float bulDirY = player.transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

        Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
        Vector2 bulDir = (bulMoveVector - transform.position).normalized;
        //edit rays not working below
        Debug.DrawRay(transform.position, bulDir * shootdist, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, bulDir, shootdist, raycastlayer);

        if (hit)
        {
            withinreach = true;
            //Debug.Log("withinreach true");
        }
        else
        {
            withinreach = false;
        }

        if (!canattack)
        {
            attacktime++;
        }

        //if (hit.collider.tag == "Player")
        if (canattack == true && withinreach)
        {
            Invoke("Fire", 0f);
            canattack = false;
        }


    }
    private void Fire()
    {
        float angleStep = (endAng - startAng) / bulletAmount;
        float angle = startAng;

        for (int i = 0; i < bulletAmount + 1; i++)
        {
            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

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
