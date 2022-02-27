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
    bool is_judged = false; //�m�[�c�̔�����������A���Ă��Ȃ���


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
            //��̐����p�^�[���̎���tempo��23,24,0,1���炢
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
            //�_���[�W����
            tempoCS.Lost_Life();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Judge.text = "";
        is_judged = false;
    }
}
