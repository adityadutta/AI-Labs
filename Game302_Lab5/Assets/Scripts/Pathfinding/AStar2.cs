using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding {
	public class AStar2 {
		public Tilemap map;

		List<Node> visited = new List<Node>();
		List<Node> unvisited = new List<Node>();

		Dictionary<Node, Node> predecessorDict = new Dictionary<Node, Node>();
		Dictionary<Node, float> fDistanceDict = new Dictionary<Node, float>();
		Dictionary<Node, float> gDistanceDict = new Dictionary<Node, float>();

		public void Init(Tilemap tileMap)
		{
			map = tileMap;
		}
		
		public List<Node> Search(Node start, Node goal)
		{
            visited.Clear();
            unvisited.Clear();
            fDistanceDict.Clear();
            gDistanceDict.Clear();

            // 1. dist[s] = 0
            // 2. set all other distances to infinity
            List<Node> nodes = new List<Node>();
            nodes = map.GetAllNodes();

            foreach (Node node in nodes)
			{
                fDistanceDict[node] = float.MaxValue;
                gDistanceDict[node] = float.MaxValue;
			}
            fDistanceDict[start] = 0;
            gDistanceDict[start] = 0;

            // 3. Initialize S(visited) and Q(unvisited)
            //    S, the set of visited nodes is initially empty
            //    Q, the queue initially conatains all nodes
            // To do: Initialize visited and unvisited
            unvisited = new List<Node>(nodes);

            predecessorDict.Clear(); // to generate the result path
			
			while (unvisited.Count > 0)
			{
				// 4. select element of Q with the minimum distance
            	// To do: Get a closest node from the unvisited list
            	// Node u = ?
            	Node u = GetClosestFromUnvisited();

				// Check if the node u is the goal.            
				if (u == goal) break;

                // 5. add u to list of S(visited)            
                // To do: add u to the visited list
                visited.Add(u);

                foreach (Node v in map.GetNeighbors(u))
				{
					if (visited.Contains(v))
						continue;

                    // 6. If new shortest path found then set new value of shortest path                
                    // To do: update fDistanceDict[v], gDistanceDict[v] and predecessorDict[v]
                    float w = Vector2.Distance(new Vector2(u.x, u.y), new Vector2(v.x, v.y));
                    if (fDistanceDict[v] > gDistanceDict[u] + w + GetEstimatedDistance(v, goal))
                    {
                        fDistanceDict[v] = gDistanceDict[u] + w + GetEstimatedDistance(v, goal);
                        gDistanceDict[v] = gDistanceDict[u] + w;
                    }


                    predecessorDict[v] = u;

                }
			}

			List<Node> path = new List<Node>();

			// Generate the shortest path
			path.Add(goal);
			Node p = predecessorDict[goal];
			
			while (p != start)
			{
				path.Add(p);            
				p = predecessorDict[p];        
			}

			path.Reverse();
			return path;
		}
		
		Node GetClosestFromUnvisited()
		{
			float shortest = float.MaxValue;
			Node shortestNode = null;
			foreach (var node in unvisited)
			{
				if (shortest > fDistanceDict[node])
				{
					shortest = fDistanceDict[node];
					shortestNode = node;
				}
			}
			
			unvisited.Remove(shortestNode);
			return shortestNode;
		}

        float GetEstimatedDistance(Node node1, Node node2)
        {
            // To do : Calculate the estimated distance
            // The direct distance can be used as an estimated value
            float dist = 0;
            dist = Vector2.Distance(new Vector2(node1.x, node1.y), new Vector2(node2.x, node2.y));
            return dist;
        }
    }
}