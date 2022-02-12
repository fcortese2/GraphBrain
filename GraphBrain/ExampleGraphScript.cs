using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphing;

public class ExampleGraphScript : MonoBehaviour
{
    float[] values = new float[200];
    string[] labels = new string[200];

    float trackedVar = 5;

    Graph graph;
    Graph liveGraph;

    private void Start()
    {
        GraphBrain.CreateGraph(gameObject, out graph, "Normal Graph");
        GraphBrain.CreateGraph(gameObject, out liveGraph, "Live Graph");


        values = new float[10];
        labels = new string[10];

        for (int i = 0; i < labels.Length; i++)
        {
            values[i] = (int)labels.Length - i;
            labels[i] = $"lbl{i + 1}";
        }

        StartCoroutine(randomize());
    }


    private void Update()
    {
        graph.SetValues(GraphBrain.FormatData(values, labels), GraphStyle.DefaultPredict);
    }

    IEnumerator randomize()
    {
        while (true)
        {
            trackedVar = Random.Range(1, 11);
            liveGraph.SetValues(liveGraph.GenerateSingleElementArray(trackedVar, "e.g."), GraphStyle.LiveAdd);
            yield return new WaitForSeconds(5);
        }
    }

}
