using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Projection : MonoBehaviour {
    public string ReferenceName;
    public GameObject ReferenceBody;
    public Vector3 distance;
    public bool hasReferenceBody=false;
    public Vector3 origin;
    public int _maxIterations = 1000;
    public int _maxIterationsPerUpdate = 10;
    public int step = 1;
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;
    CelestialBody[] celestialBodies;
    CelestialBody[] celestialBodiesSim;
    Clone[] clones;
    public GameObject player;
    public GameObject SimBody;
    private int counter = 0;
    private void Start() {
        celestialBodies = FindObjectsOfType<CelestialBody>();
        CreatePhysicsScene();
        clones = FindObjectsOfType<Clone>();
        foreach(Clone clone in clones) {
            clone.name=clone.parent.name+"(Clone)";
        }
    }
    private void Update() {
    }
    private void FixedUpdate() {
        if(counter==0)
        {
            ResetPozition();
            if(hasReferenceBody)
            {
                origin = ReferenceBody.transform.position;
            }
        }else{
            Sim();
        }
        counter++;
        if (counter>_maxIterations)
        {
            counter=0;
        }
    }
    private void ResetPozition()
    {
        foreach(Clone clone in clones)
        {
            clone.gameObject.transform.position=clone.parent.transform.position;
            clone.gameObject.transform.rotation=clone.parent.transform.rotation;
            clone.gameObject.GetComponent<Rigidbody>().position=clone.parent.GetComponent<Rigidbody>().position;
            clone.gameObject.GetComponent<Rigidbody>().velocity=clone.parent.GetComponent<Rigidbody>().velocity;
            clone.gameObject.GetComponentInChildren<TrailRenderer>().Clear();
        }
    }
    private void Sim()
    {
        for(int i=1;i<=_maxIterationsPerUpdate;i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime*step);
            if(hasReferenceBody)
        {
                origin = ReferenceBody.GetComponent<Clone>().parent.transform.position;
                distance = ReferenceBody.transform.position-origin;
            foreach(Clone go in clones)
            {
                go.transform.position -= distance;
            }
        }
            foreach(Clone clone in clones) {
                clone.gameObject.GetComponent<CelestialBody>().ApplyGravity();
                clone.gameObject.GetComponentInChildren<TrailRenderer>().AddPosition(clone.gameObject.transform.position);
            }
        }
    }
    private void CreatePhysicsScene() {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();
        foreach(CelestialBody body in celestialBodies){
            var genobj = Instantiate(SimBody, body.transform.position, body.transform.rotation);
            genobj.AddComponent<Clone>();
            genobj.GetComponent<Clone>().parent= body.gameObject;
            genobj.GetComponent<Rigidbody>().detectCollisions=false;
            genobj.GetComponent<CelestialBody>().isClone=true;
            genobj.GetComponent<CelestialBody>().radius = body.radius;
            genobj.GetComponent<CelestialBody>().mass = body.mass;
            genobj.GetComponent<CelestialBody>().surfaceGravity = body.surfaceGravity;
            genobj.GetComponentInChildren<TrailRenderer>().widthMultiplier = body.radius/10;
            SceneManager.MoveGameObjectToScene(genobj, _simulationScene);
            if (ReferenceBody == body.gameObject)
            {
                ReferenceBody = genobj;
                origin = genobj.transform.position;
                hasReferenceBody = true;
            }
        }
    }
}