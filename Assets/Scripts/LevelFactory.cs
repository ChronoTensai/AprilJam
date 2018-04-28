using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NoteInfo
{
    public float StartAt;
    public TypeOfNotes NoteType;
    public float Duration;
    public GeometryDeformation[] Deformations;
}

public class LevelFactory : MonoBehaviour {

    private const int POOL_SIZE = 15;

    public float StartSongAt;
    public NoteInfo[] NotesInfo;

    private Note[] _notePool;
    
    void Start ()
    {
        _notePool = new Note[POOL_SIZE];
	}
	
	
}

