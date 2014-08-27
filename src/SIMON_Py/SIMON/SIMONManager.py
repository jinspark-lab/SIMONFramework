from SIMON.algorithms import SIMONAlgorithmMain
from SIMON.objects import SIMONGeneticObject
from SIMON.utils import SIMONObjectSerializer, SIMONFileStream

__author__ = 'Park'

import os

from collections import OrderedDict

from SIMON import SIMONCollection
from SIMON import SIMONFunction
from SIMON import SIMONIntelligence

# @SIMONSingleton.Singleton
class SIMONManager:

    SIMONSerializer = SIMONObjectSerializer.SIMONObjectSerializer()

    ProjectName = ""

    def __init__(self, projectName):
        print("Launch SIMON Manager")
        self.ProjectName = projectName
        from SIMON import SIMONConstants
        if(not os.path.isdir(SIMONConstants.PROJECT_DIR_PATH() + "\\" + self.ProjectName)):
            self.CreateWorkspace()

    #
    #   create initial workspace for project using SIMON
    #
    #
    def create_workspace(self):
        from SIMON import SIMONConstants
        projectPath = SIMONConstants.PROJECT_DIR_PATH() + "\\" + self.ProjectName

        if(not os.path.isdir(projectPath)):
            os.makedirs(projectPath)
        if(not os.path.isdir(projectPath + "\\definition")):
            os.makedirs(projectPath + "\\definition")
        if(not os.path.isdir(projectPath + "\\history")):
            os.makedirs(projectPath + "\\history")

    #
    #   clear memory data from workspace.
    #   This method does not remove the files inside workspace
    #
    def clear_workspace(self):
        SIMONCollection.SIMONObjectCollection.clear()
        SIMONFunction.UserFunctionCollection.clear()

    #
    #   build the memory data from local workspace
    #
    #
    def load_workspace(self, projectName):
        from SIMON import SIMONConstants
        projectPath = SIMONConstants.PROJECT_DIR_PATH() + "\\" + projectName

        definitionPath = projectPath + "\\definition"
        definitionFiles = os.listdir(definitionPath)

        for filename in definitionFiles:
            definitionObject = SIMONGeneticObject()
            self.SIMONSerializer.FromJSON(definitionObject, self.SIMONSerializer.LoadJSON(fileName=filename))
            SIMONCollection.SIMONObjectCollection[definitionObject.ObjectID] = definitionObject

        self.ProjectName = projectName

    #
    #   arrange the simon objects order by its actions
    #
    #
    def create_actionmap(self, group):
        actionMap = OrderedDict()
        if(hasattr(group, '__iter__')):
            for element in group:
                for actionOfElement in element.Actions.keys():
                    if not actionMap.__contains__(actionOfElement):
                        actionMap[actionOfElement] = []
                        actionMap[actionOfElement].append(element)
                    else:
                        actionMap[actionOfElement].append(element)

            return actionMap
        else:
            for actionOfElement in group.Actions.keys():
                if not actionMap.__contains__(actionOfElement):
                    actionMap[actionOfElement] = group
                else:
                    actionMap[actionOfElement] = group

            return actionMap

    #
    #   main routine of the SIMON manager.
    #   In this method, it contains classifying, making a decision and executing it.
    #
    def run_routine(self):

        from SIMON import SIMONConstants


        for element in SIMONCollection.SIMONObjectCollection.values():
            otherObjects = OrderedDict()

            #                                                                                     #
            #                            분                          류                           #
            #                                                                                     #

            for unit in SIMONCollection.SIMONObjectCollection.values():
                if unit is not element:
                    otherObjects[unit.ObjectID] = unit

            actionValueTable = {}
            actionMaxList = []
            actionMaxValue = SIMONConstants.MINIMUM_CMP_VALUE()

            #                                                                                     #
            #                            판                          단                           #
            #                                                                                     #
            # Implement Rule-based AI


            # 각 actionFunction들을 실행하면서 최댓값(들) 을 추출해냄.
            for actionObject in element.Actions.values():
                actionValue = SIMONFunction.UserFunctionCollection[actionObject.ActionFunctionName](element, otherObjects)

                print("ActionValue : " + str(actionValue))

                # dictionary의 키값으로 실행함수의 이름을, 밸류값으로 판단값을 추가시킨다.
                if(actionValue is not None):
                    actionValueTable[actionObject.ExecutionFunctionName] = actionValue
                if(actionValue > actionMaxValue):
                    actionMaxValue = actionValue

            # 가장 판단값이 큰 리스트의 키값(판단값이 큰 액션의 실행함수의 이름을 저장한다.
            for actionValueKey, actionValueElement in actionValueTable.items():
                if(actionValueElement == actionMaxValue):
                    actionMaxList.append(actionValueKey)

            from random import randint
            actionMaxKey = ""
            if actionMaxList.__len__() > 0:
                actionMaxKey = actionMaxList[randint(0, actionMaxList.__len__()-1)]

            print("Most Action : " + actionMaxKey)

            # 가장 판단값이 큰 액션의 실행함수를 실행. Parameter로 element 자신과, 나머지 elements를 넘긴다.
            SIMONFunction.UserFunctionCollection[actionMaxKey](element, otherObjects)


            #                                                                  #
            #                   기                           록                #
            #                                                                  #

            SIMONFileStream.WriteHistory(SIMONConstants.PROJECT_DIR_PATH() + "\\" + self.ProjectName + "\\" + "\\history\\" + element.ObjectID + ".csv", element)


    #
    #   method to run machine learning for objects in the group
    #
    #
    def learn_genetic_model(self, group):

        actionMap = self.create_actionmap(group)

        actionPool = SIMONIntelligence.generateActionPool(group)

        propertyPool = SIMONIntelligence.generatePropertyPool(group)

        SIMONAlgorithmMain.run_genetic_algorithm(group, actionPool, propertyPool)



