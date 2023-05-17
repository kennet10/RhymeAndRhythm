using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_songName;
    [SerializeField] private TextMeshProUGUI m_artistName;
    [SerializeField] private TextMeshProUGUI m_designerName;
    [SerializeField] private Image m_JacketArt;

    public void SetSongName(string songName)
    {
        m_songName.text = songName;
    }

    public void SetArtistName(string artistName)
    {
        m_artistName.text = artistName;
    }

    public void SetDesignerName(string designerName)
    {
        m_designerName.text = designerName;
    }

    public void SetJacketArt(Sprite jacketArt)
    {
        m_JacketArt.sprite = jacketArt;
    }
}
