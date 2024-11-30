using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private List<DraggableObject> draggedObjects = new List<DraggableObject>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButton(0))
        {
            HandleMouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }

    void HandleMouseDown()
    {
        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (!rayHit.collider) return;

        var draggableObject = rayHit.collider.GetComponent<DraggableObject>();
        if (draggableObject)
        {
            draggableObject.OnClick();
            draggedObjects.Add(draggableObject);
        }
    }

    void HandleMouseDrag()
    {
        draggedObjects.ForEach(draggableObject => draggableObject.OnDrag());
    }

    void HandleMouseUp()
    {
        var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (!rayHit.collider) return;

        var draggableObject = rayHit.collider.GetComponent<DraggableObject>();
        if (draggableObject)
        {
            draggableObject.OnRelease();
            draggedObjects.Remove(draggableObject);
        }
    }
}
