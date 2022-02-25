using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Tempo : MonoBehaviour
{
    [SerializeField] AudioSource sound_source;
    [SerializeField] AudioSource bgm_source;
    [SerializeField] AudioClip beat_sound;
    [SerializeField] AudioClip correct_sound;
    [SerializeField] Text Judge;
    int tempo = 0;
    int beat = 0;
    bool is_start = false; //�Q�[�����X�^�[�g�������A���Ă��Ȃ���
    bool is_maked = false; //�m�[�c��z�u�������A���Ă��Ȃ���
    bool is_touched = false; //�{�^�������������A��������
    bool is_touching = false;
    bool is_judged = false; //�m�[�c�̔�����������A���Ă��Ȃ���
    //���ۂ�1FixedUpdate������̈ړ�������move_distance*108
    //1���߂�����̋�����move_distance*108 * 25�@���݂�675
    //�����߂�����̋�����move_distance*108 * 25 / 2�@���݂�337.5
    //�I�u�W�F�N�g���m�̋����͍ŒZ�Ŕ����߂�����̋����ɂ���\��
    float window_height = 1080;
    float tempo_max;
    [SerializeField] int bps = 120;
    [SerializeField] float move_distance = 1f;

    TextAsset csvFile;
    List<string[]> csvDatas = new List<string[]>(); // CSV�̒��g�����郊�X�g;

    [SerializeField] GameObject Obj_List;
    [SerializeField] GameObject HairPrefab1;
    [SerializeField] GameObject EnemyPrefab1;
    [SerializeField] GameObject Player;

    //UI�֘A
    int hair_num = 0;
    [SerializeField] Text Hair_Num;
    int life_num = 3;
    [SerializeField] GameObject Life1;
    [SerializeField] GameObject Life2;
    [SerializeField] GameObject Life3;
    GameObject[] Lifes;

    // Start is called before the first frame update
    void Start()
    {
        //1���ߓ������FixedUpdate�̌Ăяo����
        tempo_max = 60 / (0.02f * bps);
        Notes_Make();
        Lifes = new GameObject[] { Life1, Life2, Life3 };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (is_start)
        {
            tempo++;
            if (tempo == tempo_max)
            {
                tempo = 0;
                beat++;
            }

            this.transform.Translate(move_distance, 0, 0);

            is_touching = false;
            if (Input.GetButton("Jump"))
            {
                if (!is_touched)
                {
                    sound_source.PlayOneShot(beat_sound);
                    is_touched = true;
                    is_touching = true;
                }
            }
            else
            {
                is_touched = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                is_start = true;
                bgm_source.Play();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Boil")       
        {
            if (is_touching && !is_judged)
            {
                is_judged = true;
                Judge.text = "OK";
                sound_source.PlayOneShot(correct_sound);
                //��̐����p�^�[���̎���tempo��23,24,0,1���炢
                Debug.Log(tempo);

                if (collision.tag == "Hair")
                {
                    collision.GetComponent<Animator>().SetBool("get_hair", true);
                    hair_num++;
                    Hair_Num.text = "�{���F" + hair_num + "�{";
                }
                else if (collision.tag == "JumpZone")
                {
                    Player.GetComponent<Animator>().SetBool("player_jump", true);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Boil")
        {
            //�_���[�W����
            life_num--;
            Lifes[life_num].GetComponent<Animator>().SetBool("lost_life", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Judge.text = "";
        is_judged = false;
    }

    public void Notes_Make()
    {
        if (!is_maked)
        {
            is_maked = true;
            csvFile = Resources.Load("TmpNotes") as TextAsset; // Resouces����CSV�ǂݍ���
            StringReader reader = new StringReader(csvFile.text);

            // , �ŕ�������s���ǂݍ��݁A���X�g�ɒǉ����Ă���
            while (reader.Peek() != -1) // reader.Peaek��-1�ɂȂ�܂�
            {
                string line = reader.ReadLine(); // ��s���ǂݍ���
                csvDatas.Add(line.Split(',')); // , ��؂�Ń��X�g�ɒǉ�
            }
            //�v���n�u��z�u
            for (int i = 0; i < csvDatas[0].Length; i++)
            {
                switch (int.Parse(csvDatas[0][i]))
                {
                    case 1:
                        Vector3 obj_trans1 = new Vector3((transform.localPosition.x + ((float.Parse(csvDatas[1][i]) - 1) * move_distance * (window_height / 10) * tempo_max)) / (window_height / 10), 0.0f, 0.0f);
                        GameObject obj1 = Instantiate(HairPrefab1, obj_trans1, Quaternion.identity, Obj_List.transform);
                        break;
                    case 2:
                        Vector3 obj_trans2 = new Vector3((transform.localPosition.x + ((float.Parse(csvDatas[1][i]) - 1) * move_distance * (window_height / 10) * tempo_max)) / (window_height / 10), 0.0f, 0.0f);
                        GameObject obj2 = Instantiate(EnemyPrefab1, obj_trans2, Quaternion.identity, Obj_List.transform);
                        break;
                }
            }
        }
    }
}