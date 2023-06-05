/// ---------------------------------------------
/// Rhythm Timeline
/// Copyright (c) Dyplsoom. All Rights Reserved.
/// https://www.dypsloom.com
/// ---------------------------------------------

namespace Dypsloom.RhythmTimeline.Scoring
{
    using Dypsloom.RhythmTimeline.Core;
    using Dypsloom.RhythmTimeline.Core.Managers;
    using Dypsloom.RhythmTimeline.Core.Notes;
    using Dypsloom.RhythmTimeline.UI;
    using Dypsloom.Shared;
    using Dypsloom.Shared.Utility;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using UnityEngine.Playables;
    using UnityEngine.Timeline;
    using System.Linq;

    public class ScoreManager : MonoBehaviour
    {
        public event Action<RhythmTimelineAsset> OnNewHighScore;
        public event Action<Note, NoteAccuracy> OnNoteScore;
        public event Action OnBreakChain;
        public event Action<int> OnContinueChain;
        public event Action<float> OnScoreChange;

        [Tooltip("The Rhythm Director.")]
        [SerializeField] protected RhythmDirector m_RhythmDirector;
        [Tooltip("The score settings.")]
        [SerializeField] protected ScoreSettings m_ScoreSettings;
        [Tooltip("The score text.")]
        [SerializeField] protected TMPro.TextMeshProUGUI m_ScoreTmp;
        [Tooltip("The score multiplier text.")]
        [SerializeField] protected TMPro.TextMeshProUGUI m_ScoreMultiplierTmp;
        [Tooltip("The chain text.")]
        [SerializeField] protected TMPro.TextMeshProUGUI m_ChainTmp;
        [Tooltip("The rank slider.")]
        [SerializeField] protected RankSlider m_RankSlider;
        [Tooltip("(Optional) the spawn point for the accuracy pop ups.")]
        [SerializeField] protected Transform m_AccuracySpawnPoint;
    
        protected RhythmTimelineAsset m_CurrentSong;
        protected float m_CurrentScore;
        protected List<int> m_CurrentAccuracyIDHistogram;
        protected int m_CurrentMaxChain;
        protected int m_CurrentChain;
        protected float m_CurrentMaxPossibleScore;

        protected float m_ScoreMultiplier = 1;
        protected Coroutine m_ScoreMultiplierCoroutine;

        private static ScoreManager m_Instance;

        public static ScoreManager Instance => m_Instance;
    
        public ScoreSettings ScoreSettings => m_ScoreSettings;
        public RhythmTimelineAsset CurrentSong => m_CurrentSong;


        // score setup
        //public PlayableDirector playableDirector;
        [SerializeField] NoteCounter m_noteCounterManager;
        private int m_TotalNotes;
        private float m_BaseScore;
        private float m_BonusScore;
        private float m_Bonus;
        private float m_MaxHitValue;
        private float m_MaxScore;
        private float m_songScore;

        public bool bonusScoreEnable;
        private float m_bonusOffset;

        public AudioSource hitAudioSource;
        public AudioSource missAudioSource;


        string FormatFloat(float value)
        {
            return String.Format("{0:N0}", value);
        }


        protected void Awake()
        {
            m_Instance = this;
            Toolbox.Set(this);
            m_songScore = 0;
            Debug.Log("Score: " + m_songScore);
        }

        private void Start()
        {

            if (m_RhythmDirector == null) {
                m_RhythmDirector = Toolbox.Get<RhythmDirector>();
            }

            UpdateScoreVisual();
            m_RhythmDirector.OnSongPlay += HandleOnSongPlay;
            m_RhythmDirector.OnSongEnd += HandleOnSongEnd;
            m_RhythmDirector.RhythmProcessor.OnNoteTriggerEvent += HandleOnNoteTriggerEvent;

            if (m_RhythmDirector.IsPlaying) {
                HandleOnSongPlay();
            }
            
        }

        private void HandleOnNoteTriggerEvent(NoteTriggerEventData noteTriggerEventData)
        {
            if (noteTriggerEventData.Miss)
            {
                missAudioSource.Play();
            }
            else
            {
                hitAudioSource.Play();
            }

            var noteAccuracy = AddNoteAccuracyScore(
                noteTriggerEventData.Note,
                noteTriggerEventData.DspTimeDifferencePercentage,
                noteTriggerEventData.Miss);

            var spawnTransform = m_AccuracySpawnPoint != null
                ? m_AccuracySpawnPoint
                : noteTriggerEventData.Note.RhythmClipData.TrackObject.EndPoint;
           
            ScorePopup(spawnTransform, noteAccuracy);
            
        }

