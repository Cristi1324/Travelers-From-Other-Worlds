using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask walkableMask;
    public Transform feet;
    private float gravityConstant = 6.67408f;
    public float walkSpeed = 8;
    public float runSpeed = 14;
    public float jumpForce = 20;
    public float vSmoothTime = 0.1f;
    public float airSmoothTime = 0.5f;
    public bool done=false;

    public bool lockCursor;
    Rigidbody rb;

    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2 (-40, 85);
    public float rotationSmoothTime = 0.1f;

    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    public float relativeVelocityMagnitude;

    public Vector3 targetVelocity;
    Vector3 cameraLocalPos;
    Vector3 smoothVelocity;
    Vector3 smoothVRef;
    CelestialBody referenceBody;
    Camera cam;
    Camera mapCamera;
    bool readyToFlyShip;
    public Vector3 delta;
    bool isGrounded;
    public Transform playerBody;
    Vector3 strongestGravitionalPull = Vector3.zero;
    private void Update() {
        HandleMovement ();
        if(Input.GetKeyDown(KeyCode.M))
        {
            mapCamera.enabled = true;
            cam.enabled=false;
            //Time.timeScale=0;
        }
        if(Input.GetKeyUp(KeyCode.M))
        {
            mapCamera.enabled = false;
            cam.enabled=true;
            //Time.timeScale=1;
        }
    }

    void Awake () {
        mapCamera = FindObjectOfType<MapCamera>().GetComponent<Camera>();
        mapCamera.enabled=false;
        cam = GetComponentInChildren<Camera> ();
        cameraLocalPos = cam.transform.localPosition;
        InitRigidbody ();

        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void InitRigidbody () {
        rb = GetComponentInChildren<Rigidbody> ();
        rb.interpolation = RigidbodyInterpolation.None;
        rb.useGravity = false;
        rb.isKinematic = false;
    }
    bool IsGrounded () {
        // Sphere must not overlay terrain at origin otherwise no collision will be detected
        // so rayRadius should not be larger than controller's capsule collider radius
        const float rayRadius = 0.3f;
        const float groundedRayDst = 0.5f;
        bool grounded = false;

        if (referenceBody) {
            var relativeVelocity = rb.velocity - referenceBody.GetComponent<Rigidbody>().velocity;
            relativeVelocityMagnitude=relativeVelocity.magnitude;
            // Don't cast ray down if player is jumping up from surface
            if (relativeVelocity.y <= jumpForce * .5f) {
                RaycastHit hit;
                Vector3 offsetToFeet = (feet.position - transform.position);
                Vector3 rayOrigin = rb.position + offsetToFeet + transform.up * rayRadius;
                Vector3 rayDir = -transform.up;

                grounded = Physics.SphereCast (rayOrigin, rayRadius, rayDir, out hit, groundedRayDst, walkableMask);
                Debug.DrawRay(rayOrigin,rayDir,Color.red,1000f);
            }
        }

        return grounded;
    }
    void HandleMovement () {
        //DebugHelper.HandleEditorInput (lockCursor);
        // Look input
        yaw += Input.GetAxisRaw ("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxisRaw ("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp (pitch - Input.GetAxisRaw ("Mouse Y") * mouseSensitivity, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle (smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        float smoothYawOld = smoothYaw;
        smoothYaw = Mathf.SmoothDampAngle (smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);
        cam.transform.localEulerAngles = Vector3.right * smoothPitch;
        playerBody.transform.Rotate (Vector3.up * Mathf.DeltaAngle (smoothYawOld, smoothYaw), Space.Self);

        // Movement
        isGrounded = IsGrounded();
        Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
        float currentSpeed = Input.GetKey (KeyCode.LeftShift) ? runSpeed : walkSpeed;
        targetVelocity = playerBody.transform.TransformDirection (input.normalized) * currentSpeed;
        smoothVelocity = Vector3.SmoothDamp (smoothVelocity, targetVelocity, ref smoothVRef, (isGrounded) ? vSmoothTime : airSmoothTime);

        if (isGrounded) {
            if (Input.GetKeyDown (KeyCode.Space)) {
                rb.AddForce (playerBody.transform.up * jumpForce, ForceMode.VelocityChange);
                isGrounded = false;
            } else {
                // Apply small downward force to prevent player from bouncing when going down slopes
                //rb.AddForce (-transform.up * stickToGroundForce, ForceMode.VelocityChange);
            }
        }
    }

    

    void FixedUpdate() {
        done=false;
        CelestialBody[] bodies = FindObjectsOfType<CelestialBody>();
        float maxAcceleration=0;
        CelestialBody maxAccelerationBody = null;
        // Gravity
        foreach (CelestialBody body in bodies) {
            float sqrDst = (body.GetComponent<Rigidbody>().position - rb.position).sqrMagnitude;
            Vector3 forceDir = -(rb.position - body.GetComponent<Rigidbody>().position).normalized;
            Vector3 acceleration = forceDir * gravityConstant * body.mass * rb.mass / sqrDst;
            rb.AddForce (acceleration);

            // Find body with strongest gravitational pull 
            if (maxAcceleration < acceleration.magnitude) {
                maxAcceleration=acceleration.magnitude;
                maxAccelerationBody=body;
            }
            //transform.position=rb.position;
        }
        referenceBody = maxAccelerationBody;
        // Rotate to align with gravity up
        playerBody.rotation = Quaternion.FromToRotation (playerBody.transform.up, playerBody.transform.position - referenceBody.transform.position) * playerBody.transform.rotation;

        // Move
        rb.MovePosition (playerBody.position + smoothVelocity * Time.fixedDeltaTime);
        done=true;
    }
}
