using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphing
{

    public struct GraphStyle
    {
        /// <summary>
        /// Size of bottom margin for X axis labels
        /// </summary>
        public float bottomMargin;

        /// <summary>
        /// Size of left margin for Y axis labels
        /// </summary>
        public float leftMargin;

        /// <summary>
        /// Thickness of X and Y axis lines
        /// </summary>
        public float axisWidth;

        /// <summary>
        /// Height of graph area in pixels. I recommend at least 120px
        /// </summary>
        public float graphAreaHeight;

        /// <summary>
        /// Number of tags/steps on the y axis labeling
        /// </summary>
        public uint yAxisSteps;

        /// <summary>
        /// States whether graph can be changed to line or bar graph in the inspector
        /// </summary>
        public bool canSwitchToOtherTypes;

        /// <summary>
        /// Whether the graph should fit a specific Y axis dimension
        /// </summary>
        public bool scaleToDesired;

        /// <summary>
        /// Value Y axis should be fitted to
        /// </summary>
        public int fitToYval;

        /// <summary>
        /// Type of formatting to Y axis values, as accepted by .ToString(), such as F0, F2, etc... 
        /// </summary>
        public string yAxisFormatting;

        /// <summary>
        /// unit sign to append to Y axis labels. Leave empty for only numerical values
        /// </summary>
        public string yAxisUnitSign;
        
        /// <summary>
        /// Wether you want the line graph view to show trend and approximate prediction based on given data.
        /// </summary>
        public bool lineGraphPredict;

        /// <summary>
        /// What percentage of horizontal visual space should be dedicated to the prediction
        /// </summary>
        public float lineGraphPredictionSpace;

        /// <summary>
        /// Whether the graph should record the last value(s) passed to it through .SetValues() is memory
        /// </summary>
        public bool liveItemAdd;

        /// <summary>
        /// Number of values that are allowed to be recorded and backlogged for Live Item Add
        /// </summary>
        public int liveItemBuffer;


        public static GraphStyle Default
        {
            get
            {
                return new GraphStyle
                {
                    bottomMargin = 30f,
                    leftMargin = 100f,
                    axisWidth = 7f,
                    graphAreaHeight = 150f,
                    yAxisSteps = 10,
                    canSwitchToOtherTypes = true,
                    scaleToDesired = false,
                    fitToYval = 100,
                    yAxisFormatting = "F1",
                    yAxisUnitSign = "",
                    lineGraphPredict = false,
                    lineGraphPredictionSpace = 0,
                    liveItemAdd = false,
                    liveItemBuffer = 0
                };
            }
        }

        public static GraphStyle DefaultPredict
        {
            get
            {
                return new GraphStyle
                {
                    bottomMargin = 30f,
                    leftMargin = 100f,
                    axisWidth = 7f,
                    graphAreaHeight = 200f,
                    yAxisSteps = 10,
                    canSwitchToOtherTypes = true,
                    scaleToDesired = false,
                    fitToYval = 50, 
                    yAxisFormatting = "F1",
                    yAxisUnitSign = "",
                    lineGraphPredict = true,
                    lineGraphPredictionSpace = 30,
                    liveItemAdd = false,
                    liveItemBuffer = 0
                };
            }
        }

        public static GraphStyle Percentage
        {
            get
            {
                return new GraphStyle
                {
                    bottomMargin = 30f,
                    leftMargin = 100f,
                    axisWidth = 7f,
                    graphAreaHeight = 200f,
                    yAxisSteps = 10,
                    canSwitchToOtherTypes = false,
                    scaleToDesired = true,
                    fitToYval = 100,
                    yAxisFormatting = "F0",
                    yAxisUnitSign = "%",
                    liveItemAdd = false,
                    liveItemBuffer = 0
                };
            }
        }

        public static GraphStyle LiveAdd
        {
            get
            {
                return new GraphStyle
                {
                    bottomMargin = 30f,
                    leftMargin = 100f,
                    axisWidth = 7f,
                    graphAreaHeight = 150f,
                    yAxisSteps = 10,
                    canSwitchToOtherTypes = true,
                    scaleToDesired = false,
                    fitToYval = 100,
                    yAxisFormatting = "F1",
                    yAxisUnitSign = "",
                    lineGraphPredict = false,
                    lineGraphPredictionSpace = 0,
                    liveItemAdd = true,
                    liveItemBuffer = 20
                };
            }
        }

    }


}