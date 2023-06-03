namespace Dypsloom.RhythmTimeline.Core.Notes 
{
    using UnityEngine;
    using Dypsloom.RhythmTimeline.Core.Input;
    using Dypsloom.RhythmTimeline.Core.Playables;
    public class CatchNote : Note
    {
        public override void Initialize(RhythmClipData rhythmClipData)
        {
            base.Initialize(rhythmClipData);
        }

        public override void Reset()
        {
            base.Reset();
        }
        protected override void ActivateNote()
        {
            base.ActivateNote();
        }
        protected override void DeactivateNote()
        {
            base.DeactivateNote();

            //Only send the trigger miss event during play mode.
            if (Application.isPlaying == false) { return; }

            if (m_IsTriggered == false)
            {
                InvokeNoteTriggerEventMiss();
            }
        }
        public override void OnTriggerInput(InputEventData inputEventData)
        {
            //Since this is a tap note, only deal with tap inputs.
            if (!inputEventData.Hold)
            {
                return;
            }

            else if (inputEventData.Hold)
            {
                //The gameobject can be set to active false. It is returned to the pool automatically when reset.
                gameObject.SetActive(false);
                m_IsTriggered = true;

                //You may compute the perfect time anyway you want.
                //In this case the perfect time is half of the clip.
                var perfectTime = m_RhythmClipData.RealDuration / 2f;
                var timeDifference = TimeFromActivate;
                var timeDifferencePercentage = Mathf.Abs((float)(100f * timeDifference)) / perfectTime;

                //Send a trigger event such that the score system can listen to it.
                InvokeNoteTriggerEvent(inputEventData, timeDifference, (float)timeDifferencePercentage);
                RhythmClipData.TrackObject.RemoveActiveNote(this);
            }
        }
        protected override void HybridUpdate(double timeFromStart, double timeFromEnd)
        {
            //Compute the perfect timing.
            var perfectTime = m_RhythmClipData.RealDuration / 2f;
            var deltaT = (float)(timeFromStart - perfectTime);

            //Compute the position of the note using the delta T from the perfect timing.
            //Here we use the direction of the track given at delta T.
            //You can easily curve all your notes to any trajectory, not just straight lines, by customizing the TrackObjects.
            //Here the target position is found using the track object end position.
            var direction = RhythmClipData.TrackObject.GetNoteDirection(deltaT);
            var distance = deltaT * m_RhythmClipData.RhythmDirector.NoteSpeed;
            var targetPosition = m_RhythmClipData.TrackObject.EndPoint.position;

            //Using those parameters we can easily compute the new position of the note at any time.
            var newPosition = targetPosition + (direction * distance);
            transform.position = newPosition;
        }
    }
}
