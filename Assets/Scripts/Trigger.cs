using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
    /* Description:
     * 
     * Trigger, that is using OnTriggerEnter2D, OnTriggerStay2D and OnTriggerExit2D to detect collisions and invoking UnityEvents 
     * Highly customizable through editor 
     */

    //Private Variables
    private int activationsCount;
    private enum TriggerType
    {
        Enter, Stay, Exit
    }

    //Events
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;

    //Activator Settings
    [Header("Settings")]
    public bool oneTimeUse;
    public string[] ActivatorTags;

    public bool canTrigger_Enter;
    public bool canTrigger_Stay;
    public bool canTrigger_Exit;

    //Triggers
    void OnTriggerEnter(Collider col)
    {
        Invoke(col.tag, TriggerType.Enter);
    }

    void OnTriggerStay(Collider col)
    {
        Invoke(col.tag, TriggerType.Stay);
    }

    void OnTriggerExit(Collider col)
    {
        Invoke(col.tag, TriggerType.Exit);
    }

    //Helpers

    /* Invoke (string, TriggerType)
     * 
     * Checks if event can be invoked and does so if true
     */
    void Invoke(string tag, TriggerType type)
    {
        if (isValidActivator(tag))              //Check if the tag is in the whitelist
        {
            //Check if trigger can be used
            if (oneTimeUse && activationsCount > 0)
            {
                return;
            }

            //Invoke UnityEvents
            switch (type)
            {
                case TriggerType.Enter:         //OnEnter
                    if (canTrigger_Enter)
                    {
                        OnEnter.Invoke();
                    }
                    break;
                case TriggerType.Stay:          //OnStay
                    if (canTrigger_Stay)
                    {
                        OnStay.Invoke();
                    }
                    break;
                case TriggerType.Exit:          //OnExit
                    if (canTrigger_Exit)
                    {
                        OnExit.Invoke();
                    }
                    break;
            }

            //Finalize
            activationsCount++;
        }
    }

    bool isValidActivator(string tag)
    {
        if(ActivatorTags.Length == 0)
        {
            return true;
        }

        bool flag = false;
        foreach (string s in ActivatorTags)
        {
            if (s == tag)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    //Misc     
    void Reset()
    {
        activationsCount = 0;
    }

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

}
