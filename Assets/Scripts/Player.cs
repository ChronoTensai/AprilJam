using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    #region Variables
    public float _movementSpeed;
    private float _playerWidth;
    private TypeOfNotes _playerColor;
    private TypeOfNotes _noteColor;
    private float _leftBorder = 256;
    private float _rightBorder = 768;
    private MeshFilter _meshFilter;
    private Renderer _renderer;
    #endregion

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
        UpdateNote();
	}

    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.layer == 9) //consulto si el layer es = al layer "Note"
        {
            var e = c.gameObject.GetComponent<Note>();

            if (_playerColor == e.NoteType)//verifico si tanto player como nota son del mismo color
                e.PushToPool();//llamo la funcion PushToPool de la clase "Note"               
        }
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
            _playerColor = TypeOfNotes.Red;
        else if (Input.GetKey(KeyCode.S))
            _playerColor = TypeOfNotes.Green;
        else if (Input.GetKey(KeyCode.D))
            _playerColor = TypeOfNotes.Blue;
        else if (Input.GetKey(KeyCode.F))
            _playerColor = TypeOfNotes.Yellow;
    }

    public void UpdateNote()
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