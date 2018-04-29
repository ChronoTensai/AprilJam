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
    public bool Missed = false;
    public int Id;


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

    public void CutCurrentNote()
    {
        //NoteMeshFilter.mesh = GeometryCreator.CutGeometry(NoteMeshFilter.mesh);
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
        Missed = false;
    }

    #endregion


    // bool breakOne = false;
     void Update()
     {
         this.transform.Translate(Vector3.up * Velocity * -1 * Time.deltaTime);

         /*if (breakOne == false && this.transform.position.y <= -4f - Duration)
         {
             breakOne = true;
             Debug.Break();
         }*/
}

}
