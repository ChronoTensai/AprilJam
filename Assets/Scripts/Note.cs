using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : MonoBehaviour {

    private TypeOfNotes _noteType;
    private MeshFilter _meshFilter;
    private Renderer _renderer;
    private float   _xPosition;
    private float _velocity = 5;

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

        _velocity = noteInfo.Velocity;
    }

    #region Pool
    private bool isAvaialbleforPop = false;
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

    public void Pop()
    {
        gameObject.SetActive(true);
        isAvaialbleforPop = false;
    }

    private void Push()
    {
        _returnCallback(this);
        gameObject.SetActive(false);
        isAvaialbleforPop = true;
    }

    #endregion

}
