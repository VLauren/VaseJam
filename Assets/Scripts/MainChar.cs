using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainChar : MonoBehaviour
{
    public static MainChar Instance { get; private set; }

    public float MovementSpeed;
    public float RotationSpeed = 360;

    public GameObject PhysicsVasePrefab;

    public float MaxTilt = 30;

    [Space(10)]
    public float TiltControlStrength = 10;
    public float OscilationStrength;
    public float OscilationFreq;
    public float FallingStrength;

    bool CanControl = true;
    Vector3 MoveInput;
    Vector3 ControlMovement;
    float BalanceInput;

    protected Quaternion TargetRotation;
    protected Animator Animator;
    protected Transform Vase;


    float OscilationVal;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Vase = transform.Find("Vase");
        InvokeRepeating("ChangeOsc", OscilationFreq, OscilationFreq);
    }

    void ChangeOsc()
    {
        OscilationVal = -1 + Random.value * 2;
    }

    void Update()
    {
        Walk();
        Rotation();

        GetComponent<CharacterController>().Move(ControlMovement);

        if(Vase != null)
        {
            // Control de inclinacion
            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * TiltControlStrength * BalanceInput);
            // Oscilacion aleatoria
            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * OscilationStrength * OscilationVal);

            float vaseAngle = Vector3.Angle(Vector3.up, Vase.up);

            // Caida
            Vase.localEulerAngles = new Vector3(0, 0, Vase.localEulerAngles.z - Time.deltaTime * FallingStrength * vaseAngle);

            vaseAngle = Vector3.Angle(Vector3.up, Vase.up);

            if (vaseAngle > MaxTilt)
            {
                StartCoroutine(VaseFall());
            }
        }
    }

    IEnumerator VaseFall()
    {
        GameObject physVase = Instantiate(PhysicsVasePrefab, Vase.Find("VaseModel").position, Vase.Find("VaseModel").rotation);
        physVase.transform.localScale = Vase.Find("VaseModel").localScale;
        physVase.GetComponentInChildren<BreakableObject>().Breakable = false;

        Destroy(Vase.gameObject);

        physVase.GetComponentInChildren<BreakableObject>().GetComponent<Rigidbody>().AddForce((transform.right + Vector3.up) * 1000);

        yield return new WaitForSeconds(0.5f);

        physVase.GetComponentInChildren<BreakableObject>().Breakable = true;
    }

    public float GetVaseTilt()
    {
        if(Vase != null)
        {
            return Vector3.SignedAngle(Vector3.up, Vase.up, transform.forward) / MaxTilt;
        }
        return 0;
    }

    void Walk()
    {
        if (!CanControl) return;

        if (MoveInput.magnitude < 0.1f) MoveInput = Vector3.zero;

        ControlMovement = Vector3.zero;
        ControlMovement = MoveInput * Time.deltaTime * MovementSpeed;

        if (Animator != null)
        {
            float newMS = Mathf.MoveTowards(Animator.GetFloat("MovementSpeed"), MoveInput.magnitude, Time.deltaTime * 10);
            Animator.SetFloat("MovementSpeed", newMS);
        }
    }
    void Rotation()
    {
        if (!CanControl) return;

        float rCam = Camera.main.transform.eulerAngles.y;
        // Direccion relativa a camara
        ControlMovement = Quaternion.Euler(0, rCam, 0) * ControlMovement;
        if (ControlMovement != Vector3.zero)
        {
            TargetRotation = Quaternion.LookRotation(ControlMovement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 raw = value.Get<Vector2>();
        MoveInput = Vector2.zero;
        if (!CanControl) return;
        MoveInput = new Vector3(raw.x, 0, raw.y);
    }

    public void OnBalance(InputValue value)
    {
        BalanceInput = value.Get<float>();
    }

}

