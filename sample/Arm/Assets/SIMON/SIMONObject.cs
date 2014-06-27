
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFramework의 AI를 적용시킬 객체를 정의합니다. Manager를 통해 관리됩니다.
    /// </summary>
    [Serializable]
    [XmlRoot("SIMON")]
    public class SIMONObject
    {
        public string ObjectID { get; set; }
        [XmlElement("Properties")]
        public List<SIMONProperty> Properties { get; set; }
        [XmlElement("Actions")]
        public List<SIMONAction> Actions { get; set; }

        [XmlElement("ObjectFitness")]
        public string ObjectFitnessFunctionName { get; set; }
        [XmlIgnore()]
        public List<SIMONGene> PropertyDNA { get; set; }

        public SIMONObject()
        {
            Properties = new List<SIMONProperty>();
            Actions = new List<SIMONAction>();
            PropertyDNA = new List<SIMONGene>();
        }

        private void CopyFromObject(SIMONObject objInfo)
        {
            ObjectID = objInfo.ObjectID;
            Properties = objInfo.Properties;
            Actions = objInfo.Actions;
        }

        private SIMONObject CopyToObject()
        {
            SIMONObject newObject = new SIMONObject();
            newObject.ObjectID = this.ObjectID;
            newObject.Properties = this.Properties;
            newObject.Actions = this.Actions;
            return newObject;
        }

        /// <summary>
        /// DefinitionFile로부터 SIMONObject 정보를 복사해옵니다.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="fileName"></param>
        public void LoadObjectDefinition(string projectName, string fileName)
        {
            string definitionPath = @"\" + projectName + @"\" + fileName;

            SIMONObject xmlObject = (SIMONObject)SIMONUtility.GetInstance().DeserializeObject(definitionPath);
            CopyFromObject(xmlObject);
        }

        /// <summary>
        /// DefinitionFile로 현재 SIMONObject 정보를 기록해둡니다.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="fileName"></param>
        public void SaveObjectDefinition(string projectName, string fileName)
        {
            string definitionPath = @"\" + projectName + @"\" + fileName;
            SIMONObject thisObject = CopyToObject();
            SIMONUtility.GetInstance().SerializeObject(definitionPath, thisObject);
        }

        /// <summary>
        /// 입력된 propertyName에 해당하는 SIMONObject의 Property Value 값을 반환합니다.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public Double GetPropertyElement(string propertyName)
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                if (Properties[i].PropertyName.Equals(propertyName))
                    return Properties[i].PropertyValue;
            }
            throw new KeyNotFoundException("Key 값에 해당하는 Property 가 존재하지 않습니다.");
        }

        /// <summary>
        /// 입력된 propertyName에 해당하는 SIMONObject의 Property Value 값을 수정합니다.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public void SetPropertyElement(string propertyName, double propertyValue)
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                if (Properties[i].PropertyName.Equals(propertyName))
                {
                    Properties[i].PropertyValue = propertyValue;
                    return;
                }
            }
            throw new KeyNotFoundException("Key 값에 해당하는 Property 가 존재하지 않습니다.");
        }

        /// <summary>
        /// Action 이름에 해당하는 ActionObject를 반환합니다.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public SIMONAction GetActionObject(string actionName)
        {
            foreach (SIMONAction actionElement in Actions)
            {
                if (actionElement.ActionName.Equals(actionName))
                {
                    return actionElement;
                }
            }
            return null;
        }

        /// <summary>
        /// 입력된 actionName에 해당하는 SIMONObject의 Action값 DNA를 반환합니다.
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public List<SIMONGene> GetActionGeneElement(string actionName)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionName.Equals(actionName))
                    return Actions[i].ActionDNA;
            }
            throw new KeyNotFoundException("Key 값에 해당하는 Action 이 존재하지 않습니다.");
        }

        /// <summary>
        /// 입력된 actionName에 해당하는 SIMONObject의 Action값 DNA를 newDNA로 지정합니다.
        /// </summary>
        /// <param name="newDNA"></param>
        public void SetActionGeneElement(string actionName, List<SIMONGene> newDNA)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionName.Equals(actionName))
                {
                    Actions[i].ActionDNA = newDNA;
                }
            }
        }

        /// <summary>
        /// SIMONObject가 가진 Property에 대한 유전자 테이블을 갱신합니다.
        /// </summary>
        public void UpdatePropertyDNA()
        {
            foreach (SIMONProperty sProp in Properties)
            {
                SIMONGene newGene = new SIMONGene(sProp.PropertyName, sProp.PropertyValue);
                if ((sProp.Inherit) && (!PropertyDNA.Contains(newGene)))
                {
                    PropertyDNA.Add(newGene);
                }
            }
        }

        /// <summary>
        /// SIMONObject가 가진 Property DNA 테이블을 삭제합니다.
        /// </summary>
        public void ClearPropertyDNA()
        {
            PropertyDNA.Clear();
        }
    }

    /// <summary>
    /// SIMONObject가 정의하는 Property 값들을 정의합니다.
    /// </summary>
    [Serializable]
    public class SIMONProperty
    {
        [XmlElement("PropertyName")]
        public String PropertyName { get; set; }
        [XmlElement("PropertyValue")]
        public Double PropertyValue { get; set; }
        [XmlElement("Inherit")]
        public Boolean Inherit { get; set; }

        public SIMONProperty()
        {
            //Deserialize시 Parameter를 보증하기 위한 private parameterless 생성자. 실제 코드상에서 클래스를 이용할 때는 쓰이지 않는다.
            PropertyName = "NULL";
            PropertyValue = 0.0;
        }
        public SIMONProperty(string propertyName, double propertyValue, bool inherit)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            Inherit = inherit;
        }
    }

    /// <summary>
    /// SIMONObject가 정의하는 Action 값들을 정의합니다.
    /// </summary>
    [Serializable]
    public class SIMONAction
    {
        [XmlElement("ActionName")]
        public String ActionName { get; set; }                              //가리키는 항목 Action에 대한 명세.
        [XmlElement("ActionFunctionName")]
        public String ActionFunctionName { get; set; }
        [XmlElement("ExecutionFunctionName")]
        public String ExecutionFunctionName { get; set; }
        [XmlElement("FitnessFunctionName")]
        public String FitnessFunctionName { get; set; }
        [XmlElement("ActionDNA")]
        public List<SIMONGene> ActionDNA { get; set; }

        public SIMONAction()
        {
            ActionDNA = new List<SIMONGene>();
        }
        public SIMONAction(string actionName, string actionFunctionName, string executionFunctionName, string fitnessFunctionName, List<SIMONGene> actionDNA)
        {
            ActionName = actionName;
            ActionFunctionName = actionFunctionName;
            ExecutionFunctionName = executionFunctionName;
            FitnessFunctionName = fitnessFunctionName;
            ActionDNA = actionDNA;
        }

        /// <summary>
        /// elementName을 이름값으로 하는 DNA 내의 염색체의 Value를 반환합니다.
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public Double FindWeight(string elementName)
        {
            //element들에 대한 weight값을 DNA List안에서 검색해서 element 이름으로 검색해서 리턴한다. 실패시 ERROR_WEIGHT를 반환.
            double weight = 0.0;
            int dnaLength = ActionDNA.Count;
            bool findFlag = false;

            foreach (SIMONGene aGene in ActionDNA)
            {
                if (aGene.ElementName.Equals(elementName))
                {
                    weight = aGene.ElementWeight;
                    findFlag = true;
                    break;
                }
            }
            if (!findFlag)
            {
                throw new Exception("[SIMON Framework] : There is no element the name of which equals given parameter. - " + elementName.ToString());
            }
            return weight;
        }
        public Double FindWeight(string elementName, ref bool errorFlag)
        {
            //ERROR_WEIGHT는 상수값이므로, 해당 상수값으로 인한 오류 발생시 errorFlag 인자를 참조하여 weight 범위 에러를 처리하도록 함.
            double weight = 0.0;
            bool flag = false;
            errorFlag = true;
            foreach (SIMONGene aGene in ActionDNA)
            {
                if (aGene.ElementName.Equals(elementName))
                {
                    weight = aGene.ElementWeight;
                    errorFlag = false;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                throw new Exception("[SIMON Framework] : There is no element the name of which equals given parameter. - " + elementName.ToString());
            }
            return weight;
        }
        
        /// <summary>
        /// elementName에 해당하는 이름을 가진 DNA내의 단일 염색체를 반환합니다.
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public SIMONGene SelectDNA(string elementName)
        {
            foreach (SIMONGene aGene in ActionDNA)
                if (aGene.ElementName.Equals(elementName))
                    return aGene;
            return null;
        }
        
        /// <summary>
        /// elementName을 이름으로 하고, elementValue를 값으로 하는 DNA 단일 염색체를 DNA에 삽입합니다.
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="elementValue"></param>
        public void InsertDNA(string elementName, double elementValue)
        {
            //DNA 리스트에 이름을 키 값으로 하는 element value를 삽입한다.
            SIMONGene actionGene = new SIMONGene();
            actionGene.ElementName = elementName;
            actionGene.ElementWeight = elementValue;
            ActionDNA.Add(actionGene);
        }
        public void InsertDNA(SIMONGene actionGene)
        {
            ActionDNA.Add(actionGene);
        }
        public void InsertDNA(params SIMONGene[] actionGenome)
        {
            ActionDNA.AddRange(actionGenome);
        }

        /// <summary>
        /// elementName을 이름으로 하는 DNA 내의 단일 염색체를 삭제합니다.
        /// </summary>
        /// <param name="elementName"></param>
        public void DeleteDNA(string elementName)
        {
            //DNA 리스트에 이름을 키 값으로 하는 element value를 삭제한다.
            ActionDNA.Remove(SelectDNA(elementName));
        }

        /// <summary>
        /// elemenetName을 이름으로 하는 DNA 내의 단일 염색체의 값을 elementValue로 수정합니다.
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="elementValue"></param>
        public void EditDNA(string elementName, double elementValue)
        {
            DeleteDNA(elementName);
            InsertDNA(elementName, elementValue);
        }

        /// <summary>
        /// DNA 를 비웁니다.
        /// </summary>
        public void ClearDNA()
        {
            ActionDNA.Clear();
        }
    }

    /// <summary>
    /// SIMONObject가 정의하는 Action들에 대한 세부 유전자를 명세합니다.
    /// </summary>
    [Serializable]
    public class SIMONGene
    {
        [XmlElement("ElementName")]
        public String ElementName { get; set; }
        [XmlElement("ElementWeight")]
        public Double ElementWeight { get; set; }

        public SIMONGene()
        {
            ElementName = SIMONConstants.UNKNOWN_NAME;
            ElementWeight = 0;
        }
        public SIMONGene(string elementName, double elementWeight)
        {
            ElementName = elementName;
            ElementWeight = elementWeight;
        }
        public SIMONGene(SIMONGene s)
        {
            this.ElementName = s.ElementName;
            this.ElementWeight = s.ElementWeight;
        }
    }
}