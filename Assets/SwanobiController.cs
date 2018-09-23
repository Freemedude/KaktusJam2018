using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwanobiController : MonoBehaviour 
{
	public Transform outsideScreenTransform;

	private Vector3 outsideScreenPoint;

	public bool shouldWalk;

	// The point we're currently targeting
	private Vector3 currentDestination;

	// Movementspeed per seccond
	public float movementSpeed;

	private void Start()
	{
		outsideScreenPoint = outsideScreenTransform.position;
	}

    internal void WalkOut()
    {
		currentDestination = outsideScreenPoint;

    }

    private void Update()
	{
		if(!shouldWalk)
		{
			return;
		}

		// Distance we can move this frame
		var moveDist = movementSpeed * Time.deltaTime;
		
		// x value we have to move
		var xDiff = currentDestination.x - transform.position.x;

		// Distance left to move
		var remainingDist = Mathf.Abs(xDiff);
		
		var vectorToPoint = new Vector2(
			xDiff,
			0
		);

		// If we are close enough to overshoot the target
		if(moveDist > remainingDist)
		{			
			// Put us at target dest and get next target
			transform.Translate (vectorToPoint);
			shouldWalk = false;
		}

		else
		{
			// Move the desired distance
			transform.Translate(vectorToPoint.normalized * movementSpeed * Time.deltaTime);
		}
		
	}
}
