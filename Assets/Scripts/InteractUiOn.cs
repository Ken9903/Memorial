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

    int outsidePlayerMask = 1 << 6;
    int interactMask = ~(1 << 8);

    void init()
    {
        interactUiShowBox = GameObject.Find("InteractUiShowBox").GetComponent<Transform>();
        cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }
    public IEnumerator changeUiPos(GameObject target, float layDistance)
    {
        init();
        targetObject = target;
        Collider[] colliders;
        while (true)
        {
            stay = false;
            colliders =
                  Physics.OverlapBox(interactUiShowBox.position, interactUiShowBox.localScale / 2 , interactUiShowBox.rotation, interactMask);

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
                if (Physics.Raycast(target.gameObject.transform.position, cam.transform.position - target.gameObject.transform.position, out hit, Mathf.Infinity, ~outsidePlayerMask))
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
        StartCoroutine(destroyUi(this.gameObject, target));
    }
    IEnumerator destroyUi(GameObject instantUi, GameObject target)
    {
        yield return new WaitForEndOfFrame();
        target.GetComponent<objectInteracter>().uiOn = false;
        Destroy(instantUi);
    }
}
