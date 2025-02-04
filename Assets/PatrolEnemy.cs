﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour 
{
	// The spriterenderer for the cat
	private SpriteRenderer spriteRenderer;

	// The two points we're going between
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
		p1 = point1.position;
		p2 = point2.position;

		// Get SpriteRenderer
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Set initial target
		currentDestination = p1;
	}

	// Update is called once per frame
	void Update () 
	{
		// Distance we can move this frame
		var moveDist = movementSpeed * Time.deltaTime;
		
		// x value we have to move
		var xDiff = currentDestination.x - transform.position.x;

		// Distance left to move
		var remainingDist = Mathf.Abs(xDiff);

		// If we are looking right: Unflip spriterenderer's X
		if(xDiff < 0)
		{
			spriteRenderer.flipX = false;
		}
		// If we are looking right: Flip spriterenderer's X
		else if( xDiff > 0)
		{
			spriteRenderer.flipX = true;
		}

		// If we are close enough to overshoot the target
		if(moveDist > remainingDist)
		{			
			// Put us at target dest and get next target
			var direction = new Vector2(
				xDiff,
				0
			);
			transform.Translate (direction);
			GetNextDestination();
		}

		else
		{
			// Move the desired distance
			var direction = new Vector2(
				xDiff,
				0
			).normalized;
			transform.Translate(direction * movementSpeed * Time.deltaTime);
		}
	}

	// Get the next destination
	void GetNextDestination()
	{
		currentDestination = currentDestination == p1 ? p2 : p1;
	}
}
