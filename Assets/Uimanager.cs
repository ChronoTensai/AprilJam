using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Uimanager : MonoBehaviour {

    private void Awake()
    {
        GameManager.UIManager = this;
        SetPlayedNote(0);
        SetMissedNote(0);
    }

    public Text _missedText;
    public Text _playedText;
    private const string PLAYED_TEXT = "Played: {0}";
    private const string MISSED_TEXT = "Miss: {0}";


    public void SetPlayedNote(int amount)
    {
        _playedText.text = string.Format(PLAYED_TEXT, amount);
    }

    public void SetMissedNote(int amount)
    {
        _missedText.text = string.Format(MISSED_TEXT, amount);
    }

}
