using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InnerHealthBarController : MonoBehaviour
{
	private IList<GameObject> healthBarFragments;

	private void Start()
	{
		healthBarFragments = new List<GameObject>();
		for (int i = 0; i < transform.childCount; i++)
		{
			healthBarFragments.Add(transform.GetChild(i).gameObject);
		}

		Debug.Log("Health bar inner fragments count: " + healthBarFragments.Count);
	}

	public void ShowHealthBarFragments(int numberOfFragments)
	{
		Debug.Log("Showing fragments: " + numberOfFragments);

		for (int i = 0; i < healthBarFragments.Count; i++)
		{
			if (i < numberOfFragments)
			{
				healthBarFragments[i].renderer.enabled = true;
			}
			else 
			{
				healthBarFragments[i].renderer.enabled = false;
			}
		}
	}
}

