using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour {

    private TypeOfNotes _noteType;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Renderer _renderer;
    public float Duration;
    private float   _xPosition;
    public float Velocity = 5;
    public bool _missed = false;
    public int Id;
    private bool _playing = false;


    private GameObject _mask;


    protected int[] m_queues = new int[] { 3000 }; //No tengo idea por que 3000

    protected void Awake()
    {
        _mask = this.transform.Find("Mask").gameObject;
        _mask.SetActive(false);
        Material[] materials = NoteRenderer.materials;
        for (int i = 0; i < materials.Length && i < m_queues.Length; ++i)
        {
            materials[i].renderQueue = m_queues[i];
        }
    }

    public TypeOfNotes NoteType
    {
        get
        {
            return _noteType;
        }
    }

    private MeshFilter NoteMeshFilter
    {
        get
        { 
            if (_meshFilter == null)
                _meshFilter = this.GetComponent<MeshFilter>();
            return _meshFilter;
        }
    }

    private MeshCollider NoteMeshCollider
    {
        get
        {
            if (_meshCollider == null)
                _meshCollider = this.GetComponent<MeshCollider>();
            return _meshCollider;
        }
    }

    private Renderer NoteRenderer
    {
        get
        {
            if (_renderer == null)
                _renderer = this.GetComponent<Renderer>();
            return _renderer;
        }
    }

    public void UpdateNote(NoteInfo noteInfo)
    {
        Duration = noteInfo.Duration == 0 ? 0.25f : noteInfo.Duration;

        NoteMeshFilter.mesh = GeometryCreator.CreateNote(Duration, noteInfo.Deformations);
        NoteMeshCollider.sharedMesh = null;
        NoteMeshCollider.sharedMesh = NoteMeshFilter.mesh;

        _noteType = noteInfo.NoteType;

        Color newColor = Color.white;

        switch (_noteType)
        {
            case TypeOfNotes.Blue:
                newColor = Color.blue;
                break;
            case TypeOfNotes.Green:
                newColor = Color.green;
                break;
            case TypeOfNotes.Red:
                newColor = Color.red;
                break;
            case TypeOfNotes.Yellow:
                newColor = Color.yellow;
                break;

        }

        NoteRenderer.material.color = newColor;
        _xPosition = noteInfo.XPosition;
    }

    public GameObject test;

    private void CutNote(Transform playerTransform)
    {
        Mesh newMesh = GeometryCreator.CutGeometryAtPosition(NoteMeshFilter.mesh, this.transform.InverseTransformPoint(playerTransform.position));
        NoteMeshFilter.mesh = newMesh;

        Debug.Break();
    }


    #region Pool
    private bool isAvaialbleforPop = true;
    public bool IsAvaialbleforPop
    {
        get
        {
            return isAvaialbleforPop;
        }
    }

    private Action<Note> _returnCallback;

    public void SetReturnPoolCallback(Action<Note> returnCallback)
    {
        _returnCallback = returnCallback;
    }

    public void PopFromPool(float timeReturn)
    {
        Debug.Assert(IsAvaialbleforPop == true, "The pool size is too small");
        
        transform.position = new Vector3(_xPosition, transform.position.y);
        gameObject.SetActive(true);
        isAvaialbleforPop = false;
        Invoke("PushToPool", timeReturn); //Durisimo
    }

    private void PushToPool()
    {
        _mask.transform.parent = this.transform;
        CancelInvoke("PushToPool");
        if (_returnCallback != null)
            _returnCallback(this);
        gameObject.SetActive(false);
        isAvaialbleforPop = true;
        _missed = false;
        _playing = false;
    }

    #endregion

    public void StartPlaying(Vector3 maskPosition)
    {
        _mask.transform.parent = null;
        _mask.transform.position = maskPosition;
        _mask.SetActive(true);

        SetEnabledPlayingFeedback(true);
        this._playing = true;
    }

    public void StopPlaying()
    {
        SetEnabledPlayingFeedback(false);
        this.PushToPool();
    }

    public void MissedNote(Transform playerTransform = null)
    {
        this._missed = true;
        SetEnabledPlayingFeedback(false);

        if (_playing && playerTransform != null)
        {
            _mask.transform.parent = this.transform;
        }
    }

    public void SetEnabledPlayingFeedback(bool isEnabled)
    {
        if (isEnabled)
        {
            //Prender
        }
        else
        {
            //Apagar
        }
         
    }

    // bool breakOne = false;
     void Update()
     {
        this.transform.Translate(Vector3.up * Velocity * -1 * Time.deltaTime);

        if (_playing == false && _missed == false)
        {
            if (GameManager.NoteMissed(this.transform.position.y))
            {
                _missed = true;
                GameManager.MissNote();
            }
        }
    }

}
