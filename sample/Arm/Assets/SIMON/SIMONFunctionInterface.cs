
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
    public delegate Object SIMONFunction(SIMONObject One, SIMONObject[] TheOthers);

    public interface SIMONFunctionInterface
    {
        string[] GetFunctionList();
        int GetFunctionCount();
    }


}