using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugCat : Enemy
{
	[SerializeField]
	private float actionTimeout = 3f;
	
	private float timeleft;

	private void Update ()
	{
		CheckIfDead ();

		timeleft -= Time.deltaTime;

		if (timeleft < 0) {
			Action ();
			timeleft = actionTimeout;
		}

		Move ();
	}

	private void Action ()
	{
		ToggleFacing ();
	}
}
