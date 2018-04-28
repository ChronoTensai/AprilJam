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
    [HideInInspector]
    public float Velocity;
}

public class LevelFactory : MonoBehaviour {

    private const int POOL_SIZE = 15;

    public float StartSongAt = 0;
    public float TempoSong = 5;


    public List<NoteInfo> NotesInfo;

    private Note[] _notePool;

    private AudioSource MelodySource;
    private AudioSource BaseSource;

    void Awake ()
    {
        GetAudioSource();
        ApplyFastFoward();
        CreateNotePool();
    }

    private void Start()
    {
        StartLevel();
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
            foreach (var item in NotesInfo)
            {
                if (item.StartAt < StartSongAt)
                {
                    NotesInfo.Remove(item);
                }
                else
                {
                    item.StartAt -= StartSongAt;
                }
            }

            MelodySource.time = StartSongAt;
            BaseSource.time = StartSongAt;
        }
    }

    private Vector3 poolPosition = new Vector3(1000, 1000, 1000);

    private void CreateNotePool()
    {
        GameObject notePrefab = Resources.Load<GameObject>("Note");

        _notePool = new Note[POOL_SIZE];

        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(notePrefab, poolPosition, Quaternion.identity);
            _notePool[i] = go.GetComponent<Note>();
            _notePool[i].UpdateNote(NotesInfo[i]);
            _notePool[i].SetReturnPoolCallback(ReturnToPool);
        }
    }

    private void ReturnToPool(Note returnPool)
    {
        //DoSomeShit
    }

    private void StartLevel()
    {
        MelodySource.Play();
        BaseSource.Play();
    }

    private void SendNotes()
    {
        
        
    }


}

