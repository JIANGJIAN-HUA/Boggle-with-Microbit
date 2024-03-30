using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRot : MonoBehaviour
{
    public float rotateSpeed = 2f;
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",rotateSpeed * Time.time);
    }
}
