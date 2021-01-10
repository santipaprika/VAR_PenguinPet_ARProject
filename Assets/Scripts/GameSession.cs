using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public float higieneIncreaseSpeed = 5f;
    public LayerMask touchablesLayer;

    public Transform cleaningObject;
    public Toggle washToggle;
    private Transform _cleaningObject;

    [HideInInspector]
    public bool penguinTracked = false;
    [HideInInspector]
    public bool sealTracked = false;

    public Transform gameOverUI;

    void Awake()
    {
        current = this;
        _cleaningObject = Instantiate(cleaningObject);
        _cleaningObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (washToggle.isOn)
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (RaycastProvider.GetRaycastHit(touchablesLayer, out hit))
                {
                    if (hit.collider.GetComponentInParent<PenguinBehavior>() == penguin)
                    {
                        _cleaningObject.transform.GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
                        penguin.higiene += Time.deltaTime * higieneIncreaseSpeed;
                        _cleaningObject.position = hit.point;
                        _cleaningObject.forward = hit.normal;

                        penguin.audioSource.UnPause();
                        if (!penguin.audioSource.isPlaying)
                            penguin.audioSource.PlayOneShot(penguin.washAudioClip);
                    }
                }
            }
            else
            {
                _cleaningObject.transform.GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(false);
                penguin.audioSource.Pause();
            }
        }

        if ((Input.touchCount > 0 || Input.GetMouseButtonUp(0)) && !penguinBusy) {
            Transform touchedGOTransform = RaycastProvider.GetRaycastedTouchable(touchablesLayer.value);
            if (touchedGOTransform) {
                if (touchedGOTransform.GetComponentInParent<Fish>())
                    touchedGOTransform.GetComponentInParent<Fish>().Grab();
                else
                {
                    touchedGOTransform.GetComponentInParent<Animator>().SetTrigger("touch");

                    if (!penguin)
                    {
                        penguin = touchedGOTransform.GetComponent<PenguinBehavior>();   // if there is no such component, null will be set
                        if (penguin)
                        {
                            penguin.initialize(initialHunger, initialHappiness, initialHigiene);
                            washToggle.gameObject.SetActive(true);
                        }
                    }
                    else
                        penguin.audioSource.PlayOneShot(touchedGOTransform.GetComponent<PenguinBehavior>() ? penguin.greetingAudioClip : penguin.sealAudioClip);
                }
            }
        }

        if (penguin) penguin.updateState();
    }

    public void SpawnSponge() {
        if (washToggle.isOn)
        {
            _cleaningObject.position = penguin.transform.position +
                    penguin.transform.forward * penguin.GetComponentInChildren<Collider>().bounds.extents.z +
                    penguin.transform.up * penguin.GetComponentInChildren<Collider>().bounds.extents.y;
        }
        else
            penguin.audioSource.Stop();
        _cleaningObject.gameObject.SetActive(washToggle.isOn);
        penguinBusy = washToggle.isOn;
        
    }

    public void SetAllFishesKinematic(bool kinematic)
    {
        foreach (Fish fish in FindObjectsOfType<Fish>())
        {
            fish.GetComponent<Rigidbody>().isKinematic = kinematic;
        }
    }

    public void SetSealTrackedState(bool tracked)
    {
        sealTracked = tracked;
    }

    public void SetPenguinTrackedState(bool tracked)
    {
        penguinTracked = tracked;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
