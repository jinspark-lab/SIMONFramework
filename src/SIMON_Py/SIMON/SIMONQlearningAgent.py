__author__ = 'Park'


# (action, state(ordereddict)) 이렇게 튜플로 구성된 리스트인 q table을 만들자.

from collections import OrderedDict




#
#   한개의 Agent 는 한개의 Object에 대해서만 작동하게끔 만들어야 한다.
#
#
class SIMONQLearningAgent:

#   qtable에는 보상값을 넣어야한다는 것을 잊지말게.
    qtable = OrderedDict()
    reward = 0
    gamma = 0

    # state_for_agent 에 순서대로 기록한다.
    state_for_agent = OrderedDict()


    def __init__(self, reward, gamma, object_element, **kwargs):
        self.reward = reward
        self.gamma = gamma

        for arg_key, arg_val in kwargs.items():
            self.state_for_agent[arg_key] = arg_val
        for action in object_element.Actions.values():
            self.qtable[action.ActionName] = OrderedDict()
        pass

    def allocate_qtable(self, list_to_append, state_depth):
        if(len(self.state_for_agent) < 1):
            pass
        if(state_depth >= len(self.state_for_agent)):
            pass

    def set_status(self, **kwargs):
        for arg_key, arg_val in kwargs.items():
            self.state_for_agent[arg_key] = arg_val



    #
    #   select qtable element recursively.
    #
    #
    def recursive_qtable_element(self, qtable_element, key_idx, **kwargs):
        state_name = None
        iter_count = 0
        if(key_idx >= len(kwargs.keys())):
            return qtable_element
        for state_key in kwargs.keys():
            if(iter_count == key_idx):
                state_name = state_key
                break
            iter_count+=1
        if(state_name is None):
            return None
        result_element = self.recursive_qtable_element(qtable_element[state_name], key_idx+1, **kwargs)
        return result_element

    #
    #
    #
    #
    def make_decision_by_reinforcement(self, element, **kwargs):
        # kwargs 를 통해서 state값들이 dictionary 형태로 들어오도록 한다.
        # qtable의 존재때문에 클래스 안에 두는게 맞을 것으로 보인다. 그러면 utility base도 역시 같게 통일시키자.
        # 아니아니 밖으로 빼도 되는 이유는 qtable에 들어가는게 '보상값' 이 되어야 하기 때문이다.

        decision_array = []

        for act in element.Actions.values():

            qtable_value = self.recursive_qtable_element(self.qtable_element[act], 1, **kwargs)

            if(qtable_value is None):
                # 보상값이 비어있다.
                pass
            else:
                pass

            decision_array_tuple = (act.ActionName, qtable_value)

            decision_array.append(decision_array_tuple)


            pass

        pass


    def run_model(self, group, **kwargs):

        #액션을 수행하고, 보상값에 따라서 qtable을 업데이트 해서 저장해야 한다.
        #판단 행동 자체의 경우, 즉 선택하는 자체는 Utility-base 모델을 따라야 한다.
        #상태를 판단하고, 이미 qtable에 저장된 상태인 경우에는 '판단' 할 필요없이 저장된 q값 경로를 참조하면 된다.
        #그런 것이 history를 통한 '판단' 에 해당 되는 경우이며 qtable에 저장되어있지 않은 경우에는 그냥 random 하게 '판단' 시키면 된다.


        pass

    def append_qtable_tuple(self, action, state):
        pass
#        tuple_element = (action, state)
#        self.qtable.append(tuple_element)

    def update_qtable(self, element, action, state):

        pass


