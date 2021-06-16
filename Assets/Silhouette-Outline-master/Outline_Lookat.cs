using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline_Lookat : MonoBehaviour
{
    public Camera cam;
    public float dist;

    private Outline_Controller prevController;
    private Outline_Controller currentController;

    private void Update()
    {
        HandleLookAtRay();
    }

    private void HandleLookAtRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, dist))
        {
            if (hit.collider.CompareTag("Interact"))
            {
                currentController = hit.collider.GetComponent<Outline_Controller>();

                if(prevController != currentController)
                {
                    HideOutline();
                    ShowOutline();
                }

                prevController = currentController;
            }
            else
            {
                HideOutline();
            }
        }
        else
        {
            HideOutline();
        }
    }

    private void ShowOutline()
    {
        if (currentController != null)
        {
            currentController.ShowOutline();
        }
    }

    private void HideOutline()
    {
        if (prevController != null)
        {
            prevController.HideOutline();
            prevController = null;
        }
    }
}
