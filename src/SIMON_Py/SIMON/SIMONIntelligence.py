__author__ = 'Park'

import sys
from collections import OrderedDict

def preparePoolSpace(group, record_name):
    if(not group.__contains__(record_name)):
        group[record_name] = []

def generatePropertyPool(group):
    # Heritability 가 true인 요소들을 일단 먼저 전부 가져와서... 원 알고리즘 대로는 일단 통짜로 DNA를 만든다음에 알아서 가져가도록 구성되어있다.
    genePropertyPool = OrderedDict()
    for element in group:
        preparePoolSpace(genePropertyPool, element.ObjectID)
        element_property_dna = OrderedDict()
        for property in element.Properties.values():
            if (property.Heritability is True):
                element_property_dna[property.PropertyName] = property.PropertyValue
#                preparePoolSpace(genePropertyPool, property.PropertyName)
        genePropertyPool[element.ObjectID].append(element_property_dna)
    return genePropertyPool

#Object ID를 키값으로하는 Ordered Dict에 property 를 키값으로하고 value를 밸류값으로하는 OrderedDict를 2차원으로 넣어야함.

# 해당 Action의 ActionDNA에 대한 Pool을 만들고 반환합니다.
def generateActionPool(group):
    # Group 안의 Action들에 대해서 Action / List 이렇게 2차원 맵을 반환합니다.

    geneActionPool = OrderedDict()

    for element in group:
        for action in element.Actions.values():
            preparePoolSpace(geneActionPool, action.ActionName)
            geneActionPool[action.ActionName].append(action.ActionDNA)
    return geneActionPool


