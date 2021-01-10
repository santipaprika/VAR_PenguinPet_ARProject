using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBoundingBox : MonoBehaviour
{
    private PenguinBehavior penguin;
    public Transform invisibleDirectionPlane;
    [HideInInspector]
    public Transform _invisibleDirectionPlane;
    public LayerMask raycastTargetLayer;
    private bool eaten = true;

    private void Start()
    {
        penguin = transform.parent.GetComponent<PenguinBehavior>();
        _invisibleDirectionPlane = Instantiate(invisibleDirectionPlane, transform);
        _invisibleDirectionPlane.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Fish fish = other.GetComponentInParent<Fish>();
        if (!fish) return;

        if (eaten)
            StartCoroutine(HoldFish(fish));
    }

    IEnumerator HoldFish(Fish fish)
    {
        penguin.GetComponentInParent<Animator>().SetTrigger("eat");
        eaten = false;
        fish.isGrabbed = false;
        Vector3 initialFishPosition = fish.transform.position;
        Vector3 destFishPosition = transform.position;

        float lerpParam = 0;
        while(lerpParam <= 1)
        {
            fish.transform.position = Vector3.Lerp(initialFishPosition, destFishPosition, lerpParam);
            lerpParam += 2.5f * Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        Destroy(fish.gameObject);
        penguin.GetComponentInChildren<AudioSource>().PlayOneShot(penguin.eatAudioClip);
        penguin.hunger += fish.hungerContribution;
        eaten = true;
        GameSession.current.penguinBusy = false;
    }
}
