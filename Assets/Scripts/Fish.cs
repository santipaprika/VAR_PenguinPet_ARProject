using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public float hungerContribution = 20f;
    public bool isGrabbed = false;
    private EatBoundingBox eatBoundingBox;

    private void Start()
    {
        eatBoundingBox = FindObjectOfType<EatBoundingBox>(true);
    }

    public void Update()
    {
        if (isGrabbed && eatBoundingBox.gameObject.activeSelf)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
                Grab();
#else
            if (Input.touchCount > 0)
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    Grab();
#endif

            RaycastHit hit;
            if (RaycastProvider.GetRaycastHit(eatBoundingBox.raycastTargetLayer, out hit))
                if (hit.point.y > transform.parent.position.y)
                    transform.position = hit.point;
        }    
    }

    public void Grab()
    {
        if (eatBoundingBox.gameObject.activeSelf)
        {
            isGrabbed = !isGrabbed;
            GameSession.current.penguinBusy = isGrabbed;

            if (isGrabbed)
            {
                eatBoundingBox._invisibleDirectionPlane.gameObject.SetActive(true);
                Vector3 right = eatBoundingBox.transform.up;
                Vector3 forward = (eatBoundingBox.transform.position - transform.position).normalized;
                eatBoundingBox._invisibleDirectionPlane.up = Vector3.Cross(right, forward);
                print("Modifying plane");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
