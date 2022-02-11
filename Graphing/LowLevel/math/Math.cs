using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Graphing.Editor
{
    public struct GraphingMath
    {

        public static float[] NumbersUpTo(float maxExclusive)
        {
            float[] vals = new float[(int)maxExclusive];
            for (int i = 0; i < maxExclusive; i++)
            {
                vals[i] = i;
            }
            //Debug.Log($"Numbers up to {maxExclusive} exclusive are {arrayToString(vals)}");
            return vals;
        }

        private static string arrayToString(float[] array)
        {
            string res = "";
            for (int i = 0; i < array.Length; i++)
            {
                res += i.ToString() + "; ";
            }
            return res;
        }
        
        public static float SigmaVal(float[] sigmaValues)
        {
            float result = 0;
            for (int i = 0; i < sigmaValues.Length; i++)
            {
                result += sigmaValues[i];
            }
            return result;
        }

        public static float SigmaValPow(float[] sigmaValues, float pow)
        {
            float result = 0;
            for (int i = 0; i < sigmaValues.Length; i++)
            {
                result += Mathf.Pow(sigmaValues[i], pow);
            }
            return result;
        }

        public static float SigmaValMult(float[] sigmaValuesA, float[] sigmaValuesB)
        {
            if (sigmaValuesA.Length != sigmaValuesB.Length)
            {
                throw new System.Exception($"lambda values are not symmetrical    A={sigmaValuesA.Length} || B={sigmaValuesB.Length}");
            }

            float result = 0;
            for (int i = 0; i < sigmaValuesA.Length; i++)
            {
                result += (sigmaValuesA[i] * sigmaValuesB[i]);
            }
            return result;
        }
    }
}