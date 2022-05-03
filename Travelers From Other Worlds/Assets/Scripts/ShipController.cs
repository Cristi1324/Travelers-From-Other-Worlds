using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public GameObject player;
    public CameraController mainCamera;
    public Vector3 unboardPosition;
    private float gravityConstant = 6.67408f;
    bool propUp = false;
    bool propDown = false;
    bool propForward = false;
    bool propBackward = false;
    bool propLeft = false;
    bool propRight = false;
    public float pitch;
    public float roll;
    public float yaw;
    public float pitchSpeed = 100;
    public float rollSpeed = 100;
    public float yawSpeed = 100;
    public Vector3 propControl;
    public Vector3 rollControl;
    public Vector3 targetProp;
    public Vector3 targetRoll;
    public bool playerControled;
    public Vector3 propPower;
    public Vector3 propPowerNegative;
    public Camera cam;
    public Camera mapCamera;
    public Vector3 rotationChange;
    public Vector3 rotateDirection;
    public Vector3 relativeVelocity;
    ParticleSystem EngineParticlesForward;
    public bool isRotating = true;
    Rigidbody ReferenceBody;
    public bool enableMouseControls;
    public Vector3 crosshairPosition;
    public Vector3 progradePosition;
    public bool showCrosshairs;
    public bool showMotionVectors;
    public bool hasTarget;
    public Transform TargetBody;
    public Vector3 targetPosition;
    Projection projection;
    public float apparentSize;
    public Vector3 relativeTargetVelocity;
    public Vector3 targetDirection;
    public float targetDistance;
    public float relativeTargetVelocityMagnitude;
    ShipUI shipUI;
    void Start()
    {
        projection = FindObjectOfType<Projection>();
        cam.enabled=false;
        playerControled=false;
    }
    void Awake()
    {
        mapCamera = GameObject.Find("MapCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        enableMouseControls=true;
        EngineParticlesForward = GameObject.Find("ParticlesMainEngine").GetComponent<ParticleSystem>();
        shipUI = FindObjectOfType<ShipUI>();
    }

    // Update is called once per frame
    void Update()
    {
        pitch = 0f;
        roll = 0f;
        yaw = 0f;
        if(playerControled)
        {
        if(Input.GetKey(KeyCode.W))
        {
            propForward = true;
        }else propForward = false;
        if(Input.GetKey(KeyCode.A))
        {
            propLeft = true;
        }else propLeft = false;
        if(Input.GetKey(KeyCode.S))
        {
            propBackward = true;
        }else propBackward = false;
        if(Input.GetKey(KeyCode.D))
        {
            propRight = true;
        }else propRight = false;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            propDown = true;
        }else propDown = false;
        if(Input.GetKey(KeyCode.Space))
        {
            propUp = true;
        }else propUp = false;
        if(Input.GetKeyDown(KeyCode.F))
        {
            UnboardShip();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            shipUI.showUI = false;
            mapCamera.enabled = true;
            cam.enabled=false;
            mapCamera.GetComponent<MapCamera>().enabled=true;
            playerControled=false;
            FindObjectOfType<Projection>().enabled=true;
        }
        UpdateCamera();
        MouseControls();
        }
        targetProp = new Vector3(
            System.Convert.ToInt32(propForward)-System.Convert.ToInt32(propBackward),
            System.Convert.ToInt32(propUp)-System.Convert.ToInt32(propDown),
            System.Convert.ToInt32(propRight)-System.Convert.ToInt32(propLeft)
        );
    }
    private void OnCollisionStay(Collision other) {
        if(other.gameObject.layer==8)
            isRotating=false;
    }
    private void OnCollisionExit(Collision other) {
        if(other.gameObject.layer==8)
            isRotating=true;
    }
    internal void BoardPlayer(GameObject player)
    {
        shipUI.showUI = true;
        this.player = player;
        playerControled=true;
        cam.enabled=true;
        player.transform.parent = transform;
        cam.GetComponent<CameraController>().enabled = true;
    }
    void UnboardShip()
    {
        shipUI.showUI = false;
        playerControled=false;
        cam.enabled=false;
        player.GetComponentInChildren<Camera>().enabled=true;
        player.GetComponentInChildren<MeshRenderer>().enabled=true;
        player.GetComponent<PlayerController>().enabled=true;
        player.GetComponentInChildren<Rigidbody>().detectCollisions=true;
        player.GetComponentInChildren<Rigidbody>().isKinematic=false;
        player.GetComponentInChildren<PlayerPointer>().enabled=true;
        cam.GetComponent<CameraController>().enabled = false;
        player.transform.parent=null;
        player.transform.position = transform.position + transform.forward*unboardPosition.x+transform.right*unboardPosition.z+transform.up*unboardPosition.y;
        player.GetComponent<Rigidbody>().velocity=rb.velocity;
    }
    void MouseControls()
    {
        var localTarget = transform.InverseTransformDirection(cam.transform.forward).normalized * 5f;
        var targetRollAngle = localTarget.z;
        if (localTarget.x > 0f) targetRollAngle *= -1f;

        var rollAngle = FindAngle(transform.localEulerAngles.z);
        var newAngle = targetRollAngle - rollAngle;

        pitch = -Mathf.Clamp(localTarget.y, -1f, 1f);
        roll = Mathf.Clamp(newAngle, -1f, 1f);
        yaw = Mathf.Clamp(localTarget.x, -1f, 1f);
    }
    float FindAngle(float v)
    {
        if (v > 180f) v -= 360f;
        return v;
    }
    void UpdateCamera()
    {
        mainCamera.updatePosition(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
    }

    private void FixedUpdate() {
        if(playerControled&&isRotating)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,cam.transform.rotation,Time.deltaTime);
            //rb.AddRelativeTorque((rb.transform.forward-cam.transform.forward),ForceMode.VelocityChange);
            //transform.RotateAround(transform.position, transform.up, yaw * Time.fixedDeltaTime * yawSpeed);     //Yaw
            //transform.RotateAround(transform.position, transform.forward, roll * Time.fixedDeltaTime * rollSpeed);     //Roll
            //transform.RotateAround(transform.position, transform.right, pitch * Time.fixedDeltaTime * pitchSpeed);
        }
        CelestialBody[] allBodies = FindObjectsOfType<CelestialBody>();
        float maxAcceleration=0;
        foreach(var body in allBodies)
        {
            if(body!=this&&body.gameObject.scene==this.gameObject.scene)
            {
                float sqrDst = (body.GetComponent<Rigidbody>().position - rb.position).sqrMagnitude;
                Vector3 forceDir = -(rb.position - body.GetComponent<Rigidbody>().position).normalized;
                Vector3 acceleration = forceDir * gravityConstant * body.mass * rb.mass / sqrDst;
                rb.AddForce (acceleration);
                if (maxAcceleration < acceleration.magnitude) {
                    maxAcceleration=acceleration.magnitude;
                    ReferenceBody=body.GetComponent<Rigidbody>();
                }
            }
        }
        relativeVelocity = ReferenceBody.velocity-rb.velocity;
        /*if(relativeVelocity.magnitude>1)
        {
            //cam.fieldOfView = 60 + Mathf.Min(relativeVelocity.magnitude/20,20f);
            //cam.GetComponent<CameraController>().distance = 15 + Mathf.Min(relativeVelocity.magnitude/40,10f);
        }*/
        if (hasTarget)
        {
            targetDirection = (transform.position - TargetBody.position).normalized;
            apparentSize = 2 * Mathf.Atan(TargetBody.transform.localScale.x / ((TargetBody.position - cam.transform.position).magnitude)) * (60 / cam.fieldOfView);
        }
        if (relativeVelocity.magnitude>5)
        {
            showMotionVectors=true;
            progradePosition = cam.WorldToScreenPoint(cam.transform.position - (relativeVelocity.normalized * 500f));
        }else{
            showMotionVectors=false;
        }
        if(targetProp.x!=0) propControl.x = Mathf.SmoothStep(propControl.x,targetProp.x,10*Time.deltaTime);
        else propControl.x=0;
        if(targetProp.y!=0) propControl.y = Mathf.SmoothStep(propControl.y,targetProp.y,10*Time.deltaTime);
        else propControl.y=0;
        if(targetProp.z!=0) propControl.z = Mathf.SmoothStep(propControl.z,targetProp.z,10*Time.deltaTime);
        else propControl.z=0;
        
        if(propControl.x>0)
        {
            rb.AddForce(transform.forward*propControl.x*propPower.x,ForceMode.Acceleration);
            var emission = EngineParticlesForward.emission;
            emission.rateOverTime = propControl.x*100f;
        }else{
            var emission = EngineParticlesForward.emission;
            emission.rateOverTime = 0;
            rb.AddForce(transform.forward*propControl.x*propPowerNegative.x,ForceMode.Acceleration);
        }
        if(propControl.y>0)
        {
            rb.AddForce(transform.up*propControl.y*propPower.y,ForceMode.Acceleration);
        }else{
            rb.AddForce(transform.up*propControl.y*propPowerNegative.y,ForceMode.Acceleration);
        }
        if(propControl.z>0)
        {
            rb.AddForce(transform.right*propControl.z*propPower.z,ForceMode.Acceleration);
        }else{
            rb.AddForce(transform.right*propControl.z*propPowerNegative.z,ForceMode.Acceleration);
        }
        crosshairPosition = cam.WorldToScreenPoint(transform.position + (transform.forward * 500f));
        if(hasTarget)
        {
            targetDistance = (TargetBody.position-transform.position).magnitude;
            relativeTargetVelocity = (TargetBody.GetComponent<Rigidbody>().velocity-rb.velocity);
            relativeTargetVelocityMagnitude = Vector3.Dot(relativeTargetVelocity,targetDirection);
        }
    }
    private void LateUpdate() {
        targetPosition = cam.WorldToScreenPoint(cam.transform.position - (targetDirection * 500f));
    }
}
