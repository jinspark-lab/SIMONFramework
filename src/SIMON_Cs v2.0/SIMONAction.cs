
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONObject가 정의하는 Action 값들을 정의합니다.
    /// </summary>
    public interface SIMONAction
    {
        /// <summary>
        /// SIMONAction 객체를 고유 지칭하는 Action 이름에 대한 정의입니다.
        /// </summary>
        [XmlElement("ActionName")]
        String ActionName { get; set; }

        /// <summary>
        /// ActionFunction의 이름을 정의합니다.
        /// </summary>
        [XmlElement("ActionFunctionName")]
        String ActionFunctionName { get; set; }

        /// <summary>
        /// ExecutionFunction의 이름을 정의합니다.
        /// </summary>
        [XmlElement("ExecutionFunctionName")]
        String ExecutionFunctionName { get; set; }

        /// <summary>
        /// FitnessFunction의 이름을 정의합니다.
        /// </summary>
        [XmlElement("FitnessFunctionName")]
        String FitnessFunctionName { get; set; }
    }


}