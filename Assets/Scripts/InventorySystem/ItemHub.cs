using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHub : MonoBehaviour {

    public Text name;
    public Text description;

	// Use this for initialization
	void Start () {

	}

    public void Show (GameObject item) {
        //var itemInfo = item.GetComponent<Item>();
        this.enabled = true;
        name.text = "tttt";
        description.text = "long text";

        //name.text = itemInfo.itemName;
        //description.text = itemInfo.itemDescription;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (2)){
            print ("HIDE");
            gameObject.SetActive(false);
        }
	}
}
