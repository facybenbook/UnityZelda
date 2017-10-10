using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {
	public GameObject Explosion;
    public bool remoteControlled;
    protected Animator anim;
    // Use this for initialization

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Explode()
    {
        GameObject myExplosion = Instantiate(Explosion, transform.position, transform.rotation);
        GameController.control.AddToZIndex(myExplosion);
        Destroy(this.gameObject);
    }

    public void TimerIncrement()
    {
        anim.speed += 0.5f;
        if (anim.speed == 4)
            Explode();
        else if (anim.speed == 2)
        {
            Color tmp = new Color();
            ColorUtility.TryParseHtmlString("#FFB338FF", out tmp);
            GetComponent<SpriteRenderer>().color = tmp;
        }
    }
}
