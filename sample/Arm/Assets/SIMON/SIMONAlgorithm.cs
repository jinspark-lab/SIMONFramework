
/*
 * 
 * SIMON Framework에서 사용되는 알고리즘에 대한 인터페이스를 제공합니다.
 * 
 * 
 *  
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace SIMONFramework
{

    public interface SIMONAlgorithm
    {
        /// <summary>
        /// ObjectCollection 필드 내의 Object들을 대상으로 FitnessFunction 매개변수로 전달되는 함수들을 사전형태로 찾고 이용해서 알고리즘을 구현하는 인터페이스를 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection"></param>
        /// <param name="FitnessFunctions"></param>
        /// <returns></returns>
        object Implementation(SIMONCollection ObjectCollection, Dictionary<string, SIMONFunction> FitnessFunction);
        object Implementation(SIMONCollection ObjectCollection, Dictionary<string, SIMONFunction> FitnessFunction, double learningRate);
        object Implementation(SIMONCollection ObjectCollection, SIMONCollection ObjectMap, Dictionary<string, SIMONFunction> FitnessFunctions);
        object Implementation(SIMONCollection ObjectCollection, SIMONCollection ObjectMap, Dictionary<string, SIMONFunction> FitnessFunctions, double learningRate);

    }



}