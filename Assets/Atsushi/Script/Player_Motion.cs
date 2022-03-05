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
    [SerializeField] AudioClip jump_sound;
    [SerializeField] AudioClip eat_sound;
    [SerializeField] AudioClip damage_sound;
    bool is_judged = false; //ノーツの判定をしたか、していないか
    bool is_invincible = false;


    [SerializeField] GameObject Effect_Text;

    //Tempoから呼び出し
    public void Player_Jump()
    {
        if (!GetComponent<Animator>().GetBool("player_jump"))
        {
            GetComponent<Animator>().SetBool("player_jump", true);
            sound_source.PlayOneShot(jump_sound);
            Effect_Text.GetComponent<Effect_Motion>().Jump_Text();
            Effect_Text.GetComponent<Effect_Motion>().Effect_On();
        }
    }

    //Tempoから呼び出し
    public void Player_Eat_Early()
    {
        if (!GetComponent<Animator>().GetBool("player_eat_early"))
        {
            GetComponent<Animator>().SetBool("player_eat_early", true);
            sound_source.PlayOneShot(eat_sound);
            Effect_Text.GetComponent<Effect_Motion>().Eat_Text();
            Effect_Text.GetComponent<Effect_Motion>().Effect_On();
        }
    }

    void Player_Landing()
    {
        GetComponent<Animator>().SetBool("player_jump", false);
        Effect_Text.GetComponent<Effect_Motion>().Effect_Off();
    }

    //これ使わない
    void Player_Landing_Early()
    {
        GetComponent<Animator>().SetBool("player_jump_early", false);
    }

    //これ使わない
    void Player_Finish_Eat()
    {
        GetComponent<Animator>().SetBool("player_eat", false);
    }

    void Player_Finish_Eat_Early()
    {
        GetComponent<Animator>().SetBool("player_eat_early", false);
        Effect_Text.GetComponent<Effect_Motion>().Effect_Off();
    }

    void Player_Finish_Damage()
    {
        GetComponent<Animator>().SetBool("player_damage", false);
        is_invincible = false;
    }


    /*private void Start()
    {
        Judge.text = "" + transform.localPosition.x;
    }*/


    void OnTriggerStay2D(Collider2D collision)
    {
        if (!is_judged && tempoCS.touching_key != Tempo.Touching_Key.Null && collision.tag != "Boil")
        {
            is_judged = true;

            //sound_source.PlayOneShot(correct_sound);
            //大体正解パターンの時はtempoが23,24,0,1くらい
            //Debug.Log(tempoCS.tempo);

            if (tempoCS.touching_key == Tempo.Touching_Key.Enter && collision.tag == "Hair")
            {
                collision.GetComponent<Animator>().SetBool("get_hair", true);
                tempoCS.Change_Hair_Num();
                sound_source.PlayOneShot(correct_sound);
            }
            /*else if (tempoCS.touching_key == Tempo.Touching_Key.Space && collision.tag == "JumpZone")
            {

            }*/
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boil" && !is_invincible)
        {
            //ダメージ判定
            tempoCS.Lost_Life();
            sound_source.PlayOneShot(damage_sound);
            GetComponent<Animator>().SetBool("player_damage", true);
            is_invincible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Judge.text = "";
        is_judged = false;
    }
}
