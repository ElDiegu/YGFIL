using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.EventSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YGFIL.Events;
using YGFIL.Utils;

namespace YGFIL.Managers.Minigames
{
    public class MazeManager : MonoBehaviour
    {
        [SerializeField] private int height, width;
        [SerializeField] private Vector2Int startVector;
        [SerializeField] private Vector2Int endVector;
        public (int, int) start, end;
        
        [SerializeField] private int maxSteps = 100;
        List<List<int>> maze = new List<List<int>>();
        [SerializeField] private Transform mazeTransform;
        
        [SerializeField] private List<(int, int)> visited = new List<(int, int)>();
        
        public void GenerateMaze() 
        {
            maze.Clear();
            visited.Clear();
            
            for (int i = 0; i < height; i++) 
            {
                var row = new List<int>();
                for (int j = 0; j < width; j++) 
                {
                    // Wall
                    if (i % 2 == 0 && j % 2 == 0) row.Add(2);
                    // Can have a wall or an opening
                    else if(i % 2 == 0 && j % 2 != 0 || i % 2 != 0 && j % 2 == 0) row.Add(1);
                    // Always Open
                    else if(i % 2 != 0 && j % 2 != 0) row.Add(0);
                }
                maze.Add(row);
            }
            
            start = (startVector.x, startVector.y);
            end = (endVector.x, endVector.y);
            
            var currentNode = start;
            
            visited.Add(currentNode);
            maze[start.Item1][start.Item2] = 0;
            
            FindSolution(start, visited);
            
            CellSetup();
        }
        
        private void CellSetup() 
        {
            foreach(var node in visited) maze[node.Item1][node.Item2] = 0;
            
            for (int i = 0; i < height; i++) 
            {
                for (int j = 0; j < width; j++) 
                {
                    var cell = GetChildFromNode((i, j));
                    var color = Color.white;
                    var tag = "ForbiddenCell";    
                    
                    switch(maze[i][j]) 
                    {
                        case 0:
                            tag = "AllowedCell";
                            color = Color.blue;
                            break;
                        case 1:
                            color = Color.red;
                            break;
                        case 2:
                            color = Color.gray;
                            break;
                    }
                    
                    cell.tag = tag;
                    cell.GetComponent<Image>().color = color;
                }
            }
            
            EventBus<GeneratedMazeEvent>.Raise(new GeneratedMazeEvent() 
            {
                startCell = GetChildFromNode(start).transform as RectTransform
            });
        }
        
        private bool CheckValid((int, int) node, List<(int, int)> visited) 
        {
            return node.Item1 >= 0 
                && node.Item2 >= 0 
                && node.Item1 < height 
                && node.Item2 < width 
                && !visited.Contains(node)
                && maze[node.Item1][node.Item2] != 2;
        }

        private bool FindSolution((int, int) currentNode, List<(int, int)> visited) 
        {
            if (currentNode == end) return true;
            
            var isSol = false;
            var steps = 0;
            
            List<int> directions = new List<int>(){0, 1, 2};
            
            while (steps < maxSteps && directions.Count > 0) 
            {
                steps++;
                
                var direction = currentNode.Item2 == width - 1 ? MazeDirections.Down : (MazeDirections)directions[Random.Range(0, directions.Count)];
                
                //if (!directions.Contains((int)direction)) continue;
                
                (int, int) nextNode = (-1, -1);
                
                switch(direction) 
                {
                    case MazeDirections.Up:
                        nextNode = (currentNode.Item1 - 1, currentNode.Item2);
                        break;
                    case MazeDirections.Down:
                        nextNode = (currentNode.Item1 + 1, currentNode.Item2);
                        break;
                    case MazeDirections.Right:
                        nextNode = (currentNode.Item1, currentNode.Item2 + 1);
                        break;
                }
                
                directions.Remove((int)direction);
                
                if (!CheckValid(nextNode, visited)) continue;
                
                visited.Add(nextNode);
                
                isSol = FindSolution(nextNode, visited);
                
                if (isSol) return true;
                
                visited.Remove(nextNode);
            }
            
            return isSol;
        }
        
        public GameObject GetChildFromNode((int, int) index) 
        {
            return mazeTransform.GetChild(index.Item2 + index.Item1 * width).gameObject;
        }
        
        public (int, int) GetNodeFromChildIndex(int index) 
        {
            return (Mathf.FloorToInt(index / width), Mathf.FloorToInt(index % width));
        }
        
        public bool IsAdjacent((int, int) node1, (int, int) node2) 
        {
            return Mathf.Abs(node1.Item1 - node2.Item1) <= 1 || Mathf.Abs(node2.Item2 - node2.Item2) <= 1;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MazeManager))]
    public class MazeManagerEditor : Editor 
    {
        public override void OnInspectorGUI() 
        {
            base.OnInspectorGUI();
            
            var manager = (MazeManager)target;
            
            if (GUILayout.Button("Generate Maze")) 
            {
                if (Application.isPlaying) manager.GenerateMaze();
            }
        }
    }
#endif
    
    public enum MazeDirections 
    {
        Up,
        Down,
        Right
    }
}
