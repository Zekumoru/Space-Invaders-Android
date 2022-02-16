using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInputHandler : MonoBehaviour
{
    CircleCollider2D circleCollider2D;
    GameObject shipGameObject;

    ShipDraggingInputEvent draggingEvent = new ShipDraggingInputEvent();
    ShipReleasingInputEvent releasingEvent = new ShipReleasingInputEvent();

    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        shipGameObject = GameObject.FindGameObjectWithTag("Ship");
        EventManager.AddEventHandler(EventName.ShipDraggingInput, draggingEvent);
        EventManager.AddEventHandler(EventName.ShipReleasingInput, releasingEvent);
    }

    void Update()
    {
        HandleTouchInput();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (shipGameObject != null)
        {
            transform.position = shipGameObject.transform.position;
        }
    }

    void HandleTouchInput()
    {
        if (Input.GetMouseButtonDown(0) && circleCollider2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
        {
            draggingEvent.Invoke(gameObject);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            releasingEvent.Invoke(gameObject);
        }
    }
}
