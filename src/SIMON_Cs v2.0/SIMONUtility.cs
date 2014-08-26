﻿
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
    public enum SIMONObjectType { SIMONObject = 1, SIMONGeneticObject = 2 };

    /// <summary>
    /// SIMONFramework에서 내부적으로 사용하는 도구 클래스에 대해 정의합니다. Singleton 방식으로 사용됩니다.
    /// </summary>
    public class SIMONUtility
    {
        private static Random rand = new Random();

        private SIMONUtility()
        {

        }
        /// <summary>
        /// SIMONUtility 객체를 반환합니다. Singleton 사용을 권장합니다.
        /// </summary>
        /// <returns></returns>
        public static SIMONUtility GetInstance()
        {
            return new SIMONUtility();
        }
        /// <summary>
        /// Framework 에서 제공하는 System time에 기초한 랜덤 변수 발생 객체입니다.
        /// </summary>
        /// <returns>임의의 정수 난수입니다.</returns>
        public static int GenerateRandomInt()
        {
            return rand.Next();
        }

        /// <summary>
        /// Framework 에서 제공하는 System time에 기초한 랜덤 변수 발생 객체입니다.
        /// </summary>
        /// <param name="rating">0~1사이 소숫값에 대한 곱셈을 수행하는 비율입니다.</param>
        /// <returns>임의의 실수 난수입니다.</returns>
        public static double GenerateRandomDouble(double rating)
        {
            return rand.NextDouble() * rating;
        }

        /// <summary>
        /// 지정한 파일 경로에 SIMONObject를 기록합니다.
        /// </summary>
        /// <param name="filePath">파일 경로입니다.</param>
        /// <param name="sObject">대상 SIMONObject입니다.</param>
        public void SerializeObject<T, U>(string filePath, SIMONObject<T, U> sObject) where T : SIMONProperty where U : SIMONAction
        {
            SIMONSerializer serializer = new SIMONSerializer(SIMONObjectType.SIMONObject);
            serializer.SerializeObject(filePath, sObject, SIMONObjectType.SIMONObject);
        }

        /// <summary>
        /// 지정한 파일 경로에 SIMONObject를 기록합니다.
        /// </summary>
        /// <param name="filePath">파일 경로입니다.</param>
        /// <param name="sObject">대상 SIMONObject입니다.</param>
        public void SerializeObject(string filePath, SIMONGeneticObject sObject)
        {
        //    SIMONSerializer serializer = new SIMONSerializer(SIMONObjectType.SIMONGeneticObject);
        //    serializer.SerializeObject(filePath, sObject, SIMONObjectType.SIMONGeneticObject);


            if (sObject == null)
                return;
            XmlSerializer serializer = new XmlSerializer(sObject.GetType());
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


        /// <summary>
        /// 지정한 파일 경로로부터 SIMONObject를 읽어들입니다.
        /// </summary>
        /// <param name="filePath">파일 경로입니다.</param>
        /// <returns>읽어들인 객체입니다.</returns>
        public object DeserializeObject(string filePath, SIMONObjectType objectType)
        {
            object retValue = null;
            SIMONSerializer deserializer;


            switch (objectType)
            {
                case SIMONObjectType.SIMONObject:
                    deserializer = new SIMONSerializer(SIMONObjectType.SIMONObject);
                    retValue = deserializer.DeserializeObject(filePath, SIMONObjectType.SIMONObject);
                    break;
                case SIMONObjectType.SIMONGeneticObject:
                    deserializer = new SIMONSerializer(SIMONObjectType.SIMONGeneticObject);
                    retValue = deserializer.DeserializeObject(filePath, SIMONObjectType.SIMONGeneticObject);
                    break;
                default:

                    break;
            }
            return retValue;
        }

    }

    public sealed class SIMONSerializer
    {
        private XmlSerializer Serializer{get; set;}

        public SIMONSerializer(SIMONObjectType objectType)
        {
            switch (objectType)
            {
                case SIMONObjectType.SIMONObject:
                    break;
                case SIMONObjectType.SIMONGeneticObject:
                    Serializer = new XmlSerializer(typeof(SIMONGeneticObject));
                    break;

            }
        }
        public void SerializeObject(string filePath, object sObject, SIMONObjectType objectType)
        {
            if (sObject == null)
                return;

            string fullPath = Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH + filePath;
            string dirPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            FileStream fStream = new FileStream(fullPath, FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fStream, System.Text.Encoding.UTF8);
            if (fStream.CanWrite)
            {
                switch (objectType)
                {
                    case SIMONObjectType.SIMONObject:
                        break;
                    case SIMONObjectType.SIMONGeneticObject:
                        Serializer.Serialize(sWriter, (SIMONGeneticObject)sObject);
                        break;
                    default:
                        break;
                }
            }
            fStream.Close();
        }
        public object DeserializeObject(string filePath, SIMONObjectType objectType)
        {
            FileStream fStream = new FileStream(Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH + filePath, FileMode.Open);
            object sObject = null;
            StreamReader sReader = new StreamReader(fStream, System.Text.Encoding.UTF8);
            if (fStream.CanRead)
            {
                switch (objectType)
                {
                    case SIMONObjectType.SIMONObject:
                        break;
                    case SIMONObjectType.SIMONGeneticObject:
                        sObject = (SIMONGeneticObject)Serializer.Deserialize(sReader);
                        break;
                    default:
                        break;
                }
            }
            fStream.Close();
            return sObject;
        }

    }

}