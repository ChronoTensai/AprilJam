using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Variables
    public float _movementSpeed;
    private float _playerWidth;
    private TypeOfNotes _playerColor;
    private float _leftBorder = 256;
    private float _rightBorder = 768;
    private MeshFilter _meshFilter;
    private Renderer _renderer;
    #endregion

    private TypeOfNotes PlayerColor
    {
        set
        {
            if (_playerColor != value)
            {
                _playerColor = value;
                UpdatePlayerColor();
                if (currentNote != null)
                {
                    MissNote();
                }
            }
        }
    }

    private void Start()
    {
        _playerWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        _leftBorder = _leftBorder + _playerWidth;
        _rightBorder = _rightBorder - _playerWidth;
        Debug.Log(_playerWidth);
        Debug.Log(_leftBorder);
        Debug.Log(_rightBorder);
    }
    
    void Update ()
    {
        Movement();
        ChangeColor();
	}

    private Note currentNote;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9 && currentNote == null) //consulto si el layer es = al layer "Note"
        {
            currentNote = c.gameObject.GetComponent<Note>();

            if (_playerColor == currentNote.NoteType && currentNote.Missed == false)//verifico si tanto player como nota son del mismo color
            {
                StartPlayNote();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentNote != null)
        { 
            if (currentNote.transform.position.y < -currentNote.Duration - 3.5f)
            {
                if (currentNote.NoteType == _playerColor)
                {
                    currentNote.PushToPool();//llamo la funcion PushToPool de la clase "Note"
                    currentNote = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && currentNote != null && currentNote.Id == other.GetComponent<Note>().Id)
        {
            if (currentNote.transform.position.y < -currentNote.Duration - 4f)
            {
                //Debug.Break();
                currentNote.PushToPool();//llamo la funcion PushToPool de la clase "Note"
                currentNote = null;
            }
            else
            {
                currentNote.CutCurrentNote();
                MissNote();
            }
        }
    }

    private void StartPlayNote()
    {
        //Esto no deberia estar aca pero boeh
        GameManager.MelodyAudioSource.mute = false;
    }

    private void MissNote()
    {
        //Esto no deberia estar aca pero boeh x2
        GameManager.MelodyAudioSource.mute = true;
        GameManager.FailFeedbackSource.Play();
        currentNote.Missed = true;
        currentNote = null;
    }

    #region Functions
    //   gameObject.GetComponent<SpriteRenderer>().bounds.size.x - gameObject.GetComponent<SpriteRenderer>().bounds.size.x/2 > 256
    private void Movement()
    {
        Vector3 _screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        if(Input.GetKey(KeyCode.LeftArrow) &&  _screenPosition.x > _leftBorder)
            transform.position -= transform.right * _movementSpeed * Time.deltaTime;
        else if(Input.GetKey(KeyCode.RightArrow) && _screenPosition.x < _rightBorder)
            transform.position += transform.right * _movementSpeed * Time.deltaTime;
    }

    private void ChangeColor()
    {
        if (Input.GetKey(KeyCode.A))
        {
            PlayerColor = TypeOfNotes.Red;
            
        }
        else if (Input.GetKey(KeyCode.S))
        {
            PlayerColor = TypeOfNotes.Green;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            PlayerColor = TypeOfNotes.Blue;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            PlayerColor = TypeOfNotes.Yellow;
        }
    }

    public void UpdatePlayerColor()
    {
        Color newColor = Color.white;

        switch (_playerColor)
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
    #endregion
}