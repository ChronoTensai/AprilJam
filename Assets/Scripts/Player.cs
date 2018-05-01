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
    private Vector3 _maskPosition;
    #endregion

    private TypeOfNotes PlayerColor
    {
        set
        {
            if (_playerColor != value)
            {
                _playerColor = value;
                UpdatePlayerColor();
                for (int i = 0; i < currentNotes.Count; i++)
                {
                    if (currentNotes[i].NoteType != _playerColor)
                    {
                        MissNote(currentNotes[i]);
                        currentNotes.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        GameManager.MISS_NOTE_Y = this.gameObject.transform.position.y - (gameObject.GetComponent<SpriteRenderer>().bounds.size.y /2);
        Debug.Log("heigh: " + gameObject.GetComponent<SpriteRenderer>().bounds.size.y + "  "+ this.transform.position );
        _maskPosition = new Vector3(this.transform.position.x, -4.41f, 0.49f); //Jam code yay!
        currentNotes = new List<Note>();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.ResetLevel();
        }
	}

    private List<Note> currentNotes;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9 ) //consulto si el layer es = al layer "Note"
        {
            var e = c.gameObject.GetComponent<Note>();

            if (_playerColor == e.NoteType && e._missed == false)//verifico si tanto player como nota son del mismo color y si el jugador no perdio ya esa la nota
            {
                currentNotes.Add(e);
                StartPlayNote(e);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentNotes.Count != 0)
        {
            //Agarramos suficiente tiempo la nota como para considerarla tomada
            for (int i = 0; i < currentNotes.Count; i++)
            {
                if (GameManager.NotePlayed(currentNotes[i].transform.position.y, currentNotes[i].Duration))
                {
                    if (currentNotes[i].NoteType == _playerColor) 
                    {
                        if (currentNotes[i].Id == other.gameObject.GetComponent<Note>().Id)
                        { 
                            currentNotes[i].StopPlaying();
                            currentNotes.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9 && currentNotes.Count != 0)
        {
            for (int i = 0; i < currentNotes.Count; i++)
            {
                if (GameManager.NotePlayed(currentNotes[i].transform.position.y, currentNotes[i].Duration))
                {
                    if (currentNotes[i].NoteType == _playerColor)
                    {
                        if (currentNotes[i].Id == other.gameObject.GetComponent<Note>().Id)
                        {
                            currentNotes[i].StopPlaying();
                            currentNotes.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    if (currentNotes[i].Id == other.gameObject.GetComponent<Note>().Id)
                    {
                        MissNote(currentNotes[i]);
                        currentNotes.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private void StartPlayNote(Note note)
    {
        note.StartPlaying(_maskPosition);
        GameManager.MelodyAudioSource.mute = false;
    }

    private void MissNote(Note note = null)
    {
        note.MissedNote(_maskPosition);
        note = null;
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