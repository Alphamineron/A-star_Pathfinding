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

	public Node NodeFromWorldPoint(Vector3 worldPos) {
		float percentX = (worldPos.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPos.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);		// Incase worldPos is outside the grid, this is to avoid getting invalid index
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return Grid[x,y];
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
