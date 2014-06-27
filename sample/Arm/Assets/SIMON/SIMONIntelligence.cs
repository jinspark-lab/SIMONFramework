
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

    public enum AlgorithmForAI { GENETIC = 1, NEURALNETWORK = 2};
    public delegate object AlgorithmDelegate(SIMONCollection Group, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> Functions);
    public delegate object AlgorithmLearningDelegate(SIMONCollection Group, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> Functions, double learningRate);
    public delegate object AlgorithmLearnPropDelegate(SIMONCollection Group, Dictionary<string, SIMONFunction> Functions, double learningRate);

    /// <summary>
    /// SIMON Algorithm을 통해서 AI를 제공하고, 학습 모듈을 연동시킬 수 있는 모듈을 클래스화합니다.
    /// </summary>
    public class SIMONIntelligence
    {
        public AlgorithmForAI IntelligenceAlgorithm { get; private set; }
        public SIMONAlgorithm AlgorithmPerformer { get; private set; }

        private AlgorithmDelegate AlgorithmInvoker;
        private AlgorithmLearningDelegate AlgorithmLearnInvoker;
        private AlgorithmLearnPropDelegate AlgorithmLearnPropInvoker;
        private AsyncCallback LearnCallbackInvoker;
        private AsyncCallback LearnPropCallbackInvoker;
        public object LearnResult { get; private set; }
        public bool isLearning { get; private set; }

        public SIMONIntelligence()
        {
            IntelligenceAlgorithm = AlgorithmForAI.GENETIC;
            AlgorithmPerformer = new SIMONGeneticAlgorithm();

            AlgorithmInvoker = new AlgorithmDelegate(AlgorithmPerformer.Implementation);
            AlgorithmLearnInvoker = new AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
            AlgorithmLearnPropInvoker = new AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);
            LearnCallbackInvoker = new AsyncCallback(LearnAsyncCallback);
            LearnPropCallbackInvoker = new AsyncCallback(LearnPropertyAsyncCallback);
            LearnResult = null;
        }

        public SIMONIntelligence(AlgorithmForAI algorithm, SIMONAlgorithm algorithmCls)
        {
            IntelligenceAlgorithm = algorithm;
            AlgorithmPerformer = algorithmCls;
            AlgorithmInvoker = new AlgorithmDelegate(AlgorithmPerformer.Implementation);
            AlgorithmLearnInvoker = new AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
            AlgorithmLearnPropInvoker = new AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);
            LearnCallbackInvoker = new AsyncCallback(LearnAsyncCallback);
            LearnPropCallbackInvoker = new AsyncCallback(LearnPropertyAsyncCallback);
            LearnResult = null;
        }

        public SIMONIntelligence(AlgorithmForAI algorithm, SIMONAlgorithm algorithmCls, AsyncCallback learnCallback)
        {
            IntelligenceAlgorithm = algorithm;
            AlgorithmPerformer = algorithmCls;
            AlgorithmInvoker = new AlgorithmDelegate(AlgorithmPerformer.Implementation);
            AlgorithmLearnInvoker = new AlgorithmLearningDelegate(AlgorithmPerformer.Implementation);
            AlgorithmLearnPropInvoker = new AlgorithmLearnPropDelegate(AlgorithmPerformer.Implementation);
            LearnCallbackInvoker = new AsyncCallback(learnCallback);
            LearnPropCallbackInvoker = new AsyncCallback(LearnPropertyAsyncCallback);
            LearnResult = null;
        }

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

                    break;
                default:
                    throw new Exception("[SIMONIntelligence] : Invalid Algorithm class implementation.");
            }
        }

        /// <summary>
        /// SIMONObject들의 그룹과 Action들의 Map, 학습에 이용되는 SimonFunction들에 대한 Dictionary를 통한 동기 학습루틴을 구현합니다.
        /// </summary>
        /// <param name="GroupCollection"></param>
        /// <param name="ActionMap"></param>
        /// <param name="SimonFunctions"></param>
        public void Learn(SIMONCollection GroupCollection, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> SimonFunctions)
        {
            try
            {
                isLearning = true;
                LearnResult = AlgorithmPerformer.Implementation(GroupCollection, ActionMap, SimonFunctions);
                isLearning = false;
            }
            catch (Exception exp)
            {
                throw new Exception("[SIMON Framework] : Exception Occurs in learning algorithm.\n" + exp.Message, exp);
            }
        }

        /// <summary>
        /// SIMONObject들의 그룹과 Action들의 Map, 학습에 이용되는 SimonFunction들에 대한 Dictionary 및 학습률을 적용시킨 비동기 학습루틴을 구현합니다.
        /// </summary>
        /// <param name="GroupCollection"></param>
        /// <param name="ActionMap"></param>
        /// <param name="SimonFunctions"></param>
        /// <param name="learningRate"></param>
        public void LearnAsync(SIMONCollection GroupCollection, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> SimonFunctions, double learningRate)
        {
            isLearning = true;
            AlgorithmLearnInvoker.BeginInvoke(GroupCollection, ActionMap, SimonFunctions, learningRate, LearnCallbackInvoker, null);
        }

        #region 기본 학습에 대한 비동기 콜백 -> 일단 보류.

        ///// <summary>
        ///// SIMONFramework 의 기본 학습 콜백으로 호출되는 함수를 명시합니다. 비동기 콜백 종료를 대기시킵니다. [Deprecated]
        ///// </summary>
        ///// <param name="aResult"></param>
        //public void LearnCallback(IAsyncResult aResult)
        //{
        //    LearnResult = AlgorithmInvoker.EndInvoke(aResult);
        //    isLearning = false;
        //}

        #endregion

        /// <summary>
        /// SIMONFramework 의 Simulate 학습 콜백으로 호출되는 함수를 명시합니다. 비동기 콜백 종료를 대기시킵니다.
        /// </summary>
        /// <param name="aResult"></param>
        public void LearnAsyncCallback(IAsyncResult aResult)
        {
            LearnResult = AlgorithmLearnInvoker.EndInvoke(aResult);
            isLearning = false;
        }

        /// <summary>
        /// SIMONObject들의 Property 학습을 수행하는 별도의 알고리즘 구현을 제공합니다.
        /// </summary>
        /// <param name="ObjectCollection"></param>
        /// <param name="SimonFunctions"></param>
        public void LearnProperty(SIMONCollection ObjectCollection, Dictionary<string, SIMONFunction> SimonFunctions)
        {
            try
            {
                isLearning = true;
                LearnResult = AlgorithmPerformer.Implementation(ObjectCollection, SimonFunctions);
                isLearning = false;
            }
            catch (Exception exp)
            {
                throw new Exception("[SIMON Framework] : Exception Occurs in learning property algorithm.\n" + exp.Message, exp);
            }
        }

        /// <summary>
        /// Property에 대한 학습을 비동기적으로 구현합니다.
        /// </summary>
        /// <param name="ObjectCollection"></param>
        /// <param name="SimonFunctions"></param>
        public void LearnPropertyAsync(SIMONCollection ObjectCollection, Dictionary<string, SIMONFunction> SimonFunctions, double learningRate)
        {
            isLearning = true;
            AlgorithmLearnPropInvoker.BeginInvoke(ObjectCollection, SimonFunctions, learningRate, LearnPropertyAsyncCallback, null);
        }

        /// <summary>
        /// Property에 대한 학습의 비동기적 콜백함수를 구현합니다.
        /// </summary>
        /// <param name="aResult"></param>
        public void LearnPropertyAsyncCallback(IAsyncResult aResult)
        {
            LearnResult = AlgorithmLearnPropInvoker.EndInvoke(aResult);
            isLearning = false;
        }

    }


}

