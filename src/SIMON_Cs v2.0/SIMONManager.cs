

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
using System.Dynamic;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFramework 전반을 관리하는 클래스로 사용됩니다. 프레임워크의 모든 기능을 패키징화해서 이용가능합니다.
    /// </summary>
    public sealed class SIMONManager                                                                //SIMONManager는 다른 클래스에서 상속받는 것이 불가능하도록 구현...?!
    {
        /// <summary>
        /// 싱글턴 형식으로 사용하는 SIMONUtility 객체.
        /// </summary>
        public SIMONUtility SimonUtility { get; private set; }
        
        /// <summary>
        /// 고유한 Object ID라는 키값을 통해서 SIMONObject에 접근해서 객체를 얻어온다.
        /// </summary>
        public SIMONCollection SimonObjectCollections { get; set; }
        
        /// <summary>
        /// 파일 입출력에 대한 비동기작업을 제공하는 객체.
        /// </summary>
        public SIMONDataManager SimonDataManager { get; private set; }

        /// <summary>
        /// SIMON Definition Object들에 대한 Template를 저장해놓고 복사해서 사용하게끔 하는 Dictionary ADT.
        /// </summary>
        private Dictionary<string, SIMONObject<SIMONProperty, SIMONAction>> SimonDefinitionObjects { get; set; }

        /// <summary>
        /// SimonFramework에서 사용되는 함수들을 Dictionary형태로 조회해서 반환하는 ADT.
        /// </summary>
//        public Dictionary<string, SIMONFunctionInterface<SIMONProperty, SIMONAction>> SimonFunctions { get; set; }
        public SIMONCollection SimonFunctions { get; set; }

        /// <summary>
        /// Framework에서 Intelligent Layer 를 구성하는 객체입니다.
        /// </summary>
        public SIMONIntelligence IntelligenceManager { get; private set; }

        /// <summary>
        /// Framework 내 학습 수행횟수입니다.
        /// </summary>
        public int LearningCount { get; set; }

        /// <summary>
        /// Framework 내 학습 Boundary 입니다.
        /// </summary>
        private int LearningPoint;


        private AlgorithmForAI AIModel = AlgorithmForAI.GENETIC;

        public SIMONManager(AlgorithmForAI AIModel)
        {
            SimonUtility = SIMONUtility.GetInstance();
            SimonObjectCollections = new SIMONCollection();
            SimonDataManager = new SIMONDataManager();
            SimonDefinitionObjects = new Dictionary<string, SIMONObject<SIMONProperty, SIMONAction>>();
            SimonFunctions = new SIMONCollection();

            IntelligenceManager = new SIMONIntelligence();

            LearningCount = SIMONConstants.DEFAULT_LEARNING_COUNT;
            LearningPoint = SIMONConstants.DEFAULT_LEARNING_POINT;


            this.AIModel = AIModel;


            CreateWorkPath();
        }

        public SIMONManager(int learningCount, int learningPoint)
        {
            SimonUtility = SIMONUtility.GetInstance();
            SimonObjectCollections = new SIMONCollection();
            SimonDataManager = new SIMONDataManager();

            SimonDefinitionObjects = new Dictionary<string, SIMONObject<SIMONProperty, SIMONAction>>();
            SimonFunctions = new SIMONCollection();//new Dictionary<string, SIMONFunction>();
//            SimonFunctions = new Dictionary<string,SIMONFunctionInterface<SIMONProperty,SIMONAction>>();

            IntelligenceManager = new SIMONIntelligence();

            this.LearningCount = learningCount;
            this.LearningPoint = learningPoint;

            CreateWorkPath();
        }

        /// <summary>
        /// SIMON Framework를 사용할 수 있는 Workspace 디렉터리 트리를 구조합니다.
        /// </summary>
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
        /// <param name="projectName">Workspace로 사용할 project 이름을 전달합니다.</param>
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
                    dynamic newObject = null;

                    switch (AIModel)
                    {
                        case AlgorithmForAI.GENETIC:
                            newObject = SimonUtility.DeserializeObject(fInfo.FullName, SIMONObjectType.SIMONGeneticObject);
                            newObject.UpdatePropertyDNA();
                            break;
                    }

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
        /// Group의 모든 내용을 비웁니다.
        /// </summary>
        /// <param name="Group">비울 대상 Group Collection입니다.</param>
        public void CleanGroup(SIMONCollection Group)
        {
            Group.Clear();
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
        /// <param name="fileName">history를 불러올 대상 파일입니다.</param>
        /// <param name="sObject">대상 targetObject입니다.</param>
        /// <returns>history를 반환할 2차원 배열 map.</returns>
        public T[][] LoadHistory<T, U>(string fileName, SIMONObject<T, U> targetObject) where T : SIMONProperty, new() where U : SIMONAction, new()
        {
            SIMONDataIOCommand ioCmd = new SIMONDataIOCommand();
            ioCmd.order = SIMONDataIO.READ;
            ioCmd.fileName = System.IO.Directory.GetCurrentDirectory() + fileName;
            ioCmd.contents = "";

            IAsyncResult aResult = SimonDataManager.Service(ref ioCmd);
            bool readErr = false;
            string readStr = "";
            int lineCnt = 0;
            SimonDataManager.ReadResult(ref readErr, ref readStr, ref lineCnt, aResult);
            char delimiter = ',';
            
            T[][] historyData = new T[lineCnt][];
            for (int i = 0; i < lineCnt; i++)
            {
                historyData[i] = new T[targetObject.Properties.Count];
                for (int j = 0; j < targetObject.Properties.Count; j++)
                    historyData[i][j] = new T();
            }
            int prevDelimIdx = 0;
            int propIdx = 0;
            int historyLineIdx = 0;

            for (int i = 0; i < readStr.Length; i++)
            {
                if (readStr[i].Equals(delimiter))
                {
                    string sVal = "";
                    double dval = 0;
                    for (int j = prevDelimIdx+1; j < i; j++)
                    {
                        sVal += readStr[j];
                    }
                    if (!Double.TryParse(sVal, out dval))
                    {
                        prevDelimIdx = i;
                        continue;
                    }
                    prevDelimIdx = i;
                    historyData[historyLineIdx][propIdx].PropertyName = targetObject.Properties[propIdx].PropertyName;
                    historyData[historyLineIdx][propIdx].PropertyValue = dval;
                    propIdx++;
                }
                if (propIdx == targetObject.Properties.Count && targetObject.Properties.Count != 0)
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
        /// <param name="fileName">History를 저장할 파일의 이름입니다.</param>
        /// <param name="targetObject">History로 만들 대상 SIMONObject입니다.</param>
        public void SaveHistory<T, U>(string fileName, SIMONObject<T, U> targetObject) where T : SIMONProperty where U : SIMONAction
        {
            SIMONDataIOCommand iocmd = new SIMONDataIOCommand();
            iocmd.order = SIMONDataIO.WRITE;
            iocmd.fileName = System.IO.Directory.GetCurrentDirectory() + fileName;
            iocmd.contents = "";

            string delimiter = ",";
            string linefeed = "\n";

            SIMONObject<T, U> newObj = SIMONObjectFactory.CastObject(targetObject);
            SIMONObjectFactory.ReflectObject<T, U>(ref newObj, targetObject);

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
        /// <param name="fileName">Object를 내보낼 파일 이름입니다.</param>
        /// <param name="source">내보낼 대상 Object입니다.</param>
        public void PublishObject(string fileName, SIMONGeneticObject source)
        {
            SimonUtility.SerializeObject(fileName, source);
        }

        /// <summary>
        /// 그룹에 대한 ActionMap을 구현합니다.
        /// </summary>
        /// <param name="group">ActionMap을 구조할 group입니다.</param>
        /// <returns>만들어진 ActionMap SIMONCollection을 반환합니다.</returns>
        private SIMONCollection CreateActionMap<T, U>(SIMONCollection group) where T : SIMONProperty where U : SIMONAction
        {
            SIMONCollection actionMap = new SIMONCollection();
            for (int i = 0; i < group.Count; i++)
            {
                //SIMONObject<T, U> element = (SIMONObject<T, U>)group.ValueOfIndex(i);

                dynamic element = group.ValueOfIndex(i);


                for (int j = 0; j < element.Actions.Count; j++)
                {
                    if (!actionMap.Contains((String)element.Actions[j].ActionName))
                    {
                        actionMap.Add(element.Actions[j].ActionName, new List<dynamic>());
                    }
                    DictionaryEntry val = (DictionaryEntry)actionMap[element.Actions[j].ActionName];
                    var mapElement = val.Value;
                    ((List<dynamic>)mapElement).Add(element);
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
                SIMONObject<SIMONProperty, SIMONAction> elementObject = (SIMONObject<SIMONProperty, SIMONAction>)SimonObjectCollections.ValueOfIndex(i);
                SIMONObject<SIMONProperty, SIMONAction>[] otherObjects = new SIMONObject<SIMONProperty, SIMONAction>[SimonObjectCollections.Count - 1];
                int otherObjectsCnt = 0;
                double[] actionValueTable = new double[elementObject.Actions.Count];
                double actionMaxValue = SIMONConstants.MINIMUM_CMP_VALUE;
                int actionMaxIndex = SIMONConstants.MINIMUM_CMP_VALUE;

                /**********************************     분류      **************************************/

                for (int j = 0; j < SimonObjectCollections.Count; j++)
                    if (!SimonObjectCollections.ValueOfIndex(j).Equals(elementObject))
                        otherObjects[otherObjectsCnt++] = (SIMONObject<SIMONProperty, SIMONAction>)SimonObjectCollections.ValueOfIndex(j);

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
            SIMONCollection newActionMap = null;

            switch(AIModel){
                case AlgorithmForAI.GENETIC:
                    newActionMap = CreateActionMap<SIMONGeneticProperty, SIMONGeneticAction>(targetGroup);
                    break;
            }

            IntelligenceManager.Learn(targetGroup, newActionMap, SimonFunctions);

            IntelligenceManager.LearnProperty(targetGroup, SimonFunctions);

            /***************************************************************************************/
        }



        /// <summary>
        /// 학습률을 적용해서 SIMONCollection들에 대한 History를 이용해서 학습 효과를 조절할 수 있는 학습 루틴을 구현합니다.
        /// </summary>
        /// <param name="targetGroup">Simulate를 적용시킬 대상 Group입니다.</param>
        /// <param name="learningRate">적용시킬 학습률입니다.</param>
        public void LearnSimulate(SIMONCollection targetGroup, double learningRate)
        {
            /**********************************     학습      **************************************/

            SIMONCollection newActionMap = null;

            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    newActionMap = CreateActionMap<SIMONGeneticProperty, SIMONGeneticAction>(targetGroup);
                    break;
            }

            //IntelligenceManager.LearnAsync(targetGroup, newActionMap, SimonFunctions, learningRate);

            //IntelligenceManager.LearnPropertyAsync(targetGroup, SimonFunctions, learningRate);

            /***************************************************************************************/
        }

        /// <summary>
        /// 학습에 대한 바로 이전 단계의 결과값을 반환합니다.
        /// </summary>
        /// <returns>학습에 대한 비동기 결과값.</returns>
        public object LearnResult()
        {
            if (IntelligenceManager.isLearning)
                return null;
            return IntelligenceManager.LearnResult;
        }

        /// <summary>
        /// 매개변수로 전달받은 SIMONObject 독립된 객체에 관한 학습 동작을 수행합니다.
        /// </summary>
        /// <param name="sObject">학습을 진행시킬 SIMONObject 단일객체.</param>
        public void TeachObject<T, U>(SIMONObject<T, U> sObject) where T : SIMONProperty where U : SIMONAction
        {
            SIMONCollection subCollection = new SIMONCollection();
            subCollection.Add(sObject.ObjectID, sObject);

            SIMONCollection subActionMap = CreateActionMap<T, U>(subCollection);

            IntelligenceManager.Learn(subCollection, subActionMap, SimonFunctions);
            IntelligenceManager.LearnProperty(subCollection, SimonFunctions);
        }

        /// <summary>
        /// SIMONManager에 등록해서 사용할 새로운 SIMONCollection을 생성해서 반환합니다.
        /// </summary>
        /// <returns>SIMONCollection 객체를 반환합니다.</returns>
        public SIMONCollection CreateSIMONGroup()
        {
            SIMONCollection newGroup = new SIMONCollection();
            return newGroup;
        }

        /// <summary>
        /// key값을 이용하여 SIMONObjectCollection의 SIMONObject 요소를 반환합니다.
        /// </summary>
        /// <param name="key">접근할 SIMONObjectCollection에 대한 Key입니다.</param>
        /// <returns>Key값에 대한 SIMONObjectCollection Element를 반환합니다.</returns>
        public SIMONObject<T, U> CollectionElement<T, U>(object key) where T : SIMONProperty where U : SIMONAction
        {
            return (SIMONObject<T, U>)SimonObjectCollections[key];
        }
        /// <summary>
        /// index값을 이용하여 SIMONObjectCollection의 SIMONObject 요소를 반환합니다.
        /// </summary>
        /// <param name="index">SIMONObjectCollection의 순차적 번호값입니다.</param>
        /// <returns>index값에 대한 SIMONObjectCollection Element를 반환합니다.</returns>
        public SIMONObject<T, U> CollectionElement<T, U>(int index) where T : SIMONProperty where U : SIMONAction
        {
            return (SIMONObject<T, U>)SimonObjectCollections.ValueOfIndex(index);
        }

        /// <summary>
        /// SIMONObjectCollection에 SIMONObject를 등록합니다.
        /// </summary>
        /// <param name="sObject">등록할 SIMONObject입니다.</param>
        public void RegisterSIMONObject<T, U>(SIMONObject<T, U> sObject) where T : SIMONProperty where U : SIMONAction
        {
        //    SIMONObject<T> newObj = SIMONObjectFactory.CastObject(sObject);
        //    SIMONObjectFactory.ReflectObject<T>(ref newObj, sObject);

            if(typeof(T) == typeof(SIMONGeneticProperty) && typeof(U) == typeof(SIMONGeneticAction))
                SimonObjectCollections.Add((sObject as SIMONGeneticObject).ObjectID, sObject);
        }
        /// <summary>
        /// SIMONObjectCollection에 SIMONObject를 등록합니다.
        /// </summary>
        /// <param name="key">등록할 SIMONObject의 Key값입니다.</param>
        /// <param name="sObject">등록할 SIMONObject입니다.</param>
        public void RegisterSIMONObject<T, U>(string key, SIMONObject<T, U> sObject) where T : SIMONProperty where U : SIMONAction
        {
            SimonObjectCollections.Add(key, sObject);
        }

        /// <summary>
        /// SIMONObjectCollection으로부터 SIMONObject를 삭제합니다.
        /// </summary>
        /// <param name="key">등록 해제할 SIMONObject Key값입니다.</param>
        public void UnregisterSIMONObject(string key)
        {
            SimonObjectCollections.Remove(key);
        }
        /// <summary>
        /// SIMONObjectCollection으로부터 SIMONObject를 삭제합니다.
        /// </summary>
        /// <param name="sObject">등록 해제할 SIMONObject Key값입니다.</param>
        public void UnregisterSIMONObject<T, U>(SIMONObject<T, U> sObject) where T : SIMONProperty where U : SIMONAction
        {
            SimonObjectCollections.Remove(sObject.ObjectID);
        }

        /// <summary>
        /// Manager를 통해서 Default Class(SIMONUserFunction)의 methodName에 일치하는 method를 동적으로 등록할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName">추가시킬 함수 이름입니다.</param>
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
        /// <param name="methodName">추가시킬 함수 이름입니다.</param>
        /// <param name="functionPointer">함수에 대한 직접 참조입니다.</param>
        public void AddMethod<T, U>(string methodName, SIMONFunctionInterface<T, U>.SIMONDelegate functionPointer) where T : SIMONProperty where U : SIMONAction
        {
            SimonFunctions.Add(methodName, functionPointer);
        }
        /// <summary>
        /// Manager를 통해서 전달된 함수 정보에 일치하는 method를 동적으로 등록할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="methodInfo"></param>
        public void AddMethod(string methodName, System.Reflection.MethodInfo methodInfo)
        {
            SimonFunctions.Add(methodName, Delegate.CreateDelegate(typeof(SIMONFunctionInterface<SIMONProperty, SIMONAction>), methodInfo));
        }


        /// <summary>
        /// Manager를 통해서 Default Class(SIMONUserFunction)의 methodName과 FunctionPointer를 전달함으로써 Method를 동적으로 등록할 수 있는 기능을 제공합니다. 존재하는 key에 해당하는 value의 경우 대체됩니다.
        /// </summary>
        /// <param name="methodName">삽입할 함수 이름입니다.</param>
        /// <param name="functionPointer">함수에 대한 직접참조입니다.</param>
        public void InsertMethod<T, U>(string methodName, SIMONFunctionInterface<T, U>.SIMONDelegate functionPointer) where T : SIMONProperty where U : SIMONAction
        {
            if (SimonFunctions.ContainsKey(methodName))
            {
                return;
            }
            else
                AddMethod<T, U>(methodName, functionPointer);
        }

        /// <summary>
        /// Manager를 통해서 함수의 정보를 전달함으로써 런타임에 Method 를 탐색하고 삽입할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodInfo">전달하고자하는 함수의 정보입니다.</param>
        public void InsertMethod(string methodName, System.Reflection.MethodInfo methodInfo)
        {
            if (SimonFunctions.ContainsKey(methodName))
            {
                return;
            }
            else
            {
                AddMethod(methodName, methodInfo);
            }
        }

       



        /// <summary>
        /// Manager를 통해서 methodName을 이용해서 기존에 등록된 Function Pointer를 동적으로 해제할 수 있는 기능을 제공합니다.
        /// </summary>
        /// <param name="methodName">삭제할 함수 이름입니다.</param>
        public void RemoveMethod(string methodName)
        {
            SimonFunctions.Remove(methodName);
        }

        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수를 실행합니다.
        /// </summary>
        /// <param name="methodName">실행할 함수의 이름입니다.</param>
        /// <returns>함수의 결과값입니다.</returns>
        public Object RunMethod(string methodName)
        {
            object retVal = null;
            switch(AIModel){
                case AlgorithmForAI.GENETIC:
                    retVal = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).Invoke(null, null);
                break;
            }
            return retVal;
        }
        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수를 실행합니다.
        /// </summary>
        /// <param name="methodName">실행할 함수의 이름입니다.</param>
        /// <param name="thisObject">함수에 전달할 주체 Parameter입니다.</param>
        /// <returns>함수의 결과값입니다.</returns>
        public Object RunMethod<T, U>(string methodName, SIMONObject<T, U> thisObject) where T : SIMONProperty where U : SIMONAction
        {

            object retVal = null;
            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    retVal = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).Invoke(thisObject as SIMONGeneticObject, null);
                    break;
            }
            return retVal;
            //return SimonFunctions[methodName].Invoke(thisObject, null);
        }
        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수를 실행합니다.
        /// </summary>
        /// <param name="methodName">실행할 함수의 이름입니다.</param>
        /// <param name="thisObject">함수에 전달할 주체 Parameter입니다.</param>
        /// <param name="theOtherObjects">함수에 전달할 객체 Parameter배열입니다.</param>
        /// <returns>함수의 결과값입니다.</returns>
        public Object RunMethod<T, U>(string methodName, SIMONObject<T, U> thisObject, SIMONObject<T, U>[] theOtherObjects) where T : SIMONProperty where U : SIMONAction
        {

            object retVal = null;
            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    retVal = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).Invoke(thisObject as SIMONGeneticObject, theOtherObjects as SIMONGeneticObject[]);
                    break;
            }
            return retVal;

//            return SimonFunctions[methodName].Invoke(thisObject, theOtherObjects);
        }

        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수에 대한 비동기 작업을 수행합니다.
        /// </summary>
        /// <param name="methodName">함수 이름입니다.</param>
        /// <returns>함수 실행에 대한 비동기 결과.</returns>
        public IAsyncResult BeginMethod(string methodName)
        {
//            IAsyncResult result = SimonFunctions[methodName].BeginInvoke(null, null, null, null);
            IAsyncResult result = null;
            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    result = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).BeginInvoke(null, null, null, null);
                    break;
            }
            return result;
        }
        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수에 대한 비동기 작업을 수행합니다.
        /// </summary>
        /// <param name="methodName">함수 이름입니다.</param>
        /// <param name="thisObject">함수에 전달할 주체 Parameter입니다.</param>
        /// <returns>함수 실행에 대한 비동기 결과.</returns>
        public IAsyncResult BeginMethod<T, U>(string methodName, SIMONObject<T, U> thisObject) where T : SIMONProperty where U : SIMONAction
        {
//            IAsyncResult result = SimonFunctions[methodName].BeginInvoke(thisObject, null, null, null);
            IAsyncResult result = null;
            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    result = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).BeginInvoke(thisObject as SIMONGeneticObject, null, null, null);
                    break;
            }
            return result;
        }
        /// <summary>
        /// methodName을 이름으로 하는 SimonFunction에 등록된 함수에 대한 비동기 작업을 수행합니다.
        /// </summary>
        /// <param name="methodName">함수 이름입니다.</param>
        /// <param name="thisObject">함수에 전달할 주체 Parameter입니다.</param>
        /// <param name="theOtherObjects">함수에 전달할 객체 Parameter 배열입니다.</param>
        /// <returns>함수 실행에 대한 비동기 결과.</returns>
        public IAsyncResult BeginMethod<T, U>(string methodName, SIMONObject<T, U> thisObject, SIMONObject<T, U>[] theOtherObjects) where T : SIMONProperty where U : SIMONAction
        {
//            IAsyncResult result = SimonFunctions[methodName].BeginInvoke(thisObject, theOtherObjects, null, null);
            IAsyncResult result = null;
            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    result = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).BeginInvoke(thisObject as SIMONGeneticObject, theOtherObjects as SIMONGeneticObject[], null, null);
                    break;
            }
            return result;
        }

        /// <summary>
        /// methodName을 이름으로 하고 비동기 작업결과를 처리해서 결과물을 반환하는 함수를 수행합니다.
        /// </summary>
        /// <param name="methodName">함수 이름입니다.</param>
        /// <param name="aResult">반환값을 추적할 비동기 결과 객체입니다.</param>
        /// <returns>비동기 함수에 대한 Return값입니다.</returns>
        public object EndMethod(string methodName, IAsyncResult aResult)
        {
            object retVal = null;

            switch (AIModel)
            {
                case AlgorithmForAI.GENETIC:
                    retVal = ((SIMONFunctionInterface<SIMONGeneticProperty, SIMONGeneticAction>.SIMONDelegate)SimonFunctions[methodName]).EndInvoke(aResult);
                    break;
            }
            return retVal;
//            return SimonFunctions[methodName].EndInvoke(aResult);
        }

    }

}