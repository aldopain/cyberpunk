using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private GameObject _panel;
    private bool isActive = false;
    private float _baseTimeScale;
    int gridWidth = 10;
    int gridHeight = 10;
    GameObject[] myInventory;

	// Use this for initialization
	void Start () {
        myInventory = new GameObject[gridWidth*gridHeight];
        _panel = GameObject.Find ("Inventory");
        _panel.SetActive (false);
        _baseTimeScale = Time.timeScale;
	}

    public int AddItemToInventory(GameObject item) {
        int intialIndexLocation = -1;
        //Add item in
        if(item.GetComponent<Item>() != null) {
            Item temp =   item.GetComponent<Item>();
            if(SlotsRemaining() >= temp.height*temp.width )   //Simple check to see if you have slots left.
            {
                bool canFit = false;    
                bool firstFoundLocation = true;
                int count = 0;
                for(int i = 0; i < gridWidth-(temp.width-1); i ++)  //Don't need to check slots that can't handle the width
                {
                    for(int t = 0; t < (gridHeight)-(temp.height-1); t++)   //Don't need to check slots that can't handle the height.
                    {
                        if(myInventory[i+(gridHeight*t)] == null && !canFit)   //See if the slot is null at the position.
                        {
                            int neededCount = temp.height+temp.width;
                            for(int j = 0; j < temp.width; j++)  //Loop through the required spaces for the width
                            {
                                if(myInventory[i+(gridHeight*t) + j] == null && !canFit)
                                {
                                    //valid width for the location
                                    for( int k = 0; k < temp.height; k++)  //Loop through the require spaces for the height
                                    {
                                        if(myInventory[i+(gridHeight*t+k) + j] == null)
                                        {
                                            if(firstFoundLocation)
                                            {
                                                firstFoundLocation = false;
                                                intialIndexLocation = i+(gridWidth*t);
                                            }
                                            //Valid Height for the location.. Place it here.
                                            count+=2;
                                            if(count == neededCount)
                                            {
                                                //intialIndexLocation = i+(gridWidth*t);
                                                canFit = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if(count >= neededCount)
                            {
                                //intialIndexLocation = i+(gridWidth*t);
                                canFit = true;
                                break;
                            }
                        }
                    }  
                }
            }
            if(SlotsRemaining() == myInventory.Length)
            {
                intialIndexLocation = 0;
            }
            if(intialIndexLocation > -1)
            {
                for(int i = 0; i < temp.width; i++)
                {
                    for(int t = 0; t < temp.height; t++)
                    {
                        myInventory[intialIndexLocation+i+(gridHeight*t)] = item;
                    }
                }
            }
        }
        return intialIndexLocation;
    }
    
    string txtWidth = "";
    string txtHeight = "";
   
    void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 90, 20), "Height: ");
        txtHeight = GUI.TextField(new Rect(55, 5, 90, 20), txtHeight);
        GUI.Label(new Rect(5, 25, 90, 20), "Width: ");
        txtWidth = GUI.TextField(new Rect(55, 25, 90, 20), txtWidth);
        if(GUI.Button(new Rect(5, 55, 100, 30), "Add an Item"))
        {
            GameObject tempGo = (GameObject)Instantiate(new GameObject("New Item") );
            tempGo.AddComponent<Item>();
            Item temp = tempGo.GetComponent<Item>();
            temp.width = int.Parse(txtWidth);
            temp.height = int.Parse(txtHeight);
            AddItemToInventory(tempGo);
        }
        if(GUI.Button(new Rect(105, 55, 50, 30), "Clear"))
        {
            myInventory = new GameObject[gridWidth*gridHeight];
        }
       
        for(int i = 0; i < gridWidth; i ++)  //Don't need to check slots that can't handle the width
        {  
            for(int t = 0; t < gridHeight; t++)   //Don't need to check slots that can't handle the height.
            {
                if(myInventory[i+(gridHeight*t)]  == null)
                {
                    GUI.Label(new Rect((20+(i*30)), 100+(t*30), 30, 30), "["+(i+(gridHeight*t))+"]");
                }
                else
                {
                    GUI.Label(new Rect((20+(i*30)), 100+(t*30), 30, 30), "[X]");
                }
            }
        }
    }
    public int SlotsRemaining()
    {
        int count = 0;
        for(int i = 0; i < myInventory.Length; i++) {
            if(myInventory[i] == null) {
                count++;
            }
        }
        return count;
    }

    //////////
    public GameObject this[int x, int y] {
    get { return myInventory[y * gridHeight + x]; }
    set { myInventory[y * gridHeight + x] = value; }
    }
    
    public bool IsEmptyRect(int startX, int startY, int width, int height)
    {
        // Check that the rect is actually within the inventory grid
        if(startX < 0 || startX + width >= gridWidth) return false;
        if(startY < 0 || startY + height >= gridHeight) return false;
    
        // Check each covered cell
        for(int x = startX; x < startX + width; ++x)
        {
            for(int y = startY; y < startY + height; ++y)
            {
                // If any cell isn't empty, the rectangle is blocked
                if(this[x, y] != null) return false;
            }
        }
    
        // All cells were empty so this rect is clear
        return true;
    }
    
    public bool HasSpaceFor(int width, int height, out int startX, out int startY)
    {
        startX = 0;
        startY = 0;
        for(; startX < gridWidth - width; ++startX)
        {
            for(; startY < gridHeight - height; ++startY)
            {
                if(IsEmptyRect(startX, startY, width, height)) return true;
            }
        }
        return false;
    }
    
    public void FillRect(Item item, int startX, int startY)
    {
        for(int x = startX; x < startX + item.width; ++x)
            for(int y = startY; y < startY + item.height; ++y)
                this[x, y] = item.gameObject;
    }
    
    public bool TryAddItemToInventory(GameObject item)
    {
        Item c = item.GetComponent<Item>();
        if(!c) return false;
    
        int x, y;
        if(!HasSpaceFor(c.width, c.height, out x, out y)) return false;
    
        FillRect(c, x, y);
    
        return true;
    }
    
    public bool RemoveItemFromInventory(GameObject obj)
    {
        bool removedAnything = false;
        for(int i = 0; i < myInventory.Length; ++i)
        {
            if(myInventory[i] == obj)
            {
                myInventory[i] = null;
                removedAnything = true;
            }
        }
        return removedAnything;
    }
    /////////

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.I)) {
            Time.timeScale = isActive ? _baseTimeScale : 0f;
            isActive = !isActive;
            _panel.SetActive (isActive);
        }
	}
}
