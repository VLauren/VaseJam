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

    GameObject EndCamera;
    Vector3 EndLook;

    void Start()
    {
        Offset = transform.position + MainChar.Instance.transform.position;
        EndCamera = GameObject.Find("EndCamera");

        if(EndCamera != null)
        {
            Plane ground = new Plane(Vector3.up, Vector3.zero);

            float enter;
            Ray ray = new Ray(EndCamera.transform.position, EndCamera.transform.forward);
            if(ground.Raycast(ray, out enter))
            {
                EndLook = ray.GetPoint(enter);
            }
        }
    }

    void Update()
    {
        if(MainChar.Instance.CanControl)
        {
            transform.position = Vector3.SmoothDamp(transform.position, MainChar.Instance.transform.position + Offset, ref PosVel, SmoothTime);

            Vector3 rot = transform.localEulerAngles;
            rot.z = MainChar.Instance.GetVaseTilt() * MaxTiltAngle;

            Quaternion target = Quaternion.Euler(rot);

            // transform.localEulerAngles = Vector3.SmoothDamp(transform.localEulerAngles, rot, ref RotVel, SmoothTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, RotationT);
        }
        else if(EndCamera != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, EndCamera.transform.position, ref PosVel, 3f);
            // transform.rotation = Quaternion.RotateTowards(transform.rotation, EndCamera.transform.rotation, Time.deltaTime / 3f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(EndLook - transform.position, Vector3.up), Time.deltaTime * 10);

            // transform.LookAt(EndLook);
        }
    }
}
