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
    public int tempo = 0;
    int beat = 0;
    bool is_start = false; //�Q�[�����X�^�[�g�������A���Ă��Ȃ���
    bool is_maked = false; //�m�[�c��z�u�������A���Ă��Ȃ���
    bool is_touched = false; //�{�^�������������A��������
    public enum Touching_Key {Null,Space,Enter};
    public Touching_Key touching_key = Touching_Key.Null;

    //���ۂ�1FixedUpdate������̈ړ�������move_distance*108
    //1���߂�����̋�����move_distance*108 * 25�@���݂�675
    //�����߂�����̋�����move_distance*108 * 25 / 2�@���݂�337.5
    //�I�u�W�F�N�g���m�̋����͍ŒZ�Ŕ����߂�����̋����ɂ���\��
    float window_height = 1080;
    float tempo_max;
    [SerializeField] int bpm = 120;
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
        tempo_max = 60 / (0.02f * bpm);
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
                sound_source.PlayOneShot(beat_sound);
                tempo = 0;
                beat++;
            }

            this.transform.Translate(move_distance, 0, 0);

            //is_touching = false;
            touching_key = Touching_Key.Null;
            if (Input.GetButton("Jump") || Input.GetButton("Submit"))
            {
                if (!is_touched)
                {                    
                    //sound_source.PlayOneShot(beat_sound);
                    is_touched = true;
                    if (Input.GetButton("Jump"))
                    {
                        touching_key = Touching_Key.Space;
                        Player.GetComponent<Animator>().SetBool("player_jump_early", true);
                    }
                    else if (Input.GetButton("Submit"))
                    {
                        touching_key = Touching_Key.Enter;
                        Player.GetComponent<Animator>().SetBool("player_eat_early", true);
                    }
                    //is_touching = true;
                    //Debug.Log(touching_key);
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

    public void Notes_Make()
    {
        if (!is_maked)
        {
            is_maked = true;
            csvFile = Resources.Load("TmpNotes2") as TextAsset; // Resouces����CSV�ǂݍ���
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
                        Vector3 obj_trans1 = new Vector3((Player.transform.localPosition.x + ((float.Parse(csvDatas[1][i]) - 1) * move_distance * (window_height / 10) * tempo_max)) / (window_height / 10), 0.0f, 0.0f);
                        GameObject obj1 = Instantiate(HairPrefab1, obj_trans1, Quaternion.identity, Obj_List.transform);
                        break;
                    case 2:
                        Vector3 obj_trans2 = new Vector3((Player.transform.localPosition.x + ((float.Parse(csvDatas[1][i]) - 1) * move_distance * (window_height / 10) * tempo_max)) / (window_height / 10), 0.0f, 0.0f);
                        GameObject obj2 = Instantiate(EnemyPrefab1, obj_trans2, Quaternion.identity, Obj_List.transform);
                        break;
                }
            }
        }
    }

    public void Change_Hair_Num()
    {
        hair_num++;
        Hair_Num.text = "�{���F" + hair_num + "�{";
    }

    public void Lost_Life()
    {
        life_num--;
        Lifes[life_num].GetComponent<Animator>().SetBool("lost_life", true);
    }
}