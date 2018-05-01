﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static AudioSource MelodyAudioSource;
    public static AudioSource FailFeedbackSource;
    public static Uimanager UIManager;

    public const float LEFT_BORDER = 256;
    public const float RIGHT_BORDER = 768;
    public const float GET_NOTE_THRESHOLD = 0.4F;
    public const float MISSED_NOTE_TRESHOLD = 0.4F;
    public static float MISS_NOTE_Y = -1000;

    private static int MissedNoteAmount = 0;
    private static int PlayedNoteAmount = 0;

    public static bool IsLevelEditor = false;

    public static void MissNote()
    {
        if (IsLevelEditor == false)
        { 
            GameManager.MelodyAudioSource.mute = true;
            GameManager.FailFeedbackSource.Play();
        }
    }

    public static bool NoteIsBeyondPlayer(float noteY, float noteDuration)
    {
        return noteY + noteDuration < MISS_NOTE_Y;
    }

    public static bool NotePlayed(float noteY, float noteDuration)
    {
        if(noteY + noteDuration < MISS_NOTE_Y + GET_NOTE_THRESHOLD)
        {
            PlayedNoteAmount++;
            UIManager.SetPlayedNote(PlayedNoteAmount);
            return true;
        }
        return false;
    }

    public static bool NoteMissed(float noteY)
    {
        if (noteY < MISS_NOTE_Y + MISSED_NOTE_TRESHOLD)
        {
            MissedNoteAmount++;
            UIManager.SetMissedNote(MissedNoteAmount);
            return true;
        }
        return false;
    }

    public static void ResetLevel()
    {
        GameManager.MelodyAudioSource.mute = false;
        GameManager.FailFeedbackSource.mute = false;
        PlayedNoteAmount = MissedNoteAmount = 0;
        UIManager.SetPlayedNote(PlayedNoteAmount);
        UIManager.SetMissedNote(MissedNoteAmount);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Tempo
//    public static Transform PLAYERt;
}