        private void HandleOnSongEnd()
        {
            OnSongEnd(m_RhythmDirector.SongTimelineAsset);
        }

        private void HandleOnSongPlay()
        {
            SetSong(m_RhythmDirector.SongTimelineAsset);
        }

        public void SetSong(RhythmTimelineAsset song)
        {
            // pre-game score setup
            m_MaxScore = 1_000_000f;
            m_MaxHitValue = 320f;
            m_Bonus = 100f;
            m_bonusOffset = 1f;

            if(bonusScoreEnable)
            {
                m_bonusOffset = 0.5f;
            }
            Debug.Log("Bonus Enabled: " + bonusScoreEnable + "\nBonus Offset: " + m_bonusOffset);
            



            Debug.Log("m_MaxScore set to 1,000,000\n m_Bonus set to 100");
            Debug.Log("---");

            


            m_CurrentSong = song;
            m_CurrentScore = m_CurrentSong.StartScore;

            // change max score
            m_CurrentMaxPossibleScore = m_MaxScore;

            //m_noteCounterManager.NoteCounterTest();
            //m_CurrentMaxPossibleScore = m_CurrentSong.MaxScore;

            // get total # of notes
            m_TotalNotes = m_noteCounterManager.CountNotes();
            Debug.Log("ScoreManager: Total Notes = " + m_TotalNotes);
            
            

            if (m_CurrentMaxPossibleScore < 0) {
                m_CurrentMaxPossibleScore = song.RhythmClipCount * m_ScoreSettings.MaxNoteScore;
            }
            
            m_CurrentAccuracyIDHistogram = new List<int>(m_CurrentSong.RhythmClipCount);
            m_CurrentMaxChain = 0;
            m_CurrentChain = 0;

            m_ScoreMultiplier = 1;
        
            UpdateScoreVisual();
        }

        public void OnSongEnd(RhythmTimelineAsset song)
        {
            if (m_CurrentAccuracyIDHistogram == null) {
                SetSong(song);
            }
            
            var newScore = new ScoreData(m_CurrentAccuracyIDHistogram.ToArray(), m_CurrentScore, m_CurrentMaxChain, m_ScoreSettings, song);

            if (newScore.FullScore > song.HighScore.FullScore) {
                song.SetHighScore(newScore);
                OnNewHighScore?.Invoke(song);
            }
            m_songScore = m_CurrentScore;
            PlayerPrefs.SetInt("Score", (int)m_songScore);
            PlayerPrefs.SetFloat("Combo", m_CurrentMaxChain);
        }

        public ScoreData GetScoreData()
        {
            return new ScoreData(m_CurrentAccuracyIDHistogram.ToArray(),m_CurrentScore,m_CurrentMaxChain,m_ScoreSettings,m_CurrentSong);
        }

        public NoteAccuracy GetAccuracy(float offsetPercentage, bool miss)
        {
            if (miss) { return GetMissAccuracy(); }

            return GetAccuracy(offsetPercentage);
        }
        
        public NoteAccuracy GetAccuracy(float offsetPercentage)
        {
            var noteAccuracy = m_ScoreSettings.GetNoteAccuracy(offsetPercentage);
            
            if (noteAccuracy == null) {
                Debug.LogWarningFormat("Note Accuracy could not be found for offset ({0}), make sure the score settings are correctly set up", offsetPercentage);
                return null;
            }

            return noteAccuracy;
        }
        
        public NoteAccuracy GetMissAccuracy()
        {
            return  m_ScoreSettings.GetMissAccuracy();
        }
        
        public virtual NoteAccuracy AddNoteAccuracyScore(Note note, float offsetPercentage, bool miss)
        {
            NoteAccuracy noteAccuracy = GetAccuracy(offsetPercentage, miss);

            AddNoteAccuracyScore(note, noteAccuracy);

            return noteAccuracy;
        }

