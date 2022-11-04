using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject arrow;
    public Transform shotPoint;

    public float launchForce = 20;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector2 direction;
    private Camera camera1;
    bool isShoting = false;

    public GameObject point;
    public GameObject trajectory;
    private GameObject[] points;
    private GameObject newArrow;
    public int numberPoint;
    public float spacePoint;

    // Start is called before the first frame update
    void Start()
    {
        camera1 = Camera.main;
        points = new GameObject[numberPoint];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Instantiate(point, shotPoint.position, Quaternion.identity);
            points[i].transform.parent = trajectory.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isShoting = CheckArrowFlying();
        HandleShotArrow();
    }

    bool CheckArrowFlying() {
        //Check if any arrows are flying
        if (!newArrow) return false;
        return newArrow.GetComponent<Arrow>().IsFlying();
    }

    void BowLookDirection(Vector2 direction)
    {
        //change the bow in the direction
        transform.right = direction;
    }

    void HandleShotArrow()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Get the starting point at the left click position
        if (Input.GetMouseButtonDown(0))
        {
            startPos = camera1.ScreenToWorldPoint(Input.mousePosition);
        }

        //Get current location
        //Update the predicted trajectory of the arrow
        //Change the direction of the bow according to the trajectory
        if (Input.GetMouseButton(0) && !isShoting)
        {
            Vector3 current = camera1.ScreenToWorldPoint(Input.mousePosition);
            direction = startPos - current;
            BowLookDirection(direction);
            UpdateTrajectory();
        }
        else
        {
            BowLookDirection(mousePosition - bowPosition);
        }

        //Get the end position when the mouse is released
        //Fire a arrow
        if (Input.GetMouseButtonUp(0) && !isShoting)
        {
            endPos = camera1.ScreenToWorldPoint(Input.mousePosition);
            direction = startPos - endPos;
            Shot();
        }
    }
    private void Shot()
    {
        //Fire an arrow
        //Set arrow parameters such as starting position, direction, force
        newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        newArrow.GetComponent<Arrow>().Set(shotPoint.position, direction,launchForce);
    }
    Vector2 CalculatorPositionOfPoint(float t)
    {
        //The formula for calculating arrow trajectory by time
        Vector2 position = (Vector2)shotPoint.position + direction * launchForce * t + 0.5f * Physics2D.gravity * (t * t);
        return position;
    }
    private void UpdateTrajectory()
    {
        //Update points according to flight trajectory

        Vector2 prePoint = shotPoint.position;
        RaycastHit2D raycast = new RaycastHit2D() ;
        bool hit = false;

        for (int i = 0; i < numberPoint; i++)
        {
            //If the object collides, set the point at the initial position
            if (hit)
            {
                points[i].transform.position = shotPoint.position;
                continue;
            }

            //Create raycast to detect collision between 2 points
            //If a collision occurs, mark and set the movePoint to the default position
            Vector2 movePoint = CalculatorPositionOfPoint(i *spacePoint);
            raycast = Physics2D.Raycast(prePoint, movePoint-prePoint, Vector2.Distance(prePoint, movePoint));
            if (raycast)
            {
                Debug.Log(raycast.collider.name +" "+ Time.time);
                hit = true;
                movePoint = prePoint;
            }
            prePoint = movePoint;
            points[i].transform.position = movePoint;
        }
    }
}
