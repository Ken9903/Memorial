using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class InteractUiSetter : MonoBehaviour
{
    public float centerLayDistance = 3f;
    public float layDistance = 4f;
    public float optimization_centerUiSet = 0.1f;

    public GameObject interactUi;
    public Camera cam;
    public Canvas mainCanvas;

    public GameObject interactUiShowBox;

    private RaycastHit hit;
    private RaycastHit hit_object;

    public LayerMask playerMask;
    public LayerMask outsidePlayerMask;

    private void Start()
    {
        StartCoroutine(centerUiSet());
    }

    IEnumerator centerUiSet() //������ ���� ������ Ui -> 
    {
        bool showing = false;
        while (true)
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit_object, centerLayDistance, ~outsidePlayerMask))
            {
                if (hit_object.collider.gameObject.CompareTag("InteractUi") && showing == false && hit_object.collider.GetComponent<objectInteracter>().uiOn == false)
                {
                    showing = true;
                    makeInteractUi(hit_object.collider.gameObject);
                }
                else if(!hit_object.collider.gameObject.CompareTag("InteractUi"))
                {
                    showing = false;
                }
            }
            yield return new WaitForSeconds(optimization_centerUiSet); //����ȭ
        }
    }

    public void makeInteractUi(GameObject target)
    {
        //Ui��ü���� �ѱ��
        GameObject instantUi = Instantiate(target.GetComponent<objectInteracter>().ui, mainCanvas.transform);
        target.GetComponent<objectInteracter>().uiOn = true;
        StartCoroutine(instantUi.GetComponent<InteractUiOn>().changeUiPos(target, layDistance));
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("InteractUi") && other.GetComponent<objectInteracter>().uiOn == false)
        {
            if (Physics.Raycast(other.gameObject.transform.position, cam.transform.position - other.gameObject.transform.position  , out hit, Mathf.Infinity, ~outsidePlayerMask)) //�ѹ� ȣ���� �Ǿ ���ѹ��� ���̸���
            {
                if (hit.collider.gameObject.layer == 3)
                {
                    makeInteractUi(other.gameObject);
                }
            }
        }

        #region DebugcheckPoint
        /*
         * ����ĳ��Ʈ ���ܽ� Ʈ���Ű� ���� �� ���·� ����ĳ��Ʈ Hit ���� ������ Enter�� Ư���� �ѹ� �������� ���� �÷��̰� �Ų����� ���� �� ����
         * ���� �߻��� Stay�� �����ϰų� ���� ���� ���
         */

        #endregion
    }
}
