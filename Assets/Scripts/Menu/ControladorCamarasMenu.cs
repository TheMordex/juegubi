using UnityEngine;

public class ControladorCamarasMenu : MonoBehaviour
{
    [Header("Camaras")]
    public Camera camera1;
    public Camera camera2;

    [Header("Canvases")]
    public Canvas canvas1;
    public Canvas canvas2;
    
    private bool _isCamera1Active = true;

    private void Start()
    {
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        _isCamera1Active = true;
        
        canvas1.gameObject.SetActive(true);
        canvas2.gameObject.SetActive(false);
    }
    public void SwitchCamera()
    {
        if (_isCamera1Active)
        {
            camera1.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
            
            canvas1.gameObject.SetActive(false);
            canvas2.gameObject.SetActive(true);
        }
        else
        {
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
            
            canvas1.gameObject.SetActive(true);
            canvas2.gameObject.SetActive(false);
        }

        _isCamera1Active = !_isCamera1Active;
    }
}

