using System.Collections.Generic;
using UnityEngine;
using System.Linq;

class Dijkstra
{
    public List<Corner> FindShortestPath(Corner start, Corner end, Corner[,] corners, float porcentajeError)
    {
        Dictionary<Corner, int> distances = new Dictionary<Corner, int>();
        Dictionary<Corner, Corner> previousCorners = new Dictionary<Corner, Corner>();
        
        // Cambiamos a SortedDictionary para un acceso más eficiente al mínimo
        SortedDictionary<int, HashSet<Corner>> priorityQueue = new SortedDictionary<int, HashSet<Corner>>();

        foreach (var corner in corners)
        {
            distances[corner] = int.MaxValue; 
            if (!priorityQueue.ContainsKey(int.MaxValue))
            {
                priorityQueue[int.MaxValue] = new HashSet<Corner>();
            }
            priorityQueue[int.MaxValue].Add(corner);
        }
        distances[start] = 0;
        priorityQueue[int.MaxValue].Remove(start);
        priorityQueue[0] = new HashSet<Corner> { start };

        while (priorityQueue.Count > 0)
        {
            // Obtener el nodo con la menor distancia
            var minEntry = priorityQueue.First();
            Corner current = minEntry.Value.First();
            int currentDistance = minEntry.Key;

            // Eliminar el nodo actual de la cola
            minEntry.Value.Remove(current);
            if (minEntry.Value.Count == 0) priorityQueue.Remove(currentDistance);

            if (current == end)
            {
                return BuildPath(previousCorners, end);
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                int pesoOriginal = current.GetWeight(neighbor);
                int peso = IntroducirError(pesoOriginal, porcentajeError);

                int alt = distances[current] + peso;
                if (alt < distances[neighbor])
                {
                    if (priorityQueue.ContainsKey(distances[neighbor]))
                    {
                        priorityQueue[distances[neighbor]].Remove(neighbor);
                        if (priorityQueue[distances[neighbor]].Count == 0)
                        {
                            priorityQueue.Remove(distances[neighbor]);
                        }
                    }

                    distances[neighbor] = alt;
                    previousCorners[neighbor] = current;

                    if (!priorityQueue.ContainsKey(alt))
                    {
                        priorityQueue[alt] = new HashSet<Corner>();
                    }
                    priorityQueue[alt].Add(neighbor);
                }
            }
        }

        return new List<Corner>(); // Retornar lista vacía si no hay camino
    }

    private int IntroducirError(int peso, float porcentajeError)
    {
        if (porcentajeError == 0) return peso;

        if (Random.Range(0f, 100f) < porcentajeError)
        {
            int signo = Random.value < 0.5f ? -1 : 1;
            return Mathf.Max(peso - signo, 1); 
        }
        return peso;
    }

    private List<Corner> BuildPath(Dictionary<Corner, Corner> previousCorners, Corner end)
    {
        List<Corner> path = new List<Corner>();
        for (Corner at = end; at != null; at = previousCorners.ContainsKey(at) ? previousCorners[at] : null)
        {
            path.Add(at);
        }
        path.Reverse();
        return path;
    }

    private List<Corner> GetNeighbors(Corner corner)
    {
        return new List<Corner>(corner.GetNeighbors());
    }
}
