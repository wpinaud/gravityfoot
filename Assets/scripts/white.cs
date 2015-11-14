using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 * This is the script to generate the world of the GravityFoot
 * 
 * This is a foot game:
 * 	Put a maximum of ballon in the area !
 * 
 * Enjoy !
 */

public class white : MonoBehaviour {
	
	public Text countText; 

	public GameObject CubeCreator;
	public Texture cube_tex;
	GameObject[,,] CubeArray = new GameObject[3,3,3];


	//if a position is already visited by the visit() function: the cell will be "true"
	bool[,,] visited = new bool[3,3,3]; 

	//A list with all the black cube
	List<GameObject> blackArray = new List<GameObject>();

	//A way to instanciate all the possible spheres (14 at maximum)
	GameObject[] spheres = new GameObject[14];
	public Texture sphere_tex;

	//The output list of the test you gave me, this time I doesn't take the diagonals neighbors =)
	List<List<GameObject>> outputList = new List<List<GameObject>>();

	//The mean is the center of gravity of a black cube's group
	//This is the position where a sphere will be put
	Vector3 mean = new Vector3(0, 0, 0);

	void Start ()
	{	
		//The base sphere primitive
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//Far far away
		sphere.transform.position = new Vector3 (-300,-300,-300);
		sphere.transform.localScale = new Vector3 (1.3f, 1.3f, 1.3f);

		for (var i=0; i<14; i++) { //maximum 14 spheres in the world
			spheres[i] = (GameObject)Instantiate (sphere);
			spheres[i].GetComponent<Renderer>().material.SetTexture("_MainTex",sphere_tex);
		}

		//and this create the cube of cube
		for(int x=0; x<3; x++)
		{
			for(int y=0; y<3; y++)
			{  
				for(int z=0; z<3; z++)
				{  
					visited[x,y,z] = false; //initialization of the visited array
					CubeArray[x,y,z] = (GameObject)Instantiate (CubeCreator, new Vector3(x*2,y*2, z*2), transform.rotation);
					CubeArray[x,y,z].GetComponent<Renderer>().material.SetTexture("_MainTex",cube_tex);
				}
			}
		}

	}

	void Update()
	{
		//Change the color on click
		if( Input.GetMouseButtonDown(0) )
		{
			Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit hit;
			
			if( Physics.Raycast( ray, out hit, 100 ) )
			{
				GameObject currentGridPiece = hit.transform.gameObject;
				if (currentGridPiece.GetComponent<Renderer>().material.color == Color.black){
					currentGridPiece.GetComponent<Renderer>().material.color = Color.white;
				}
				else{
					currentGridPiece.GetComponent<Renderer>().material.color = Color.black;
				}
			
			}
		}


		//Visit all the neighbors
		foreach (GameObject cube in CubeArray){
			if (cube.GetComponent<Renderer>().material.color == Color.black && !visited[(int) cube.transform.position.x/2,(int) cube.transform.position.y/2, (int)cube.transform.position.z/2]){
				visited[(int) cube.transform.position.x/2,(int) cube.transform.position.y/2, (int)cube.transform.position.z/2]=true;
				blackArray.Add(cube);
				visit(cube.transform.position); //visit all the neighbor recursivly
				//and add the array of neighbors to the outputList
				outputList.Add(blackArray);
				//reinitialize the list of neighbor
				blackArray = new List<GameObject>();
			}
		}

			for(int x=0; x<3; x++)
			{
				for(int y=0; y<3; y++)
				{  
					for(int z=0; z<3; z++)
					{  
						visited[x,y,z] = false;
					}
				}
			}

		//draw spheres
		int c = 0;

		foreach (List<GameObject> array in outputList) {
			mean = new Vector3(0,0,0);
			int count = 0;
			foreach (GameObject cube in array) {
				mean += cube.transform.position;
				count++;
			}
			mean = mean / count;
			//draw the sphere where at the center of gravity 
			spheres[c].transform.position = mean;
			c++;
		}

		//move the unused sphere
		for (int i=c; i<14; i++) {
			spheres[i].transform.position = new Vector3 (-300,-300,-300);
		}

		outputList.Clear (); //reinitialize the outputList

		//a small text to know your score
		countText.text = "Your score is: " + c.ToString ();
		if (c == 14)
		{
			countText.text = "You Win!!!!!!";
		}

		}

	void visit(Vector3 position){

		List<Vector3> neighborPosVec = new List<Vector3>();
		neighborPosVec.Add (new Vector3 (2, 0, 0));
		neighborPosVec.Add (new Vector3 (-2, 0, 0));
		neighborPosVec.Add (new Vector3 (0, 2, 0));
		neighborPosVec.Add (new Vector3 (0, -2, 0));
		neighborPosVec.Add (new Vector3 (0, 0, 2));
		neighborPosVec.Add (new Vector3 (0, 0, -2));

		foreach (GameObject cube1 in CubeArray){
			foreach (Vector3 neighborPos in neighborPosVec){
				if (cube1.transform.position == position + neighborPos){
					if (cube1.GetComponent<Renderer>().material.color == Color.black && !visited[(int) cube1.transform.position.x/2,(int) cube1.transform.position.y/2, (int)cube1.transform.position.z/2]){
						visited[(int) cube1.transform.position.x/2,(int) cube1.transform.position.y/2, (int)cube1.transform.position.z/2]=true;
						blackArray.Add(cube1);
						visit(cube1.transform.position);
					}
				}
			}


		}
	}


	}
