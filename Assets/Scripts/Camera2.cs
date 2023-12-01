using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform attachedPlayer;
    Camera thisCam;
    public float blendAmount = 0.5f;


    void Start()
    {
        thisCam = GetComponent<Camera>();
    }


    void Update()
    {
        Vector3 player = attachedPlayer.transform.position;
        Vector3 newCamPos = player * blendAmount + transform.position * (1.0f - blendAmount);

        transform.position = new Vector3(newCamPos.x, newCamPos.y, transform.position.z);
    }
}
