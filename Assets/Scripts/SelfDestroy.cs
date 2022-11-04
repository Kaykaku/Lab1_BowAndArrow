using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float rangeX = 30f;
    public float rangeY = 10f;
    private float timer = 0f;
    public float destroyAfter = 3f;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy the object when it exceeds the limit on the position or the time allowed (from the start of the move)
        if (transform.position.x<-rangeX || transform.position.x > rangeX || transform.position.y<-rangeY)
        {
            Destroy(gameObject);
        }else if (transform.position != startPos)
        {
            timer += Time.deltaTime;
            if (timer < destroyAfter) return;
            Destroy(gameObject);
        }
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
