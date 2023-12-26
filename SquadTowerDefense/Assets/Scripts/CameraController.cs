using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    public float zoomSpeed = 5000f;

    public float rotateSpeed = 0.25f;

    public float maxHeight = 80f;
    public float maxRotation = 40f; // Maximum rotation limit in both upward and downward directions
    
    //This will probably be level dependent, but testing shows that if the camera goes into an object bad things happen.
    public float minHeight = 30f;
    private float totalRotation = 0f;

    private Vector2 p1;
    private Vector2 p2;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;;
            zoomSpeed = 5000f;
        }
        else
        {
            speed = 5f;
            zoomSpeed = 5000f;
        }
        
        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal");
        float vsp = transform.position.y * speed * Input.GetAxis("Vertical");
        float scrollSpeed = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        if ((transform.position.y >= maxHeight))
            scrollSpeed = 0;
        else if ((transform.position.y <= minHeight))
            scrollSpeed = 0;

        if ((transform.position.y + scrollSpeed) > maxHeight)
            scrollSpeed = maxHeight - transform.position.y;
        else if ((transform.position.y + scrollSpeed) < minHeight)
            scrollSpeed = minHeight - transform.position.y;
        
        Vector3 verticalMove = new Vector3(0, scrollSpeed * 5, 0);
        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;

        forwardMove.y = 0;
        forwardMove.Normalize();

        forwardMove *= vsp;

        Vector3 move = (verticalMove + lateralMove + forwardMove) * Time.deltaTime;
        
        transform.position += move;

        GetCameraRotation();
    }

    void GetCameraRotation()
    {
        if(Input.GetMouseButtonDown(1))
        {
            p1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            p2 = Input.mousePosition;
            float dx = (p1 - p2).x * rotateSpeed * Time.deltaTime;
            float dy = (p2 - p1).y * rotateSpeed * Time.deltaTime;
            
            if (Mathf.Abs(dy) > 0)
            {
                totalRotation += dy;
                if (Mathf.Abs(totalRotation) > maxRotation)
                {
                    dy -= Mathf.Sign(totalRotation) * (Mathf.Abs(totalRotation) - maxRotation);
                    totalRotation = Mathf.Sign(totalRotation) * maxRotation;
                }
            }

            transform.Rotate(new Vector3(0, -dx, 0), Space.World);
            transform.Rotate(new Vector3(-dy, 0, 0), Space.Self);
        }
    }
}
