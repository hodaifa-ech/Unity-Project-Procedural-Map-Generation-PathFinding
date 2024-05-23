using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Transform target;
	float speed = 5;
	Vector3[] path;
	int targetIndex;



    private void Start()
    {
		
		// Find the GameObject with the tag "MiniBoss"
		GameObject miniBossGameObject = GameObject.FindWithTag("MiniBoss");

		if (miniBossGameObject != null)
		{
			target = miniBossGameObject.GetComponent<Transform>();
		}
		else
		{
			Debug.Log("MiniBoss GameObject not found!");
		}
		
    }



    public void Pathfind()
    {
		if (target)
			PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
		else
			Debug.Log("target Not Found !"); 
	}

   


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath() {

        if (path == null || path.Length == 0)
        {
            yield break; // Exit the coroutine if path is null or empty
        }


        Vector3 currentWaypoint = path[0];
		
		while (true) {
			if (transform.position == currentWaypoint) {
				targetIndex ++;
				if (targetIndex >= path.Length) {
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			// if click the controller inputs stop the player 
			if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
			{
				break;
            }
            transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;

		}
	}

	public void OnDrawGizmos() {
		if (path != null) {
			for (int i = targetIndex; i < path.Length; i ++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else {
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
