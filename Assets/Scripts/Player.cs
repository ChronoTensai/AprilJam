using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Variables
    public float _movementSpeed;
    private TypeOfNotes _playerColor = TypeOfNotes.None;
    private float _leftBorder;
    private float _rightBorder;
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

    private void Awake()
    {
        GameManager.MISS_NOTE_Y = this.gameObject.transform.position.y - (gameObject.GetComponent<SpriteRenderer>().bounds.size.y /2);
        GameManager.PLAYERt = this.transform;
    }

    private void Start()
    {
        float _playerWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        _leftBorder = GameManager.LEFT_BORDER + _playerWidth;
        _rightBorder = GameManager.RIGHT_BORDER - _playerWidth;
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

            if (_playerColor == currentNote.NoteType && currentNote._missed == false)//verifico si tanto player como nota son del mismo color y si el jugador no perdio ya esa la nota
            {
                StartPlayNote();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentNote != null)
        {
            //Agarramos suficiente tiempo la nota como para considerarla tomada
            if (GameManager.NotePlayed(currentNote.transform.position.y, currentNote.Duration))
            {
                if (currentNote.NoteType == _playerColor)
                {
                    currentNote.StopPlaying();
                    currentNote = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && currentNote != null && currentNote.Id == other.GetComponent<Note>().Id)
        {
            if (GameManager.NoteIsBeyondPlayer(currentNote.transform.position.y, currentNote.Duration))
            {
                //Terminanos la nota! Deberia de entrar en el otro chequeo antes
                currentNote.StopPlaying();
                currentNote = null;
            }
            else
            {
                //Nos fuimos antes de que termine la nota
                MissNote();
            }
        }
    }

    private void StartPlayNote()
    {
        currentNote.StartPlaying();
        GameManager.MelodyAudioSource.mute = false;
    }

    private void MissNote()
    {
        currentNote.MissedNote(this.transform);
        currentNote = null;
        GameManager.MissNote();
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