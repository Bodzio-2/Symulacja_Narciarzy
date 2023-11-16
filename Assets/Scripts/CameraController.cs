using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject pivotPoint;
    public float camSensitivity = 5f;
    public float updateSkierListCooldown = 1f;
    public float scrollSpeed = 0.5f;
    Camera cam;


    private List<GameObject> skiers;

    private float skierUpdateTimer = 0f;

    private void Start()
    {
        skiers = new List<GameObject>();
        cam = GetComponentInChildren<Camera>();
        UpdateSkierList();
    }


    private void Update()
    {
        transform.position = pivotPoint.transform.position;
        MouseControl();
        UpdateSkierList();
        if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            ChangeViewedSkier();
    }

    void MouseControl()
    {
        float movX = Input.GetAxis("Mouse X") * camSensitivity;
        float movY = Input.GetAxis("Mouse Y") * camSensitivity;

        Vector3 eulerRotation = new(movY, movX);

        Vector3 mouseZoom = new Vector3(0, 0, Input.mouseScrollDelta.y * scrollSpeed);
        cam.transform.Translate(mouseZoom);

        // Keep Z rotation at 0
        Vector3 eulerAngles = transform.eulerAngles;
		transform.eulerAngles = new Vector3(Mathf.Clamp(eulerAngles.x+eulerRotation.x, 1f, 45f), eulerAngles.y + eulerRotation.y , 0 );
    }

    void UpdateSkierList()
    {
        if (Time.time > skierUpdateTimer)
        {
            GameObject[] new_skiers = GameObject.FindGameObjectsWithTag("Skier");
            foreach (GameObject skier in new_skiers)
            {
                if (!skiers.Contains(skier))
                {
                    skiers.Add(skier);
                }
            }
            skierUpdateTimer = Time.time + updateSkierListCooldown;
        }
    }

    void ChangeViewedSkier()
    {
        int currentIndex = skiers.IndexOf(pivotPoint);
        if (Input.GetMouseButtonDown(0))
        {
            // Go forward in list
            if (currentIndex < skiers.Count - 1)
                currentIndex++;
            else
                currentIndex = 0;
        }
        else if(Input.GetMouseButtonDown(1))
        {
            // Go backwards in list
            if (currentIndex > 0)
                currentIndex--;
            else
                currentIndex = skiers.Count - 1;
        }
        pivotPoint = skiers[currentIndex];
    }
}
