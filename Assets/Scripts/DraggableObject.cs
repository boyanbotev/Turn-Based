using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class DraggableObject : MonoBehaviour
{
    public static event Action onSnapped;
    public static event Action<DraggableObject> onSelected;
    
    [SerializeField] bool isAdjustable = false; // whether can be adjusted after being placed in pos
    [SerializeField] bool useMultipleTargets = true;
    [SerializeField] public Transform targets; // Can be multiple targets or single target depending on useMultipleTargets
    private bool isDragging = false;
    public bool isSnapped = false;
    private float minSnapDistance = 1f;

    public virtual void OnClick() {
        if (isSnapped && !isAdjustable) return;

        isSnapped = false;
        isDragging = true;
        onSelected?.Invoke(this);
    }

    public virtual void OnDrag()
    {
        if (!isDragging || isSnapped) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    public virtual void OnRelease()
    {
        isDragging = false;

        if (useMultipleTargets)
        {
            SnapToMultipleTargets();
        }
        else
        {
            Snap(targets);
        }
    }

    private void SnapToMultipleTargets()
    {
        for (int i = 0; i < targets.childCount; i++)
        {
            Transform target = targets.GetChild(i);
            Snap(target);
        }
    }
    public void Snap(Transform target)
    {
        if (Vector2.Distance(transform.position, target.position) < minSnapDistance)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
            isSnapped = true;
            onSnapped?.Invoke();
            if (!isAdjustable)
            {
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public virtual void Reset()
    {
        isSnapped = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public virtual void UpdateZIndex(float zIndex)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);
    }
}
