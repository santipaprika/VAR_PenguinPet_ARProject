using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PenguinBehavior : MonoBehaviour
{
    public Transform statsUI;
    private Renderer renderer;
    [HideInInspector]
    public float hunger = 100f, happiness = 100f, higiene = 100f;

    void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (statsUI.gameObject.activeSelf && renderer.isVisible) {
            updateUIPosition();
            GameSession.current.washToggle.gameObject.SetActive(true);
        } else GameSession.current.washToggle.gameObject.SetActive(false);
        
    }

    public void initialize(float hunger0, float happiness0, float higiene0)
    {
        hunger = hunger0;
        happiness = happiness0;
        higiene = higiene0;

        enableUI();
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
        hunger -= GameSession.current.hungerDecayingSpeed * Time.deltaTime;
        statsUI.Find("HungerBar").Find("HungerFillBar").GetComponent<Image>().fillAmount = hunger / GameSession.current.maxStatValue;

        happiness -= GameSession.current.happinessDecayingSpeed * Time.deltaTime;
        statsUI.Find("HappinessBar").Find("HappinessFillBar").GetComponent<Image>().fillAmount = happiness / GameSession.current.maxStatValue;

        higiene -= GameSession.current.higieneDecayingSpeed * Time.deltaTime;
        statsUI.Find("HigieneBar").Find("HigieneFillBar").GetComponent<Image>().fillAmount = higiene / GameSession.current.maxStatValue;

        return;
    }

   
}
