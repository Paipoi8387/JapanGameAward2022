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
    bool is_start = false;
    bool is_touched = false;
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

    // Start is called before the first frame update
    void Start()
    {
        //1���ߓ������FixedUpdate�̌Ăяo����
        tempo_max = 60 / (0.02f * bps);
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
                //sound_source.PlayOneShot(beat_sound);
            }

            this.transform.Translate(move_distance, 0, 0);
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
        if (Input.GetButton("Jump") && !is_touched)
        {
            sound_source.PlayOneShot(beat_sound);
            is_touched = true;
            Judge.text = "OK";
            //��̐����p�^�[���̎���tempo��23,24,0,1���炢
            Debug.Log(tempo);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        is_touched = false;
        Judge.text = "";
    }

    public void Start_Button()
    {
        Notes_Make();
    }

    void Notes_Make()
    {
        csvFile = Resources.Load("TmpNotes") as TextAsset; // Resouces����CSV�ǂݍ���
        StringReader reader = new StringReader(csvFile.text);

        // , �ŕ�������s���ǂݍ���
        // ���X�g�ɒǉ����Ă���
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
                    Vector3 obj_trans = new Vector3((transform.localPosition.x + (float.Parse(csvDatas[1][i]) * move_distance * (window_height / 10) * tempo_max)) / (window_height / 10), -180 / (window_height / 10), 0.0f);
                    GameObject obj = Instantiate(HairPrefab1, obj_trans, Quaternion.identity,Obj_List.transform);
                    break;
            }
        }
    }
}
