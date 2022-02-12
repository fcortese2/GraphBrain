using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Graphing.Editor;
using SimpleColorPicker;
using Graphing;
using System.Linq;

[System.Serializable]
public class Graph: MonoBehaviour
{
    /// <summary>
    /// DO NOT EDIT DIRECTLY! GO THROUGH SetValues()
    /// </summary>
    public GraphValue[] graphElements;


    /// <summary>
    /// DO NOT EDIT DIRECTLY!
    /// </summary>
    public Queue<GraphValue> graphMemoryBuffer;

    /// <summary>
    /// Change this whenever you want to alter the appearance of your graph.
    /// </summary>
    public GraphStyle style;

    /// <summary>
    /// The name that appears on the graph's header
    /// </summary>
    public string graphName;
    
    /// <summary>
    /// Set points ready for plotting
    /// </summary>
    /// <param name="points">Points to plot</param>
    public void SetValues(GraphValue[] points, GraphStyle graphStyle)
    {
    #if UNITY_EDITOR

        if (graphStyle.liveItemAdd == true)
        {
            if (graphMemoryBuffer == null)
            {
                graphMemoryBuffer = new Queue<GraphValue>();
            }

            graphMemoryBuffer.Enqueue(points[0]);
            if (graphMemoryBuffer.Count > graphStyle.liveItemBuffer)
            {
                graphMemoryBuffer.Dequeue();
            }


            graphElements = graphMemoryBuffer.ToArray();
        }
        else
        {
            graphElements = points;
        }

        style = graphStyle;

    #endif
    }

    public GraphValue[] GenerateSingleElementArray(float value, string tag)
    {
        GraphValue val = new GraphValue()
        {
            Value = value,
            Tag = tag
        };

        return new GraphValue[] { val };
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Graph))]
public class BaseGraphDrawer : Editor
{
    private bool barGraphDraw = true;
    Color[] colors;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        GUILayout.Space(1f);

        Graph graph = (Graph)target;

        if (graph.graphElements != null)
        {
            if (colors == null || colors.Length != graph.graphElements.Length)
            {
                colors = new Color[graph.graphElements.Length];
                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] = ColorPicker.nextColor();
                }
            }

            GraphDrawer.DrawGraph(graph.graphElements, colors, graph.style, ref barGraphDraw, graph.graphName);
        }
    }

}
#endif