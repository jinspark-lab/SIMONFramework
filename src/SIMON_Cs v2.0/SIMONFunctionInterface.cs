
/*
 * 
 * SIMONObject에서 사용하게 될 Function 들에 대한 정의 및 관리하기 위한 클래스를 구조한다.
 * 
 * 
 * 
 *  
 */

using System;
using System.Collections.Generic;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFunction에 대한 Delegate 지정자를 선언합니다.
    /// </summary>
    /// <param name="One">함수의 Parameter로 지정될 주체 객체입니다.</param>
    /// <param name="TheOthers">함수의 Parameter로 지정될 객체 배열입니다.</param>
    /// <returns>함수의 결과값입니다.</returns>
    public delegate Object SIMONFunction(SIMONObject<SIMONProperty, SIMONAction> One, SIMONObject<SIMONProperty, SIMONAction>[] TheOthers);

    public delegate Object SIMONGeneticFunction(SIMONObject<SIMONProperty, SIMONAction> One, SIMONObject<SIMONProperty, SIMONAction>[] TheOthers);


    /// <summary>
    /// SIMONFunction 들을 Container로 관리할 경우 사용할 수 있게 제공하는 Interface입니다.
    /// </summary>
    public class SIMONFunctionInterface<T, U> where T : SIMONProperty where U : SIMONAction
    {
        public delegate Object SIMONDelegate(SIMONObject<T, U> One, SIMONObject<T, U>[] TheOthers);
    }


}