using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    public float MaxTiltAngle = 10;
    public float SmoothTime = 1;
    public float RotationT = 0.02f;

    Vector3 Offset;
    Vector3 PosVel, RotVel, EulerRot;

    void Start()
    {
        Offset = transform.position + MainChar.Instance.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, MainChar.Instance.transform.position + Offset, ref PosVel, SmoothTime);

        Vector3 rot = transform.localEulerAngles;
        rot.z = MainChar.Instance.GetVaseTilt() * MaxTiltAngle;

        Quaternion target = Quaternion.Euler(rot);

        // transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, rot, ref RotVel, SmoothTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, RotationT);
    }
}
