

/*
 * 
 * SIMON Framework 의 Main Routine을 정의하는 클래스입니다.
 * 
 * 이 클래스는 다음과 같은 기능을 구현합니다.
 * 
 * 
 *  
 */



using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFramework 전반을 관리하는 클래스로 사용됩니다. 프레임워크의 모든 기능을 패키징화해서 이용가능합니다.
    /// </summary>
    public sealed class SIMONManager                                                                //SIMONManager는 다른 클래스에서 상속받는 것이 불가능하도록 구현...?!
    {
        public SIMONUtility SimonUtility { get; private set; }                                      //싱글턴 형식으로 사용하는 SIMONUtility 객체.
        public SIMONCollection SimonObjectCollections { get; set; }                                 //고유한 Object ID라는 키값을 통해서 SIMONObject에 접근해서 객체를 얻어온다.
        public SIMONDataManager SimonDataManager { get; private set; }                              //파일 입출력에 대한 비동기작업을 제공하는 객체.

        private Dictionary<string, SIMONObject> SimonDefinitionObjects { get; set; }                //SIMON Definition Object들에 대한 Template를 저장해놓고 복사해서 사용하게끔 하는 Dictionary ADT.
        public Dictionary<string, SIMONFunction> SimonFunctions { get; set; }                       //보정 필요. SimonFramework에서 사용되는 함수들을 Dictionary형태로 조회해서 반환하는 ADT.

        public SIMONIntelligence IntelligenceManager { get; private set; }

        public int LearningCount { get; set; }
        private int LearningPoint;

        public SIMONManager()
        {
            SimonUtility = SIMONUtility.GetInstance();
            SimonObjectCollections = new SIMONCollection();
            SimonDataManager = new SIMONDataManager();
            SimonDefinitionObjects = new Dictionary<string, SIMONObject>();
            SimonFunctions = new Dictionary<string, SIMONFunction>();

            IntelligenceManager = new SIMONIntelligence();

            LearningCount = SIMONConstants.DEFAULT_LEARNING_COUNT;
            LearningPoint = SIMONConstants.DEFAULT_LEARNING_POINT;

            CreateWorkPath();
        }

        public SIMONManager(int learningCount, int learningPoint)
        {
            SimonUtility = SIMONUtility.GetInstance();
            SimonObjectCollections = new SIMONCollection();
            SimonDataManager = new SIMONDataManager();

            SimonDefinitionObjects = new Dictionary<string, SIMONObject>();
            SimonFunctions = new Dictionary<string, SIMONFunction>();

            IntelligenceManager = new SIMONIntelligence();

            this.LearningCount = learningCount;
            this.LearningPoint = learningPoint;

            CreateWorkPath();
        }

        private void CreateWorkPath()
        {
            DirectoryInfo dInfo;
            if (Directory.Exists(Directory.GetCurrentDirectory() + SIMONConstants.API_ROOT_PATH))
                return;
            dInfo = Directory.CreateDirectory(Directory.GetCurrentDirectory() + SIMONConstants.API_ROOT_PATH);
            if (!dInfo.Exists)
                throw new DirectoryNotFoundException("Cannot make API Root Directory");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH);
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + SIMONConstants.API_HISTORY_PATH);
        }

        /// <summary>
        /// projectName에 맞는 Definition 항목들을 모두 로드해서 SIMON Framework의 환경을 구축합니다. 툴을 통한 개발을 돕습니다. projectName이 null이면 definition path 전체를 검색합니다.
        /// </summary>
        /// <param name="projectName"></param>
        public void LoadWorkSpace(string projectName)
        {
            string workPath;
            if (projectName.Equals(null))
                workPath = Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH;
            else
                workPath = Directory.GetCurrentDirectory() + SIMONConstants.API_DEFINITION_PATH + @"\" + projectName;
            string[] fileNames = Directory.GetFiles(workPath);

            foreach (string name in fileNames)
            {
                FileInfo fInfo = new FileInfo(name);
                if (fInfo.Extension.Equals(".xml"))
                {
                    SIMONObject newObject = SimonUtility.DeserializeObject(fInfo.FullName);
                    newObject.UpdatePropertyDNA();
                    SimonDefinitionObjects.Add(newObject.ObjectID, newObject);                                                  //SIMONDefinitionObjects에 Definition 파일로부터 로드한 새로운 객체를 등록한다.
                    for (int i = 0; i < newObject.Actions.Count; i++)
                    {
                        AddMethod(newObject.Actions[i].ActionFunctionName);                                                     //SIMONObject의 ActionFunction을 등록합니다.
                        AddMethod(newObject.Actions[i].ExecutionFunctionName);                                                  //SIMONObject의 ExecutionFunction을 등록합니다.
                        AddMethod(newObject.Actions[i].FitnessFunctionName);                                                    //SIMONObject의 FitnessFunction을 등록합니다.
                    }
                }
            }
        }

        /// <summary>
        /// SIMONManager WorkSpace 전부를 초기화합니다.
        /// </summary>
        public void CleanWorkSpace()
        {
            SimonObjectCollections.Clear();
            SimonDefinitionObjects.Clear();
            SimonFunctions.Clear();
        }

        /// <summary>
        /// Definition 으로부터 정의된 Workspace를 초기화합니다.
        /// </summary>
        public void CleanDefinitionWorkSpace()
        {
            SimonDefinitionObjects.Clear();
            SimonFunctions.Clear();
        }

        /// <summary>
        /// 지정된 이름의 파일로부터 SIMONObject target에 대한 History Record를 전부 로드해서 2차원 배열로 반환합니다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sObject"></param>
        /// <returns></returns>
        public SIMONProperty[][] LoadHistory(string fileName, SIMONObject targetObject)
        {
            SIMONDataIOCommand ioCmd = new SIMONDataIOCommand();
            ioCmd.order = SIMONDataIO.READ;
            ioCmd.fileName = fileName;
            ioCmd.contents = "";

            IAsyncResult aResult = SimonDataManager.Service(ref ioCmd);
            bool readErr = false;
            string readStr = "";
            int lineCnt = 0;
            SimonDataManager.ReadResult(ref readErr, ref readStr, ref lineCnt, aResult);
            char delimiter = ',';
            
            SIMONProperty[][] historyData = new SIMONProperty[lineCnt][];
            for (int i = 0; i < lineCnt; i++)
            {
                historyData[i] = new SIMONProperty[targetObject.Properties.Count];
                for (int j = 0; j < targetObject.Properties.Count; j++)
                    historyData[i][j] = new SIMONProperty();
            }
            int prevDelimIdx = 0;
            int propIdx = 0;
            int historyLineIdx = 0;

            for (int i = 0; i < readStr.Length; i++)
            {
                if (readStr[i].Equals(delimiter))
                {
                    string sVal = "";
                    for (int j = prevDelimIdx+1; j < i; j++)
                    {
                        sVal += readStr[j];
                    }
                    double dval = Double.Parse(sVal);
                    prevDelimIdx = i;
                    historyData[historyLineIdx][propIdx].PropertyName = targetObject.Properties[propIdx].PropertyName;
                    historyData[historyLineIdx][propIdx].PropertyValue = dval;
                    propIdx++;
                }
                if (propIdx == targetObject.Properties.Count)
                {
                    historyLineIdx++;
                    propIdx = 0;
                }
            }

            return historyData;
        }

        /// <summary>
        /// fileName 을 경로로 하는 히스토리 파일에 대해서 targetObject의 프로퍼티 내용들을 기록합니다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="targetObject"></param>
        public void SaveHistory(string fileName, SIMONObject targetObject)
        {
            SIMONDataIOCommand iocmd = new SIMONDataIOCommand();
            iocmd.order = SIMONDataIO.WRITE;
            iocmd.fileName = System.IO.Directory.GetCurrentDirectory() + fileName;
            iocmd.contents = "";

            string delimiter = ",";
            string linefeed = "\n";

            if (!File.Exists(iocmd.fileName))
            {
                for (int i = 0; i < targetObject.Properties.Count; i++)
                    iocmd.contents += targetObject.Properties[i].PropertyName + delimiter;
                iocmd.contents += linefeed;
            }

            for (int i = 0; i < targetObject.Properties.Count; i++)
                iocmd.contents += targetObject.Properties[i].PropertyValue + delimiter;
            iocmd.contents += linefeed;

            IAsyncResult aResult = SimonDataManager.Service(ref iocmd);
            bool error = false;
            SimonDataManager.WriteResult(ref error, aResult);
        }

        /// <summary>
        /// SIMONObject를 Definition 의 형태로 Publish합니다.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="source"></param>
        public void PublishObject(string fileName, SIMONObject source)
        {
            SimonUtility.SerializeObject(fileName, source);
        }

        /// <summary>
        /// 그룹에 대한 ActionMap을 구현합니다.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private SIMONCollection CreateActionMap(SIMONCollection group)
        {
            SIMONCollection actionMap = new SIMONCollection();
            for (int i = 0; i < group.Count; i++)
            {
                SIMONObject element = (SIMONObject)group.ValueOfIndex(i);
                for (int j = 0; j < element.Actions.Count; j++)
                {
                    if (!actionMap.Contains(element.Actions[j].ActionName))
                    {
                        actionMap.Add(element.Actions[j].ActionName, new List<SIMONObject>());
                    }
                    DictionaryEntry val = (DictionaryEntry)actionMap[element.Actions[j].ActionName];
                    var mapElement = val.Value;
                    ((List<SIMONObject>)mapElement).Add(element);
                    actionMap[element.Actions[j].ActionName] = mapElement;
                }
            }
            return actionMap;
        }

        /// <summary>
        /// SIMONFramework의 분류 -> 판단 -> 행동 -> 학습 으로 이어지는 메인 루틴을 구현합니다.
        /// </summary>
        /// <param name="customLearnPoint">customLearnPoint 가 0 이면 Default History 기록 루틴, -1 이면 History 기록 루틴을 취소합니다.</param>
        public void RunRoutine(int customLearnPoint)
        {
            for (int i = 0; i < SimonObjectCollections.Count; i++)
            {
                SIMONObject elementObject = (SIMONObject)SimonObjectCollections.ValueOfIndex(i);
                SIMONObject[] otherObjects = new SIMONObject[SimonObjectCollections.Count - 1];
                int otherObjectsCnt = 0;
                double[] actionValueTable = new double[elementObject.Actions.Count];
                double actionMaxValue = SIMONConstants.MINIMUM_CMP_VALUE;
                int actionMaxIndex = SIMONConstants.MINIMUM_CMP_VALUE;

                /**********************************     분류      **************************************/

                for (int j = 0; j < SimonObjectCollections.Count; j++)
                    if (!SimonObjectCollections.ValueOfIndex(j).Equals(elementObject))
                        otherObjects[otherObjectsCnt++] = (SIMONObject)SimonObjectCollections.ValueOfIndex(j);

                /***************************************************************************************/

                /**********************************     판단      **************************************/

                List<int> actionMaxList = new List<int>();
                for (int j = 0; j < elementObject.Actions.Count; j++)
                {
                    object methodReturnVal = RunMethod(elementObject.Actions[j].ActionFunctionName, elementObject, otherObjects);
                    if (methodReturnVal.Equals(null))
                        actionValueTable[j] = SIMONConstants.DEFAULT_LEARNING_NULL;
                    else
                        actionValueTable[j] = (double)methodReturnVal;

                    if (actionValueTable[j] > actionMaxValue)
                    {
                        actionMaxValue = actionValueTable[j];
                        actionMaxIndex = j;
                    }
                }

                for (int j = 0; j < elementObject.Actions.Count; j++)
                    if (actionValueTable[j].Equals(actionMaxValue))
                        actionMaxList.Add(j);

                /***************************************************************************************/

                /**********************************     실행      **************************************/

                if (actionMaxList.Count > 1)
                {
                    //같은 값들 사이에서 랜덤으로 선택하게 함.
                    Random rand = new Random();
                    int choice = rand.Next(0, actionMaxList.Count);
                    actionMaxIndex = actionMaxList[choice];
                }

                RunMethod(elementObject.Actions[actionMaxIndex].ExecutionFunctionName, elementObject, otherObjects);

                /***************************************************************************************/


                /**********************************     기록      **************************************/

                if (-1 == customLearnPoint)
                    continue;
                else if ((0 < customLearnPoint) && (LearningCount == customLearnPoint))
                    SaveHistory(SIMONConstants.API_HISTORY_PATH + @"\" + elementObject.ObjectID + ".csv", elementObject);
                else if ((0 == customLearnPoint) && (LearningCount == LearningPoint))
                    SaveHistory(SIMONConstants.API_HISTORY_PATH + @"\" + elementObject.ObjectID + ".csv", elementObject);

                /***************************************************************************************/
            }
            if (-1 != customLearnPoint)
            {
                if ((0 < customLearnPoint) && (LearningCount >= customLearnPoint))
                    LearningCount = 0;
                else if ((0 == customLearnPoint) && (LearningCount >= LearningPoint))
                    LearningCount = 0;
                LearningCount++;
            }
        }

        /// <summary>
        /// SIMONFramework에서 제공되는 유전 알고리즘 클래스에 대한 옵션값을 설정합니다.
        /// </summary>
        /// <param name="optionInfo">GeneticAlgorithm 클래스의 프로퍼티값</param>
        public void ConfigureGeneticLearn(SIMONGeneticAlgorithm.GeneOption optionInfo)
        {
            IntelligenceManager.ConfigureLearning(AlgorithmForAI.GENETIC, optionInfo);
        }

        /// <summary>
        /// SimonCollection들에 대한 History를 이용해서 학습 루틴을 구현합니다.
        /// </summary>
        public void LearnRoutine(SIMONCollection targetGroup)
        {
            /**********************************     학습      **************************************/
            
            SIMONCollection newActionMap = CreateActionMap(targetGroup);

            IntelligenceManager.Learn(targetGroup, newActionMap, SimonFunctions);

            IntelligenceManager.LearnProperty(targetGroup, SimonFunctions);

            /***************************************************************************************/
        }

        /// <summary>
        /// 학습률을 적용해서 SIMONCollection들에 대한 History를 이용해서 학습 효과를 조절할 수 있는 학습 루틴을 구현합니다.
        /// </summary>
        /// <param name="targetGroup"></param>
        /// <param name="learningRate"></param>
        public void LearnSimulate(SIMONCollection targetGroup, double learningRate)
        {
            /**********************************     학습      **************************************/

            SIMONCollection newActionMap = CreateActionMap(targetGroup);

            IntelligenceManager.LearnAsync(targetGroup, newActionMap, SimonFunctions, learningRate);

            IntelligenceManager.LearnPropertyAsync(targetGroup, SimonFunctions, learningRate);

            /***************************************************************************************/
        }

        /// <summary>
        /// 학습에 대한 바로 이전 단계의 결과값을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public object LearnResult()
        {
            if (IntelligenceManager.isLearning)
                return null;
            return IntelligenceManager.LearnResult;
        }

        /// <summary>
        /// 매개변수로 전달받은 SIMONObject 독립된 객체에 관한 학습 동작을 수행합니다.
        /// </summary>
        /// <param name="sObject"></param>
        public void TeachObject(SIMONObject sObject)
        {
            SIMONCollection subCollection = new SIMONCollection();
            subCollection.Add(sObject.ObjectID, sObject);
            SIMONCollection subActionMap = CreateActionMap(subCollection);
            IntelligenceManager.Learn(subCollection, subActionMap, SimonFunctions);
            IntelligenceManager.LearnProperty(subCollection, SimonFunctions);
        }

        /// <summary>
        /// DefinitionObjects 로부터 정의된 Object 들에 대한 카피본을 반환합니다.        -> 자체 Dictionary Collection으로 만들경우 해당 클래스 내에서 새로 짜줘야 한다.
        /// </summary>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public SIMONObject CopyDefinitionObject(string objectID)
        {
            SIMONObject copyObject = new SIMONObject();
            copyObject = SimonDefinitionObjects[objectID];
            return copyObject;
        }

        /// <summary>
        /// SIMONManager에 등록해서 사용할 새로운 SIMONCollection을 생성해서 반환합니다.
        /// </summary>
        /// <returns></returns>
        public SIMONCollection CreateSIMONGroup()
        {
            SIMONCollection newGroup = new SIMONCollection();
            return newGroup;
        }

        /// <summary>
        /// key값을 이용하여 SIMONObjectCollection의 SIMONObject 요소를 반환합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SIMONObject CollectionElement(object key)
        {
            return (SIMONObject)SimonObjectCollections[key];
        }
        /// <summary>
        /// index값을 이용하여 SIMONObjectCollection의 SIMONObject 요소를 반환합니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SIMONObject CollectionElement(int index)
        {
            return (SIMONObject)SimonObjectCollections.ValueOfIndex(index);
        }

        /// <summary>
        /// SIMONObjectCollection에 SIMONObject를 등록합니다.
        /// </summary>
        /// <param name="sObject"></param>
        public void RegisterSIMONObject(SIMONObject sObject)
        {
            SimonObjectCollections.Add(sObject.ObjectID, sObject);
        }
        /// <summary>
        /// SIMONObjectCollection에 SIMONObject를 등록합니다.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sObject"></param>
        public void RegisterSIMONObject(string key, SIMONObject sObject)
        {
            SimonObjectCollections.Add(key, sObject);
        }

        /// <summary>
        /// SIMONObjectCollection으로부터 SIMONObject를 삭제합니다.
        /// </summary>
        /// <param name="key"></param>
        public void UnregisterSIMONObject(string key)
        {
            SimonObjectCollections.Remove(key);
        }
        /// <summary>
        /// SIMONObjectCollection으로부터 SIMONObject를 삭제합니다.
        /// </summary>
        /// <param name="sObject"></param>
        public void UnregisterSIMONObject(SIMONObject sObject)
        {
            SimonObjectCollections.Remove(sObject.ObjectID);
        }

        /// <summary>
        /// Manager를 통해서 Default Class(SIMONUserFunction)의 methodName에 일치하는 method를 동적으로 등록할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName"></param>
        public void AddMethod(string methodName)
        {
            string className = SIMONConstants.DEFAULT_FUNCTION_CLSNAME;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type defaultClassType = assembly.GetType(className);
            MethodInfo methodInfo = defaultClassType.GetMethod(methodName);
            SimonFunctions.Add(methodName, (SIMONFunction)Delegate.CreateDelegate(typeof(SIMONFunction), methodInfo));
        }
        /// <summary>
        /// Manager를 통해서 Default Class(SIMONUserFunction)의 methodName에 일치하는 method를 동적으로 등록할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="functionPointer"></param>
        public void AddMethod(string methodName, SIMONFunction functionPointer)
        {
            SimonFunctions.Add(methodName, functionPointer);
        }
        /// <summary>
        /// Manager를 통해서 Default Class(SIMONUserFunction)의 methodName에 일치하는 method를 동적으로 등록할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="classType"></param>
        public void AddMethod(string methodName, Object classType)
        {
            MethodInfo methodInfo = classType.GetType().GetMethod(methodName);
            if (methodInfo.Equals(null))
            {
                throw new MissingMethodException("Method Linking Error");
            }
            SimonFunctions.Add(methodName, (SIMONFunction)Delegate.CreateDelegate(typeof(SIMONFunction), methodInfo));
        }

        /// <summary>
        /// Manager를 통해서 Default Class(SIMONUserFunction)의 methodName과 FunctionPointer를 전달함으로써 Method를 동적으로 등록할 수 있는 기능을 제공합니다. 존재하는 key에 해당하는 value의 경우 대체됩니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="functionPointer"></param>
        public void InsertMethod(string methodName, SIMONFunction functionPointer)
        {
            if (SimonFunctions.ContainsKey(methodName))
            {
                return;
            }
            else
                AddMethod(methodName, functionPointer);
        }
        /// <summary>
        /// Manager를 통해서 methodName과 FunctionPointer를 전달함으로써 Method를 동적으로 삽입할 수 있는 기능을 제공합니다. 존재하는 key에 해당하는 value의 경우 대체됩니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="functionPointer"></param>
        public void InsertMethod(string methodName, Object classType)
        {
            MethodInfo methodInfo = classType.GetType().GetMethod(methodName);                        //함수포인터가 가리키는 함수에 대한 정보를 취득.
            if (methodInfo.Equals(null))
            {
                throw new MissingMethodException("Method Linking Error");
            }
            if (SimonFunctions.ContainsKey(methodName))
            {
                return;
                //SimonFunctions[methodName] = (SIMONFunction)Delegate.CreateDelegate(typeof(SIMONFunction), methodInfo);     //함수포인터를 해당 Dictionary의 Value 값에 매핑시킴.
            }
            else
                AddMethod(methodName, classType);
        }

        /// <summary>
        /// Manager를 통해서 methodName을 이용해서 기존에 등록된 Function Pointer를 동적으로 해제할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName"></param>
        public void RemoveMethod(string methodName)
        {
            SimonFunctions.Remove(methodName);
        }

        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수를 실행합니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public Object RunMethod(string methodName)
        {
            return SimonFunctions[methodName].Invoke(null, null);
        }
        public Object RunMethod(string methodName, SIMONObject thisObject)
        {
            return SimonFunctions[methodName].Invoke(thisObject, null);
        }
        public Object RunMethod(string methodName, SIMONObject thisObject, SIMONObject[] theOtherObjects)
        {
            return SimonFunctions[methodName].Invoke(thisObject, theOtherObjects);
        }

        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수에 대한 비동기 작업을 수행합니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public IAsyncResult BeginMethod(string methodName)
        {
            IAsyncResult result = SimonFunctions[methodName].BeginInvoke(null, null, null, null);
            return result;
        }
        public IAsyncResult BeginMethod(string methodName, SIMONObject thisObject)
        {
            IAsyncResult result = SimonFunctions[methodName].BeginInvoke(thisObject, null, null, null);
            return result;
        }
        public IAsyncResult BeginMethod(string methodName, SIMONObject thisObject, SIMONObject[] theOtherObjects)
        {
            IAsyncResult result = SimonFunctions[methodName].BeginInvoke(thisObject, theOtherObjects, null, null);
            return result;
        }

        /// <summary>
        /// methodName을 이름으로 하고 비동기 작업결과를 처리해서 결과물을 반환하는 함수를 수행합니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="aResult"></param>
        /// <returns></returns>
        public object EndMethod(string methodName, IAsyncResult aResult)
        {
            return SimonFunctions[methodName].EndInvoke(aResult);
        }

    }

}