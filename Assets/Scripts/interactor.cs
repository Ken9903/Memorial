using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Camera cam;

    private RaycastHit hit;
    public float layDistance = 4f;

    int layermaskPlayer = 3;

    string currentContent = "";
    public void checkInteracting()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, layDistance, layermaskPlayer))
        {
            if (hit.collider.gameObject.CompareTag("InteractUi"))
            {
                currentContent = hit.collider.gameObject.GetComponent<objectInteracter>().content;
              switch (currentContent)
                {
                    case "search":
                        Debug.Log("search");
                        break;
                    case "anim":
                        Debug.Log("anim");
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
