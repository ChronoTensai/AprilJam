﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour {

    private TypeOfNotes _noteType;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private Renderer _renderer;
    private float   _xPosition;
    private float _velocity = 5;

    public float Velocity
    {
        set
        {
            _velocity = value;
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
        NoteMeshFilter.mesh = GeometryCreator.CreateNote(noteInfo.Duration, noteInfo.Deformations);
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

    public void PushToPool()
    {
        CancelInvoke("PushToPool");
        if (_returnCallback != null)
            _returnCallback(this);
        gameObject.SetActive(false);
        isAvaialbleforPop = true;
    }

    #endregion


    void Update()
    {
        this.transform.Translate(Vector3.up * _velocity * -1 * Time.deltaTime);
    }

}
