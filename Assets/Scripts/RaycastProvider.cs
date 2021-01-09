using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class RaycastProvider
{
    public static Transform GetRaycastedTouchable(int layer) {
        
        int layerMask = layer;

#if UNITY_EDITOR
        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f, layerMask))
        {
            return hit.collider.GetComponentInParent<Animator>() ? hit.collider.GetComponentInParent<Animator>().transform : hit.collider.transform;
        }
#else

        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                Debug.Log(layerMask);
                if (Physics.Raycast(ray, out hit, 50f, layerMask))
                {
                    return hit.collider.GetComponentInParent<Animator>().transform;
                }
            }
        }
#endif
        return null;
    }


    public static bool GetRaycastHit(int layer, out RaycastHit hit)
    {
        int layerMask = layer;

#if UNITY_EDITOR
        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        return (Physics.Raycast(ray, out hit, 50f, layerMask));
#else

        Touch touch = Input.GetTouch(0);
        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        return (Physics.Raycast(ray, out hit, 50f, layerMask));
#endif
    }
}
