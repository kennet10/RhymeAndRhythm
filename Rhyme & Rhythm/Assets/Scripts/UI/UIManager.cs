using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("The Playable Director.")]
    [SerializeField] protected PlayableDirector m_PlayableDirector;

    [SerializeField] protected GameObject m_ScoreMenu;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ScoreText;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ComboText;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ScoreDisplay;
    [SerializeField] protected TMPro.TextMeshProUGUI m_ComboDisplay;

    [SerializeField] protected GameObject m_PauseMenu;

    private bool m_GameIsPaused;

    // Start is called before the first frame update
    void Awake()
    {
        m_PlayableDirector.timeUpdateMode = DirectorUpdateMode.DSPClock;
        m_PlayableDirector.stopped += HandleSongEnded;

        m_GameIsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (!m_GameIsPaused)
        {
            m_PauseMenu.SetActive(true);
            m_PlayableDirector.Pause();
            m_GameIsPaused = true;
        }
        else
        {
            m_PauseMenu.SetActive(false);
            m_PlayableDirector.Resume();
            m_GameIsPaused = false;
        }
    }

    protected void HandleSongEnded(PlayableDirector playableDirector)
    {
        EndSong();
    }

    public void EndSong()
    {
        m_PlayableDirector.Stop();

        if(m_ScoreMenu != null)
        {
            m_ScoreMenu.SetActive(true);
        }

        m_ScoreText.text = m_ScoreDisplay.text;
        m_ComboText.text = m_ComboDisplay.text;
    }
}
