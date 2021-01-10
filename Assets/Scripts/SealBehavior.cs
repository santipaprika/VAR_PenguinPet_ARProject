using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealBehavior : MonoBehaviour
{
    public float happinessIncreaseSpeed = 2f;
    public float max_distance = 0.2f;
    private bool interacting = false;

    private PenguinBehavior penguin;

    // Update is called once per frame
    void Update()
    {
        if (!penguin)
        {
            penguin = GameSession.current.penguin;
            return;
        }

        transform.position = transform.parent.position;

        if (GameSession.current.penguinBusy && !interacting) return;

        if (!interacting)
        {
            if (Vector3.Distance(transform.position, penguin.transform.position) <= max_distance && GameSession.current.penguinTracked && GameSession.current.sealTracked)
            {
                StopAllCoroutines();
                happinessIncreaseSpeed = Mathf.Max(happinessIncreaseSpeed, 1.5f * GameSession.current.happinessDecayingSpeed);
                StartCoroutine(PerformHappiness());
                GameSession.current.penguinBusy = true;
                interacting = true;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, penguin.transform.position) > max_distance || !GameSession.current.penguinTracked || !GameSession.current.sealTracked)
            {
                StopAllCoroutines();
                StartCoroutine(ReturnToPlace());
                GameSession.current.penguinBusy = false;
                interacting = false;
            }
        }

    }

    IEnumerator PerformHappiness()
    {
        float lerpParam = 0;
        Transform sealTransform = transform.GetChild(0);
        Vector3 initialPos = sealTransform.position;

        Transform penguinTransform = GameSession.current.penguin.transform;
        sealTransform.LookAt(penguinTransform);
        float penguinWidth = penguinTransform.GetComponentInChildren<Collider>().bounds.extents.x *  2;

        Vector3 initialSealFwd = sealTransform.forward;
        Vector3 dstSealFwd = (penguinTransform.position - sealTransform.position).normalized;
        Vector3 initialPenguinFwd = penguinTransform.forward;
        Vector3 dstPenguinFwd = (sealTransform.position - penguinTransform.position).normalized;

        sealTransform.GetComponent<Animator>().SetTrigger("happiness");
        penguinTransform.GetComponent<Animator>().SetTrigger("happiness");

        while (true)
        {
            sealTransform.position = Vector3.Lerp(initialPos, penguinTransform.position - sealTransform.forward * penguinWidth, lerpParam);
            penguinTransform.forward = Vector3.Lerp(initialPenguinFwd, dstPenguinFwd, 2f * lerpParam);
            //sealTransform.forward = Vector3.Lerp(initialSealFwd, dstSealFwd, lerpParam);
            
            lerpParam += Time.fixedDeltaTime;

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
        //Vector3 initialSealFwd = sealTransform.forward;

        sealTransform.GetComponent<Animator>().SetTrigger("happiness");
        penguinTransform.GetComponent<Animator>().SetTrigger("happiness");

        while (lerpParam <= 1f)
        {
            sealTransform.position = Vector3.Lerp(initialPos, transform.position, lerpParam);
            penguinTransform.forward = Vector3.Lerp(initialPenguinFwd, -penguinTransform.parent.forward, 2f * lerpParam);
            //sealTransform.forward = Vector3.Lerp(initialSealFwd, -sealTransform.parent.forward, lerpParam);
            lerpParam += Time.fixedDeltaTime;
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
