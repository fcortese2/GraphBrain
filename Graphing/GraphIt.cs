using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graphing.Editor;

namespace Graphing
{
    public class GraphIt
    {
        public static void CreateGraph(GameObject pointToObject, out Graph graph, string title = "Graph")
        {
            graph = pointToObject.AddComponent<Graph>();
            graph.graphName = title;
        }

        /// <summary>
        /// Prepare data for plotting on graph
        /// </summary>
        /// <param name="values">Values to plot</param>
        /// <param name="tags">Labels of values to plot</param>
        /// <returns>Generates a list of GraphValues which the graph can interpret</returns>
        /// <exception cref="System.Exception">Arrays are not equal in length</exception>
        public static GraphValue[] FormatData(float[] values, string[] tags)
        {
            if (values.Length != tags.Length)
            {
                throw new System.Exception("FormatData call was not given the same amount of values and tags. Make sure data is parallel.");
            }
            else
            {
                GraphValue[] points = new GraphValue[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    points[i] = new GraphValue()
                    {
                        Tag = tags[i],
                        Value = values[i]
                    };
                }

                return points;
            }
        }
    }
}

