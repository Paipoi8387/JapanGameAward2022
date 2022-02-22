using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Motion : MonoBehaviour
{
    void Player_Jump()
    {
        GetComponent<Animator>().SetBool("player_jump", true);
    }

    void Player_Landing()
    {
        GetComponent<Animator>().SetBool("player_jump", false);
    }
}
