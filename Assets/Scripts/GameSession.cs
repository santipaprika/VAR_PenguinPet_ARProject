using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [HideInInspector]
    public static GameSession current;
    private PenguinBehavior penguin;
    public int initialHunger = 100;
    public int initialHappiness = 100;
    public float hungerDecayingSpeed = 2f;
    public float happinessDecayingSpeed = 2f;
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
                    penguin.enableUI();
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
        print(_cleaningObject.name);
        _cleaningObject.gameObject.SetActive(washToggle.isOn);
    }
}
