using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class NoteCounter : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void Start()
    {
        int clipCount = CountClips(playableDirector);
        Debug.Log("Total Clips (Notes): " + clipCount);
    }

    public int CountClips(PlayableDirector director)
    {
        if (director == null)
        {
            return 0;
        }

        TimelineAsset timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
        {
            return 0;
        }

        int clipCounter = 0;
        foreach (var track in timeline.GetOutputTracks())
        {
            clipCounter += track.GetClips().Count();
        }

        return clipCounter;
    }
}


