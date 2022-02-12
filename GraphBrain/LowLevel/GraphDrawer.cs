using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Graphing.Editor
{
#if UNITY_EDITOR

    public class GraphDrawer
    {

        public static void DrawGraph(GraphValue[] points, Color[] barColors, GraphStyle style, ref bool hasToDrawBarGraph, string graphName)
        {
            //HEADER SECTION
            GUIStyle centeredHeaderStyle = new GUIStyle(EditorStyles.helpBox);
            centeredHeaderStyle.alignment = TextAnchor.MiddleCenter;
            centeredHeaderStyle.fontStyle = FontStyle.Bold;
            centeredHeaderStyle.fontSize = 25;
            GUILayout.Label(graphName.ToUpper(), centeredHeaderStyle);


            //get last gui element pos
            Rect lastElement = GUILayoutUtility.GetLastRect();

            //calculate graph position and area relative to last GUI element pos...
            Vector2 areaSize = new Vector2(EditorGUIUtility.currentViewWidth - 30f, style.graphAreaHeight);
            Vector2 topLeftCorner = lastElement.position + new Vector2(0, lastElement.height + 15f);

            Rect graphRect = new Rect(topLeftCorner, areaSize);

            //create basic flat color texture for the time being
            Texture2D bg = new Texture2D(1, 1);
            bg.SetPixel(0, 0, new Color(.1f, .1f, .1f, 1));
            bg.Apply();

            //draw graph background
            GUI.DrawTexture(graphRect, bg, ScaleMode.StretchToFill);

            

            if (hasToDrawBarGraph)
            {
                DrawLabels(points, style, graphRect);
                DrawBarGraph(points, barColors, style, graphRect);
            }
            else
            {
                DrawLabels(points, style, graphRect, false);
                DrawPoints(points, barColors, style, graphRect);
                //draw line graph here...
            }

            //get x and y axis coords
            Vector3[] xAxisCoords = new Vector3[2];
            xAxisCoords[0] = new Vector3(graphRect.x + style.leftMargin, graphRect.y + graphRect.height - style.bottomMargin + ((style.axisWidth / 2 - 2)), 0);
            xAxisCoords[1] = new Vector3(graphRect.x + graphRect.width, xAxisCoords[0].y, 0);
            Vector3[] yAxisCoords = new Vector3[2];
            yAxisCoords[0] = new Vector3(graphRect.x + style.leftMargin, graphRect.y + graphRect.height - style.bottomMargin, 0);
            yAxisCoords[1] = new Vector3(yAxisCoords[0].x, graphRect.y, 0);


            //GUI.BeginClip(graphRect);
            //GUI.EndClip();

            //draw x and y root axis lines
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(style.axisWidth, xAxisCoords);
            Handles.DrawAAPolyLine(style.axisWidth, yAxisCoords);



            GUILayout.Space(style.graphAreaHeight + 35);

            if (style.canSwitchToOtherTypes)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Bar Graph"))
                {
                    hasToDrawBarGraph = true;
                }
                else if (GUILayout.Button("Line Graph"))
                {
                    hasToDrawBarGraph = false;
                }
                GUILayout.EndHorizontal();
            }

        }

        private static void DrawBarGraph(GraphValue[] points, Color[] barColors, GraphStyle style, Rect graphRect)
        {
            //create basic flat color texture for the time being
            Texture2D bg = new Texture2D(1, 1);
            bg.SetPixel(0, 0, new Color(.1f, .1f, .1f, 1));
            bg.Apply();


            GUILayout.BeginVertical();
            //do label work here ??
            GUILayout.EndVertical();

            if (points == null)
            {
                return;
            }

            //calculate number of bars, width of each bar to fit in width and get highest value within the data to plot
            int barCount = points.Length;
            float barWidth = (graphRect.width - style.leftMargin) / (float)barCount;
            float highestPoint = 0;

            foreach (GraphValue val in points)
            {
                if (val.Value > highestPoint)
                {
                    highestPoint = val.Value;
                }
            }

            //if wanted to fit to a specific scale, set scale here in place of max value
            if (style.scaleToDesired)
            {
                highestPoint = style.fitToYval;
            }

            //get value of how many units a pixel represents on-screen
            float pixelValue = (graphRect.height - style.bottomMargin) / highestPoint;
            //get array of bar heights in pixels based on just calculated pixel weight
            float[] pixelBarHeights = new float[barCount];
            for (int i = 0; i < points.Length; i++)
            {
                pixelBarHeights[i] = points[i].Value * pixelValue;
            }

            //bars' draw loop
            for (int i = 0; i < points.Length; i++)
            {
                //get bar rect based on current loop iteration
                Vector2 size = new Vector2(barWidth, -pixelBarHeights[i]);
                Vector2 anchor = new Vector2(graphRect.x + (barWidth * i) + style.leftMargin, graphRect.y + graphRect.height - style.bottomMargin);

                Rect anc = new Rect(anchor, size);
                //change color
                bg.SetPixel(0, 0, barColors[i]);
                bg.Apply();
                //draw bar
                GUI.DrawTexture(anc, bg, ScaleMode.StretchToFill);
            }

        }
        
        private static void DrawPoints(GraphValue[] points, Color[] pointColors, GraphStyle style, Rect graphRect)
        {
            //use formula to get trend and future projection
            float n = points.Length;
            float sigmaValsMult = GraphingMath.SigmaValMult(GraphValue.ParseToArray(points), GraphingMath.NumbersUpTo(n));
            float sigmaYs = GraphingMath.SigmaVal(GraphValue.ParseToArray(points));
            float sigmaXPow = GraphingMath.SigmaValPow(GraphValue.ParseToArray(points), 2);
            float sigmaXs = GraphingMath.SigmaVal(GraphingMath.NumbersUpTo(n));

            float m = (n * (sigmaValsMult)) - (sigmaXs) * (sigmaYs);
            m /= (n * (sigmaXPow)) - Mathf.Pow(sigmaXs, 2);

            float b = sigmaYs - (m * (sigmaXs));
            b /= n;

            //Debug.Log($"y={m}x+{b}");


            float plotAreaWidth = graphRect.width - style.leftMargin;

            float knowValuesPlottingAreaWidth = plotAreaWidth;

            if (style.lineGraphPredict)
            {
                knowValuesPlottingAreaWidth -= (plotAreaWidth / 100) * style.lineGraphPredictionSpace;
            }

            float distanceBetweenKnownPoints = knowValuesPlottingAreaWidth / (points.Length - 1);


            //get x pixel value
            float xPixelValue = (points.Length) / knowValuesPlottingAreaWidth;

            ///PLOT KNOWN VALUES AND THEN PLOT PREDICTION LINE

            float plotAreaHeight = graphRect.height - style.bottomMargin;

            float highestPoint = Mathf.NegativeInfinity;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].Value > highestPoint)
                {
                    highestPoint = points[i].Value;
                }
            }

            //get last prediction value
            float finalPredX = plotAreaWidth * xPixelValue;
            float finalPredY = (m * finalPredX) + b;
            Vector2 predictionLineEndPoint = new Vector2(finalPredX, finalPredY);


            //Debug.Log($"Last Prodiction point = {finalPredX}; {finalPredY}");

            //get first prediction point

            float initialPredX = 0;
            float initialPredY = (m * initialPredX) + b;
            Vector2 predictionLineStartPoint = new Vector2(initialPredX, initialPredY);

            //compare highest point again with predicted values
            if (finalPredY > highestPoint)
            {
                highestPoint = finalPredY;
            }
            
            if (initialPredY > highestPoint)
            {
                highestPoint = initialPredY;
            }

            //get Y pixel value

            float yPixelValue = 0;
            if (style.scaleToDesired == false)
            {
                yPixelValue = highestPoint / plotAreaHeight;
            }
            else yPixelValue = style.fitToYval / plotAreaHeight;

            //Debug.Log($"First known point: {points[0].Value} | Last known point: {points[points.Length - 1].Value}");
            //Debug.Log($"y={m}x+{b}");
            //Debug.Log($"Draw start: {predictionLineStartPoint} | end: {predictionLineEndPoint}");
            DrawPredictionLineToScale(predictionLineStartPoint, predictionLineEndPoint, xPixelValue, yPixelValue, graphRect, style);

            //Convert all known points to local coords
            Vector3[] pts = ConvertLinePointsToCoords(points, graphRect, style, distanceBetweenKnownPoints, yPixelValue);

            for (int i = 0; i < pts.Length - 1; i++)
            {
                Handles.color = Color.yellow;
                Handles.DrawPolyLine(pts);
            }

        }

        private static void DrawPredictionLineToScale(Vector2 pointA, Vector2 pointB, float xPixelValue, float yPixelValue, Rect graphRect, GraphStyle style)
        {
            Vector2 scaledPointA, scaledPointB;

            scaledPointA = new Vector2(graphRect.x + style.leftMargin, graphRect.y + graphRect.height - style.bottomMargin);
            scaledPointB = new Vector2(graphRect.x + style.leftMargin, graphRect.y + graphRect.height - style.bottomMargin);
            
            scaledPointA += new Vector2(pointA.x / xPixelValue, - (pointA.y / yPixelValue));
            scaledPointB += new Vector2(pointB.x / xPixelValue, - (pointB.y / yPixelValue));

            if (scaledPointB.y < scaledPointA.y)
            {
                Handles.color = Color.green;
            }
            else if (scaledPointB.y > scaledPointA.y)
            {
                Handles.color = Color.red;
            }
            else
            {
                Handles.color = Color.grey;
            }

            Handles.DrawDottedLine(scaledPointA, scaledPointB, 5f);

        }

        private static Vector3[] ConvertLinePointsToCoords(GraphValue[] points, Rect graphRect, GraphStyle style, float distanceBetweenPoints, float yPixelValue)
        {
            Vector3[] pts = new Vector3[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                float x = graphRect.x + style.leftMargin + (distanceBetweenPoints * i);
                float y = graphRect.y + graphRect.height - style.bottomMargin - (points[i].Value / yPixelValue);
                pts[i].x = x;
                pts[i].y = y;
            }

            return pts;
        }

        private static void DrawLabels(GraphValue[] points, GraphStyle style, Rect graphRect, bool drawXlabels = true)
        {
            int numberOfLabels;
            float distanceBetweenLabels;
            Vector2[] labelAnchorPoints;
            GUIStyle centeredStyle;

            //bottom labels
            if (drawXlabels)
            {
                numberOfLabels = points.Length;
                distanceBetweenLabels = (graphRect.width - style.leftMargin) / numberOfLabels;
                labelAnchorPoints = new Vector2[numberOfLabels];

                for (int i = 0; i < labelAnchorPoints.Length; i++)
                {
                    labelAnchorPoints[i] = new Vector2(graphRect.x + style.leftMargin + (distanceBetweenLabels * i), graphRect.y + graphRect.height - style.bottomMargin);
                }

                centeredStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

                for (int i = 0; i < labelAnchorPoints.Length; i++)
                {
                    Rect pos = new Rect(labelAnchorPoints[i], new Vector2(distanceBetweenLabels, style.bottomMargin));
                    GUI.Label(pos, points[i].Tag, centeredStyle);
                }
            }
            

            //left labels
            numberOfLabels = (int)style.yAxisSteps + 1;
            distanceBetweenLabels = (graphRect.height - style.bottomMargin) / (numberOfLabels - 1);
            labelAnchorPoints = new Vector2[numberOfLabels - 1];

            for (int i = 0; i < labelAnchorPoints.Length; i++)
            {
                labelAnchorPoints[i] = new Vector2(graphRect.x, graphRect.y + graphRect.height - style.bottomMargin - (distanceBetweenLabels * i));
            }

            //get highest Y val
            float highestVal = 0;
            foreach (GraphValue val in points)
            {
                if (val.Value > highestVal)
                {
                    highestVal = val.Value;
                }
            }
            //if wanted to fit to a specific scale, set scale here in place of max value
            if (style.scaleToDesired)
            {
                highestVal = style.fitToYval;
            }

            float[] intervals = new float[style.yAxisSteps + 1];
            intervals = GetIntervalValues(style.yAxisSteps, highestVal);

            centeredStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight};

            for (int i = 0; i < labelAnchorPoints.Length; i++)
            {
                Rect pos = new Rect(labelAnchorPoints[i] - new Vector2(0, distanceBetweenLabels/2), new Vector2(style.leftMargin, 15));
                GUI.Label(pos, intervals[i].ToString(style.yAxisFormatting) + style.yAxisUnitSign + "   ", centeredStyle);
                Vector2 lineStart = new Vector2(graphRect.x + style.leftMargin, labelAnchorPoints[i].y);
                Vector2 lineEnd = new Vector2(graphRect.x + graphRect.width, labelAnchorPoints[i].y);
                Handles.color = Color.gray;
                Handles.DrawLine(lineStart, lineEnd, 2);
            }

        }

        private static float[] GetIntervalValues(uint numberOfSteps, float highestVal = 100f)
        {
            float[] vals = new float[numberOfSteps + 1];

            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = (highestVal / (float)numberOfSteps) * i;
            }

            return vals;
        }
    }

#endif

    [System.Serializable]
    public struct GraphValue
    {
        public string Tag;
        public float Value;

        public static float[] ParseToArray(GraphValue[] points)
        {
            float[] pts = new float[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pts[i] = points[i].Value;
            }

            return pts;
        }

        public static implicit operator float(GraphValue e) => e.Value;
        public static implicit operator int(GraphValue e) => (int)e.Value;
        public override string ToString() => Tag;
    }



}