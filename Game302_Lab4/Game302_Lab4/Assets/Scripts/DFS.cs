﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour {
    public GameObject[] nodes;    
    List<int>[] adjacencyList;
    
    void Start()
    {
        // create an adjacencyList as List<int>[]
        adjacencyList = new List<int>[nodes.Length];
        // create a list to contain the connected nodes
        // Node 0
        adjacencyList[0] = new List<int>();
        adjacencyList[0].Add(1); 
        adjacencyList[0].Add(2); 
        
        // Node 1
        adjacencyList[1] = new List<int>();
        adjacencyList[1].Add(3);
        adjacencyList[1].Add(4);
            
        // Node 2
        adjacencyList[2] = new List<int>();
        adjacencyList[2].Add(5);        

        // Node 3
        adjacencyList[3] = new List<int>();
        adjacencyList[3].Add(6);
            
        // Node 4
        adjacencyList[4] = new List<int>();

        // Node 5
        adjacencyList[5] = new List<int>();
        adjacencyList[5].Add(7);
        adjacencyList[5].Add(8);

        // Node 6
        adjacencyList[6] = new List<int>();        

        // Node 7
        adjacencyList[7] = new List<int>();
        adjacencyList[7].Add(9);

        // Node 8
        adjacencyList[8] = new List<int>();        

        // Node 9
        adjacencyList[9] = new List<int>();

        // Do DFS
        StartCoroutine( DoDepthFirstSearch());

    }

    IEnumerator DoDepthFirstSearch()
    {
        List<int> visited = new List<int>();
        // To do: Create an empty stack S
        Stack<int> s = new Stack<int>();
        // To do: Push root(0) to stack S
        s.Push(0);

        // To do: while S is not empty
        while(s.Count > 0)
        {
            // To do: Pop one from stack S and assign it to v
            int v = s.Pop();
            //int v = 0;
            
            if (!visited.Contains(v))
            {
                Debug.Log("Visited " + v);
                // Change the colour of the visited node to green
                nodes[v].GetComponent<SpriteRenderer>().color = Color.green;
                yield return new WaitForSeconds(0.5f);

                foreach (int adjacent in GetConnectedNodes(v, true))
                {
                    // To do: Push the adjacent nodes to stack S
                    s.Push(adjacent);
                }
            }
        }
        yield return null;
    }
    
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < adjacencyList.GetLength(0); i++)
        {
            foreach (int connectedNode in GetConnectedNodes(i))
            {
                Debug.DrawLine(nodes[i].transform.position, nodes[connectedNode].transform.position);
            }
        }
    }

    List<int> GetConnectedNodes(int fromNode, bool reversed = false)
    {
        List<int> connectedNodes = null;        
        connectedNodes = adjacencyList[fromNode];

        if (reversed)
        {
            connectedNodes.Reverse();
        }

        return connectedNodes;
    }
}
