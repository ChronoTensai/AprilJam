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
        NoteMeshFilter.mesh = GeometryCreator.CutGeometryAtPlayerPosition(NoteMeshFilter.mesh, this.transform.InverseTransformPoint(playerTransform.position));
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
        CancelInvoke("PushToPool");
        if (_returnCallback != null)
            _returnCallback(this);
        gameObject.SetActive(false);
        isAvaialbleforPop = true;
        _missed = false;
        _playing = false;
    }

    #endregion

    public void StartPlaying()
    {
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
            //CutGeometry
            //CutNote(playerTransform);
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
