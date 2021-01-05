using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinBehavior : MonoBehaviour
{
    public Transform statsUI;
    private Renderer renderer;
    private float hunger;

    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (renderer.isVisible && statsUI.gameObject.activeSelf) {
            updateUIPosition();
            GameSession.current.washToggle.gameObject.SetActive(true);
        } else GameSession.current.washToggle.gameObject.SetActive(false);

    }

    public void enableUI() {
        statsUI.gameObject.SetActive(true);
    }

    public void updateUIPosition() {
        // Offset position above object bbox (in world space)
        float offsetPosY = transform.position.y + 2*GetComponent<Collider>().bounds.extents.y;
        
        // Final position of marker above GO in world space
        Vector3 offsetPos = new Vector3(transform.position.x, offsetPosY, transform.position.z);
        
        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
        
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(statsUI.parent.GetComponent<RectTransform>(), screenPoint, null, out canvasPos);
        
        // Set
        statsUI.localPosition = canvasPos;
    }

    public void updateState() {
        return;
    }
}
