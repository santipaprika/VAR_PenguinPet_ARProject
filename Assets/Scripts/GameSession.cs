using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [HideInInspector]
    public static GameSession current;
    [HideInInspector]
    public PenguinBehavior penguin;
    [HideInInspector]
    public bool penguinBusy = false;
    public float actionTriggerDistance = .5f;
    public float initialHunger = 50;
    public float initialHappiness = 30;
    public float initialHigiene = 30;
    public float maxStatValue = 100; 
    public float hungerDecayingSpeed = 1f;
    public float happinessDecayingSpeed = .5f;
    public float higieneDecayingSpeed = .5f;
    public LayerMask touchablesLayer;

    public Transform cleaningObject;
    public Toggle washToggle;
    private Transform _cleaningObject;

    void Awake()
    {
        current = this;
        _cleaningObject = Instantiate(cleaningObject);
        _cleaningObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonUp(0)) {
            Transform touchedGOTransform = RaycastProvider.CheckRaycastWithTouchables(touchablesLayer.value);
            if (!penguin && touchedGOTransform) {
                penguin = touchedGOTransform.GetComponent<PenguinBehavior>();   // if there is no such component, null will be set
                if (penguin) {
                    penguin.initialize(initialHunger, initialHappiness, initialHigiene);
                    washToggle.gameObject.SetActive(true);
                }
            }
        }

        if (penguin) penguin.updateState();
    }

    public void SpawnSponge() {
        if (washToggle.isOn) {
            _cleaningObject.position = penguin.transform.position + 
                    penguin.transform.forward * penguin.GetComponent<Collider>().bounds.extents.z +
                    penguin.transform.up * penguin.GetComponent<Collider>().bounds.extents.y;
        }
        _cleaningObject.gameObject.SetActive(washToggle.isOn);
    }
}
