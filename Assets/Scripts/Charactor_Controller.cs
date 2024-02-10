using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor_Controller : MonoBehaviour
{
    public Interactor interactor;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            interactor.checkInteracting();
        }
    }

}
