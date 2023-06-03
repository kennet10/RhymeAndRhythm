using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsDisplay : MonoBehaviour
{
    [Header("Song Info")]
    [SerializeField] private TextMeshProUGUI songTitleTMP;
    [SerializeField] private Image songJacketArt;
    [SerializeField] private TextMeshProUGUI songArtistTMP;
    
    [Header("Score Results")]
    [SerializeField] private TextMeshProUGUI rankTMP;
    [SerializeField] private TextMeshProUGUI scoreTMP;
    [SerializeField] private TextMeshProUGUI comboTMP;

    SceneTransitionManager sceneTransition;

    // Start is called before the first frame update
    void Start()
    {
        SongClearResults();
    }

    public void SongClearResults()
    {
        scoreTMP.text = "Score: " + PlayerPrefs.GetFloat("Score").ToString();
        comboTMP.text = "Combo: " + PlayerPrefs.GetFloat("Combo").ToString();
        sceneTransition.SetSongName(songTitleTMP.text);
        sceneTransition.SetJacketArt(songJacketArt.sprite);
        sceneTransition.SetArtistName(songArtistTMP.text);
    }
}
