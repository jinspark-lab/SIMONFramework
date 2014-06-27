
/* 
 * 
 * SIMONFramework에서 내부적으로 자주 사용하는 유틸리티들을 모아서 정리해놓는다. 
 * 모든 유틸리티 기능들은 Singleton 패턴으로 사용할 수 있게끔 Instance를 반환해준다. 문서에서와는 다르게 매니저에서 쓰는 모듈로써가 아니라 프레임워크에서 자주 쓰이는 유틸리티를 정리하도록 한다.
 * 
 * 1. Serializer,Deserializer 
 * 
 * 
 * 
 * 
 */ 

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFramework에서 내부적으로 사용하는 도구 클래스에 대해 정의합니다. Singleton 방식으로 사용됩니다.
    /// </summary>
    public class SIMONUtility
    {
        private static Random rand = new Random();

        private SIMONUtility()
        {

        }
        public static SIMONUtility GetInstance()
        {
            return new SIMONUtility();
        }
        public static int GenerateRandomInt()
        {
            return rand.Next();
        }
        public static double GenerateRandomDouble(double rating)
        {
            return rand.NextDouble() * rating;
        }
        public void SerializeObject(string filePath, SIMONObject sObject)
        {
            if (sObject == null)
                return;
            XmlSerializer serializer = new XmlSerializer(typeof(SIMONObject));
            string fullPath = Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH + filePath;
            string dirPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            FileStream fStream = new FileStream(fullPath, FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fStream, System.Text.Encoding.UTF8);
            if (fStream.CanWrite)
                serializer.Serialize(sWriter, sObject);
            fStream.Close();
        }
        public SIMONObject DeserializeObject(string filePath)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(SIMONObject));
            FileStream fStream = new FileStream(Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH + filePath, FileMode.Open);
            SIMONObject sObject = null;
            StreamReader sReader = new StreamReader(fStream, System.Text.Encoding.UTF8);
            if (fStream.CanRead)
                sObject = (SIMONObject)deserializer.Deserialize(sReader);
            fStream.Close();
            return sObject;
        }

    }


}