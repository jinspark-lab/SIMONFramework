

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Serialization;

namespace SIMONFramework
{
    public class SIMONObjectFactory
    {

        public static SIMONGeneticObject CopyObject(SIMONGeneticObject from)
        {
            SIMONGeneticObject to = new SIMONGeneticObject();
            to.ObjectID = from.ObjectID;
            to.Properties = from.Properties;
            to.Actions = from.Actions;
            to.PropertyDNA = from.PropertyDNA;
            return to;
        }


        public static SIMONGeneticObject CopyDefinitionObject(SIMONCollection dictionary, string primaryKey)//Dictionary<string, SIMONObject<SIMONElement>> dictionary, string primaryKey)
        {
            SIMONGeneticObject copiedObject = new SIMONGeneticObject();
            copiedObject = (SIMONGeneticObject)dictionary[primaryKey];
            return copiedObject;
        }

        public static T CastObject<T>(T targetObject) where T : class
        {
            var type = targetObject.GetType();
            return Activator.CreateInstance(type) as T;
        }
        public static void ReflectObject<T, U>(ref SIMONObject<T, U> destinationObject, SIMONObject<T, U> sourceObject) where T : SIMONProperty where U : SIMONAction
        {
            if (typeof(T) == typeof(SIMONGeneticProperty))
            {
                (destinationObject as SIMONGeneticObject).ObjectID = (sourceObject as SIMONGeneticObject).ObjectID;
                (destinationObject as SIMONGeneticObject).Properties = (sourceObject as SIMONGeneticObject).Properties;
                (destinationObject as SIMONGeneticObject).Actions = (sourceObject as SIMONGeneticObject).Actions;
                (destinationObject as SIMONGeneticObject).PropertyDNA = (sourceObject as SIMONGeneticObject).PropertyDNA;
                (destinationObject as SIMONGeneticObject).ObjectFitnessFunctionName = (sourceObject as SIMONGeneticObject).ObjectFitnessFunctionName;
            }
        }
        public static object RecastObject<T, U>(dynamic sourceObject)
            where T : SIMONProperty
            where U : SIMONAction
        {
            dynamic returnObject = null;
            if (typeof(T) == typeof(SIMONGeneticProperty) && typeof(U) == typeof(SIMONGeneticAction))
            {
                var type = typeof(SIMONGeneticObject);
                returnObject = Activator.CreateInstance(type);
                returnObject.ObjectID = sourceObject.ObjectID;
                returnObject.Properties = sourceObject.Properties;
                returnObject.Actions = sourceObject.Actions;
                returnObject.ObjectFitnessFunctionName = sourceObject.ObjectFitnessFunctionName;
                returnObject.PropertyDNA = sourceObject.PropertyDNA;
            }
            return returnObject;
        }

        public static List<dynamic> RecastObjects<T, U>(List<dynamic> sourceList) 
            where T : SIMONProperty 
            where U : SIMONAction
        {
            List<dynamic> returnObject = new List<dynamic>();
            if (typeof(T) == typeof(SIMONGeneticProperty) && typeof(U) == typeof(SIMONGeneticAction))
            {
                foreach (dynamic source in sourceList)
                {
                    returnObject.Add((SIMONGeneticObject)RecastObject<T, U>(source));
                }
            }
            return returnObject;
        }

        public static dynamic[] RecastObjects<T, U>(dynamic[] sourceArray) 
            where T : SIMONProperty 
            where U : SIMONAction
        {
            dynamic[] returnArray = new dynamic[SIMONConstants.MAX_SIMONOBJ_NUM];
            if (typeof(T) == typeof(SIMONGeneticProperty) && typeof(U) == typeof(SIMONGeneticAction))
            {
                int sourceIdx = 0;
                foreach (dynamic source in sourceArray)
                {
                    returnArray[sourceIdx++] = (SIMONGeneticObject)source;
                }
            }
            return returnArray;
        }


        public static dynamic ConvertObject(dynamic source, Type dest)
        {
            return Convert.ChangeType(source, dest);
        }
    }

}