using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCamarasMenu : MonoBehaviour
{
    [Header("Camaras")]
    public Camera camera1;
    public Camera camera2;

    private bool isCamera1Active = true;
    
    void SwitchCamera()
    {
         if (isCamera1Active) 
         {
            camera1.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
         }
         else
         {
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
         }
         isCamera1Active = !isCamera1Active;
    }
}

