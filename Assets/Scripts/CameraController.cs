using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 resetCamera;
    private Vector3 origin;
    private Vector3 difference;
    
    private bool drag = false;

    private void Start() {
        resetCamera = Camera.main.transform.position;
    }
 
    private void LateUpdate() {
        // Click and Drag Camera with left click
        if (Input.GetMouseButton(0)) {
            
            difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (!drag) {
                drag = true;
                origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else {
            drag = false;
        }

        if (drag) {
            Camera.main.transform.position = origin - difference;
        }

        // Reset Camera Position with right click
        if (Input.GetMouseButton(1)) {
            Camera.main.transform.position = resetCamera;
        }
    }
}
