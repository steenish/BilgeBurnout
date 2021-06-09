using System.Collections.Generic;
using UnityEngine;

public enum ClothShape {
    Triangular = 3,
    Quadratic
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WindCloth : MonoBehaviour {

 //   private struct ClothVertex {
 //       public ClothVertex(Vector3 position, bool pinned) {
 //           this.position = position;
 //           this.pinned = pinned;
 //           connections = int[];
	//	}

 //       public Vector3 position;
 //       public bool pinned;
 //       public int[] connections;
	//}

    [SerializeField]
    [Range(1, 10)]
    private int numCells = 1;
    [SerializeField]
    private ClothShape clothShape = ClothShape.Quadratic;

    //private ClothVertex[] cloth;
    private int numPointsPerAxis;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Transform[] boundsPoints;

    private void Start() {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        List<Transform> tempBoundsPoints = new List<Transform>();
        foreach (Transform child in transform) {
            tempBoundsPoints.Add(child);
		}
        boundsPoints = tempBoundsPoints.ToArray();
        numPointsPerAxis = numCells + 1;

        meshFilter.sharedMesh = ConstructMesh();
        meshFilter.sharedMesh.RecalculateNormals();
    }

    private Mesh ConstructMesh() {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(numCells + 1) * (numCells + 1)];
        int[] triangles = new int[2 * 6 * numCells * numCells];
        //List<ClothVertex> clothVertices = new List<ClothVertex>();

        if (clothShape == ClothShape.Quadratic) {
            for (int i = 0; i < numPointsPerAxis; ++i) {
                float ty = (float) i / numCells;
                for (int j = 0; j < numPointsPerAxis; ++j) {
                    float tz = (float) j / numCells;
                    Vector3 intermediate1 = Vector3.Lerp(boundsPoints[0].position, boundsPoints[1].position, tz);
                    Vector3 intermediate2 = Vector3.Lerp(boundsPoints[2].position, boundsPoints[3].position, tz);
                    Vector3 vertex = Vector3.Lerp(intermediate1, intermediate2, ty);
                    vertices[j + i * numPointsPerAxis] = vertex;

                    //ClothVertex clothVertex = new ClothVertex(vertex, false);

     //               if (IsBounds(i, j)) {
     //                   clothVertex.pinned = true;
					//}
				}
			}
            int triangleOffset = 0;
            for (int i = 0; i < numCells; ++i) {
                for (int j = 0; j < numCells; ++j) {
                    int cellOffset = i * numPointsPerAxis + j;

                    // Lower left triangle of grid cell (clockwise assigned).
                    triangles[triangleOffset++] = cellOffset;
                    triangles[triangleOffset++] = cellOffset + numPointsPerAxis;
                    triangles[triangleOffset++] = cellOffset + 1;

                    // Upper right triangle of grid cell (clockwise assigned).
                    triangles[triangleOffset++] = cellOffset + 1;
                    triangles[triangleOffset++] = cellOffset + numPointsPerAxis;
                    triangles[triangleOffset++] = cellOffset + numPointsPerAxis + 1;

					// Lower left triangle of grid cell (counter-clockwise assigned).
					triangles[triangleOffset++] = cellOffset;
					triangles[triangleOffset++] = cellOffset + 1;
					triangles[triangleOffset++] = cellOffset + numPointsPerAxis;

					// Upper right triangle of grid cell (counter-clockwise assigned).
					triangles[triangleOffset++] = cellOffset + 1;
					triangles[triangleOffset++] = cellOffset + numPointsPerAxis + 1;
					triangles[triangleOffset++] = cellOffset + numPointsPerAxis;
				}
            }
		}

		mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.name = "ClothMesh";
        return mesh;
	}

    private bool IsBounds(int i, int j) {
        return (j == 0 || j == numCells) && (i == 0 || i == numCells);
	}
}
