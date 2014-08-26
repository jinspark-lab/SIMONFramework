
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SIMONFramework
{
    
    /// <summary>
    /// SIMONFramework의 AI를 적용시킬 객체를 정의합니다. Manager를 통해 관리됩니다.
    /// </summary>
//    public abstract class SIMONObject<T> where T : SIMONElement
    public interface SIMONObject<T, U> where T : SIMONProperty where U : SIMONAction
    {
        /// <summary>
        /// SIMONObject를 구별하기 위한 고유값(Primary Key)을 정의합니다.
        /// </summary>
        string ObjectID { get; set; }

        /// <summary>
        /// SIMONObject가 갖는 Property 집합을 정의합니다.
        /// </summary>
        List<T> Properties { get; set; }

        /// <summary>
        /// SIMONObject가 갖는 Action 집합을 정의합니다.
        /// </summary>
        List<U> Actions { get; set; }

    }


}