using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1f;
    Vector2 directionOfArrow; 
    Vector2 startPos; 
    float launchForce;
    float time;
    bool isflying=true;
    bool isHit=false;
    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        UpdatePosByTime();
        DestroyTargetIfCollision();
        DetectCollisionTarget();
    }

    public void Set(Vector2 start, Vector2 direction,float launchForce)
    {
        //Set the information as start location, direction, force of arrow name
        this.directionOfArrow = direction;
        this.startPos = start;
        this.launchForce = launchForce;
    }

    public bool IsFlying()
    {
        return isflying;
    }

    void UpdatePosByTime()
    {
        //Update arrow position over time
        time += Time.deltaTime * speed;
        Vector3 position = CalculatorPosByTime(time);

        if (position == transform.position) return;
        transform.right = position - transform.position; //Update the direction of the arrow according to the trajectory
        transform.position = position;
    }

    Vector2 CalculatorPosByTime(float time)
    {
        //The formula for calculating arrow trajectory by time
        return startPos + directionOfArrow * launchForce * time + 0.5f * Physics2D.gravity * (time * time);
    }

    void DetectCollisionTarget()
    {
        //Detect LayerMask Target collision in next frame
        float timeNextFrame = time + Time.deltaTime;
        Vector2 posNextFrame = CalculatorPosByTime(timeNextFrame);
        Vector2 directionBetweenTwoPoint = posNextFrame - (Vector2)transform.position;

        hit = Physics2D.Raycast(transform.position, directionBetweenTwoPoint, Vector2.Distance(transform.position, posNextFrame), LayerMask.GetMask("Target"));

        if (hit.collider!=null)
        {
            isHit = true;
        }
    }

    void DestroyTargetIfCollision()
    {
        //Destroys an arrow if it hits an object
        //Destroy the collided object if it is a Target
        if (isHit)
        {
            transform.GetComponent<SelfDestroy>().DestroySelf();
            if(hit.collider.CompareTag("Target")) Destroy(hit.collider.gameObject);
        }
    }
}
