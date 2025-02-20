using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public Vector3 offset;
    public bool isDragging = false;
    public Camera cam;

    public float gridSize = .5f; // Adjust based on your grid

    void Start()
    {
        cam = Camera.main; 
    }

    void OnMouseDown()
    {
        if (cam == null) return;

        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 newPos = GetMouseWorldPosition() + offset;
        newPos = SnapToGrid(newPos);
        transform.position = newPos;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = cam.WorldToScreenPoint(transform.position).z;
        return cam.ScreenToWorldPoint(mousePoint);
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float y = Mathf.Round(pos.y / gridSize) * gridSize;
        return new Vector3(x, y, pos.z);
    }
}
