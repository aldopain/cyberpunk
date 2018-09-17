using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ActorUI : MonoBehaviour {
    //Example of a string: Player's Max Ammo is _GenericGun.MaxMagazineCapacity_; Player's health is *HealthController.GetHealth*

    //public GameObject[] UsedObjects;
    public string Text;
    string formattedText;
	// Use this for initialization
	void Start () {
        UpdateUI();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void UpdateUI()
    {
        formattedText = Text;

        int first = 0, second = 0;
        string result;
        bool firstPass = true;

        //variable detection
        while(first != -1 && second != -1)
        {
            if(firstPass)
            {
                first = Text.IndexOf("_");
            }
            else
            {
                first = Text.IndexOf("_", second + 1);
            }

            if(first == -1) { break; }
            second = Text.IndexOf("_", first + 1);
            if(second - first > 0)
            {
                result = Text.Substring(first + 1, second - first - 1);

                if (!result.Contains(".")){
                    Debug.LogError("Invalid form of the string: " + result + ". It should look like <class>.<variable>");
                }else
                {
                    int dotPosition = result.IndexOf(".");
                    string _class = result.Substring(0, dotPosition);
                    print(_class);
                    string _var = result.Substring(dotPosition + 1, result.Length - dotPosition - 1);
                    print(_var);
                    formattedText = formattedText.Replace("_" + result + "_", GetComponent(_class).GetType().GetField(_var).GetValue(gameObject.GetComponent(_class)).ToString());
                }
            }

            firstPass = false;
        }

        //function detection
        firstPass = true;
        first = 0;
        second = 0;
        result = "";

        while (first != -1 && second != -1)
        {
            if (firstPass)
            {
                first = Text.IndexOf("*");
            }
            else
            {
                first = Text.IndexOf("*", second + 1);
            }

            if (first == -1) { break; }
            second = Text.IndexOf("*", first + 1);
            if (second - first > 0)
            {
                result = Text.Substring(first + 1, second - first - 1);

                if (!result.Contains("."))
                {
                    Debug.LogError("Invalid form of the string: " + result + ". It should look like <class>.<function name>");
                }
                else
                {
                    int dotPosition = result.IndexOf(".");
                    string _class = result.Substring(0, dotPosition);
                    print(_class);
                    string _var = result.Substring(dotPosition + 1, result.Length - dotPosition - 1);
                    print(_var);
                    MethodInfo _info = GetComponent(_class).GetType().GetMethod(_var);
                    formattedText = formattedText.Replace("*" + result + "*", _info.Invoke(gameObject.GetComponent(_class), null).ToString());
                }
            }

            firstPass = false;
        }
    }
}
