
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

    /// <summary>
    /// SIMON Framework 에서 Algorithm Layer를 구축하는 구현 모듈에 대한 Interface를 제공합니다.
    /// </summary>
    public interface SIMONAlgorithm
    {
        /// <summary>
        /// ObjectCollection 필드 내의 Object들을 대상으로 FitnessFunction 매개변수로 전달되는 함수들을 사전형태로 찾고 이용해서 알고리즘을 구현하는 인터페이스를 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection">2차원 대상 Property Collection입니다.</param>
        /// <param name="FitnessFunctions">구현에 사용될 SIMONFunction ADT입니다.</param>
        /// <returns>함수 구현의 결과값입니다.</returns>
        object Implementation(SIMONCollection ObjectCollection, SIMONCollection FitnessFunction);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> FitnessFunction);

        /// <summary>
        /// ObjectCollection 필드 내의 Object들을 대상으로 FitnessFunction 매개변수로 전달되는 함수들을 사전형태로 찾고 이용해서 알고리즘을 구현하는 인터페이스를 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection">2차원 대상 Property Collection입니다.</param>
        /// <param name="FitnessFunctions">구현에 사용될 SIMONFunction ADT입니다.</param>
        /// <param name="learningRate">학습률입니다.</param>
        /// <returns>함수 구현의 결과값입니다.</returns>
        object Implementation(SIMONCollection ObjectCollection, SIMONCollection FitnessFunction, double learningRate);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> FitnessFunction, double learningRate);
        
        /// <summary>
        /// ObjectCollection 필드 내의 Object들을 대상으로 FitnessFunction 매개변수로 전달되는 함수들을 사전형태로 찾고 이용해서 알고리즘을 구현하는 인터페이스를 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection">3차원 대상 Action Object Collection입니다.</param>
        /// <param name="ObjectMap">Group에 대한 ActionMap입니다.</param>
        /// <param name="FitnessFunctions">구현에 사용될 SIMONFunction ADT입니다.</param>
        /// <returns>함수 구현의 결과값입니다.</returns>
        object Implementation(SIMONCollection ObjectCollection, SIMONCollection ObjectMap, SIMONCollection FitnessFunction);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> FitnessFunctions);

        /// <summary>
        /// ObjectCollection 필드 내의 Object들을 대상으로 FitnessFunction 매개변수로 전달되는 함수들을 사전형태로 찾고 이용해서 알고리즘을 구현하는 인터페이스를 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection">3차원 대상 Action Object Collection입니다.</param>
        /// <param name="ObjectMap">Group에 대한 ActionMap입니다.</param>
        /// <param name="FitnessFunctions">구현에 사용될 SIMONFunction ADT입니다.</param>
        /// <param name="learningRate">학습률입니다.</param>
        /// <returns>함수 구현의 결과값입니다.</returns>
        object Implementation(SIMONCollection ObjectCollection, SIMONCollection ObjectMap, SIMONCollection FitnessFunctions, double learningRate);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> FitnessFunctions, double learningRate);




    }



}