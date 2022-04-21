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
    private int counter = 0;
    private void Start() {
        celestialBodies = FindObjectsOfType<CelestialBody>();
        CreatePhysicsScene();
        clones = FindObjectsOfType<Clone>();
        foreach(Clone go in clones)
        {
            go.GetComponent<MeshRenderer>().enabled=false;
            if(go.name==ReferenceName+"(Clone)") 
            {
                ReferenceBody = go.gameObject;
                origin = go.transform.position;
                hasReferenceBody=true;
            }
        }
    }
    private void Update() {
    }
    private void FixedUpdate() {
        counter++;
        Sim();
        if(counter>_maxIterations)
        {
            ResetPozition();
            if(hasReferenceBody)
            {
                origin=ReferenceBody.transform.position;
            }
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
        }
    }
    private void Sim()
    {
        for(int i=1;i<=_maxIterationsPerUpdate;i++)
        {
            if(hasReferenceBody)
        {
            distance = ReferenceBody.transform.position-origin;
            foreach(Clone go in clones)
            {
                go.transform.position -= distance;
            }
        }
            _physicsScene.Simulate(Time.fixedDeltaTime*step);
            foreach(Clone clone in clones) {
                clone.gameObject.GetComponent<CelestialBody>().ApplyGravity();
                //clone.gameObject.GetComponentInChildren<TrailRenderer>().AddPosition(clone.gameObject.transform.position);
            }
        }
    }
    private void CreatePhysicsScene() {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();
        foreach(CelestialBody body in celestialBodies){
            var genobj = Instantiate(body.gameObject, body.transform.position, body.transform.rotation);
            genobj.AddComponent<Clone>();
            genobj.GetComponent<Clone>().parent= body.gameObject;
            genobj.GetComponent<Rigidbody>().detectCollisions=false;
            genobj.GetComponent<CelestialBody>().isClone=true;
            SceneManager.MoveGameObjectToScene(genobj, _simulationScene);
        }
    }
}