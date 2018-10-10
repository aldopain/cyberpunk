using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_Container : MonoBehaviour {

	protected int gridWidth = 10;
    protected int gridHeight = 10;
    protected ItemSlot[] container;

	protected ItemSlot this[int x, int y] {
        get { return container[y * gridHeight + x]; }
        set { container[y * gridHeight + x] = value; }
	}

	void Start () {
        Init();
	}

    protected void Init () {
		container = new ItemSlot[ gridWidth * gridHeight ];
    }

    protected bool IsEmptyRect(int startX, int startY, int width, int height)
    {
        // Check that the rect is actually within the Container grid
        if(startX < 0 || startX + width >= gridWidth) return false;
        if(startY < 0 || startY + height >= gridHeight) return false;
    
        // Check each covered cell
        for(int x = startX; x < startX + width; ++x)
        {
            for(int y = startY; y < startY + height; ++y)
            {
                // If any cell isn't empty, the rectangle is blocked
                if(this[x, y].item != null) return false;
            }
        }
    
        // All cells were empty so this rect is clear
        return true;
    }

    protected bool HasSpaceFor(int width, int height, out int startX, out int startY)
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

    protected void FillRect(Item item, int startX, int startY)
    {
        for(int x = startX; x < startX + item.width; ++x)
            for(int y = startY; y < startY + item.height; ++y)
                this[x, y].item = item.gameObject;
    }

    protected bool TryAddItemToContainer(GameObject item)
    {
        Item c = item.GetComponent<Item>();
        if(!c) return false;
    
        int x, y;
        if(!HasSpaceFor(c.width, c.height, out x, out y)) return false;
    
        FillRect(c, x, y);
    
        return true;
    }

    protected bool RemoveItemFromContainer(GameObject obj)
    {
        bool removedAnything = false;
        for(int i = 0; i < container.Length; ++i)
        {
            if(container[i] == obj)
            {
                container[i].item = null;
                removedAnything = true;
            }
        }
        return removedAnything;
    }
}