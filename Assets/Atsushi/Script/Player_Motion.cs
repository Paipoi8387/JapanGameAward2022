using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Motion : MonoBehaviour
{
    [SerializeField] Tempo tempoCS;
    [SerializeField] Text Judge;
    [SerializeField] AudioSource sound_source;
    [SerializeField] AudioClip correct_sound;
    bool is_judged = false; //ノーツの判定をしたか、していないか


    void Player_Jump()
    {
        GetComponent<Animator>().SetBool("player_jump", true);
    }

    void Player_Landing()
    {
        GetComponent<Animator>().SetBool("player_jump", false);
    }

    void Player_Finish_Eat()
    {
        GetComponent<Animator>().SetBool("player_eat", false);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!is_judged && tempoCS.touching_key != Tempo.Touching_Key.Null && collision.tag != "Boil")
        {
            is_judged = true;
            Judge.text = "OK";
            sound_source.PlayOneShot(correct_sound);
            //大体正解パターンの時はtempoが23,24,0,1くらい
            Debug.Log(tempoCS.tempo);

            if (tempoCS.touching_key == Tempo.Touching_Key.Enter && collision.tag == "Hair")
            {
                collision.GetComponent<Animator>().SetBool("get_hair", true);
                tempoCS.Change_Hair_Num();
            }
            else if (tempoCS.touching_key == Tempo.Touching_Key.Space && collision.tag == "JumpZone")
            {

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boil")
        {
            //ダメージ判定
            tempoCS.Lost_Life();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Judge.text = "";
        is_judged = false;
    }
}
