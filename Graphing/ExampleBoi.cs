using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphing;

public class ExampleBoi : MonoBehaviour
{
    float[] values = new float[200];
    string[] labels = new string[200];

    Graph graph;

    private void Start()
    {
        GraphIt.CreateGraph(gameObject, out graph, "yeet");

        values = new float[10];
        labels = new string[10];

        for (int i = 0; i < labels.Length; i++)
        {
            values[i] = (int)labels.Length - i;
            labels[i] = $"lbl{i + 1}";
        }

        //StartCoroutine(randomize());
    }


    private void Update()
    {
        graph.SetValues(GraphIt.FormatData(values, labels), GraphStyle.DefaultPredict);
    }

    IEnumerator randomize()
    {
        while (true)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Random.Range(0, 11);
            }

            yield return new WaitForSeconds(5);
        }
    }

}
