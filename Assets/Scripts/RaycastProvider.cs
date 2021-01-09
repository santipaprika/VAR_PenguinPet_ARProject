using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class RaycastProvider
{
    public static Transform CheckRaycastWithTouchables(int touchablesLayer) {
        
        int layerMask = touchablesLayer;

#if UNITY_EDITOR
        // Construct a ray from the current touch coordinates
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f, layerMask))
        {
            hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("touch");
            return hit.collider.gameObject.GetComponentInParent<Animator>().transform;
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
                    hit.collider.gameObject.GetComponentInParent<Animator>().SetTrigger("touch");
                    return hit.collider.gameObject.GetComponentInParent<Animator>().transform;
                }
            }
        }
#endif
        return null;
    }
}
