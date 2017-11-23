using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Conditionable))]
public class ChestController : Conditionable
{
    public Conditionable conditionable;
    public Collectible content;
    public string messageOnOpen;
    
    protected override void Start()
    {
        base.Start();
        if (conditionable == null)
            conditionable = GetComponent<Conditionable>();
    }
    public void OpenChest()
    {
        StartCoroutine(_OpenChest());
    }

    private IEnumerator _OpenChest()
    {
        ChangeState(true);
        GameController.control.GetItem(content);
        yield return StartCoroutine((WaitForAnimation("Open")));
        GetComponent<AudioSource>().Play();
        yield return StartCoroutine(GameController.control.DisplayMessage(messageOnOpen));
        yield return null;
    }

    private IEnumerator WaitForAnimation(string name)
    {
        if (anim)
        {
            while (!anim.GetCurrentAnimatorStateInfo(0).IsName(name))
            {
                yield return null;
            }
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
    
}
