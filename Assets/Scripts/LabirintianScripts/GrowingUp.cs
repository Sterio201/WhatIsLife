using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingUp : MonoBehaviour
{
    [SerializeField] Color startColor;
    [SerializeField] Color shiftColor;
    [SerializeField] EventsLabirintian events;
    [SerializeField] GameObject player;

    TrailRenderer trail;
    SpriteRenderer render;

    [SerializeField] List<AudioClip> audioClips;
    [SerializeField] AudioSource musicPlayer;

    bool timeRepaiting;
    float delay = 5f;

    private void Start()
    {
        timeRepaiting = true;
        
        trail = player.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
        render = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        musicPlayer.clip = audioClips[0];
        musicPlayer.Play();
    }

    private void Update()
    {
        if(events.currentTimeLife <= 240f)
        {
            if (timeRepaiting)
            {
                if(musicPlayer.clip != audioClips[1])
                {
                    musicPlayer.clip = audioClips[1];
                    musicPlayer.Play();
                }

                if(render.color == shiftColor)
                {
                    timeRepaiting = false;
                }
                else
                {
                    Repaiting();
                }
            }
        }

        if (events.currentTimeLife <= 60f)
        {
            if (musicPlayer.clip != audioClips[2])
            {
                musicPlayer.clip = audioClips[2];
                musicPlayer.Play();
            }

            if(delay <= 0f)
            {
                PlayerMoveScript moveScript = player.GetComponent<PlayerMoveScript>();
                moveScript.speed -= 0.00019f;
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }

    void Repaiting()
    {
        AllIvents.shiftColorMaze.Invoke(shiftColor);

        trail.startColor = Color.Lerp(trail.startColor, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));
        render.color = Color.Lerp(render.color, shiftColor, Mathf.Abs(Mathf.Sin(Time.deltaTime)));
    }
}