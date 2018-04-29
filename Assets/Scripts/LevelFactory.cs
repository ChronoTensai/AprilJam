using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[System.Serializable]
public class NoteInfo
{
    public float StartAt;
    public TypeOfNotes NoteType;
    public float XPosition;
    public float Duration;
    public GeometryDeformation[] Deformations;
    public float Velocity;
    [HideInInspector]
    public float TimeDelta = 0;
}

public class LevelFactory : MonoBehaviour {

    private const int POOL_SIZE = 15;

    public float StartSongAt = 0;
    public float Tempo = 5;
    public float TimeToReturnPool = 4;

    public List<NoteInfo> NotesInfo;

    private Note[] _notePool;

    private AudioSource MelodySource;
    private AudioSource BaseSource;

    public bool ShowSongSeconds = true;

    void Awake ()
    {
        GetAudioSource();
        ApplyFastFoward();
        CreateNotePool();
        GetStartNotesPosition();
    }

    private void Start()
    {
        StartLevel();
    }

    private void OnGUI()
    {
        if (ShowSongSeconds && MelodySource != null)
        {
            GUI.Label(new Rect(20, 100, 500, 30), "Song Seconds: " + MelodySource.time);
        }   
    }

    private void GetAudioSource()
    {
        GameObject audioSource = GameObject.FindGameObjectWithTag("AudioSource").gameObject;

        MelodySource = audioSource.transform.Find("Melody").gameObject.GetComponent<AudioSource>();
        BaseSource = audioSource.transform.Find("Base").gameObject.GetComponent<AudioSource>();
    }

    private void ApplyFastFoward()
    {
        if (StartSongAt != 0)
        {
            for (int i = 0; i < NotesInfo.Count; i++)
            {
                if (NotesInfo[i].StartAt < StartSongAt)
                {
                    NotesInfo.RemoveAt(i);
                    i--;
                }
                else
                {
                    NotesInfo[i].StartAt -= StartSongAt;
                }
            }

            MelodySource.time = StartSongAt;
            BaseSource.time = StartSongAt;
        }
    }

    float StartYPosition = 0;

    private void GetStartNotesPosition()
    {
        StartYPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 1024)).y -3f;
    }

    private Vector3 poolPosition = new Vector3(1000, 1000, 1000);

    private void CreateNotePool()
    {
        GameObject notePrefab = Resources.Load<GameObject>("Note");

        _notePool = new Note[POOL_SIZE];

        for (int i = 0; i < POOL_SIZE && i < NotesInfo.Count ; i++)
        {

            GameObject go = GameObject.Instantiate<GameObject>(notePrefab, poolPosition, Quaternion.identity);
            _notePool[i] = go.GetComponent<Note>();
            _notePool[i].Velocity = Tempo;

            _notePool[i].UpdateNote(NotesInfo[i]);
            _notePool[i].SetReturnPoolCallback(ReturnToPool);
        }
    }

    private void ReturnToPool(Note returnPool)
    {
        //DoSomeShit
    }


    int noteIndex = 0;
    int poolIndex = 0;
    float timePassed = 0;

    private void StartLevel()
    {
        MelodySource.Play();
        BaseSource.Play();
        timePassed += NotesInfo[noteIndex].StartAt;
        Invoke("SendNote", NotesInfo[0].StartAt);
    }

    private void SendNote()
    {
        if (poolIndex == POOL_SIZE)
        {
            poolIndex = 0;
        }

        if (noteIndex >= POOL_SIZE)
        {
            _notePool[poolIndex].UpdateNote(NotesInfo[noteIndex]);
        }

        if (NotesInfo[noteIndex].Velocity == 0)
        {
            _notePool[poolIndex].Velocity = Tempo;
        }
        else
        {
            _notePool[poolIndex].Velocity = NotesInfo[noteIndex].Velocity;
        }

        _notePool[poolIndex].gameObject.transform.position = new Vector3(0, StartYPosition);
        _notePool[poolIndex].PopFromPool(TimeToReturnPool);
        timePassed += NotesInfo[noteIndex].TimeDelta;

        noteIndex++;
        poolIndex++;
        
        if (noteIndex < NotesInfo.Count)
        {
            NotesInfo[noteIndex].TimeDelta = NotesInfo[noteIndex].StartAt - timePassed;
            Invoke("SendNote", NotesInfo[noteIndex].TimeDelta);
        }
    }


}

