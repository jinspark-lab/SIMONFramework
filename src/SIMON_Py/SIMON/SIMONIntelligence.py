__author__ = 'Park'

import sys
from collections import OrderedDict

def preparePoolSpace(group, record_name):
    if(not group.__contains__(record_name)):
        group[record_name] = []

#
#   make the genetic pool of the properties
#
#
def generatePropertyPool(group):
    genePropertyPool = OrderedDict()
    for element in group:
        preparePoolSpace(genePropertyPool, element.ObjectID)
        element_property_dna = OrderedDict()
        for property in element.Properties.values():
            if (property.Heritability is True):
                element_property_dna[property.PropertyName] = property.PropertyValue
        genePropertyPool[element.ObjectID].append(element_property_dna)
    return genePropertyPool


#
#   make the genetic pool of the actions
#
#
def generateActionPool(group):
    geneActionPool = OrderedDict()

    for element in group:
        for action in element.Actions.values():
            preparePoolSpace(geneActionPool, action.ActionName)
            geneActionPool[action.ActionName].append(action.ActionDNA)
    return geneActionPool


