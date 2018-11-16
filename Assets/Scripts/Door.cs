using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public enum Direction{
		Up, Down, Left, Right
	}
	[System.Serializable]
	public struct DoorPanel
	{
		public GameObject Panel;
		public Direction OpenDirection;
		public float MaxDistance;
		public float OpenSpeed;
		
		public Vector3 startingPosition;

	}

	public DoorPanel[] Panels;

	private float DISTANCE_OFFSET = .05f;
	private bool isOpening;
	private bool isClosing;

	// Use this for initialization
	void Start () {
        SetStartingPositions();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Open(){
		foreach(DoorPanel p in Panels){
			StartCoroutine(openPanel(p));
		}
	}

	public void Close(){
		foreach(DoorPanel p in Panels){
			StartCoroutine(closePanel(p));
		}
	}

	IEnumerator openPanel(DoorPanel p){
		if(!isClosing){
			isOpening = true;
			Vector2Int dir = enum2dir(p.OpenDirection);
			while(p.MaxDistance - Vector3.Distance(p.Panel.transform.position, p.startingPosition) > DISTANCE_OFFSET && isOpening){
                p.Panel.transform.Translate(new Vector3(p.OpenSpeed * dir.x, p.OpenSpeed * dir.y));	
				yield return 0;
			}

			print("Finished opening");
			isOpening = false;
			yield return null;
		}

	}

	IEnumerator closePanel(DoorPanel p){
		if(isOpening){
			isOpening = false;
		}

		isClosing = true;
		Vector2Int dir = enum2dir(p.OpenDirection);
		while(Vector3.Distance(p.Panel.transform.position, p.startingPosition) > DISTANCE_OFFSET){
            print(Vector3.Distance(p.Panel.transform.position, p.startingPosition));
			p.Panel.transform.Translate(new Vector3(p.OpenSpeed * -dir.x, p.OpenSpeed * -dir.y));	
			yield return 0;
		}
		print("Finished closing");
		isClosing = false;
		yield return null;

	}

	Vector2Int enum2dir(Direction d){
		Vector2Int dir = new Vector2Int(0,0);
		switch(d){
			case Direction.Up:
				dir.y = 1;
				break;
			case Direction.Down:
				dir.y = -1;
				break;
			case Direction.Right:
				dir.x = 1;
				break;
			case Direction.Left:
				dir.x = -1;
				break;
		}

		return dir;
	}

	void SetStartingPositions(){
		for(int i = 0; i < Panels.Length; i++){
			Panels[i].startingPosition = Panels[i].Panel.transform.position;
		}
	}
}
