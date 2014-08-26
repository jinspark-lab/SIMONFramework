
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
        /// <summary>
        /// SIMONObject 를 구분할 수 있는 Primary Key 값으로서의 ID를 정의합니다.
        /// </summary>
        public string ObjectID { get; set; }
        
        /// <summary>
        /// SIMONObject가 갖는 Property 집합을 정의합니다.
        /// </summary>
        [XmlElement("Properties")]
        public List<SIMONProperty> Properties { get; set; }

        /// <summary>
        /// SIMONObject가 갖는 Action 집합을 정의합니다.
        /// </summary>
        [XmlElement("Actions")]
        public List<SIMONAction> Actions { get; set; }
        
        /// <summary>
        /// SIMONObject 자체의 Property를 대상으로 Fitness Value를 산출하는 함수의 이름을 정의합니다.
        /// </summary>
        [XmlElement("ObjectFitness")]
        public string ObjectFitnessFunctionName { get; set; }
        
        /// <summary>
        /// SIMONObject가 갖는 Property 들의 DNA 집합을 정의합니다.
        /// </summary>
        [XmlIgnore()]
        public List<SIMONGene> PropertyDNA { get; set; }

        public SIMONObject()
        {
            Properties = new List<SIMONProperty>();
            Actions = new List<SIMONAction>();
            PropertyDNA = new List<SIMONGene>();
        }

        /// <summary>
        /// 다른 SIMONObject로부터 모든 Object 특성을 Copy 합니다.
        /// </summary>
        /// <param name="objInfo">Copy 대상이 되는 SIMONObject입니다.</param>
        private void CopyFromObject(SIMONObject objInfo)
        {
            ObjectID = objInfo.ObjectID;
            Properties = objInfo.Properties;
            Actions = objInfo.Actions;
        }

        /// <summary>
        /// SIMONObject 자신의 특성을 가진 새로운 객체 Copy 본을 생성합니다.
        /// </summary>
        /// <returns>동일 AI 특성을 지는 SIMONObject</returns>
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
        /// <param name="projectName">프로젝트 단위로 관리시, 폴더 이름입니다.</param>
        /// <param name="fileName">Object를 명시하는 Definition 파일입니다.</param>
        public void LoadObjectDefinition(string projectName, string fileName)
        {
            string definitionPath = @"\" + projectName + @"\" + fileName;

            SIMONObject xmlObject = (SIMONObject)SIMONUtility.GetInstance().DeserializeObject(definitionPath);
            CopyFromObject(xmlObject);
        }

        /// <summary>
        /// DefinitionFile로 현재 SIMONObject 정보를 기록해둡니다.
        /// </summary>
        /// <param name="projectName">Definition을 저장할 Project 경로입니다.</param>
        /// <param name="fileName">Project 내에서 명시할 Definition 이름입니다.</param>
        public void SaveObjectDefinition(string projectName, string fileName)
        {
            string definitionPath = @"\" + projectName + @"\" + fileName;
            SIMONObject thisObject = CopyToObject();
            SIMONUtility.GetInstance().SerializeObject(definitionPath, thisObject);
        }

        /// <summary>
        /// 입력된 propertyName에 해당하는 SIMONObject의 Property Value 값을 반환합니다.
        /// </summary>
        /// <param name="propertyName">찾고자 하는 Property의 고유 이름입니다.</param>
        /// <returns>반환하는 SIMONObject의 Property Value입니다.</returns>
        public Double GetPropertyElement(string propertyName)
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                if (Properties[i].PropertyName.Equals(propertyName))
                {
                    return Properties[i].PropertyValue;
                }
            }
            throw new SIMONFramework.KeyNotFoundException(SIMONConstants.EXP_KEY_NOT_FOUND_MSG + propertyName.ToString());
        }

        /// <summary>
        /// 입력된 propertyName에 해당하는 SIMONObject의 Property Value 값을 수정합니다.
        /// </summary>
        /// <param name="propertyName">Property Value를 수정할 대상 Property 이름입니다.</param>
        /// <param name="propertyValue">수정할 Property Value 값입니다.</param>
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
            throw new SIMONFramework.KeyNotFoundException(SIMONConstants.EXP_KEY_NOT_FOUND_MSG + propertyName.ToString());
        }

        /// <summary>
        /// Action 이름에 해당하는 ActionObject를 반환합니다.
        /// </summary>
        /// <param name="actionName">SIMONAction 객체를 가져올 action 이름입니다.</param>
        /// <returns>action 이름을 갖는 SIMONAction 객체입니다. 존재하지 않을 경우 null을 반환합니다.</returns>
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
        /// <param name="actionName">Action 유전자를 가져올 action 이름입니다.</param>
        /// <returns>action 이름을 갖는 유전자 셋입니다. 존재하지 않을 경우 null을 반환합니다.</returns>
        public List<SIMONGene> GetActionGeneElement(string actionName)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionName.Equals(actionName))
                    return Actions[i].ActionDNA;
            }
            return null;
        }

        /// <summary>
        /// 입력된 actionName에 해당하는 SIMONObject의 Action값 DNA를 newDNA로 지정합니다.
        /// </summary>
        /// <param name="actionName">수정할 ActionDNA를 가지는 action 이름입니다.</param>
        /// <param name="newDNA">수정할 새로운 DNA 값입니다.</param>
        public void SetActionGeneElement(string actionName, List<SIMONGene> newDNA)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionName.Equals(actionName))
                {
                    Actions[i].ActionDNA = newDNA;
                    return;
                }
            }
            throw new SIMONFramework.KeyNotFoundException(SIMONConstants.EXP_KEY_NOT_FOUND_MSG + actionName.ToString());
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
        /// <summary>
        /// SIMONAction 객체를 고유 지칭하는 Action 이름에 대한 정의입니다.
        /// </summary>
        [XmlElement("ActionName")]
        public String ActionName { get; set; }

        /// <summary>
        /// ActionFunction의 이름을 정의합니다.
        /// </summary>
        [XmlElement("ActionFunctionName")]
        public String ActionFunctionName { get; set; }

        /// <summary>
        /// ExecutionFunction의 이름을 정의합니다.
        /// </summary>
        [XmlElement("ExecutionFunctionName")]
        public String ExecutionFunctionName { get; set; }

        /// <summary>
        /// FitnessFunction의 이름을 정의합니다.
        /// </summary>
        [XmlElement("FitnessFunctionName")]
        public String FitnessFunctionName { get; set; }

        /// <summary>
        /// ActionDNA 리스트를 정의합니다.
        /// </summary>
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
        /// <param name="elementName">가중치 검사를 수행할 elementName 키값입니다.</param>
        /// <returns>가중치를 반환합니다.</returns>
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
                throw new SIMONFramework.KeyNotFoundException(SIMONConstants.EXP_KEY_NOT_FOUND_MSG + elementName.ToString());
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
                throw new SIMONFramework.KeyNotFoundException(SIMONConstants.EXP_KEY_NOT_FOUND_MSG + elementName.ToString());
            }
            return weight;
        }
        
        /// <summary>
        /// elementName에 해당하는 이름을 가진 DNA내의 단일 염색체를 반환합니다.
        /// </summary>
        /// <param name="elementName">조회할 DNA의 이름입니다.</param>
        /// <returns>DNA 값입니다. 존재하지 않을 경우 null을 반환합니다.</returns>
        public SIMONGene SelectDNA(string elementName)
        {
            foreach (SIMONGene aGene in ActionDNA)
                if (aGene.ElementName.Equals(elementName))
                    return aGene;
            return null;
        }
        
        /// <summary>
        /// DNA 단일 염색체를 DNA에 삽입합니다.
        /// </summary>
        /// <param name="elementName">삽입할 DNA 요소의 이름입니다.</param>
        /// <param name="elementValue">삽입할 DNA 요소의 값입니다.</param>
        public void InsertDNA(string elementName, double elementValue)
        {
            //DNA 리스트에 이름을 키 값으로 하는 element value를 삽입한다.
            SIMONGene actionGene = new SIMONGene();
            actionGene.ElementName = elementName;
            actionGene.ElementWeight = elementValue;
            ActionDNA.Add(actionGene);
        }
        /// <summary>
        /// DNA 단일 염색체를 DNA에 삽입합니다.
        /// </summary>
        /// <param name="actionGene">삽입할 염색체 객체입니다.</param>
        public void InsertDNA(SIMONGene actionGene)
        {
            ActionDNA.Add(actionGene);
        }
        /// <summary>
        /// DNA 단일 염색체를 DNA에 삽입합니다.
        /// </summary>
        /// <param name="actionGenome">삽입할 DNA 가변 배열입니다.</param>
        public void InsertDNA(params SIMONGene[] actionGenome)
        {
            ActionDNA.AddRange(actionGenome);
        }

        /// <summary>
        /// elementName을 이름으로 하는 DNA 내의 단일 염색체를 삭제합니다.
        /// </summary>
        /// <param name="elementName">삭제할 단일 염색체 이름입니다.</param>
        public void DeleteDNA(string elementName)
        {
            //DNA 리스트에 이름을 키 값으로 하는 element value를 삭제한다.
            ActionDNA.Remove(SelectDNA(elementName));
        }

        /// <summary>
        /// elemenetName을 이름으로 하는 DNA 내의 단일 염색체의 값을 elementValue로 수정합니다.
        /// </summary>
        /// <param name="elementName">수정할 단일 염색체 이름입니다.</param>
        /// <param name="elementValue">수정할 단일 염색체 값입니다.</param>
        public void EditDNA(string elementName, double elementValue)
        {
            foreach (SIMONGene gene in ActionDNA)
            {
                if (gene.ElementName.Equals(elementName))
                    gene.ElementWeight = elementValue;
            }
            throw new SIMONFramework.KeyNotFoundException(SIMONConstants.EXP_KEY_NOT_FOUND_MSG + elementName.ToString());
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
        /// <summary>
        /// DNA 염색체 이름입니다.
        /// </summary>
        [XmlElement("ElementName")]
        public String ElementName { get; set; }

        /// <summary>
        /// DNA 염색체 가중치입니다.
        /// </summary>
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