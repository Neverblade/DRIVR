using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour {

    public GameObject road;
    public GameObject car;
    public float roadLength;
    public int numRoads;
    public float despawnDistance;

    private Vector3 nextPos;
    private Queue<GameObject> roads;

	// Use this for initialization
	void Start () {
        road.SetActive(true);
        roads = new Queue<GameObject>();
        nextPos = car.transform.position - despawnDistance * Vector3.forward;
        for (int i = 0; i < numRoads; i++) {
            roads.Enqueue(Instantiate(road, nextPos, road.transform.rotation, transform));
            nextPos += roadLength * Vector3.forward;
        }
        road.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(roads.Peek().transform.position, car.transform.position) > despawnDistance) {
            GameObject r = roads.Dequeue();
            r.transform.position = nextPos;
            nextPos += roadLength * Vector3.forward;
            roads.Enqueue(r);
        }
	}
}
