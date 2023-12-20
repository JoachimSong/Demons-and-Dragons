using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public float spawnTime;
    public float width;
    public bool isVertical;
    private float countTime;
    void Start()
    {
        countTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        countTime += Time.deltaTime;
        if (countTime >= spawnTime)
        {
            SpawnBullet();
            countTime = 0;
        }
    }
    public void SpawnBullet()
    {
        Vector3 spawnPosition = transform.position;
        if (isVertical)
        {
            spawnPosition.y = Random.Range(-width, width);
        }
        else
        {
            spawnPosition.x = Random.Range(-width, width);
        }
        GameObject go = Instantiate(bullet, spawnPosition, Quaternion.identity);
        go.transform.SetParent(this.gameObject.transform);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, width);
    }
}