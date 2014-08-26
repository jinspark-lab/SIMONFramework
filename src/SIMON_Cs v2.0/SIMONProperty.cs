
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SIMONFramework
{

    /// <summary>
    /// SIMONObject가 정의하는 Property 값들을 정의합니다.
    /// </summary>
    public interface SIMONProperty
    {
        [XmlElement("PropertyName")]
        String PropertyName { get; set; }
        [XmlElement("PropertyValue")]
        Double PropertyValue { get; set; }

    }


}