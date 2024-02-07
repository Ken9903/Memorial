using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class InteractUiSetter : MonoBehaviour
{
    public GameObject interactUi;
    public Camera cam;
    public Canvas mainCanvas;

    public GameObject interactUiShowBox;

    private RaycastHit hit;


    public void makeInteractUi(GameObject target)
    {
        GameObject instantUi = Instantiate(interactUi, mainCanvas.transform);
        StartCoroutine(instantUi.GetComponent<InteractUiOn>().changeUiPos(target));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "InteractUi")
        {
            Debug.DrawRay(other.gameObject.transform.position, cam.transform.position - other.gameObject.transform.position , Color.green);
            if (Physics.Raycast(other.gameObject.transform.position, cam.transform.position - other.gameObject.transform.position, out hit,Mathf.Infinity))
            {
                if (hit.collider.gameObject.layer == 3) 
                {
                    makeInteractUi(other.gameObject);
                }
            }
        }
    }
}
