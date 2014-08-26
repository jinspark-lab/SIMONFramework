from SIMON.objects import SIMONGeneticAction, SIMONGeneticProperty

__author__ = 'PARKJINSANG'

from collections import OrderedDict
import json

from SIMON import SIMONCollection
from SIMON.objects.SIMONGeneticProperty import SIMONGeneticProperty
from SIMON.objects.SIMONGeneticAction import SIMONGeneticAction
from SIMON.objects.SIMONGene import SIMONGene

class SIMONGeneticObject:

    ObjectID = ""

    def __init__(self, objectID, simon_json = None):
        if(objectID is not None):
            self.ObjectID = objectID
        else:
            self.ObjectID = "default"
        if(simon_json is not None):
            self.__dict__ = json.loads(simon_json)
        self.Properties = OrderedDict()
        self.Actions = OrderedDict()
        self.PropertyDNA = OrderedDict()
        self.ObjectFunctionName = ""
        SIMONCollection.SIMONObjectCollection[self.ObjectID] = self

#    def __del__(self):
#        if(SIMONCollection.SIMONObjectCollection[self.ObjectID] is not None):
#            SIMONCollection.SIMONObjectCollection.pop(self.ObjectID, None)

    def CopyFromObject(self, sObject):
        self.ObjectID = sObject.ObjectID
        self.Properties = sObject.Properties
        self.Actions = sObject.Actions

    def CopyToObject(self):
        newObject = SIMONGeneticObject
        newObject.ObjectID = self.ObjectID
        newObject.Properties = self.Properties
        newObject.Actions = self.Actions
        newObject.ObjectFunctionName = self.ObjectFunctionName
        newObject.PropertyDNA = self.PropertyDNA
        return newObject

    # LoadObjectDefinition Function

    # SaveObjectDefinition Function


    # SIMONGeneticObject의 ID값을 할당합니다.
    def AssignID(self, objectID):
        self.ObjectID = objectID


    # SIMONProperty를 구성하는 각 요소들을 인자로 전달하여 Property를 Object에 추가.
    def AssignProperty(self, propertyName, propertyValue, heritability):
        self.Properties[propertyName] = SIMONGeneticProperty()
        self.Properties[propertyName].PropertyName = propertyName
        self.Properties[propertyName].PropertyValue = propertyValue
        self.Properties[propertyName].Heritability = heritability
        if(heritability is True):
            self.PropertyDNA[propertyName] = SIMONGeneticProperty()
            self.PropertyDNA[propertyName].PropertyName = propertyName
            self.PropertyDNA[propertyName].PropertyValue = propertyValue
            self.PropertyDNA[propertyName].Heritability = heritability

    # SIMONAction을 구성하는 각 요소들을 인자로 전달받아 Action을 Object에 추가.
    def AssignAction(self, actionName, actionFunctionName, executionFunctionName, fitnessFunctionName, actionDNA=None):
        self.Actions[actionName] = SIMONGeneticAction()
        self.Actions[actionName].ActionName = actionName
        self.Actions[actionName].ActionFunctionName = actionFunctionName
        self.Actions[actionName].ExecutionFunctionName = executionFunctionName
        self.Actions[actionName].FitnessFunctionName = fitnessFunctionName
        if(actionDNA is not None):
            self.Actions[actionName].ActionDNA = actionDNA


    # 가변 길이를 갖는 dictionary 열거형 입력을 이용해서 baseAction 에 actionDNA 를 삽입.
    def AssignDNA(self, baseAction, **kwargs):
        for elementName, elementWeight in kwargs.items():
            self.Actions[baseAction].InsertDNA(elementName, elementWeight)


    def GetActionDNA(self, actionName):
        return self.Actions[actionName].ActionDNA

    def SetActionDNA(self, actionName, actionDNA):
        self.Actions[actionName].ActionDNA = actionDNA

    def DelActionDNA(self, actionName):
        self.Actions[actionName].clear()

    def ClearPropertyDNA(self):
        self.PropertyDNA.clear()

    def ToJSON(self):
        return json.dumps(self, default=lambda o:o.__dict__, sort_keys=True, indent=4)



