using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnObstacle : MonoBehaviour
{
    [SerializeField] GameObject obstaclePrefab = null;
    private Camera cam = null;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Time.timeScale != 0) {
            SpawnAtMousePosition();
            DeleteAtMousePosition();
            RotateAtMousePosition();
        }
    }

    private void SpawnAtMousePosition()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(obstaclePrefab, hit.point, Quaternion.identity);
            }
        }
    }
    private void DeleteAtMousePosition()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {   
                if (hit.collider.gameObject.tag == "Obstacle")
                {
                    RemoveObjectAndChildrenRecursive(hit.collider.gameObject);
                }        
            }
        }
    }

    private void RotateAtMousePosition()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {   
                if (hit.collider.gameObject.tag == "Obstacle")
                {
                    Debug.Log("hit");
                    hit.collider.gameObject.transform.Rotate(90, 0, 0);
                }        
            }
        }
    }

     private void RemoveObjectAndChildrenRecursive(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            RemoveObjectAndChildrenRecursive(child.gameObject);
        }

        Destroy(obj);
    }
}
