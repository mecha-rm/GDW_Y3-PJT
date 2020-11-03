using UnityEngine;

public class AudioClip : MonoBehaviour
{
    // audio source
    public AudioSource audioSource = null;

    // start start and end of the audio clip being played. This is in seconds

    // the start of the clip. If this value is negative, the clip continues like normal.
    // if clip start is less than 0, then it doesn't function.
    public float clipStart = 0.0F;

    // the end of the clip.
    // i don't know what happens if the time is set beyond the clip length but I assume it just errors out.
    public float clipEnd = 0.0F;

    // if 'true', a song will be limited to the clip rnage.
    // if 'false', the start of the song will play normally,...
    // but once within the clip range it will stay within the clip.
    public bool playAtClipStart = false;

    // Start is called before the first frame update
    void Start()
    {
        // gets the start and end of the clip if not set.
        if (audioSource != null)
        {
            // if the clip start and end are both set to zero (i.e. they weren't set)
            // clip start
            if (clipStart <= 0.0F)
                clipStart = 0.0F;

            // clip end
            if (audioSource.clip != null && clipEnd <= 0.0F)
                clipEnd = audioSource.clip.length;

            // start at clip start instead of start of song.
            if(playAtClipStart)
                audioSource.time = clipStart;
        }
    }

    // plays the audio.
    // if limited to the clip start, it starts from the loop point.
    public void PlayAudio()
    {
        // audio source or audio clip doesn't exist.
        if (audioSource == null || audioSource.clip == null)
            return;

        // stops audio if it's currently playing.
        audioSource.Stop();

        // if the audio should start at the clip start when first played.
        if (playAtClipStart && clipStart >= 0.0F && clipStart < audioSource.clip.length)
            audioSource.time = clipStart;
        else // start source at the start of the audio.
            audioSource.time = 0.0F;

        // plays the audio
        audioSource.Play();
    }

    // stops te audio
    public void StopAudio()
    {
        // audio source or audio clip doesn't exist.
        if (audioSource == null || audioSource.clip == null)
            return;

        audioSource.Stop();

        // bring audio to clip start
        if (playAtClipStart && clipStart >= 0.0F && clipStart < audioSource.clip.length)
            audioSource.time = clipStart;
        else // bring audio to start of the song
            audioSource.time = 0.0F;
    }

    // if the audio is set to loop
    public bool GetLooping()
    {
        if (audioSource != null)
            return audioSource.loop;
        else
            return false;
    }

    // sets the audio to loop
    public void SetLooping(bool looping)
    {
        if (audioSource != null)
            audioSource.loop = looping;
    }

    // returns the length of the audio clip
    public float GetClipLength()
    {
        return clipEnd - clipStart;
    }

    // returns the value of clip start
    public float GetClipStart()
    {
        return clipStart;
    }

    // sets the value of clip start in seconds
    // this only works if the audioClip has been set.
    public void SetClipStartInSeconds(float seconds)
    {
        // if the audio source is null.
        if (audioSource == null)
            return;

        // if the audio clip is null.
        if (audioSource.clip == null)
            return;


        // setting value to clip start.
        clipStart = (seconds >= 0.0F && seconds <= audioSource.clip.length) ? 
            seconds : clipStart;
    }
    
    // sets the clip start time as a percentage, with 0 being 0% and 1 being 100%.
    public void SetClipStartAsPercentage(float t)
    {
        // if the audio source is null.
        if (audioSource == null)
            return;

        // if the audio clip is null.
        if (audioSource.clip == null)
            return;

        t = Mathf.Clamp(t, 0.0F, 1.0F);
        clipStart = Mathf.Lerp(0.0F, audioSource.clip.length, t);
    }

    // returns the value of clip end
    public float GetClipEnd()
    {
        return clipEnd;
    }

    // sets the value of clip end in seconds
    // this only works if the audioClip has been set.
    public void SetClipEndInSeconds(float seconds)
    {
        // if the audio source is null.
        if (audioSource == null)
            return;

        // if the audio clip is null.
        if (audioSource.clip == null)
            return;


        // setting value to clip start.
        clipEnd = (seconds >= 0.0F && seconds <= audioSource.clip.length) ?
            seconds : clipEnd;
    }

    // sets the clip end time as a percentage, with 0 being 0% and 1 being 100%.
    public void SetClipEndAsPercentage(float t)
    {
        // if the audio source is null.
        if (audioSource == null)
            return;

        // if the audio clip is null.
        if (audioSource.clip == null)
            return;

        t = Mathf.Clamp(t, 0.0F, 1.0F);
        clipEnd = Mathf.Lerp(0.0F, audioSource.clip.length, t);
    }

    // gets the variable that says whether or not to start the song at the start of the clip.
    public bool GetPlayAtClipStart()
    {
        return playAtClipStart;
    }

    // sets the play at clip start
    public void SetPlayAtClipStart(bool pacs)
    {
        playAtClipStart = pacs;

        // if the audio source is null.
        if (audioSource == null)
            return;

        // if the audio clip is null.
        if (audioSource.clip == null)
            return;

        // if the audio should play at the start of the clip.
        if(playAtClipStart)
        {
            // if the start of the clip is greater than the current time of the clip...
            // the audioSouce is set to the start of the clip.
            if (clipStart > audioSource.time)
                audioSource.time = clipStart;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // audio source, or the clip are not set.
        if (audioSource == null)
            return;
        if (audioSource.clip == null)
            return;

        // clamp clipStart and clipEnd
        // TODO: see if using if statements is less computationally expensive (2 conditional statements per clamp)
        clipStart = Mathf.Clamp(clipStart, 0.0F, audioSource.clip.length);
        clipEnd = Mathf.Clamp(clipEnd, 0.0F, audioSource.clip.length);

        // if the clips are the same, no audio can play.
        if (clipStart == clipEnd)
        {
            return;
        }
        // if the clip end is greater than the clip start, then the values are swapped.
        else if (clipStart > clipEnd)
        {
            float temp = clipStart;
            clipStart = clipEnd;
            clipEnd = temp;
        }

        // if the audio source is playing
        if(audioSource.isPlaying)
        {
            // this isn't needed since using the Play() function in this class handles this.
            // puts the audio source at the clip start.
            // if (audioSource.time < clipStart && !playAtClipStart)
            //     audioSource.time = clipStart;

            // the audioSource has reached the end of the clip.
            if (audioSource.time >= clipEnd)
            {
                // checks to see if the audio is looping
                switch(audioSource.loop)
                {
                    case true: // audio is looping
                        audioSource.time = clipStart;
                        break;

                    case false: // audio is not looping
                        // audio is stopped, and returns to clip start.
                        audioSource.Stop();
                        audioSource.time = clipStart;
                        break;
                }
            }
        }
    }
}
