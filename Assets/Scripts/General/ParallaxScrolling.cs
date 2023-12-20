using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    // Start is called before the first frame update
    //public Vector2 centerPoint;
    public Transform cameraTrans;
    private Vector3 lastCameraPos;
    public float speedFactor;
    public Vector3 deltaMovement;
    public bool startScroll;
    void Start()
    {
        deltaMovement = Vector3.zero;
        StartCoroutine(Adjust());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (startScroll)
        {
            deltaMovement = cameraTrans.position - lastCameraPos;
            transform.position += new Vector3(deltaMovement.x * speedFactor, 0, 0);
            lastCameraPos = cameraTrans.position;
        }
    }
    void Update()
    {
        
        

    }

    IEnumerator Adjust()
    {
        yield return new WaitForSeconds(0.5f);
        cameraTrans = Camera.main.transform;
        lastCameraPos = cameraTrans.position;
        startScroll = true;
    }
}
