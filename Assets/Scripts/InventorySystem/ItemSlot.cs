using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	public GameObject item;

    //add here name, description, info, image, buttons and other stuff
    //maybe move this in separate UI class
    public void ShowItemHub () {
        // if (item != null) {
           // var itemInfo = item.GetComponent<Item>();
            GameObject hub = new GameObject("ItemHub");
            hub.AddComponent<CanvasRenderer>();
            Image i = hub.AddComponent<Image>();
            i.color = Color.grey;

            var name_go = new GameObject("ItemName");
            var description_go = new GameObject("DescriptionName");
            name_go.transform.SetParent(hub.transform, false);
            description_go.transform.SetParent(hub.transform, false);

            var name = name_go.AddComponent<Text>();
            var description = description_go.AddComponent<Text>();
            name.text = "tttt";
            description.text = "long text";
            name.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            description.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

            // name.text = itemInfo.itemName;
            // description.text = itemInfo.itemDescription;
            hub.transform.SetParent(this.transform, false);
        // }
    }

    public void SetImage () {
        
    }
}
