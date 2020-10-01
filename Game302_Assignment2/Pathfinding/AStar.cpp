#include "AStar.h"
#include "Map.h"
#include "SDL.h"
#include <iostream>

AStar::AStar(Map* m)
	:map(m),
	isSearching(false)
{
	graph = m->GetGraph();
}

AStar::~AStar()
{
}

bool AStar::IsSearching()
{
	return isSearching;
}

void AStar::Search(Node* start, Node* goal)
{
	isSearching = true;
	startNode = start;
	goalNode = goal;

	thread = SDL_CreateThread(SearchThread, "", this);
}

void AStar::OnSearchDone()
{
	isSearching = false;

	// Draw the shortest path
	for (auto p : pathFound)
	{
		map->SetPathMap(p->position, Map::RESULT_PATH_FOUND); // the second param value '2' means that it will draw
	}

}

int AStar::SearchThread(void * data)
{
	AStar* astar = static_cast<AStar*>(data);

	if (!astar->startNode || !astar->goalNode)
	{
		astar->OnSearchDone();
		return 0;
	}

	//clear the distance dicts
	astar->distanceDict.clear();
	astar->actualDistanceDict.clear();

	// To do: Complete this function.
	 // 1. dist[s] = 0
		// 2. set all other distances to infinity
	std::vector<Node>& nodes = astar->graph->GetAllNodes();
	for (auto& node : nodes)
	{
		astar->ValidateDistanceDict(&node);
	}

	astar->distanceDict[astar->startNode] = 0.0f;
	astar->actualDistanceDict[astar->startNode] = 0.0f;



	// 3. Initialize S(visited) and Q(unvisited)
	//    S, the set of visited nodes is initially empty
	//    Q, the queue initially contains all nodes
	// To do: Initialize visited and unvisited
	astar->visited.clear();
	astar->unvisited.clear();

	for (auto& node : nodes)
	{
		astar->unvisited.push_back(&node);
	}

	astar->predecessorDict.clear(); // to generate the result path

	while (astar->unvisited.size() > 0)
	{
		// 4. select element of Q with the minimum distance
		// To do: Get a closest node from the unvisited list
		// Node u = ?
		Node* u = astar->GetClosestFromUnvisited();

		astar->map->SetPathMap(u->position, Map::SEARCH_IN_PROGRESS);
		SDL_Delay(200.0f);

		// Check if the node u is the goal.
		if (u == astar->goalNode) break;

		// 5. add u to list of S(visited)
		// To do: add u to the visited list
		astar->visited.push_back(u);

		for (Node* v : astar->graph->GetAdjacentNodes(u))
		{
			if (std::find(astar->visited.begin(), astar->visited.end(), v) != astar->visited.end())
				continue;

			// 6. If new shortest path found then set new value of shortest path
			// To do: update fDistanceDict[v] and fDistanceDict[v]
			// if f_dist[v] > g_dist[u] + w(u,v) + h(v,G) then 
			//      f_dist[v] = g_dist[u] + w(u, v) + h(v, G)
			//		g_dist[v] = g_dist[u] + w(u, v)
			//		update predecessorDict to build the result path
			float w = astar->graph->GetDistance(u, v);
			if (astar->distanceDict[v] > astar->actualDistanceDict[u] + w + astar->graph->GetDistance(v, astar->goalNode))
			{
				astar->distanceDict[v] = astar->actualDistanceDict[u] + w + astar->graph->GetDistance(v, astar->goalNode);
				astar->actualDistanceDict[v] = astar->actualDistanceDict[u] + w;
			}


			astar->predecessorDict[v] = u;
		}
	}

	std::vector<Node*> path = std::vector<Node*>();
	path.clear();

	path.push_back(astar->goalNode);
	Node* p = astar->predecessorDict[astar->goalNode];
	int count = 200;
	while (p != astar->startNode)
	{
		path.push_back(p);
		p = astar->predecessorDict[p];

		if (count < 0) break;
		count--;
	}

	std::reverse(path.begin(), path.end());

	astar->pathFound = path;


	astar->OnSearchDone();
	return 0;
}

Node * AStar::GetClosestFromUnvisited()
{

	float shortest = std::numeric_limits<float>::max();
	Node* shortestNode = nullptr;

	//Complete this function.
	for (auto node : unvisited)
	{
		if (shortest > distanceDict[node])
		{
			shortest = distanceDict[node];
			shortestNode = node;
		}
	}

	//Remove from vector list
	auto it = std::find(unvisited.begin(), unvisited.end(), shortestNode);
	if (it != unvisited.end())
	{
		unvisited.erase(it);
	}

	return shortestNode;
}

void AStar::ValidateDistanceDict(Node * n)
{
	float max = std::numeric_limits<float>::max();
	if (distanceDict.find(n) == distanceDict.end())
	{
		distanceDict[n] = max;
	}
	if (actualDistanceDict.find(n) == actualDistanceDict.end())
	{
		actualDistanceDict[n] = max;
	}
}
