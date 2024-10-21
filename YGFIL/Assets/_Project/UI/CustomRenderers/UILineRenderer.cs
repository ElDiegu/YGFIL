using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Credits go to Sam Dunlap https://www.youtube.com/watch?v=x_CfGJh17Z8
/// </summary>
public class UILineRenderer : Graphic
{
    [field: SerializeField] public List<Vector2> points { get; private set;}
    [SerializeField] private float thickness = 10f;
    [SerializeField] private bool center;

    private float width, height;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2) return;
        
        for (int i = 0; i < points.Count - 1; i++) 
        {
            CreateLineSegment(points[i], points[i+1], vh);
            
            int index = i * 5;
            
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);
            
            if (i != 0) 
            {
                vh.AddTriangle(index, index - 1, index - 3);
                vh.AddTriangle(index + 1, index - 1, index - 2);
            }
        }

    }

    private void CreateLineSegment(Vector2 origin, Vector2 target, VertexHelper vh)
    {
        Vector2 offset = center ? (rectTransform.sizeDelta / 2) : Vector2.zero;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Quaternion point1rotation = Quaternion.Euler(0, 0, RotatePointTowards(origin, target) + 90);

        vertex.position = point1rotation * new Vector3(-thickness / 2, 0);
        vertex.position += (Vector3)(origin - offset);
        vh.AddVert(vertex);

        vertex.position = point1rotation * new Vector3(thickness / 2, 0);
        vertex.position += (Vector3)(origin - offset);
        vh.AddVert(vertex);

        Quaternion point2Rotation = Quaternion.Euler(0, 0, RotatePointTowards(target, origin) - 90);

        vertex.position = point2Rotation * new Vector3(-thickness / 2, 0);
        vertex.position += (Vector3)(target - offset);
        vh.AddVert(vertex);

        vertex.position = point2Rotation * new Vector3(thickness / 2, 0);
        vertex.position += (Vector3)(target - offset);
        vh.AddVert(vertex);

        vertex.position = target - offset;
        vh.AddVert(vertex);
    }
    private float RotatePointTowards(Vector2 vertex, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * (180 / Mathf.PI));
    }
    
    public void AddPointToLine(Vector2 position) 
    {
        if (!points.Contains(position)) points.Add(position);
        SetAllDirty();
    }
    
    public void RemovePointsFromLine(int startIndex, int numberOfElements) 
    {
        points.RemoveRange(startIndex, numberOfElements);
        SetAllDirty();
    }
}

