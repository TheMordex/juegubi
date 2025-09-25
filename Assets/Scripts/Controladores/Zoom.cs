using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private float ZoomSpeed = 5f;

    void Update()
    {
        if (Input.touchCount != 2)
        {
            return;
        }

        Touch firstTouch = Input.touches[0];
        Touch secondTouch = Input.touches[1];

        Vector2 firstTouchLastPost = firstTouch.position - firstTouch.deltaPosition;
        Vector2 secondTouchLastPost = firstTouch.position - secondTouch.deltaPosition;

        Vector2 initialTouchPos = firstTouch.position - secondTouch.position;
        Vector2 finalTouchPos = firstTouchLastPost - secondTouchLastPost;

        float zoom = initialTouchPos.magnitude - finalTouchPos.magnitude;

        if (zoom != 0f)
        {
            if (Camera.main.orthographic)
            {
                Camera.main.orthographicSize += zoom + ZoomSpeed + Time.deltaTime;
            }
            else
            {
                Camera.main.fieldOfView += zoom + ZoomSpeed + Time.deltaTime;
            }
        }
    }
}
