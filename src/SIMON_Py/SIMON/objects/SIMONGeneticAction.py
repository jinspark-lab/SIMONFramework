__author__ = 'PARKJINSANG'

from collections import OrderedDict
import json

class SIMONGeneticAction:
    ActionName = ""
    ActionFunctionName = ""
    ExecutionFunctionName = ""
    FitnessFunctionName = ""

#    @property
#    def ActionName(self):
#        return self._ActionName
#    @ActionName.setter
#    def ActionName(self, actionName):
#        self._ActionName = actionName
#    @ActionName.deleter
#    def ActionName(self):
#        del self._ActionName

#    @property
#    def ActionFunctionName(self):
#        return self._ActionFunctionName
#    @ActionFunctionName.setter
#    def ActionFunctionName(self, actionFunctionName):
#        self._ActionFunctionName = actionFunctionName
#    @ActionFunctionName.deleter
#    def ActionFunctionName(self):
#        del self._ActionFunctionName

#    @property
#    def ExecutionFunctionName(self):
#        return self._ExecutionFunctionName
#    @ExecutionFunctionName.setter
#    def ExecutionFunctionName(self, executionFunctionName):
#        self._ExecutionFunctionName = executionFunctionName
#    @ExecutionFunctionName.deleter
#    def ExecutionFunctionName(self):
#        del self._ExecutionFunctionName

#    @property
#    def FitnessFunctionName(self):
#        return self._FitnessFunctionName
#    def FitnessFunctionName(self, fitnessFunctionName):
#        self._FitnessFunctionName = fitnessFunctionName
#    @FitnessFunctionName.deleter
#    def FitnessFunctionName(self):
#        del self._FitnessFunctionName

#    ActionDNA = OrderedDict()

    def __init__(self, actionName = None, actionFunctionName = None, executionFunctionName = None, fitnessFunctionName = None, actionDNA = None):
        if(actionName is not None):
            self.ActionName = actionName
        if(actionFunctionName is not None):
            self.ActionFunctionName = actionFunctionName
        if(executionFunctionName is not None):
            self.ExecutionFunctionName = executionFunctionName
        if(fitnessFunctionName is not None):
            self.FitnessFunctionName = fitnessFunctionName
        if(actionDNA is not None):
            self.ActionDNA = actionDNA
        self.ActionDNA = OrderedDict()

    def FindWeight(self, elementName):
        return self.ActionDNA[elementName].ElementWeight

    def SelectDNA(self, elementName):
        return self.ActionDNA[elementName]

    def InsertDNA(self, elementName, elementWeight):
        self.ActionDNA[elementName] = elementWeight

    def AppendDNA(self, elementGene):
        self.ActionDNA[elementGene.ElementName] = elementGene.ElementWeight

    def DeleteDNA(self, elementName):
        del self.ActionDNA[elementName]

    def ClearDNA(self):
        for gene in self.ActionDNA:
            del gene

    def ToJSON(self):
        return json.dumps(self, default=lambda o:o.__dict__, sort_keys=True, indent=4)
