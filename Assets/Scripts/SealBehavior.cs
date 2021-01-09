using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealBehavior : MonoBehaviour
{
    public float happinessIncreaseSpeed = 2f;

    private bool penguinInColliderActive = false;

    private Coroutine addHappiness;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameSession.current.penguin && !GameSession.current.penguinBusy)
            if (other.GetComponentInParent<PenguinBehavior>() == GameSession.current.penguin)
            {
                StopAllCoroutines();
                penguinInColliderActive = true;
                happinessIncreaseSpeed = Mathf.Max(happinessIncreaseSpeed, 1.5f * GameSession.current.happinessDecayingSpeed);
                addHappiness = StartCoroutine(AddHappiness());
                GameSession.current.penguinBusy = true;
            }
    }

    // check in case penguin is enabled while inside the collider
    private void OnTriggerStay(Collider other)
    {
        if (GameSession.current.penguin)
            if (other.GetComponentInParent<PenguinBehavior>() == GameSession.current.penguin && !penguinInColliderActive)
                OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameSession.current.penguin)
            if (other.GetComponentInParent<PenguinBehavior>() == GameSession.current.penguin)
            {
                StopAllCoroutines();
                penguinInColliderActive = false;
                StartCoroutine(ReturnToPlace());
                GameSession.current.penguinBusy = false;
            }

    }

    IEnumerator AddHappiness()
    {
        float lerpParam = 0;
        Transform sealTransform = transform.GetChild(0);
        Vector3 initialPos = sealTransform.position;

        Transform penguinTransform = GameSession.current.penguin.transform;
        sealTransform.LookAt(penguinTransform);
        float penguinWidth = penguinTransform.GetComponentInChildren<Collider>().bounds.extents.x * 2;

        Vector3 initialPenguinFwd = penguinTransform.forward;

        sealTransform.GetComponent<Animator>().SetTrigger("happiness");
        penguinTransform.GetComponent<Animator>().SetTrigger("happiness");

        while (true)
        {
            sealTransform.position = Vector3.Lerp(initialPos, penguinTransform.position - sealTransform.forward * penguinWidth, lerpParam);
            penguinTransform.forward = Vector3.Lerp(initialPenguinFwd, (sealTransform.position - penguinTransform.position).normalized, 2 * lerpParam);
            lerpParam += 1f * Time.fixedDeltaTime;

            GameSession.current.penguin.happiness += happinessIncreaseSpeed * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator ReturnToPlace()
    {
        float lerpParam = 0;
        Transform sealTransform = transform.GetChild(0);
        Transform penguinTransform = GameSession.current.penguin.transform;
        Vector3 initialPos = sealTransform.position;
        sealTransform.LookAt(transform);
        Vector3 initialPenguinFwd = penguinTransform.forward;

        sealTransform.GetComponent<Animator>().SetTrigger("happiness");
        penguinTransform.GetComponent<Animator>().SetTrigger("happiness");

        while (Vector3.Distance(sealTransform.position, transform.position) > 0.01)
        {
            sealTransform.position = Vector3.Lerp(initialPos, transform.position, lerpParam);
            penguinTransform.forward = Vector3.Lerp(initialPenguinFwd, -penguinTransform.parent.forward, 2 * lerpParam);
            lerpParam += 1f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        lerpParam = 0;
        Vector3 initialSealFwd = sealTransform.forward;
        while (lerpParam <= 1f)
        {
            sealTransform.forward = Vector3.Lerp(initialSealFwd, -transform.forward, lerpParam);
            lerpParam += 2f * Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
