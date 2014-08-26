
/*
 * 
 * SIMONFramework 전반에서 사용하게 되는 SIMON AI 전반에 대한 기능을 구현하고 제공한다.
 * 
 * 
 * 
 *  
 */

using System;
using System.Collections;
using System.Collections.Generic;



namespace SIMONFramework
{
    /// <summary>
    /// Framework 내에서 사용하는 AI 알고리즘에 대한 enum값입니다.
    /// </summary>
    public enum AlgorithmForAI { GENETIC = 1, NEURALNETWORK = 2};


    public class AlgorithmFunctionInterface
    {

        /// <summary>
        /// Algorithm 을 수행하는 구현에 대한 함수 포인터 타입입니다.
        /// </summary>
        /// <param name="Group">Algorithm이 적용될 대상 그룹을 명시합니다.</param>
        /// <param name="ActionMap">Algorithm 적용 대상에 대한 Map 구조를 명시합니다.</param>
        /// <param name="Functions">Algorithm 이 적용되는데 사용될 사용자 함수의 Dictionary입니다.</param>
        /// <returns>구현 함수의 반환값입니다.</returns>
        public delegate object AlgorithmDelegate(SIMONCollection Group, SIMONCollection ActionMap, SIMONCollection Functions);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> Functions);

        /// <summary>
        /// Algorithm 을 수행하는 구현에 대한 함수 포인터 타입입니다.
        /// </summary>
        /// <param name="Group">Algorithm이 적용될 대상 그룹을 명시합니다.</param>
        /// <param name="ActionMap">Algorithm 적용 대상에 대한 Map 구조를 명시합니다.</param>
        /// <param name="Functions">Algorithm 이 적용되는데 사용될 사용자 함수의 Dictionary입니다.</param>
        /// <param name="learningRate">학습률을 명시합니다.</param>
        /// <returns>구현 함수의 반환값입니다.</returns>
        public delegate object AlgorithmLearningDelegate(SIMONCollection Group, SIMONCollection ActionMap, SIMONCollection Functions, double learningRate);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> Functions, double learningRate);

        /// <summary>
        /// Property에 관한 Algorithm 을 수행하는 구현에 대한 함수 포인터 타입입니다.
        /// </summary>
        /// <param name="Group">Algorithm이 적용될 대상 그룹을 명시합니다. </param>
        /// <param name="Functions">Algorithm 이 적용되는데 사용될 사용자 함수의 Dictionary입니다.</param>
        /// <param name="learningRate">학습률을 명시합니다.</param>
        /// <returns></returns>
        public delegate object AlgorithmLearnPropDelegate(SIMONCollection Group, SIMONCollection Functions, double learningRate);//Dictionary<string, SIMONFunctionInterface<T>.SIMONDelegate> Functions, double learningRate);
    }


    /// <summary>
    /// SIMON Algorithm을 통해서 AI를 제공하고, 학습 모듈을 연동시킬 수 있는 모듈을 클래스화합니다.
    /// </summary>
    public class SIMONIntelligence
    {
        /// <summary>
        /// Intelligence Layer에서 사용하게 될 Algorithm 을 명세합니다.
        /// </summary>
        public AlgorithmForAI IntelligenceAlgorithm { get; private set; }

        /// <summary>
        /// Algorithm Layer의 구현에 대한 지칭자입니다.
        /// </summary>
        public SIMONAlgorithm AlgorithmPerformer { get; private set; }

        //public SIMONGeneticAlgorithm GeneticAlgorithm { get; private set; }

        private AlgorithmFunctionInterface.AlgorithmDelegate AlgorithmInvoker;
        private AlgorithmFunctionInterface.AlgorithmLearningDelegate AlgorithmLearnInvoker;
        private AlgorithmFunctionInterface.AlgorithmLearnPropDelegate AlgorithmLearnPropInvoker;
        private AsyncCallback LearnCallbackInvoker;
        private AsyncCallback LearnPropCallbackInvoker;

        /// <summary>
        /// 학습 결과값을 갖습니다.
        /// </summary>
        public object LearnResult { get; private set; }

        /// <summary>
        /// 현재 학습중인지 상태값을 갖습니다.
        /// </summary>
        public bool isLearning { get; private set; }

        public SIMONIntelligence()
        {
            IntelligenceAlgorithm = AlgorithmForAI.GENETIC;
            AlgorithmPerformer = new SIMONGeneticAlgorithm();

            //AlgorithmInvoker = new AlgorithmDelegate(AlgorithmPerformer.Implementation);
            //AlgorithmLearnInvoker = new AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
            //AlgorithmLearnPropInvoker = new AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);

            //AlgorithmInvoker = new AlgorithmFunctionInterface<SIMONElement>.AlgorithmDelegate(AlgorithmPerformer.Implementation);
            //AlgorithmLearnInvoker = new AlgorithmFunctionInterface<SIMONElement>.AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
            //AlgorithmLearnPropInvoker = new AlgorithmFunctionInterface<SIMONElement>.AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);

//            LearnCallbackInvoker = new AsyncCallback(LearnAsyncCallback);
            LearnPropCallbackInvoker = new AsyncCallback(LearnPropertyAsyncCallback);
            LearnResult = null;
        }


        //public SIMONIntelligence(AlgorithmForAI algorithm, SIMONAlgorithm algorithmCls)
        //{
        //    IntelligenceAlgorithm = algorithm;
        //    AlgorithmPerformer = algorithmCls;
        //    AlgorithmInvoker = new AlgorithmDelegate(AlgorithmPerformer.Implementation);
        //    AlgorithmLearnInvoker = new AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
        //    AlgorithmLearnPropInvoker = new AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);
        //    LearnCallbackInvoker = new AsyncCallback(LearnAsyncCallback);
        //    LearnPropCallbackInvoker = new AsyncCallback(LearnPropertyAsyncCallback);
        //    LearnResult = null;
        //}

        //public SIMONIntelligence(AlgorithmForAI algorithm, SIMONAlgorithm algorithmCls, AsyncCallback learnCallback)
        //{
        //    IntelligenceAlgorithm = algorithm;
        //    AlgorithmPerformer = algorithmCls;
        //    AlgorithmInvoker = new AlgorithmDelegate(AlgorithmPerformer.Implementation);
        //    AlgorithmLearnInvoker = new AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
        //    AlgorithmLearnPropInvoker = new AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);
        //    LearnCallbackInvoker = new AsyncCallback(learnCallback);
        //    LearnPropCallbackInvoker = new AsyncCallback(LearnPropertyAsyncCallback);
        //    LearnResult = null;
        //}



        /// <summary>
        /// 설정된 AI 알고리즘 클래스에 알려져있는 형태의 옵션 정보를 설정합니다.
        /// </summary>
        /// <param name="algorithm">SIMONFramework 내에 등록된 AI Algorithm 클래스입니다.</param>
        /// <param name="optionParam">AI Algorithm 클래스 내의 설정에 대한 정보를 갖는 옵션 클래스입니다.</param>
        public void ConfigureLearning(AlgorithmForAI algorithm, object optionParam)
        {
            switch (algorithm)
            {
                case AlgorithmForAI.GENETIC:
                    ((SIMONGeneticAlgorithm)AlgorithmPerformer).SetMutationChance(((SIMONGeneticAlgorithm.GeneOption)optionParam).mutationChance);
                    ((SIMONGeneticAlgorithm)AlgorithmPerformer).SetMutationProportion(((SIMONGeneticAlgorithm.GeneOption)optionParam).mutationProportion);
                    break;
                case AlgorithmForAI.NEURALNETWORK:
                    //Not Supported Yet.
                    break;
                default:
                    throw new SIMONFramework.InvalidAlgorithmClassException(SIMONConstants.EXP_INVALID_ALGORITHM_CLS);
            }
        }

        /// <summary>
        /// SIMONObject들의 그룹과 Action들의 Map, 학습에 이용되는 SimonFunction들에 대한 Dictionary를 통한 동기 학습루틴을 구현합니다.
        /// </summary>
        /// <param name="GroupCollection">학습 대상 SIMONObject 그룹입니다.</param>
        /// <param name="ActionMap">학습 대상에 대한 ActionMap입니다.</param>
        /// <param name="SimonFunctions">학습에 사용될 SIMONFunction ADT입니다.</param>
        public void Learn(SIMONCollection GroupCollection, SIMONCollection ActionMap, SIMONCollection SimonFunctions)//Dictionary<string, SIMONFunctionInterface<SIMONElement>.SIMONDelegate> SimonFunctions)
//        public void Learn(SIMONCollection GroupCollection, SIMONCollection ActionMap, Dictionary<string, SIMONFunctionInterface<SIMONProperty, SIMONAction>.SIMONDelegate> SimonFunctions)
        {
            try
            {
                isLearning = true;
                switch (IntelligenceAlgorithm)
                {
                    case AlgorithmForAI.GENETIC:
                        LearnResult = AlgorithmPerformer.Implementation(GroupCollection, ActionMap, SimonFunctions);
                        break;
                    case AlgorithmForAI.NEURALNETWORK:
                        break;
                }
//                LearnResult = AlgorithmPerformer.Implementation(GroupCollection, ActionMap, SimonFunctions);
                isLearning = false;
            }
            catch (Exception exp)
            {
                throw new SIMONFramework.LearningFaultException(SIMONConstants.EXP_LEARNING_FAULT);
            }
        }

        /// <summary>
        /// SIMONObject들의 그룹과 Action들의 Map, 학습에 이용되는 SimonFunction들에 대한 Dictionary를 통한 동기 학습루틴을 구현합니다.
        /// </summary>
        /// <param name="GroupCollection">학습 대상 SIMONObject 그룹입니다.</param>
        /// <param name="ActionMap">학습 대상에 대한 ActionMap입니다.</param>
        /// <param name="SimonFunctions">학습에 사용될 SIMONFunction ADT입니다.</param>
        public void Learn(AlgorithmForAI AIModel, SIMONCollection GroupCollection, SIMONCollection ActionMap, SIMONCollection SimonFunctions)//Dictionary<string, SIMONFunctionInterface<SIMONElement>.SIMONDelegate> SimonFunctions)
        {
            try
            {
                isLearning = true;

                switch (IntelligenceAlgorithm)
                {
                    case AlgorithmForAI.GENETIC:
                        LearnResult = AlgorithmPerformer.Implementation(GroupCollection, ActionMap, SimonFunctions);
                        break;
                }

//                LearnResult = AlgorithmPerformer.Implementation(GroupCollection, ActionMap, SimonFunctions);

                isLearning = false;
            }
            catch (Exception exp)
            {
                throw new SIMONFramework.LearningFaultException(SIMONConstants.EXP_LEARNING_FAULT);
            }
        }


        /// <summary>
        /// SIMONObject들의 그룹과 Action들의 Map, 학습에 이용되는 SimonFunction들에 대한 Dictionary 및 학습률을 적용시킨 비동기 학습루틴을 구현합니다.
        /// </summary>
        /// <param name="GroupCollection">학습 대상 SIMONObject 그룹입니다.</param>
        /// <param name="ActionMap">학습 대상에 대한 ActionMap입니다.</param>
        /// <param name="SimonFunctions">학습에 사용될 SIMONFunction ADT입니다.</param>
        /// <param name="learningRate">학습률입니다.</param>
        /// <returns>비동기 결과 객체입니다.</returns>
        public IAsyncResult LearnAsync(SIMONCollection GroupCollection, SIMONCollection ActionMap, SIMONCollection SimonFunctions, double learningRate)//Dictionary<string, SIMONFunction> SimonFunctions, double learningRate)
        {
            isLearning = true;
            return AlgorithmLearnInvoker.BeginInvoke(GroupCollection, ActionMap, SimonFunctions, learningRate, LearnCallbackInvoker, null);
        }

        /// <summary>
        /// SIMONFramework 의 Simulate 학습 콜백으로 호출되는 함수를 명시합니다. 비동기 콜백 종료를 대기시킵니다.
        /// </summary>
        /// <param name="aResult">비동기 결과 객체입니다.</param>
        public void LearnAsyncCallback(IAsyncResult aResult)
        {
            LearnResult = AlgorithmLearnInvoker.EndInvoke(aResult);
            isLearning = false;
        }

        /// <summary>
        /// SIMONObject들의 Property 학습을 수행하는 별도의 알고리즘 구현을 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection">Property 학습을 수행할 대상 Group입니다.</param>
        /// <param name="SimonFunctions">학습에 사용될 SIMONFunction ADT입니다.</param>
        public void LearnProperty(SIMONCollection ObjectCollection, SIMONCollection SimonFunctions)//Dictionary<string, SIMONFunctionInterface<SIMONElement>.SIMONDelegate> SimonFunctions)
//        public void LearnProperty(SIMONCollection ObjectCollection, Dictionary<string, SIMONFunctionInterface<SIMONProperty, SIMONAction>.SIMONDelegate> SimonFunctions)
        {
            try
            {
                isLearning = true;

                switch (IntelligenceAlgorithm)
                {
                    case AlgorithmForAI.GENETIC:
                        LearnResult = AlgorithmPerformer.Implementation(ObjectCollection, SimonFunctions);
                        break;

                }
//                LearnResult = AlgorithmPerformer.Implementation(ObjectCollection, SimonFunctions);

                isLearning = false;
            }
            catch (Exception exp)
            {
                throw new SIMONFramework.LearningFaultException(SIMONConstants.EXP_LEARNING_FAULT);
            }
        }

        /// <summary>
        /// Property에 대한 학습을 비동기적으로 구현합니다.
        /// </summary>
        /// <param name="ObjectCollection">Property 학습을 수행할 대상 Group입니다.</param>
        /// <param name="SimonFunctions">학습에 사용될 SIMONFunction ADT입니다.</param>
        /// <param name="learningRate">학습률입니다.</param>
        /// <returns>비동기 결과객체입니다.</returns>
        public IAsyncResult LearnPropertyAsync(SIMONCollection ObjectCollection, SIMONCollection SimonFunctions, double learningRate)// Dictionary<string, SIMONFunction> SimonFunctions, double learningRate)
        {
            isLearning = true;
            return AlgorithmLearnPropInvoker.BeginInvoke(ObjectCollection, SimonFunctions, learningRate, LearnPropertyAsyncCallback, null);
        }

        /// <summary>
        /// Property에 대한 학습의 비동기적 콜백함수를 구현합니다.
        /// </summary>
        /// <param name="aResult">비동기 결과 객체입니다.</param>
        public void LearnPropertyAsyncCallback(IAsyncResult aResult)
        {
            LearnResult = AlgorithmLearnPropInvoker.EndInvoke(aResult);
            isLearning = false;
        }


    }
}


