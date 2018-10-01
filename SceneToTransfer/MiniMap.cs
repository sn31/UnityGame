using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MiniMap : MonoBehaviour
{

    public Camera MiniMapCamera;

    //target for minimap to follow
    public Transform Target;

    //zoom variables
    public float ZoomAmount; //per click
    public float MinZoom;
    public float MaxZoom;
    public float CurrentZoom;

    void start()
    {
    }

    void LateUpdate()
    {
        //get target location and store in variable
        Vector3 newPosition = Target.position;

        //set Y position of target to y position of minimap camera
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        //to rotate minimap with rotation of target
        transform.rotation = Quaternion.Euler(90f, Target.eulerAngles.y, 0f);

         CurrentZoom = MiniMapCamera.orthographicSize;
    }

    public void ZoomIn()
    {
        if (MiniMapCamera.orthographicSize > MinZoom * 2)
        {
            MiniMapCamera.orthographicSize -= ZoomAmount;
        }
    }

    public void ZoomOut()
    {
        if (MiniMapCamera.orthographicSize < MaxZoom)
        {
            MiniMapCamera.orthographicSize += ZoomAmount;
        }
    }
}
