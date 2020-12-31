using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public LayerMask touchablesLayer;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonUp(0)) {
            RaycastProvider.CheckRaycastWithTouchables(touchablesLayer.value);
        }
    }
}
