using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform bulletStart;
    private void Awake()
    {
        bulletStart = this.transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletStart = this.transform;
    }

    public void GenerateBullet(GameObject bulletPrefab)
    {
        bullet = bulletPrefab;
        Instantiate(bullet, bulletStart.position, bulletStart.rotation);
    }
}
