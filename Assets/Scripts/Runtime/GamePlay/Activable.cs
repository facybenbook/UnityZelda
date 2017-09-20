using UnityEngine;
using System.Collections.Generic;

public class Activable : MonoBehaviour
{
    public List<GameObject> conditions = new List<GameObject>();
    protected bool state;
    public Sprite onSprite;
    public Sprite offSprite;
    // Use this for initialization
    void Start()
    {
        state = false;
        this.GetComponent<Animator>().SetBool("is_on", false);
    }
    // Update is called once per frame
    void Update()
    {
        CheckConditions();
    }
    protected void CheckConditions()
    {
        if(state == false)
        {
            foreach(GameObject trigger in conditions)
            {
                if(trigger.tag == "Enemy")
                {
                    if(trigger.GetComponent<LifeController>().health > 0)
                        return;
                }
                if(trigger.tag == "Trigger")
                {
                    if(trigger.GetComponent<Trigger>().state == false)
                        return;
                }
            }
            GetComponent<BoxCollider2D>().enabled = true;
            state = true;
            if(onSprite != null && offSprite != null)
                GetComponent<SpriteRenderer>().sprite = onSprite;
            else if(GetComponent<Animator>() != null)
                this.GetComponent<Animator>().SetBool("is_on", true);
            else
                print(this.name + "is badly made");
        }
    }
}
