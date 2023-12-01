using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    public TextMeshProUGUI waveCountTxt;
    int waveCount =1;

    [SerializeField]
    public float spawnRate = 1.0f;
    public float timeBetweenWaves = 20.0f;
    public int enemyCount =5;
    public int timer = 30;
    private int timenow = 0;
    Quaternion quant= Quaternion.identity;


    public GameObject enemy;

    bool waveIsDone = true;

    private void FixedUpdate()
    {
        if(timenow <timer)
        {
            timenow++;
        }
        if (waveIsDone == true && timenow >= timer)
        {
            //waveCountTxt.text = "Wave: " + waveCount.ToString();
            StartCoroutine(waveSpawner());
            timenow = 0;
        }
    }

    IEnumerator waveSpawner()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy,transform.position,quant);

            yield return new WaitForSeconds(spawnRate);
        }

        spawnRate -= 1.0f;
        //enemyCount += 5;
        waveCount++;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }
}
