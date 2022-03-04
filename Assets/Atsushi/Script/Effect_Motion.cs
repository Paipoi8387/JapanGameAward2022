using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect_Motion : MonoBehaviour
{
    [SerializeField] Sprite Baku;
    [SerializeField] Sprite Yoisho;

    //ëSÇƒPlayer_MotionÇ©ÇÁåƒÇ—èoÇµÇÃÇΩÇﬂpublic

    public void Effect_On()
    {
        transform.localPosition = new Vector3(Random.Range(-10f,10f), Random.Range(-10f, 10f),0);
        transform.eulerAngles = new Vector3(0f, 0f, Random.Range(-10f, 10f));
        float scale_rn = Random.Range(1f, 2f);
        transform.localScale = new Vector3(scale_rn, scale_rn, 1);
        GetComponent<Animator>().SetBool("effect_text", true);
    }

    public void Effect_Off()
    {
        GetComponent<Animator>().SetBool("effect_text", false);
    }

    public void Eat_Text()
    {
        GetComponent<Image>().sprite = Baku;
    }

    public void Jump_Text()
    {
        GetComponent<Image>().sprite = Yoisho;
    }
}
