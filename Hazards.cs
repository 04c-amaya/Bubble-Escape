using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hazards : MonoBehaviour
{
    Camera cam;

    [HideInInspector]
    public bool usingDropPoint;
    [HideInInspector]
    public bool goingBetweenTwoPoints;
    [HideInInspector]
    public bool usingGlide;
    [HideInInspector]
    public bool goingDirection;

    [HideInInspector]
    public MovementType type;
    Rigidbody2D _rb;

    [Header("Two Point Movement")]
    [HideInInspector]
    public GameObject pointA;
    [HideInInspector]
    public GameObject pointB;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public bool isAtPointA;

    [Header("Two Point Movement")]
    [HideInInspector]
    public GameObject dropPoint;
    [HideInInspector]
    public float _gravity;
    bool teleportedToPoint;
    [HideInInspector]
    public bool verticalMovement;
    [HideInInspector]
    public bool horizontalMovement;

    [Header("Glide")]
    [HideInInspector]
    public bool glideUp;
    [HideInInspector]
    public bool glideDown;
    //PointA
    //PointB
    //Speed
    //go left
    // go right

    [Header("Go Direction")]
    [HideInInspector]
    public GameObject spawnPoint;
    [HideInInspector]
    public bool goLeft;
    [HideInInspector]
    public bool GoRight;
    //Speed

    Vector2 screenView;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (goingBetweenTwoPoints)
        {
            usingDropPoint = false;
            //Debug.Log("Going between two points");
            MoveBetweenTwoPoints();
        }
        else if (usingDropPoint)
        {
            DropHazard();
        }
        else if (usingGlide)
        {
            //Debug.Log("isGliding");
            Gliding();
        }
        else if (goingDirection)
        {

            GoDirection();
        }
        CameraViewToScreen();
    }

    public void MoveBetweenTwoPoints()
    {
        if (horizontalMovement)
        {
            if (isAtPointA)
            {
                Debug.Log("Going To B");
                gameObject.transform.Translate(Vector2.right * speed / 100);
                if (gameObject.transform.localScale.x<0)
            {
                    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                }
                else
                {
                    gameObject.transform.localScale = new Vector2(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                }
               
            }
            else
            {
                Debug.Log("Going To A");
                gameObject.transform.Translate(-Vector2.right * speed / 100);
                if (gameObject.transform.localScale.x < 0)
                {
                    gameObject.transform.localScale = new Vector2(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                }
                else
                {
                    gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x, gameObject.transform.localScale.y);
                }
            }
        } else if (verticalMovement)
        {
            if (isAtPointA)
            {
                Debug.Log("Going To B");
                gameObject.transform.Translate(-Vector2.up * speed / 100);
                gameObject.transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                Debug.Log("Going To A");
                gameObject.transform.Translate(Vector2.up * speed / 100);
                gameObject.transform.localScale = new Vector2(1, 1);
            }
        }
       

        if (pointA.name != "PointA")
        {
            pointA.name = "PointA";
        }
        if (pointB.name != "PointB")
        {
            pointB.name = "PointB";
        }
    }
    public void DropHazard()
    {
        _rb.gravityScale = _gravity;
    }
    public void Gliding()
    {
        if (glideUp)
        {
            if (goLeft)
            {
                gameObject.transform.Translate(-Vector2.right * speed / 100);
            }
            else if (GoRight)
            {
                gameObject.transform.Translate(Vector2.right * speed / 100);
            }
            gameObject.transform.Translate(Vector2.up * speed / 100);

        }
        else if (glideDown)
        {
            if (goLeft)
            {
                gameObject.transform.Translate(-Vector2.right * speed / 100);
            }
            else if (GoRight)
            {
                gameObject.transform.Translate(Vector2.right * speed / 100);
            }
            gameObject.transform.Translate(Vector2.down * speed / 100);
        }
    }
    public void GoDirection()
    {
        if (goLeft)
        {
            gameObject.transform.Translate(-Vector2.right * speed / 100);
        }
        else if (GoRight)
        {
            gameObject.transform.Translate(Vector2.right * speed / 100);
        }
    }
    void SelfDestruct()
    {
        if (gameObject.transform.parent != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void CameraViewToScreen()
    {
        screenView = cam.WorldToViewportPoint(transform.position);
        if (screenView.y < 0)
        {
            SelfDestruct();
           // Debug.Log("Destroyed");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<PlayerManager>().Dead();
            WorldManager.Instance.End();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (goingBetweenTwoPoints)
        {
            if (collision.gameObject.name == "PointA")
            {
                isAtPointA = true;
            }
            if (collision.gameObject.name == "PointB")
            {
                isAtPointA = false;
            }
        }
    }
}

[CustomEditor(typeof(Hazards))]
public class MovementType : Editor
{

    bool hide;
    private void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        var hazards = (Hazards)target;
        hazards.type = this;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        hazards.goingBetweenTwoPoints = EditorGUILayout.Toggle("Going Between Two Points", hazards.goingBetweenTwoPoints);
        hazards.usingDropPoint = EditorGUILayout.Toggle("Drop Point", hazards.usingDropPoint);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        hazards.usingGlide = EditorGUILayout.Toggle("Glide", hazards.usingGlide);
        hazards.goingDirection = EditorGUILayout.Toggle("Go Direction", hazards.goingDirection);
        EditorGUILayout.EndHorizontal();


        if (hazards.goingBetweenTwoPoints)
        {
            hazards.pointA = (GameObject)EditorGUILayout.ObjectField("Point A", hazards.pointA, typeof(GameObject), true);
            hazards.pointB = (GameObject)EditorGUILayout.ObjectField("Point B", hazards.pointB, typeof(GameObject), true);

            hazards.verticalMovement = EditorGUILayout.Toggle("Vertical Movement", hazards.verticalMovement);
            hazards.horizontalMovement = EditorGUILayout.Toggle("Horizontal Movement", hazards.horizontalMovement);

            hazards.speed = EditorGUILayout.FloatField("Speed", hazards.speed);
        }
        else if (hazards.usingDropPoint)
        {
            hazards._gravity = EditorGUILayout.FloatField("Gravity", hazards._gravity);
        }
        else if (hazards.usingGlide)
        {
            hazards.glideDown = EditorGUILayout.Toggle("Glide Down", hazards.glideDown);
            hazards.glideUp = EditorGUILayout.Toggle("Glide Up", hazards.glideUp);

            hazards.goLeft = EditorGUILayout.Toggle("Go left", hazards.goLeft);
            hazards.GoRight = EditorGUILayout.Toggle("Go right", hazards.GoRight);

            hazards.speed = EditorGUILayout.FloatField("Speed", hazards.speed);
        }
        else if (hazards.goingDirection)
        {
            hazards.goLeft = EditorGUILayout.Toggle("Go left", hazards.goLeft);
            hazards.GoRight = EditorGUILayout.Toggle("Go right", hazards.GoRight);
            hazards.speed = EditorGUILayout.FloatField("Speed", hazards.speed);
        }
    }

}
