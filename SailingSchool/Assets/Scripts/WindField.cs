using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour {

    public float globalContribution;
    public float globalWindAttrition;
    public float maxWindStrength;
    public float minWindStrength;
    public float size;
    public int resolution;


    private Vector2 globalWind;
    private Vector2[,] grid;


    public Vector2 SampleWind(Vector3 position) {
        return Vector2.zero;
    }

    private void Awake() {
        grid = new Vector2[resolution, resolution];
        maxWindStrength = Mathf.Abs(maxWindStrength);
        minWindStrength = Mathf.Abs(minWindStrength);
        maxWindStrength = Mathf.Max(minWindStrength, maxWindStrength);
        minWindStrength = Mathf.Min(minWindStrength, maxWindStrength);
        globalWind = Random.insideUnitCircle.normalized * Random.Range(minWindStrength, maxWindStrength);
    }

    private void Update() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        globalWind = new Vector2(horizontal, vertical);

        for (int i = 0; i < resolution; ++i) {
            for (int j = 0; j < resolution; ++j) {
                grid[i, j] -= grid[i, j] * globalWindAttrition * Time.deltaTime;
                grid[i, j] += globalWind * globalContribution * Time.deltaTime;
                Utility.DrawHorizontalArrow(Utility.ToHorizontalVector(transform.position) - size * new Vector2(1, 1) + i * new Vector2(1, 0) * resolution / size + j * new Vector2(0, 1) * resolution / size, grid[i, j]);
            }
        }
    }
}
