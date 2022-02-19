using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rism : MonoBehaviour
{

    [SerializeField] AudioSource sound_source;
    [SerializeField] AudioSource bgm_source;
    [SerializeField] AudioClip beat_sound;
    [SerializeField] AudioClip correct_sound;
    int sample_rate_Hz = 44100;
    [SerializeField] double bpm = 120;
    double beat_num = 1;
    double frediv;
    double diff;
    [SerializeField] GameObject Start_Back;
    [SerializeField] Text Judge;
    [SerializeField] Text Judge2;

    int beat = 0;
    int beat_quarter = 1;

    void Start()
    {
        frediv = sample_rate_Hz / (bpm / 60);
    }

    void FixedUpdate()
    {
        diff = beat_num - bgm_source.timeSamples / frediv;

        //Ç¢ÇÌÇ‰ÇÈ 1/4Å@îèÇ…çsÇ§èàóù
        if (diff < 0)
        {
            sound_source.PlayOneShot(beat_sound);
            beat_num++;
            beat_quarter++;

            if (beat_quarter == 5)
            {
                beat_quarter = 1;
            }
            Judge2.text = beat_quarter.ToString();
            if (beat_quarter % 4 == 1)
            {
                //sound_source.Play();
                beat++;
            }
            Judge.text = beat.ToString();
        }

        bool tmp = false;
        if ((diff < 0.5 && beat_quarter == 3) || (diff > 0.5 && beat_quarter == 4))
        {
            tmp = true;
            if (Input.GetKey(KeyCode.Space))
            {
                sound_source.PlayOneShot(correct_sound);
            }
        }
        if (tmp)
        {
            Start_Back.SetActive(true);
        }
        else
        {
            Start_Back.SetActive(false);
        }
    }

    public void Push_Start_Button()
    {
        Start_Back.SetActive(false);
        bgm_source.Play();
    }
}
