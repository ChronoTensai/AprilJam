using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolNotesCreator : MonoBehaviour {

    public GeometryDeformation[] Deformations;
    public float Duration;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MeshFilter meshFilter = this.GetComponent<MeshFilter>();
            meshFilter.mesh = GeometryCreator.CreateNote(Duration, Deformations);
        }
	}
}