        public virtual void AddNoteAccuracyScore(Note note, NoteAccuracy noteAccuracy)
        {
            
            if (noteAccuracy == null) {
                Debug.LogWarningFormat("Note Accuracy is null");
                return;
            }

            //Chain
            if (noteAccuracy.breakChain) {
                m_CurrentChain = 0;
                OnBreakChain?.Invoke();
            } else {
                m_CurrentChain++;
                m_CurrentMaxChain = Mathf.Max(m_CurrentChain, m_CurrentMaxChain);
                OnContinueChain?.Invoke(m_CurrentChain);
            }
            

            // base score = = (MaxScore * 0.5 / TotalNotes) * (HitValue / 320)
            // 0.5 if bonus enabled, 1.0 if bonus disabled // score = hitValue
            float tempScore = (m_MaxScore * m_bonusOffset / m_TotalNotes) 
                            * (noteAccuracy.score / m_MaxHitValue);

            // bonus score = (MaxScore * 0.5 / TotalNotes) * (HitBonusValue * Sqrt(Bonus) / 320)
            float tempScore2 = 0f;
            if (bonusScoreEnable)
            {
                tempScore2 = (m_MaxScore * m_bonusOffset / m_TotalNotes) 
                            * (noteAccuracy.hitBonusValue * Mathf.Sqrt(m_Bonus) / m_MaxHitValue);
            }

            // adjust bonus
            m_Bonus = m_Bonus + noteAccuracy.hitBonus - noteAccuracy.hitPunishment;
            if (m_Bonus < 0)
            {
                m_Bonus = 0f;
            }
            else if (m_Bonus > 100)
            {
                m_Bonus = 100f;
            }
            

            //AddScore(noteAccuracy.score);
            AddScore(tempScore + tempScore2);
            OnNoteScore?.Invoke(note,noteAccuracy);

            if (m_CurrentSong == null) { return; }

            m_CurrentAccuracyIDHistogram.Add(m_ScoreSettings.GetID(noteAccuracy));
        }

        public virtual void AddScore(float score)
        {

            if (score < 0) {
                m_CurrentScore += score;
            } else {
                m_CurrentScore += m_ScoreMultiplier*score;
            }

            if (m_CurrentSong.PreventMaxScoreOvershoot) {
                m_CurrentScore = Mathf.Min(m_CurrentScore, m_CurrentMaxPossibleScore);
            }
            
            if (m_CurrentSong.PreventMinScoreOvershoot) {
                m_CurrentScore = Mathf.Max(m_CurrentScore, m_CurrentSong.MinScore);
            }
            
            OnScoreChange?.Invoke(m_CurrentScore);
            UpdateScoreVisual();
        }

        public void SetMultiplier(float multiplier)
        {
            m_ScoreMultiplier = multiplier;
            UpdateScoreVisual();
        }
        
        public void SetMultiplier(float multiplier,float time)
        {
            SetMultiplier(multiplier);

            if (m_ScoreMultiplierCoroutine != null) { StopCoroutine(m_ScoreMultiplierCoroutine); }
            m_ScoreMultiplierCoroutine = StartCoroutine(ResetMultiplierDelayed(time));
        }

        public IEnumerator ResetMultiplierDelayed(float delay)
        {
            var start = DspTime.AdaptiveTime;
            while (start + delay > DspTime.AdaptiveTime) { yield return null; }
            SetMultiplier(1);
        }
        
        public int GetChain()
        {
            return m_CurrentChain;
        }
        
        public float GetChainPercentage()
        {
            var maxScore = m_CurrentSong.RhythmClipCount;
            var percentage = 100 * m_CurrentChain / maxScore;
            return percentage;
        }
        
        public float GetMaxChain()
        {
            return m_CurrentMaxChain;
        }
        
        public float GetMaxChainPercentage()
        {
            var maxScore = m_CurrentSong.RhythmClipCount;
            var percentage = 100 * m_CurrentMaxChain / maxScore;
            return percentage;
        }
        
        public float GetScore()
        {
            m_songScore = m_CurrentScore;
            return m_CurrentScore;
        }

        public float GetScorePercentage()
        {
            var percentage = m_CurrentScore * 100 / m_CurrentMaxPossibleScore;
            return percentage;
        }
        
        public ScoreRank GetRank()
        {
            return m_ScoreSettings.GetRank(GetScorePercentage());
        }

        public void UpdateScoreVisual()
        {
            if (m_ScoreTmp != null) {
                m_ScoreTmp.text = FormatFloat(m_CurrentScore);
            }
            if (m_ScoreMultiplierTmp != null) {
                m_ScoreMultiplierTmp.text = m_ScoreMultiplier == 1 ? "" : $"X{m_ScoreMultiplier}";
            }
            if (m_ChainTmp != null) {
                m_ChainTmp.text = m_CurrentChain.ToString();
            }

            if (m_RankSlider != null) {
                if (m_CurrentSong == null) {
                    m_RankSlider.SetRank(0, m_ScoreSettings.GetRank(0));
                } else {
                    var percentage =  GetScorePercentage();
                    m_RankSlider.SetRank(percentage, m_ScoreSettings.GetRank(percentage));
                }
            }
        }

        public virtual void ScorePopup(Transform spawnPoint, NoteAccuracy noteAccuracy)
        {
            Pop(noteAccuracy.popPrefab, spawnPoint);
        }

        public virtual void Pop(GameObject prefab, Transform spawnPoint)
        {
            PoolManager.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
