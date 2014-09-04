__author__ = 'Park'


from SIMON import SIMONFunction

#
#   make decision by utility base
#   select the action which is evaluated the best
#
def make_decision_by_utility_base(element, otherObjects):

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


