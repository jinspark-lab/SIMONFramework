
/*
 * 
 * SIMONFramework에서 사용하게 되는 유전 알고리즘에 대한 구현을 제공합니다.
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
    public sealed class SIMONGeneticAlgorithm : SIMONAlgorithm
    {
        //이 두 값은 기본적으로 0~10.0 사이의 값을 받도록 한다.
        private double MutationChance;
        private double MutationProportion;

        public struct GeneValue
        {
            public List<SIMONGene> dna;
            public double fitnessValue;
        }

        public class GeneOption
        {
            public double mutationChance;
            public double mutationProportion;

            public GeneOption(double mutationChance, double mutationProportion)
            {
                this.mutationChance = mutationChance;
                this.mutationProportion = mutationProportion;
            }
        }

        public enum GeneSelectionLaw
        {
            DOMINION = 0,
            RECESSIVE = 1,
            RANDOM = 2
        };

        public SIMONGeneticAlgorithm()
        {
            MutationChance = SIMONConstants.GENE_MUTATION_CHANCE;
            MutationProportion = SIMONConstants.GENE_MUTATION_PROPORTION;
        }

        public SIMONGeneticAlgorithm(double mutateChance, double mutateProportion)
        {
            MutationChance = mutateChance;
            MutationProportion = mutateProportion;
        }

        /// <summary>
        /// 현재 설정된 돌연변이가 발생하는 확률값을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public double GetMutationChance()
        {
            return MutationChance;
        }

        /// <summary>
        /// 돌연변이 발생 확률을 설정합니다.
        /// </summary>
        /// <param name="mutateChance"></param>
        public void SetMutationChance(double mutateChance)
        {
            MutationChance = mutateChance;
        }

        /// <summary>
        /// 현재 설정된 돌연변이의 변이율을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public double GetMutationProportion()
        {
            return MutationProportion;
        }

        /// <summary>
        /// 돌연변이 변이 정도를 설정합니다.
        /// </summary>
        /// <param name="mutateProportion"></param>
        public void SetMutationProportion(double mutateProportion)
        {
            MutationProportion = mutateProportion;
        }

        /// <summary>
        /// SIMONAlgorithm의 구현을 담당하는 Interface의 기능을 호출합니다.
        /// </summary>
        /// <param name="PropertyCollection">진화에 대한 구현을 담당하는 2차원 SIMONCollection입니다.</param>
        /// <param name="SimonFunctions">SIMONFunction 집합입니다.</param>
        /// <returns></returns>
        public object Implementation(SIMONCollection GroupCollection, Dictionary<string, SIMONFunction> SimonFunctions)
        {
            List<List<SIMONGene>> selectedDNAPool = Selection(GroupCollection, SimonFunctions);
            List<List<SIMONGene>> crossedDNAPool = CrossOver(selectedDNAPool);
            List<List<SIMONGene>> mutatedDNAPool = Mutation(crossedDNAPool);
            Evolution(GroupCollection, mutatedDNAPool);
            UpdateObjectsPropertyDNA(GroupCollection);

            return null;
        }
        /// <summary>
        /// SIMONAlgorithm의 구현을 담당하는 Interface의 기능을 호출합니다.
        /// </summary>
        /// <param name="GroupCollection">진화에 대한 구현을 담당하는 2차원 SIMONCollection입니다.</param>
        /// <param name="SimonFunctions">SIMONFunction 집합입니다.</param>
        /// <param name="learningRate">학습률을 정의합니다.</param>
        /// <returns></returns>
        public object Implementation(SIMONCollection GroupCollection, Dictionary<string, SIMONFunction> SimonFunctions, double learningRate)
        {
            List<List<SIMONGene>> selectedDNAPool = Selection(GroupCollection, SimonFunctions);
            List<List<SIMONGene>> crossedDNAPool = CrossOver(selectedDNAPool);
            List<List<SIMONGene>> mutatedDNAPool = Mutation(crossedDNAPool);
            Evolution(GroupCollection, mutatedDNAPool, learningRate);
            UpdateObjectsPropertyDNA(GroupCollection);

            return null;
        }

        /// <summary>
        /// SIMONAlgorithm의 구현을 담당하는 Interface의 기능을 호출합니다.
        /// </summary>
        /// <param name="GroupCollection">Algorithm의 적용 대상 Collection입니다.</param>
        /// <param name="ActionMap">Algorithm의 적용 대상 Action에 대한 매핑 테이블을 선언합니다.</param>
        /// <param name="SimonFunctions">Algorithm에 사용되는 함수를 링크합니다.</param>
        /// <returns></returns>
        public object Implementation(SIMONCollection GroupCollection, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> SimonFunctions)
        {
            List<List<List<SIMONGene>>> selectedDNAPool = Selection(GroupCollection, ActionMap, SimonFunctions);
            List<List<List<SIMONGene>>> crossedDNAPool = CrossOver(selectedDNAPool);
            List<List<List<SIMONGene>>> mutatedDNAPool = Mutation(crossedDNAPool);
            Evolution(GroupCollection, ActionMap, mutatedDNAPool);

            return null;
        }
        /// <summary>
        /// SIMONAlgorithm의 구현을 담당하는 Interface의 기능을 호출합니다.
        /// </summary>
        /// <param name="GroupCollection">Algorithm의 적용 대상 Collection입니다.</param>
        /// <param name="ActionMap">Algorithm의 적용 대상 Action에 대한 매핑 테이블을 선언합니다.</param>
        /// <param name="SimonFunctions">Algorithm에 사용되는 함수를 링크합니다.</param>
        /// <param name="learningRate">알고리즘의 학습률을 지정합니다.</param>
        /// <returns></returns>
        public object Implementation(SIMONCollection GroupCollection, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> SimonFunctions, double learningRate)
        {
            List<List<List<SIMONGene>>> selectedDNAPool = Selection(GroupCollection, ActionMap, SimonFunctions);
            List<List<List<SIMONGene>>> crossedDNAPool = CrossOver(selectedDNAPool);
            List<List<List<SIMONGene>>> mutatedDNAPool = Mutation(crossedDNAPool);
            Evolution(GroupCollection, ActionMap, mutatedDNAPool, learningRate);

            return null;
        }

        private void QuickSort(GeneValue[] dnaList, int left, int right)
        {
            int index = left, jndex = right;
            double pivot = dnaList[(left + right) / 2].fitnessValue;

            while (index <= jndex)
            {
                while (dnaList[index].fitnessValue < pivot)
                    index++;
                while (dnaList[jndex].fitnessValue > pivot)
                    jndex--;
                if (index <= jndex)
                {
                    GeneValue temp = dnaList[index];
                    dnaList[index] = dnaList[jndex];
                    dnaList[jndex] = temp;
                    index++;
                    jndex--;
                }
            }
            if (left < jndex)
                QuickSort(dnaList, left, jndex);
            if (right > index)
                QuickSort(dnaList, index, right);
        }

        /// <summary>
        /// History로부터 읽어온 데이터를 바탕으로, 유전 선택 알고리즘을 수행합니다. 우성 : 열성이 3:1 비율이 되도록 집단에서 우성 행동들과 열성 행동의 2차원 배열값을 반환합니다.
        /// </summary>
        /// <param name="PropertyCollection">2차원으로 구성된 Selection 대상 ObjectCollection 입니다.</param>
        /// <param name="SimonFunctions">Selection 연산에 사용될 SimonFunction 집합입니다.</param>
        /// <returns></returns>
        public List<List<SIMONGene>> Selection(SIMONCollection PropertyCollection, Dictionary<string, SIMONFunction> SimonFunctions)
        {
            List<List<SIMONGene>> selectedDNA = new List<List<SIMONGene>>();
            int dnaObjectCount = PropertyCollection.Count;
            GeneValue[] propertyGenes = new GeneValue[dnaObjectCount];
            int[] divIndex = new int[SIMONConstants.GENE_SUM_RATING];

            for (int i = 0; i < dnaObjectCount; i++)
            {
                SIMONObject elementObject = (SIMONObject)PropertyCollection.ValueOfIndex(i);
                SIMONObject[] otherObjects = new SIMONObject[dnaObjectCount - 1];
                int otherObjectsCnt = 0;
                propertyGenes[i] = new GeneValue();

                //otherObject 리스트 추가시키면됨.
                for (int j = 0; j < dnaObjectCount; j++)
                    if (elementObject != (SIMONObject)PropertyCollection.ValueOfIndex(j))
                        otherObjects[otherObjectsCnt++] = (SIMONObject)PropertyCollection.ValueOfIndex(j);

                propertyGenes[i].dna = elementObject.PropertyDNA;
                double fitnessValue = (double)SimonFunctions[elementObject.ObjectFitnessFunctionName].Invoke(elementObject, otherObjects);

                //Upper Boundary와 Lower Boundary 경계 내의 Fitness Value를 채택.
                if (fitnessValue < SIMONConstants.FITNESS_MIN_VALUE)
                {
                    throw new Exception("[SIMON Framework] : Fitness Value underflow exception occurs.\n");
                }
                else if (fitnessValue > SIMONConstants.FITNESS_MAX_VALUE)
                {
                    throw new Exception("[SIMON Framework] : Fitness Value overflow exception occurs.\n");
                }
                propertyGenes[i].fitnessValue = fitnessValue;
            }
            QuickSort(propertyGenes, 0, dnaObjectCount - 1);

            for (int i = 0; i < SIMONConstants.GENE_SUM_RATING; i++)
            {
                int divNum = (int)(((i + 1) * dnaObjectCount) / SIMONConstants.GENE_SUM_RATING);
                if (divNum == 0)
                    divNum = -1;
                else
                    divNum--;
                divIndex[i] = divNum;
            }

            List<GeneValue> recessiveGroup = new List<GeneValue>();
            List<GeneValue> dominionGroup = new List<GeneValue>();

            List<GeneValue> selectedGene = new List<GeneValue>();

            for (int i = divIndex[SIMONConstants.GENE_RECESSIVE_RATING - 1]; i >= 0; i--)
            {
                recessiveGroup.Add(propertyGenes[i]);
            }
            selectedGene.AddRange(RouletteWheel(recessiveGroup, GeneSelectionLaw.RECESSIVE));

            for (int i = divIndex[SIMONConstants.GENE_DOMINION_RATING]; i >= divIndex[SIMONConstants.GENE_RECESSIVE_RATING - 1] + 1; i--)
            {
                dominionGroup.Add(propertyGenes[i]);
            }
            selectedGene.AddRange(RouletteWheel(dominionGroup, GeneSelectionLaw.DOMINION));

            int selectingCount = SIMONConstants.GENE_REAL_SELECT_NUM;
            if (selectedGene.Count < selectingCount)
                selectingCount = selectedGene.Count;
            int[] selectedIdxTable = new int[selectingCount];
            int selectedIdx = 0;

            if (selectedGene.Count <= 0)
                return selectedDNA;

            while (selectedIdx < selectingCount)
            {
				Random rand = new Random((int)DateTime.Now.Ticks);
                int selectIdx = rand.Next(0, selectedGene.Count);
                bool retryFlag = false;

                for (int i = 0; i < selectedIdx; i++)
                {
                    if (selectedIdxTable[i] == selectIdx)
                    {
                        retryFlag = true;
                        break;
                    }
                }
                if (retryFlag)
                    continue;
                selectedDNA.Add(selectedGene[selectIdx].dna);
                selectedIdxTable[selectedIdx++] = selectIdx;
            }

            return selectedDNA;
        }

        /// <summary>
        /// History로부터 읽어온 데이터를 바탕으로, 유전 선택 알고리즘을 수행합니다. 우성 : 열성이 3:1 비율이 되도록 집단에서 우성 행동들과 열성 행동의 3차원 배열값을 반환합니다.
        /// </summary>
        /// <param name="ObjectCollection">3차원으로 구성된 Selection 대상 ObjectCollection 입니다.</param>
        /// <param name="ActionMap">Selection 대상 ActionMap 입니다.</param>
        /// <param name="SimonFunctions">Selection 연산에 사용될 SimonFunction 집합입니다.</param>
        /// <returns></returns>
        public List<List<List<SIMONGene>>> Selection(SIMONCollection ObjectCollection, SIMONCollection ActionMap, Dictionary<string, SIMONFunction> SimonFunctions)
        {
            List<List<List<SIMONGene>>> selectedDNA = new List<List<List<SIMONGene>>>();
            List<GeneValue[]> recordMap = new List<GeneValue[]>();                                            //ObjectCollection 내 각 객체들에 대한 Action들에 대한 Record 값들을 저장하는 Map.

            int ObjectCount = ObjectCollection.Count;
            int ActionCount = ActionMap.Count;

            for (int i = 0; i < ActionCount; i++)
            {
                selectedDNA.Add(new List<List<SIMONGene>>());
            }

            #region 현재 ActionMap 구조를 이용한 RecordMap 구조화

            for (int i = 0; i < ActionCount; i++)
            {
                List<SIMONObject> elementList = (List<SIMONObject>)ActionMap.ValueOfIndex(i);

                int actionObjectCount = elementList.Count;

                GeneValue[] gene = new GeneValue[actionObjectCount];
                recordMap.Add(gene);

                for (int j = 0; j < actionObjectCount; j++)
                {
                    recordMap[i][j] = new GeneValue();
                    SIMONObject[] otherObjectsList;
                    if (ObjectCount > 1)
                        otherObjectsList = new SIMONObject[ObjectCollection.Count - 1];
                    else
                        otherObjectsList = null;
                    int otherObjectCnt = 0;

                    for (int k = 0; k < ObjectCount; k++)
                        if ((otherObjectsList != null) && (!ObjectCollection.ValueOfIndex(k).Equals(elementList[j])))
                            otherObjectsList[otherObjectCnt++] = (SIMONObject)ObjectCollection.ValueOfIndex(k);

                    for (int k = 0; k < elementList[j].Actions.Count; k++)
                    {
                        if (elementList[j].Actions[k].ActionName.Equals(ActionMap.KeyOfIndex(i)))
                        {
                            recordMap[i][j].dna = elementList[j].Actions[k].ActionDNA;
                            double fitnessValue = (double)SimonFunctions[elementList[j].Actions[k].FitnessFunctionName].Invoke(elementList[j], otherObjectsList);

                            //Upper Boundary와 Lower Boundary 내의 Fitness Value들을 채택.
                            if (fitnessValue < SIMONConstants.FITNESS_MIN_VALUE)
                            {
                                throw new Exception("[SIMON Framework] : Fitness Value underflow exception occurs.\n");
                            }
                            else if (fitnessValue > SIMONConstants.FITNESS_MAX_VALUE)
                            {
                                throw new Exception("[SIMON Framework] : Fitness Value overflow exception occurs.\n");
                            }
                            recordMap[i][j].fitnessValue = fitnessValue;
                            break;
                        }
                    }
                }
            }

            #endregion

            #region 현재 division 나누는 코드. Action 별로 4등분해서 나누기 때문에 2차원 배열임.

            int[][] divIndex = new int[ActionCount][];
            for (int i = 0; i < ActionCount; i++)
            {
                int numberOfActionObjects = ((List<SIMONObject>)ActionMap.ValueOfIndex(i)).Count;
                divIndex[i] = new int[SIMONConstants.GENE_SUM_RATING];
                for (int j = 0; j < SIMONConstants.GENE_SUM_RATING; j++)
                {
                    int divPosition = (numberOfActionObjects * (j + 1)) / SIMONConstants.GENE_SUM_RATING;
                    if (divPosition == 0)
                        divPosition = -1;
                    else
                        divPosition--;
                    divIndex[i][j] = divPosition;
                }
            }

            #endregion

            //QuickSort를 통해서 각 Action별로 fitness 값들을 정렬.
            for (int i = 0; i < recordMap.Count; i++)
                QuickSort(recordMap[i], 0, recordMap[i].Length - 1);

            List<List<GeneValue>> firstSelectGene = new List<List<GeneValue>>();
            List<List<GeneValue>> lastSelectGene = new List<List<GeneValue>>();

            for (int i = 0; i < recordMap.Count; i++)
            {
                firstSelectGene.Add(new List<GeneValue>());
                lastSelectGene.Add(new List<GeneValue>());
                List<GeneValue> recessiveGroup = new List<GeneValue>();
                List<GeneValue> dominionGroup = new List<GeneValue>();

                //열성집합 중 1만큼의 비율을 선택. 경계값부터 시작인덱스 까지 내려가면서 집합에 포함시킨다.
                for (int j = divIndex[i][(SIMONConstants.GENE_RECESSIVE_RATING - 1)]; j >= 0; j--)
                {
                    recessiveGroup.Add(recordMap[i][j]);
                }
                List<GeneValue> rouletteRecessive = RouletteWheel(recessiveGroup, GeneSelectionLaw.RECESSIVE);      //각 Object 들의 열성 집합 중 대표값 배열을 우성 열성 비율별로 선택
                if (rouletteRecessive != null)
                    firstSelectGene[i].AddRange(rouletteRecessive);

                //우성집합 중 3만큼의 비율을 선택. 경계값부터 시작인덱스까지 내려가면서 집합에 포함시킨다.
                for (int j = divIndex[i][SIMONConstants.GENE_DOMINION_RATING]; j >= divIndex[i][(SIMONConstants.GENE_RECESSIVE_RATING - 1)] + 1; j--)
                {
                    dominionGroup.Add(recordMap[i][j]);
                }
                List<GeneValue> rouletteDominion = RouletteWheel(dominionGroup, GeneSelectionLaw.DOMINION);         //각 Object 들의 우성 집합 중 대표값 배열을 우성 열성 비율별로 선택
                if (rouletteDominion != null)
                    firstSelectGene[i].AddRange(rouletteDominion);

                lastSelectGene[i].AddRange(RouletteWheel(firstSelectGene[i], GeneSelectionLaw.DOMINION));
            }

            for (int i = 0; i < ActionCount; i++)
            {
                int selectedCount = SIMONConstants.GENE_REAL_SELECT_NUM;
                int[] selectedIdxList = new int[lastSelectGene[i].Count];
                int selectedIdxListIndex = 0;

                //만약 실제 유전자 숫자가 Default로 정한 유전자 추출 갯수보다 작으면 실제 유전자 갯수 만큼을 선택 횟수로 지정한다.
                if (lastSelectGene[i].Count < SIMONConstants.GENE_REAL_SELECT_NUM)
                    selectedCount = lastSelectGene[i].Count;

                if (selectedCount <= 0)
                    continue;

                //실제 유전시킬 유전자 갯수만큼 랜덤값을 통한 추출
                while (selectedIdxListIndex < selectedCount)
                {
					Random rand = new Random((int)DateTime.Now.Ticks);
                    int selectIdx = rand.Next(0, selectedCount);
                    bool retryFlag = false;

                    for (int j = 0; j < selectedIdxListIndex; j++)
                    {
                        if (selectIdx == selectedIdxList[j])
                        {
                            retryFlag = true;
                            break;
                        }
                    }
                    if (retryFlag)
                        continue;

                    selectedDNA[i].Add(lastSelectGene[i][selectIdx].dna);
                    selectedIdxList[selectedIdxListIndex++] = selectIdx;
                }
            }
            //selectedDNA 리턴.
            return selectedDNA;
        }

        /// <summary>
        /// 인자로 넘겨받는 geneSet 유전자 셋에 대하여 유전법칙 law를 적용시켜서 해당 법칙을 만족하는 유전자 리스트를 룰렛휠 방식을 통해 반환합니다.
        /// </summary>
        /// <param name="geneSet"></param>
        /// <param name="law"></param>
        /// <returns></returns>
        private List<GeneValue> RouletteWheel(List<GeneValue> geneSet, GeneSelectionLaw law)
        {
            List<GeneValue> rouletteGene = new List<GeneValue>();
            int selectCount = -1;
            List<int> roulette = new List<int>();
            int[] dartSelection;
            int dartSelectIdx = 0;
			Random rand = new Random((int)DateTime.Now.Ticks);

            if (geneSet.Count == 0)
            {
                return null;
            }
            else if (geneSet.Count == 1)
            {
                return geneSet;
            }

            //전체 대비 부분의 비율을 구하고 룰렛을 정규화시켜서 세팅.
            double geneValueMin = SIMONConstants.FITNESS_CMP_ONE;                                           //최소값에 대한 정규화 연산을 위한 Boundary 입니다.
            double geneRegulationValue = 0;
            foreach (GeneValue gene in geneSet)
            {
                if (gene.fitnessValue < SIMONConstants.FITNESS_CMP_ONE)
                {
                    geneValueMin = gene.fitnessValue;
                    continue;
                }
            }
            if (geneValueMin == SIMONConstants.FITNESS_CMP_ZERO)                                            //최솟값이 0일 경우에는 1로 간주합니다.
            {
                geneValueMin = SIMONConstants.FITNESS_CMP_ONE;
                geneRegulationValue = (geneValueMin * 2);
            }
            else if (geneValueMin < SIMONConstants.FITNESS_CMP_ZERO)                                        //음수 최솟값에 대한 Boundary 정규화 과정을 수행합니다.
            {
                geneRegulationValue = (geneValueMin * -2);
            }
            else
            {
                geneRegulationValue = 0;
            }

            //정규화된 fitness 값 만큼의 Index를 룰렛에 저장합니다.
            for (int i = 0; i < geneSet.Count; i++)
            {
                int assign = (int)((geneSet[i].fitnessValue + geneRegulationValue));
                for (int j = 0; j < assign; j++)
                {
                    roulette.Add(i);
                }
            }

            //Genome Set이 수렴해서 차잇값이 존재하지 않을 경우 수렴값 자체를 Roulette에 포함시킵니다.
            if (roulette.Count < 1)
            {
                rouletteGene.AddRange(geneSet);
                return rouletteGene;
            }

            if (law == GeneSelectionLaw.RANDOM)
            {
                //random일 경우 단순히 랜덤 유전자들을 반환
                selectCount = rand.Next(0, roulette.Count);
            }
            else if (law == GeneSelectionLaw.RECESSIVE)
            {
                //열성 유전에 대한 룰렛 선택의 경우 열성 비율만큼의 유전자들을 선택 반환
                selectCount = (int)((geneSet.Count * SIMONConstants.GENE_RECESSIVE_RATING) / SIMONConstants.GENE_SUM_RATING);

            }
            else if (law == GeneSelectionLaw.DOMINION)
            {
                //우성 유전에 대한 룰렛 선택의 경우 우성 비율만큼의 유전자들을 선택 반환
                selectCount = (int)((geneSet.Count * SIMONConstants.GENE_DOMINION_RATING) / SIMONConstants.GENE_SUM_RATING);
            }
            dartSelection = new int[selectCount];

            /*********************************************************/

            if (roulette.Count < selectCount)
            {
                for (int i = 0; i < roulette.Count; i++)
                    rouletteGene.Add(geneSet[roulette[i]]);
                return rouletteGene;
            }

            /*********************************************************/


            while (selectCount > 0)
            {
                int dartIdx = rand.Next(0, roulette.Count);
                bool retryFlag = false;

                //만약 동일한 룰렛의 다트를 선택했을 경우 다시 선택.
                for (int i = 0; i < dartSelectIdx; i++)
                {
                    if (dartIdx == dartSelection[i])
                    {
                        retryFlag = true;
                        break;
                    }
                }
                if (retryFlag)
                    continue;

                rouletteGene.Add(geneSet[roulette[dartIdx]]);
                dartSelection[dartSelectIdx++] = dartIdx;
                selectCount--;
            }
            return rouletteGene;
        }

        /// <summary>
        /// 감수분열 연산을 통해 다양화된 DNA 데이터셋을 반환합니다. (Not supported yet)
        /// </summary>
        /// <param name="selectedDNA"></param>
        /// <returns></returns>
        public List<SIMONGene> Meoisis(List<SIMONGene> selectedDNA)
        {
            List<SIMONGene> meoisisDNA = new List<SIMONGene>();

            return meoisisDNA;
        }

        /// <summary>
        /// 선택된 DNA 집합에 대한 교배 연산을 수행하고 결과 Set을 리턴합니다.
        /// </summary>
        /// <param name="selectedDNA">선택된 2차원 유전자 풀입니다.</param>
        /// <returns></returns>
        public List<List<SIMONGene>> CrossOver(List<List<SIMONGene>> selectedDNA)
        {
            List<List<SIMONGene>> crossedDNA = new List<List<SIMONGene>>();
            int selectedDNACount = selectedDNA.Count;

            if (selectedDNACount < 2)
            {
                crossedDNA = selectedDNA;
                return crossedDNA;
            }

            for (int i = 0; i < selectedDNACount; i += 2)
            {
                if (selectedDNA[i].Count <= 1 || selectedDNA[i + 1].Count <= 1)
                    continue;
				Random rand = new Random((int)DateTime.Now.Ticks);
                int backFrontValue = rand.Next(0, 2);
                int crossPoint = rand.Next(1, selectedDNA[i].Count - 1);
                int gArraySize = 0;
                double[] gArray1, gArray2;

                if (i + 1 >= selectedDNACount)
                    break;

                if (backFrontValue == 0)
                {
                    gArraySize = crossPoint;
                    gArray1 = new double[gArraySize];
                    gArray2 = new double[gArraySize];

                    for (int j = 0; j < crossPoint; j++)
                    {
                        gArray1[j] = selectedDNA[i][j].ElementWeight;
                        gArray2[j] = selectedDNA[i + 1][j].ElementWeight;
                    }
                    for (int j = 0; j < crossPoint; j++)
                    {
                        selectedDNA[i][j].ElementWeight = gArray2[j];
                        selectedDNA[i + 1][j].ElementWeight = gArray1[j];
                    }
                }
                else if (backFrontValue == 1)
                {
                    gArraySize = selectedDNA[i].Count - crossPoint;
                    gArray1 = new double[gArraySize];
                    gArray2 = new double[gArraySize];

                    for (int j = crossPoint; j < selectedDNA[i].Count; j++)
                    {
                        gArray1[j - crossPoint] = selectedDNA[i][j].ElementWeight;
                        gArray2[j - crossPoint] = selectedDNA[i + 1][j].ElementWeight;
                    }
                    for (int j = crossPoint; j < selectedDNA[i + 1].Count; j++)
                    {
                        selectedDNA[i][j].ElementWeight = gArray2[j - crossPoint];
                        selectedDNA[i + 1][j].ElementWeight = gArray1[j - crossPoint];
                    }
                }
            }
            crossedDNA = selectedDNA;
            return crossedDNA;
        }
        /// <summary>
        /// 선택된 DNA 집합에 대한 교배 연산을 수행하고 결과 Set을 리턴합니다.
        /// </summary>
        /// <param name="selectedDNA">선택된 3차원 유전자 풀입니다.</param>
        /// <returns></returns>
        public List<List<List<SIMONGene>>> CrossOver(List<List<List<SIMONGene>>> selectedDNA)
        {
            //교차 연산을 통한 DNA Pool의 증대를 구현합니다.
            List<List<List<SIMONGene>>> crossedDNA = new List<List<List<SIMONGene>>>();
            int poolCnt = 0;
            for (int l = 0; l < selectedDNA.Count; l++)
            {
                crossedDNA.Add(new List<List<SIMONGene>>());
                for (int i = 0; i < selectedDNA[l].Count; i++)
                {
                    if (selectedDNA[l].Count == 1)
                    {
                        List<SIMONGene> gOne = new List<SIMONGene>();
                        for (int j = 0; j < selectedDNA[l][i].Count; j++)
                        {
                            SIMONGene newG = new SIMONGene();
                            newG.ElementName = selectedDNA[l][i][j].ElementName;
                            newG.ElementWeight = selectedDNA[l][i][j].ElementWeight;
                            gOne.Add(newG);
                        }
                        crossedDNA[l].Add(gOne);
                        poolCnt++;
                        continue;
                    }
                    for (int k = 1; k < selectedDNA[l].Count - i; k++)
                    {
                        int backFrontValue = 0;
                        for (int h = 0; h < selectedDNA[l][i].Count- 1; h++)
                        {
                            List<SIMONGene> gA = new List<SIMONGene>();
                            List<SIMONGene> gB = new List<SIMONGene>();
                            for (int j = 0; j < selectedDNA[l][i].Count; j++)
                            {
                                if (backFrontValue >= j)
                                {
                                    SIMONGene newG = new SIMONGene();
                                    newG.ElementName = selectedDNA[l][i][j].ElementName;
                                    newG.ElementWeight = selectedDNA[l][i][j].ElementWeight;
                                    gA.Add(newG);
                                }
                                else
                                {
                                    SIMONGene newG = new SIMONGene();
                                    newG.ElementName = selectedDNA[l][i + k][j].ElementName;
                                    newG.ElementWeight = selectedDNA[l][i + k][j].ElementWeight;
                                    gA.Add(newG);
                                }

                            }
                            for (int j = 0; j < selectedDNA[l][i].Count; j++)
                            {
                                if (backFrontValue >= j)
                                {
                                    SIMONGene newG = new SIMONGene();
                                    newG.ElementName = selectedDNA[l][i + k][j].ElementName;
                                    newG.ElementWeight = selectedDNA[l][i + k][j].ElementWeight;
                                    gB.Add(newG);
                                }
                                else
                                {
                                    SIMONGene newG = new SIMONGene();
                                    newG.ElementName = selectedDNA[l][i][j].ElementName;
                                    newG.ElementWeight = selectedDNA[l][i][j].ElementWeight;
                                    gB.Add(newG);
                                }
                            }
                            crossedDNA[l].Add(gA);
                            poolCnt++;
                            crossedDNA[l].Add(gB);
                            poolCnt++;
                            backFrontValue++;
                        }
                    }
                }
            }
            return crossedDNA;
        }

        /// <summary>
        /// 교차된 교DNA 집합에 대한 돌연변이 연산을 수행하고 결과 Set을 리턴합니다.
        /// </summary>
        /// <param name="crossedDNA">교차된 2차원 DNA 풀입니다.</param>
        /// <returns></returns>
        public List<List<SIMONGene>> Mutation(List<List<SIMONGene>> crossedDNA)
        {
            List<List<SIMONGene>> mutatedDNA = new List<List<SIMONGene>>();

			Random rand = new Random((int)DateTime.Now.Ticks);
            int crossedDNACount = crossedDNA.Count;

            for (int i = 0; i < crossedDNACount; i++)
            {
                if (crossedDNA[i].Count == 0)
                    break;
                List<SIMONGene> g = new List<SIMONGene>();
                for (int j = 0; j < crossedDNA[i].Count; j++)
                    g.Add(crossedDNA[i][j]);
                for (int j = 0; j < crossedDNA[i].Count; j++)
                {
                    double flag = rand.NextDouble();
                    int sign = rand.Next(0, 2);

                    if (flag <= (MutationChance / SIMONConstants.GENE_MUTATION_PERCENT))
                    {
                        if (sign == 0)
                        {
                            g[j].ElementWeight += (g[j].ElementWeight * (MutationProportion / SIMONConstants.GENE_MUTATION_PERCENT));
                        }
                        else
                        {
                            g[j].ElementWeight -= (g[j].ElementWeight * (MutationProportion / SIMONConstants.GENE_MUTATION_PERCENT));
                        }
                    }
                }
                mutatedDNA.Add(g);
            }
            return mutatedDNA;
        }

        /// <summary>
        /// 교차된 DNA 집합에 대한 돌연변이 연산을 수행하고 결과 Set을 리턴합니다.
        /// </summary>
        /// <param name="crossedDNA">교차된 3차원 DNA 풀입니다.</param>
        /// <returns></returns>
        public List<List<List<SIMONGene>>> Mutation(List<List<List<SIMONGene>>> crossedDNA)
        {
            List<List<List<SIMONGene>>> mutatedDNA = new List<List<List<SIMONGene>>>();

            Random r = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < crossedDNA.Count; i++)
            {
                mutatedDNA.Add(new List<List<SIMONGene>>());

                for (int j = 0; j < crossedDNA[i].Count; j++)
                {
                    List<SIMONGene> g = crossedDNA[i][j];
                    List<SIMONGene> tmp1 = new List<SIMONGene>();
                    for (int k = 0; k < g.Count; k++)
                    {
                        tmp1.Add(new SIMONGene(g[k]));
                    }
                    for (int k = 0; k < g.Count; k++)
                    {
                        double flag = r.NextDouble();
                        int sign = r.Next(0, 2);

                        if (flag <= (MutationChance / SIMONConstants.GENE_MUTATION_PERCENT))
                        {
                            if (sign == 0)
                            {
                                tmp1[k].ElementWeight += (tmp1[k].ElementWeight * (MutationProportion / SIMONConstants.GENE_MUTATION_PERCENT));
                            }
                            else
                            {
                                tmp1[k].ElementWeight -= (tmp1[k].ElementWeight * (MutationProportion / SIMONConstants.GENE_MUTATION_PERCENT));
                            }
                        }
                    }
                    mutatedDNA[i].Add(tmp1);
                }
            }

            return mutatedDNA;
        }

        /// <summary>
        /// SIMONObjectCollection 집합내의 Property DNA를 진화시키는 함수를 제공합니다.
        /// </summary>
        /// <param name="PropertyCollection">2차원의 대상 SIMON Collection 입니다.</param>
        /// <param name="nextDNA">진화시킬 다음 세대의 2차원 DNA입니다.</param>
        private void Evolution(SIMONCollection PropertyCollection, List<List<SIMONGene>> nextDNA)
        {
            if (PropertyCollection.Count <= 0 || nextDNA == null || nextDNA.Count <= 0)
                return;
            int objectCount = PropertyCollection.Count;
			Random rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < objectCount; i++)
            {
                int selectDNAIdx = rand.Next(0, nextDNA.Count - 1);
                for (int j = 0; j < nextDNA[selectDNAIdx].Count; j++)
                {
                    for (int k = 0; k < ((SIMONObject)PropertyCollection.ValueOfIndex(i)).PropertyDNA.Count; k++)
                    {
                        if (((SIMONObject)PropertyCollection.ValueOfIndex(i)).PropertyDNA[k].ElementName == nextDNA[selectDNAIdx][j].ElementName)
                        {
                            ((SIMONObject)PropertyCollection.ValueOfIndex(i)).PropertyDNA[k] = nextDNA[selectDNAIdx][j];
                        }
                    }
                }
            }
        }
        /// <summary>
        /// SIMONObjectCollection 집합내의 Property DNA를 진화시키는 함수를 제공합니다.
        /// </summary>
        /// <param name="PropertyCollection">2차원의 대상 SIMON Collection 입니다.</param>
        /// <param name="nextDNA">진화시킬 다음 세대의 2차원 DNA입니다.</param>
        /// <param name="learningRate">학습률을 적용합니다.</param>
        private void Evolution(SIMONCollection PropertyCollection, List<List<SIMONGene>> nextDNA, double learningRate)
        {
            if (PropertyCollection.Count <= 0 || nextDNA == null || nextDNA.Count <= 0)
                return;
            int objectCount = PropertyCollection.Count;
			Random rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < nextDNA.Count; i++)
            {
                int nextGeneElementCount = nextDNA[i].Count;
                for (int j = 0; j < nextGeneElementCount; j++)
                    nextDNA[i][j].ElementWeight *= learningRate;
            }

            for (int i = 0; i < objectCount; i++)
            {
                int selectDNAIdx = rand.Next(0, nextDNA.Count - 1);
                ((SIMONObject)PropertyCollection.ValueOfIndex(i)).PropertyDNA = nextDNA[selectDNAIdx];
            }
        }

        /// <summary>
        /// SIMONObjectCollection 집합에 대해서 다음 세대의 DNA를 적용시켜서 진화시킵니다.
        /// </summary>
        /// <param name="ObjectCollection">3차원의 대상 SIMON Collection 입니다.</param>
        /// <param name="ActionMap">3차원의 ActionMap 입니다.</param>
        /// <param name="nextDNA">진화시킬 다음 세대의 3차원 DNA입니다.</param>
        private void Evolution(SIMONCollection ObjectCollection, SIMONCollection ActionMap, List<List<List<SIMONGene>>> nextDNA)
        {
            try
            {
                if (ObjectCollection.Count <= 0 || nextDNA == null || nextDNA.Count <= 0)
                    return;
                int numberOfDNA = nextDNA.Count;

                for (int i = 0; i < ActionMap.Count; i++)
                {
                    if (nextDNA[i].Count < 1)
                        continue;
					Random rand = new Random((int)DateTime.Now.Ticks);

                    List<SIMONObject> actionObjectList = (List<SIMONObject>)ActionMap.ValueOfIndex(i);

                    for (int j = 0; j < actionObjectList.Count; j++)
                    {
                        for (int k = 0; k < actionObjectList[j].Actions.Count; k++)
                        {
                            if (ActionMap.KeyOfIndex(i).Equals(actionObjectList[j].Actions[k].ActionName))
                            {
                                int geneIdx = rand.Next(0, nextDNA[i].Count);
                                ((List<SIMONObject>)ActionMap.ValueOfIndex(i))[j].Actions[k].ActionDNA = nextDNA[i][geneIdx];
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// SIMONObjectCollection 집합에 대해서 다음 세대의 DNA를 적용시켜서 진화시킵니다.
        /// </summary>
        /// <param name="ObjectCollection">3차원의 대상 SIMON Collection 입니다.</param>
        /// <param name="ActionMap">3차원의 ActionMap 입니다.</param>
        /// <param name="nextDNA">진화시킬 다음 세대의 3차원 DNA입니다.</param>
        /// <param name="learningRate">진화의 학습률입니다.</param>
        private void Evolution(SIMONCollection ObjectCollection, SIMONCollection ActionMap, List<List<List<SIMONGene>>> nextDNA, double learningRate)
        {
            try
            {
                if (ObjectCollection.Count <= 0 || nextDNA == null || nextDNA.Count <= 0)
                    return;
                int numberOfDNA = nextDNA.Count;

                for (int i = 0; i < ActionMap.Count; i++)
                {
                    if (nextDNA[i].Count < 1)
                        continue;
                    int actionObjDNACnt = nextDNA[i].Count;
                    for (int j = 0; j < actionObjDNACnt; j++)
                    {
                        int nextDNAGeneCnt = nextDNA[i][j].Count;
                        for (int k = 0; k < nextDNAGeneCnt; k++)
                        {
                            nextDNA[i][j][k].ElementWeight *= learningRate;
                        }
                    }
                }

                for (int i = 0; i < ActionMap.Count; i++)
                {
                    if (nextDNA[i].Count < 1)
                        continue;
					Random rand = new Random((int)DateTime.Now.Ticks);
                    int randIdx = rand.Next(0, numberOfDNA);

                    List<SIMONObject> actionObjectList = (List<SIMONObject>)ActionMap.ValueOfIndex(i);

                    for (int j = 0; j < actionObjectList.Count; j++)
                    {
                        for (int k = 0; k < actionObjectList[j].Actions.Count; k++)
                        {
                            if (ActionMap.KeyOfIndex(i).Equals(actionObjectList[j].Actions[k].ActionName))
                            {
                                int geneIdx = rand.Next(0, nextDNA[i].Count);
                                ((List<SIMONObject>)ActionMap.ValueOfIndex(i))[j].Actions[k].ActionDNA = nextDNA[i][geneIdx];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// ObjectCollection에 대해서 Property DNA를 업데이트합니다.
        /// </summary>
        /// <param name="ObjectCollection"></param>
        private void UpdateObjectsPropertyDNA(SIMONCollection ObjectCollection)
        {
            for (int i = 0; i < ObjectCollection.Count; i++)
            {
                SIMONObject elementObject = (SIMONObject)ObjectCollection.ValueOfIndex(i);
                for (int j = 0; j < elementObject.Properties.Count; j++)
                {
                    if (elementObject.Properties[j].Inherit)
                    {
                        for (int k = 0; k < elementObject.PropertyDNA.Count; k++)
                        {
                            if (elementObject.PropertyDNA[k].ElementName.Equals(elementObject.Properties[j].PropertyName))
                            {
                                elementObject.Properties[j].PropertyValue = elementObject.PropertyDNA[k].ElementWeight;
                            }
                        }
                    }
                }
            }
        }

    }
}
