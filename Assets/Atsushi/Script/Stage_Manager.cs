using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage_Manager : MonoBehaviour
{
    enum Select_Stage { Map, Check, Loading };
    enum Check_Button { Null, Start, Back };
    Check_Button check_button = Check_Button.Null;
    Select_Stage select_stage = Select_Stage.Map;

    [SerializeField] GameObject PlayerCharacter;
    [SerializeField] GameObject Stage1;
    [SerializeField] GameObject Stage2;
    [SerializeField] GameObject Stage3;
    [SerializeField] GameObject Stage4;
    [SerializeField] GameObject Stage5;

    [SerializeField] GameObject Check_Window;
    [SerializeField] GameObject Start_Button;
    [SerializeField] GameObject Back_Button;

    [SerializeField] GameObject FadeIn;

    [SerializeField] int release_stage_num = 3;
    int select_stage_num = 1;
    bool is_moving = false;
    float arrived_rate = 0;
    [SerializeField] float arrived_distance = 0.1f;

    GameObject[] All_Stage;

    [SerializeField] Text Best_Score;


    // Start is called before the first frame update
    void Start()
    {
        All_Stage = new GameObject[] { Stage1, Stage2, Stage3, Stage4, Stage5 };
        //現在、解除されているステージまで表示
        for (int i = 0; i < All_Stage.Length; i++)
        {
            if(i < release_stage_num)
            {
                All_Stage[i].SetActive(true);
            }
            else
            {
                All_Stage[i].SetActive(false);
            }            
        }

        PlayerCharacter.transform.localPosition = All_Stage[select_stage_num - 1].transform.localPosition;
        select_stage = Select_Stage.Map;
    }

    // Update is called once per frame
    void Update()
    {
        if (select_stage == Select_Stage.Map)
        {
            if (!is_moving)
            {
                Stage_Select();
            }
            else
            {
                arrived_rate += 0.001f;
                PlayerCharacter.transform.localPosition = Vector3.Lerp(PlayerCharacter.transform.localPosition, All_Stage[select_stage_num - 1].transform.localPosition, arrived_rate);
            }

            if (Vector3.Distance(PlayerCharacter.transform.localPosition, All_Stage[select_stage_num - 1].transform.localPosition) < arrived_distance)
            {
                is_moving = false;
                arrived_rate = 0;
            }
        }
        else if (select_stage == Select_Stage.Check)
        {
            Stage_Check();
        }
    }

    void Stage_Check()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            check_button = Check_Button.Back;
            Back_Button.GetComponent<Animator>().SetBool("button_select", true);
            Start_Button.GetComponent<Animator>().SetBool("button_select", false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            check_button = Check_Button.Start;
            Back_Button.GetComponent<Animator>().SetBool("button_select", false);
            Start_Button.GetComponent<Animator>().SetBool("button_select", true);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if (check_button == Check_Button.Back)
            {
                select_stage = Select_Stage.Map;
                check_button = Check_Button.Null;
            }
            else if (check_button == Check_Button.Start)
            {
                FadeIn.GetComponent<Animator>().SetBool("fadein", true);
                select_stage = Select_Stage.Loading;
                Invoke("Change_Scene", 2f);
            }
            Check_Window.SetActive(false);
            Back_Button.GetComponent<Animator>().SetBool("button_select", false);
            Start_Button.GetComponent<Animator>().SetBool("button_select", false);
        }
    }

    void Change_Scene()
    {
        //SceneManager.LoadScene("Rism" + select_stage);
        SceneManager.LoadScene("Rism");
    }

    void Stage_Select()
    {
        int before_stage_num = select_stage_num;
        switch (select_stage_num)
        {
            case 1:
                if (release_stage_num >= 2 && Input.GetKeyDown(KeyCode.RightArrow)) { select_stage_num = 2; }
                else if (release_stage_num >= 5 && Input.GetKeyDown(KeyCode.UpArrow)) { select_stage_num = 5; }
                break;
            case 2:
                if (release_stage_num >= 2 && Input.GetKeyDown(KeyCode.LeftArrow)) { select_stage_num = 1; }
                else if (release_stage_num >= 3 && Input.GetKeyDown(KeyCode.UpArrow)) { select_stage_num = 3; }
                break;
            case 3:
                if (release_stage_num >= 3 && Input.GetKeyDown(KeyCode.DownArrow)) { select_stage_num = 2; }
                else if (release_stage_num >= 4 && Input.GetKeyDown(KeyCode.LeftArrow)) { select_stage_num = 4; }
                break;
            case 4:
                if (release_stage_num >= 4 && Input.GetKeyDown(KeyCode.RightArrow)) { select_stage_num = 3; }
                else if (release_stage_num >= 5 && Input.GetKeyDown(KeyCode.DownArrow)) { select_stage_num = 5; }
                break;
            case 5:
                if (release_stage_num >= 5 && Input.GetKeyDown(KeyCode.UpArrow)) { select_stage_num = 4; }
                else if (release_stage_num >= 1 && Input.GetKeyDown(KeyCode.DownArrow)) { select_stage_num = 1; }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Check_Window.SetActive(true);
            select_stage = Select_Stage.Check;
            Best_Score.text = PlayerPrefs.GetInt("Best_Score"+select_stage,0) + "　てん";
        }
        else if (before_stage_num != select_stage_num) { is_moving = true; }
    }
}
