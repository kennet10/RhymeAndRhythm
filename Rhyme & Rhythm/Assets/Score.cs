using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Tooltip("The Playable Director.")]
    [SerializeField] protected PlayableDirector m_PlayableDirector;
    [SerializeField] protected GameObject m_ScorePanel;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ScoreText;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ComboText;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ScoreDisplay;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ComboDisplay;

    // Start is called before the first frame update
    void Awake()
    {
        m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.DSPClock;
        m_PlayableDirector.stopped += HandleSongEnded;
    }

    protected void HandleSongEnded(PlayableDirector playableDirector)
    {
        EndSong();
    }

    public void EndSong()
    {
        m_PlayableDirector.Stop();

        if(m_ScorePanel != null)
        {
            m_ScorePanel.SetActive(true);
        }

        m_ScoreText.text = m_ScoreDisplay.text;
        m_ComboText.text = m_ComboDisplay.text;
    }
}
