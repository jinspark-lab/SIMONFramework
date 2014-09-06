__author__ = 'Park'


from SIMON import SIMONFunction
from SIMON.utils import SIMONObjectSerializer, SIMONFileStream

from collections import OrderedDict


class SIMONUtilitybaseAgent:

    def __init__(self):

        pass


    #
    #   make decision by utility base
    #   select the action which is evaluated the best
    #
    def make_decision_by_utility_base(self, element, otherObjects):

        from SIMON import SIMONConstants

        actionValueTable = {}
        actionMaxList = []
        actionMaxValue = SIMONConstants.MINIMUM_CMP_VALUE()

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

        return actionMaxKey


    #
    #   agent module for utility based AI
    #
    #
    def run_agent(self, group):

        from SIMON import SIMONConstants

        for element in group.values():
            otherObjects = OrderedDict()

            #                                                                                     #
            #                            분                          류                           #
            #                                                                                     #

            for unit in group.values():
                if unit is not element:
                    otherObjects[unit.ObjectID] = unit

            #                                                                                     #
            #                            판                          단                           #
            #                                                                                     #
            actionMaxKey = ""
            actionMaxKey = self.make_decision_by_utility_base(element, otherObjects)

            #                                                                                               #
            #                           실                           행                                     #
            # 가장 판단값이 큰 액션의 실행함수를 실행. Parameter로 element 자신과, 나머지 elements를 넘긴다.#
            SIMONFunction.execute_action(actionMaxKey, element, otherObjects)


            #                                                                  #
            #                   기                           록                #
            #                                                                  #

            SIMONFileStream.WriteHistory(SIMONConstants.PROJECT_DIR_PATH() + "\\" + self.ProjectName + "\\" + "\\history\\" + element.ObjectID + ".csv", element)

