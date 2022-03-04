using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Manager : MonoBehaviour
{
    [SerializeField] GameObject PlayerCharacter;
    [SerializeField] GameObject Stage1;
    [SerializeField] GameObject Stage2;
    [SerializeField] GameObject Stage3;
    [SerializeField] GameObject Stage4;
    [SerializeField] GameObject Stage5;

    [SerializeField] int release_stage_num = 3;
    int select_stage_num = 1;
    bool is_moving = false;
    float arrived_rate = 0;
    [SerializeField] float arrived_distance = 0.1f;

    GameObject[] All_Stage;


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
    }

    // Update is called once per frame
    void Update()
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

        if (Vector3.Distance(PlayerCharacter.transform.localPosition, All_Stage[select_stage_num-1].transform.localPosition) < arrived_distance)
        {
            is_moving = false;
            arrived_rate = 0;
        }
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
                else if (release_stage_num >= 4 && Input.GetKeyDown(KeyCode.RightArrow)) { select_stage_num = 4; }
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
        if(before_stage_num != select_stage_num) { is_moving = true; }
    }
}
