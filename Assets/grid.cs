﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] Grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start() {
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		Grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for(int x=0; x<gridSizeX; x++) {
			for(int y=0; y<gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x*nodeDiameter + nodeRadius) + Vector3.forward * (y*nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				Grid[x,y] = new Node(walkable, worldPoint);
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if(Grid != null) {
			foreach(Node n in Grid) {
				Gizmos.color = (n.walkable)? Color.white : Color.red;
				Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter-0.1f));
			}
		}
	}
}