using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public CameraController mainCamera;
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
    public bool hasPlayer = false;
    public Vector3 propPower;
    public Vector3 propPowerNegative;
    public Camera cam;
    public Vector3 rotationChange;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        pitch = 0f;
        roll = 0f;
        yaw = 0f;
        if(hasPlayer)
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
        }
        targetProp = new Vector3(
            System.Convert.ToInt32(propForward)-System.Convert.ToInt32(propBackward),
            System.Convert.ToInt32(propUp)-System.Convert.ToInt32(propDown),
            System.Convert.ToInt32(propLeft)-System.Convert.ToInt32(propRight)
        );
        if(Input.GetKey(KeyCode.Q))
        {

        }
        MouseControls();
        UpdateCamera();
    }
    void MouseControls()
    {
        var localTarget = transform.InverseTransformDirection(cam.transform.forward).normalized * 5f;
        var targetRollAngle = Mathf.Lerp(0f, 30f, Mathf.Abs(localTarget.x));
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
        if(hasPlayer)
        {
            transform.RotateAround(transform.position, transform.up, yaw * Time.fixedDeltaTime * yawSpeed);     //Yaw
            transform.RotateAround(transform.position, transform.forward, roll * Time.fixedDeltaTime * rollSpeed);     //Roll
            transform.RotateAround(transform.position, transform.right, pitch * Time.fixedDeltaTime * pitchSpeed);
        }
        CelestialBody[] allBodies = FindObjectsOfType<CelestialBody>();
        foreach(var body in allBodies)
        {
            if(body!=this&&body.gameObject.scene==this.gameObject.scene)
            {
                float sqrDst = (body.GetComponent<Rigidbody>().position - rb.position).sqrMagnitude;
                Vector3 forceDir = -(rb.position - body.GetComponent<Rigidbody>().position).normalized;
                Vector3 acceleration = forceDir * gravityConstant * body.mass * rb.mass / sqrDst;
                rb.AddForce (acceleration);
            }
        }
        propControl.x = Mathf.SmoothStep(propControl.x,targetProp.x,10*Time.deltaTime);
        propControl.y = Mathf.SmoothStep(propControl.y,targetProp.y,10*Time.deltaTime);
        propControl.z = Mathf.SmoothStep(propControl.z,targetProp.z,10*Time.deltaTime);
        if(propControl.x>0)
        {
            rb.AddForce(new Vector3(propControl.x*propPower.x,0,0),ForceMode.Acceleration);
        }else{
            rb.AddForce(new Vector3(propControl.x*propPowerNegative.x,0,0),ForceMode.Acceleration);
        }
        if(propControl.y>0)
        {
            rb.AddForce(new Vector3(0,propControl.y*propPower.y,0),ForceMode.Acceleration);
        }else{
            rb.AddForce(new Vector3(0,propControl.y*propPowerNegative.y,0),ForceMode.Acceleration);
        }
        if(propControl.z>0)
        {
            rb.AddForce(new Vector3(0,0,propControl.z*propPower.z),ForceMode.Acceleration);
        }else{
            rb.AddForce(new Vector3(0,0,propControl.z*propPowerNegative.z),ForceMode.Acceleration);
        }
    }
}
