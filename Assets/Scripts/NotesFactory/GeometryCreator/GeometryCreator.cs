using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GeometryDeformation
{
    public float startIn;
    public float Xoffset;
}


public static class GeometryCreator  {

    private const float width = 0.5f;

    public static Mesh CreateNote(float duration, GeometryDeformation[] deformations)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices;

        if (deformations == null || deformations.Length == 0)
        {
            vertices = new Vector3[4];
            vertices[0] = Vector3.zero;
            vertices[1] = new Vector3(width, 0, 0);
            vertices[2] = new Vector3(0, duration, 0);
            vertices[3] = new Vector3(width, duration, 0);
        }
        else
        {
            vertices = new Vector3[2 + (deformations.Length * 2)];

            vertices[0] = Vector3.zero;
            vertices[1] = new Vector3(width, 0, 0);

            float lastX = 0;
            float lastY = 0;
            int currendIndex = 2;

            for (int i = 0; i < deformations.Length; i++)
            {

                float y = deformations[i].startIn + lastY;
                if (i + 1 >= deformations.Length)
                {
                    y = duration;
                }

                float x = deformations[i].Xoffset + lastX;
                
                vertices[currendIndex] = new Vector3(x,y,0);
                currendIndex++;
                vertices[currendIndex] = new Vector3(x + width, y, 0);
                currendIndex++;

                lastX = x;
                lastY = y;

            }
        }

        mesh.vertices = vertices;
        mesh.triangles = CalculateTriangles(vertices.Length);

        Vector3[] normals = new Vector3[vertices.Length];

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -Vector3.forward;
        }

        mesh.normals = normals;

        return mesh;
    }

    

    private static int[] CalculateTriangles(int verticesCount)
    {
        int number = verticesCount - 4;
        number = number / 2 * 6;

        int[] triangles = new int[6 + number];

        int startVertice = 0;
        int currentIndex = 0;

        for (int i = 0; i < triangles.Length; i += 6)
        {
            currentIndex = i;
            triangles[currentIndex] = startVertice;
            currentIndex++;
            triangles[currentIndex] = startVertice + 2;
            currentIndex++;
            triangles[currentIndex] = startVertice + 1;
            currentIndex++;


            triangles[currentIndex] = startVertice + 2;
            currentIndex++;
            triangles[currentIndex] = startVertice + 3;
            currentIndex++;
            triangles[currentIndex] = startVertice + 1;
            currentIndex++;
            

            startVertice += 2; 
        }



        return triangles;
    }

    public static Mesh CutGeometryAtPosition(Mesh currentMesh, Vector3 cutPosition)
    {
        Mesh newMesh = new Mesh();

        List<Vector3> vertex = new List<Vector3>();
        currentMesh.GetVertices(vertex);

        for (int i = 0; i < vertex.Count; i++)
        {
            if (vertex[i].y <= cutPosition.y)
            {
                vertex.RemoveAt(i);
                i--;
            }
        }

        List<Vector3> newVertices = new List<Vector3>();

        newVertices.Add(new Vector3(cutPosition.x, cutPosition.y, 0));
        newVertices.Add(new Vector3(cutPosition.x + width, cutPosition.y, 0));
        newVertices.AddRange(vertex);
        

        newMesh.vertices = newVertices.ToArray();

        newMesh.triangles = CalculateTriangles(newVertices.Count);

        Vector3[] normals = new Vector3[newVertices.Count];

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -Vector3.forward;
        }

        newMesh.normals = normals;

        return newMesh;
    }


    public static Mesh CreateTest ()
    {

        Mesh _mesh = new Mesh();

        Vector3[] geometry = new Vector3[6];

        geometry[0] = Vector3.zero;
        geometry[1] = new Vector3(width/2, 0, 0);
        geometry[2] = new Vector3(0, 5/2, 0);
        geometry[3] = new Vector3(width/2, 5/2, 0);

        geometry[4] = new Vector3(width/2, 5, 0);
        geometry[5] = new Vector3(width, 5, 0);


        _mesh.vertices = geometry;

        int[] triangles = new int[12];

        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;

        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        triangles[6] = 2;
        triangles[7] = 4;
        triangles[8] = 3;

        triangles[9] = 4;
        triangles[10] = 5;
        triangles[11] = 3;

        _mesh.triangles = triangles;

        Vector3[] normals = new Vector3[geometry.Length];

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -Vector3.forward;
        }

        return _mesh;
        
	}
}
