using UnityEngine;


public class keyboard : MonoBehaviour {
	public float distance = 9.0f;
	public float zoomSpd = 2.0f;
	
	public float xSpeed = 240.0f;
	public float ySpeed = 123.0f;
	
	public int yMinLimit = -723;
	public int yMaxLimit = 877;
	
	private float x = 22.0f;
	private float y = 33.0f;
	
	public void Start () {
		x = 22f;
		y = 33f;
	}
	
	public void LateUpdate () {
		x -= Input.GetAxis("Horizontal") * xSpeed * 0.02f;
		y += Input.GetAxis("Vertical") * ySpeed * 0.02f;
		
		y = ClampAngle(y, yMinLimit, yMaxLimit);
		
		Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + new Vector3(2,2,2);
		
		transform.rotation = rotation;
		transform.position = position;
	}
	
	public static float ClampAngle (float angle, float min, float max) {
		if (angle < -360.0f)
			angle += 360.0f;
		if (angle > 360.0f)
			angle -= 360.0f;
		return Mathf.Clamp (angle, min, max);
	}
}
