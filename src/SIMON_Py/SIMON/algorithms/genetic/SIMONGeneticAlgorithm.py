from collections import OrderedDict
from SIMON.objects.SIMONGene import SIMONGene
import copy
import random

__author__ = 'KCP'
__author__ = "ParkJinSang"

from SIMON import SIMONFunction

class SIMONGeneticAlgorithm:
    def __init__(self):
        pass

    #
    #   class for evaluating algorithmetic value in the algorithm layer.
    #   It includes fitness value for evaluating and dnaList.
    #
    class GeneValue:
        fitness_value = 0
        dnaList = []
        def __getitem__(self, item):
            return self.fitness_value

    #
    #   evaluate action by using its fitness value
    #
    #
    def action_fitness_evaluation(self, group, actionName):
        gene_evaluation_list = []
        for element in group:
            theOthers = {}
            for other in group:
                if(other is not element):
                    theOthers[other.ObjectID] = other

            gene_eval = self.GeneValue()
            if(not element.Actions.__contains__(actionName)):
                continue
            gene_eval.fitness_value = SIMONFunction.UserFunctionCollection[element.Actions[actionName].FitnessFunctionName](element, theOthers)
            gene_eval.dnaList = element.Actions[actionName].ActionDNA

            gene_evaluation_list.append(gene_eval)
        gene_evaluation_list = sorted(gene_evaluation_list, key=lambda x:x[0])
        return gene_evaluation_list


    #
    #   evaluate property by using its fitness value
    #
    #
    def property_fitness_evaluation(self, group):
        gene_evaluation_list = []

        for element in group:
            theOthers = {}
            for other in group:
                if(element is not other):
                    theOthers[other.ObjectID] = other

            gene_eval = self.GeneValue()

            gene_eval.fitness_value = SIMONFunction.UserFunctionCollection[element.ObjectFunctionName](element, theOthers)
            gene_eval.dnaList = element.PropertyDNA

            gene_evaluation_list.append(gene_eval)
        gene_evaluation_list = sorted(gene_evaluation_list, key=lambda x:x[0])
        return gene_evaluation_list


    #
    #   classify the genetic elements. If the element's fitness value is greater than threshold, then it is classified in the dominion group. If not, it is included in the recessive group
    #
    #
    def fitness_classify(self, gene_evaluation_list,  recessive_group, dominion_group, recessive_dominion_threshold):
        for evaluation_classify_iterator in range(0, len(gene_evaluation_list)):
            if(evaluation_classify_iterator < recessive_dominion_threshold):
                recessive_group.append(gene_evaluation_list[evaluation_classify_iterator])
            else:
                dominion_group.append(gene_evaluation_list[evaluation_classify_iterator])

    #
    #   selection function of the genetic algorithm for property.
    #
    #
    def selection_action(self, group, actionName, DNAPool):
        selected_dna = []

        gene_evaluation_list = self.action_fitness_evaluation(group, actionName)

        from SIMON import SIMONConstants

        recessive_dominion_threshold = int(len(gene_evaluation_list) * SIMONConstants.RECESSIVE_RATIO())

        recessive_group = []
        dominion_group = []

        self.fitness_classify(gene_evaluation_list, recessive_group, dominion_group, recessive_dominion_threshold)

        if(len(recessive_group) > 0):
            rouletteGene = self.RouletteWheel(recessive_group, False)
            for roulette_gene_element in rouletteGene:
                selected_dna.append(roulette_gene_element)
        if(len(dominion_group) > 0):
            rouletteGene = self.RouletteWheel(dominion_group, True)
            for roulette_gene_element in rouletteGene:
                selected_dna.append(roulette_gene_element)

        return selected_dna


    #
    #   selection function of the genetic algorithm for property
    #
    #
    def selection_property(self, group, DNAPool):
        selected_dna = []

        gene_evaluation_list = self.property_fitness_evaluation(group)

        from SIMON import SIMONConstants

        recessive_dominion_threshold = int(len(gene_evaluation_list) * SIMONConstants.RECESSIVE_RATIO())

        recessive_group = []
        dominion_group = []

        self.fitness_classify(gene_evaluation_list, recessive_group, dominion_group, recessive_dominion_threshold)

        if(len(recessive_group) > 0):
            rouletteGene = self.RouletteWheel(recessive_group, False)
            for roulette_gene_element in rouletteGene:
                selected_dna.append(roulette_gene_element)
        if(len(dominion_group) > 0):
            rouletteGene = self.RouletteWheel(dominion_group, True)
            for roulette_gene_element in rouletteGene:
                selected_dna.append(roulette_gene_element)

        return selected_dna

    #
    #   regularization of the fitness values in the roulette. The values less then 0 would be moved to proper value.
    #
    #
    def roulette_regularization(self, dnaPool , fitness_min_value):
        regularization_value = 0;
        if fitness_min_value > 0 :
            pass
        elif fitness_min_value == 0 :
            regularization_value = 1 * 2
        else :
            regularization_value = fitness_min_value * -2
        for dna in dnaPool:
            dna.fitness_value += regularization_value

    #
    #   assign the index values to play roulette wheel.
    #
    #
    def roulette_assign(self, roulette, dnaPool):
        for dna_iteration_idx in range(0, len(dnaPool)):
            assign = dnaPool[dna_iteration_idx].fitness_value
            for assign_iter in range(0, assign):
                roulette.append(dna_iteration_idx)

    #
    #   roulette wheel algorithm to randomize the frequency of dna
    #   use 1:3 ratio which means the ratio of the recessive selection is 1 and it of the dominion selection is 3.
    #
    def RouletteWheel(self, dnaPool, dominion_select):
        rouletteGene = []

        if len(dnaPool) < 1:
            return dnaPool

        min_fitness_value_of_dna = dnaPool[0].fitness_value

        self.roulette_regularization(dnaPool, min_fitness_value_of_dna)

        roulette = []

        self.roulette_assign(roulette, dnaPool)

        from SIMON import SIMONConstants

        number_to_select = 0
        if(dominion_select is True):
            number_to_select = int(len(dnaPool) * SIMONConstants.DOMINION_RATIO())
        else:
            number_to_select = int(len(dnaPool) * SIMONConstants.RECESSIVE_RATIO())

        import random
        roulette_selected = []
        while number_to_select > 0:
            dart_idx = random.randint(0, len(roulette)-1)

            flag = 0
            while (flag < len(roulette_selected)) and (roulette_selected[flag] != dart_idx):
                flag += 1
            if(flag == len(roulette_selected)):
                roulette_selected.append(dart_idx)
            else:
                continue

            rouletteGene.append(dnaPool[roulette[dart_idx]].dnaList)
            number_to_select -= 1

        return rouletteGene

    #
    #   cross the dna combination
    #
    #
    def crossover_routine(self, dA = None, dB = None):
        if dA is not None and dB is not None:
            result = dA

            if(len(dA) - 2 < 1):
                return None

            point_idx = random.randint(1, len(dA)-2)

            # 대상의 앞을 자르느냐 뒤를 자르느냐를 결정.
            cross_point_flag = random.randint(0, 1)

            if(cross_point_flag is 0):
                dictionary_iterator = 0
                for dkey, delement in dB.items():
                    if(dictionary_iterator < point_idx):
                        if(result.__contains__(dkey)):
                            result[dkey] = delement
                    dictionary_iterator += 1
            else:
                dictionary_iterator = 1
                for dkey, delement in dB.items():
                    if(dictionary_iterator >= point_idx):
                        if(result.__contains__(dkey)):
                            result[dkey] = delement
                    dictionary_iterator += 1
            return result
        else:
            return None

    #
    #   make the combination from the dna pool.
    #
    #
    def crossover_combination(self, combination_list, com_idx, list_len ):
        selected = ()
        while True:
            j = 0
            while True:
                j = random.randint(0,list_len)
                if com_idx != j :
                    break
            flag  = True
            for k in range(len(combination_list)):
                if combination_list[k] == (com_idx, j) or combination_list[k] == (j, com_idx):
                    flag = False
                    break
            if flag :
                selected = (com_idx,j)
