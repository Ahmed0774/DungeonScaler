using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldajBullet : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField]
    private float moveSpeed;

    private void OnEnable()
    {
        Invoke("Destroy", 10f);
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
