using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FPCamMovement : MonoBehaviour
{
    [SerializeField] Transform cameraPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPos.position;
    }
}
