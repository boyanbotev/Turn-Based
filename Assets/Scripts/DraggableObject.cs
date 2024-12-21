using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class DraggableObject : MonoBehaviour
{
    public static event Action onPuzzlePieceSnapped;

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
  
        GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
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
        GetComponent<SpriteRenderer>().sortingLayerName = "Default";
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
        if (Vector3.Distance(transform.position, target.position) < minSnapDistance)
        {
            transform.position = target.position;
            isSnapped = true;
            onPuzzlePieceSnapped?.Invoke();
        }
    }
}
