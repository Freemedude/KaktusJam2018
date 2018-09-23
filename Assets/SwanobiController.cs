using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwanobiController : MonoBehaviour 
{
	public Transform point1;
	public Transform point2;

	private Vector3 p1;
	private Vector3 p2;


	// The point we're currently targeting
	private Vector3 currentDestination;

	// Movementspeed per seccond
	public float movementSpeed;

	private void Start()
	{
		transform.position = p2;
		p1 = point1.position;
		p2 = point2.position;
	}

	public void MoveToPoint1()
	{
		StartCoroutine("MoveToPoint", p1);
	}

	public void MoveToPoint2()
	{
		StartCoroutine("MoveToPoint", p2);
	}

	private IEnumerable MoveToPoint(Vector3 dest)
	{
		while(true)
		{
		
		// Distance we can move this frame
			var moveDist = movementSpeed * Time.deltaTime;
			
			// x value we have to move
			var xDiff = currentDestination.x - transform.position.x;

			// Distance left to move
			var remainingDist = Mathf.Abs(xDiff);

				// Move the desired distance
			var direction = new Vector2(
				xDiff,
				0);
			if(moveDist > remainingDist)
			{
				transform.Translate (direction);
				break;
			}
			else
			{
				transform.Translate(direction.normalized * movementSpeed * Time.deltaTime);
			}
			yield return null;
		}
	}
}
