using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Manager : MonoBehaviour
{
    [SerializeField] GameObject pointer;
    [SerializeField] GameObject scale;

    static float width = 1920;
    static float height = 1080;

    Vector3 click_down_location = new Vector3(0, 0, 0);
    Vector3 click_up_location = new Vector3(0,0,0);
    Vector3[] laser_location;

    bool is_display = false;

    [SerializeField] Text click_position_text;

    // Start is called before the first frame update
    void Start()
    {
        laser_location = new Vector3[] { click_down_location, click_down_location };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            click_down_location = new Vector3(Input.mousePosition.x - width / 2, Input.mousePosition.y - height / 2, Input.mousePosition.z);
            laser_location[0] = click_down_location;
            scale.transform.localPosition = click_down_location;
            scale.SetActive(true);
            is_display = true;
            click_position_text.text = "" + click_down_location;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            click_up_location = new Vector3(Input.mousePosition.x - width / 2, Input.mousePosition.y - height / 2, Input.mousePosition.z); ;
            is_display = false;
        }
        if (is_display)
        {
            laser_location[1] = new Vector3(Input.mousePosition.x - width / 2, Input.mousePosition.y - height / 2, Input.mousePosition.z);
            pointer.GetComponent<LineRenderer>().SetPositions(laser_location);
        }
    }

}
