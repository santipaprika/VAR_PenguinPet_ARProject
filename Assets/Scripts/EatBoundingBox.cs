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
        {
            penguin.hunger += fish.hungerContribution;
            StartCoroutine(HoldFish(fish));
        }
    }

    IEnumerator HoldFish(Fish fish)
    {
        eaten = false;
        fish.isGrabbed = false;
        Vector3 initialFishPosition = fish.transform.position;
        Vector3 destFishPosition = transform.position;

        float lerpParam = 0;
        while(lerpParam <= 1)
        {
            fish.transform.position = Vector3.Lerp(initialFishPosition, destFishPosition, lerpParam);
            lerpParam += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        Destroy(fish.gameObject);
        eaten = true;
    }
}
