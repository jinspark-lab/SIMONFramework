from SIMON.agents import SIMONUtilitybaseAgent
from SIMON.algorithms import SIMONAlgorithmMain
from SIMON.objects import SIMONGeneticObject
from SIMON.utils import SIMONObjectSerializer, SIMONFileStream

__author__ = 'Park'

import os

from collections import OrderedDict

from SIMON import SIMONCollection
from SIMON import SIMONFunction
from SIMON import SIMONIntelligence

from SIMON.agents.SIMONAgent import SIMONAgent
from SIMON.agents.SIMONUtilitybaseAgent import SIMONUtilitybaseAgent
from SIMON.agents.SIMONQLearningAgent import SIMONQLearningAgent


# @SIMONSingleton.Singleton
class SIMONManager:

    SIMONSerializer = SIMONObjectSerializer.SIMONObjectSerializer()

    ProjectName = ""

    running_agent = SIMONAgent.utility_based_agent

    def __init__(self, projectName, agent=None):
        print("Launch SIMON Manager")
        self.ProjectName = projectName
        from SIMON import SIMONConstants
        if(not os.path.isdir(SIMONConstants.PROJECT_DIR_PATH() + "\\" + self.ProjectName)):
            self.CreateWorkspace()

        # initialize the agent object
        if agent is SIMONAgent.utility_based_agent:
            running_agent = SIMONUtilitybaseAgent
        elif agent is SIMONAgent.qlearning_agent:
            running_agent = SIMONQLearningAgent


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

        pass


    #
    #   method to run machine learning for objects in the group
    #
    #
    def learn_genetic_model(self, group):

        actionMap = self.create_actionmap(group)

        actionPool = SIMONIntelligence.generateActionPool(group)

        propertyPool = SIMONIntelligence.generatePropertyPool(group)

        SIMONAlgorithmMain.run_genetic_algorithm(group, actionPool, propertyPool)