#                combination_list.append(selected)
                break
        return selected

    #
    #   crossover function of the genetic algorithm for action
    #
    #
    def crossover_action(self, DNAPool = None):
        if DNAPool is not None:
            result = []
            if len(DNAPool)<2 :
                return DNAPool
            for i in range(len(DNAPool)):
                #DNA select routine
                selected_combination_list = []

                selected = self.crossover_combination(selected_combination_list, i, len(DNAPool)-1)
                selected_combination_list.append(selected)

                Child = self.crossover_routine(DNAPool[selected[0]], DNAPool[selected[1]])
                if(Child is None):
                    continue
                result.append(Child)
            return result
        else:
            return None

    #
    #   crossover function of the genetic algorithm for property
    #
    #
    def crossover_property(self, DNAPool = None):
        if DNAPool is not None:
            result = []
            if len(DNAPool)<2 :
                return DNAPool
            for i in range(len(DNAPool)):
                #DNA select routine
                selected_combination_list = []

                selected = self.crossover_combination(selected_combination_list, i, len(DNAPool)-1)
                selected_combination_list.append(selected)

                Child = self.crossover_routine(DNAPool[selected[0]], DNAPool[selected[1]])
                if(Child is None):
                    continue
                result.append(Child)
            return result
        else:
            return None

    #
    #   mutation function of the genetic algorithm for action
    #
    #
    def mutation_action(self, crossedDNA):
        mutatedDNA = []
        mutation_chance = 0.4
        mutation_proportion = 0.1
        GENE_MUTATION_PERCENT = 0.8

        for i in range(len(crossedDNA)):
            tmp1 = []

            g = crossedDNA[i]
            if(len(crossedDNA[i]) <= 1):
                    # 이부분 파이썬의 자동 캐스팅 때문에 걍 pass 시킴.
                continue

            for gene_key, gene_value in g.items():
                flag = random.random()
                sign = random.randrange(0,2)
                if flag <= (mutation_chance):
                    if sign == 0:
                        g[gene_key] += (gene_value*mutation_proportion)
                    else:
                        g[gene_key] -= (gene_value*mutation_proportion)

            tmp1.append(g)
            if(len(tmp1) > 0):
                mutatedDNA.append(tmp1)
        if(len(mutatedDNA) < 1):
            mutatedDNA = crossedDNA
        return mutatedDNA

    #
    #   mutation function of the genetic algorithm for property
    #
    #
    def mutation_property(self, crossedDNA):
        mutatedDNA = []
        mutation_chance = 0.4
        mutation_proportion = 0.1
        GENE_MUTATION_PERCENT = 0.8

        for i in range(len(crossedDNA)):
            tmp1 = []

            g = crossedDNA[i]
            if(len(crossedDNA[i]) <= 1):
                    # 이부분 파이썬의 자동 캐스팅 때문에 걍 pass 시킴.
                continue

            for gene_key, gene_value in g.items():
                flag = random.random()
                sign = random.randrange(0,2)
                if flag <= (mutation_chance):
                    if sign == 0:
                        g[gene_key].PropertyValue += (gene_value.PropertyValue*mutation_proportion)
                    else:
                        g[gene_key].PropertyValue -= (gene_value.PropertyValue*mutation_proportion)

            tmp1.append(g)
            if(len(tmp1) > 0):
                mutatedDNA.append(tmp1)
        if(len(mutatedDNA) < 1):
            mutatedDNA = crossedDNA
        return mutatedDNA

