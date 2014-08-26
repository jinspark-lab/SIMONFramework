__author__ = 'PARKJINSANG'

from SIMON.algorithms.genetic.SIMONGeneticAlgorithm import SIMONGeneticAlgorithm

#
#   genetic algorithm for learning and evaluating
#
#
def run_genetic_algorithm(group, actionPool, propertyPool):

    algorithm = SIMONGeneticAlgorithm()
    for actionName, actionDnaList in actionPool.items():
        if(len(actionDnaList) < 1):
            continue
        selectedPool = algorithm.selection_action(group, actionName, actionDnaList)
        crossedPool = algorithm.crossover_action(selectedPool)
        mutatedPool = algorithm.mutation_action(crossedPool)
        update_action(group, actionName, mutatedPool)

    selectedPool = algorithm.selection_property(group, propertyPool)
    crossedPool = algorithm.crossover_property(selectedPool)
    mutatedPool = algorithm.mutation_property(crossedPool)
    update_property(group, mutatedPool)

#
#   update action dna in the group
#
#
def update_action(group, actionName, actionPool=None):

    import random
    for element in group:
        if(len(actionPool)-1 < 0):
            continue
        evolve_idx = random.randint(0, len(actionPool)-1)
        if(element.Actions.__contains__(actionName)):
            element.Actions[actionName].ActionDNA = actionPool[evolve_idx]

#
#   update property dna in the group
#
#
def update_property(group, propertyPool=None):

    import random
    for prop_list in propertyPool:
        for element in group:
            if(len(prop_list)-1 < 0):
                continue
            update_idx = random.randint(0, len(prop_list)-1)
            for key_prop_list, element_prop_list in prop_list[update_idx].items():
                if(element.PropertyDNA.__contains__(key_prop_list)):
                    element.PropertyDNA[key_prop_list] = element_prop_list
                    element.Properties[key_prop_list] = element_prop_list


