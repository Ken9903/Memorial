using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUiOn : MonoBehaviour
{
    bool stay = false;
    Transform interactUiShowBox;
    Camera cam;

    private RaycastHit hit;

    GameObject targetObject;

    void init()
    {
        interactUiShowBox = GameObject.Find("InteractUiShowBox").GetComponent<Transform>();
        cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }
    public IEnumerator changeUiPos(GameObject target, float layDistance)
    {
        init();
        targetObject = target;
        while (true)
        {
            stay = false;
            Collider[] colliders =
                  Physics.OverlapBox(interactUiShowBox.position, interactUiShowBox.localScale, interactUiShowBox.rotation);

            foreach (Collider col in colliders)
            {
                if (col.gameObject.Equals(targetObject))
                {
                    stay = true;
                }
            }
            if (stay == false)
            {
                break;
            }
            else
            {
                //2Áß°Ë»ç
                if (Physics.Raycast(target.transform.position, cam.transform.position - target.gameObject.transform.position, out hit, layDistance))
                {
                    if (hit.collider.gameObject.layer == 3)
                    {
                        Vector3 screenPos = cam.WorldToScreenPoint(target.transform.position);
                        this.transform.position = screenPos;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            yield return null;

        }
        StartCoroutine(destroyUi(this.gameObject));
    }
    IEnumerator destroyUi(GameObject instantUi)
    {
        Debug.Log("destroy");
        yield return new WaitForEndOfFrame();
        Destroy(instantUi);
    }
}
