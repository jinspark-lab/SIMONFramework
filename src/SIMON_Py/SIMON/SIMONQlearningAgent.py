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

    def set_status(self, **kwargs):
        for arg_key, arg_val in kwargs.items():
            self.state_for_agent[arg_key] = arg_val


    #
    #   select qtable element recursively.
    #
    #
    def recursive_find_qtable_element(self, qtable_element, key_idx, **kwargs):
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
        result_element = self.recursive_find_qtable_element(qtable_element[state_name], key_idx+1, **kwargs)
        return result_element

    #
    #   add the qtable element into the dictionary structure recursively.
    #
    #
    def recursive_insert_qtable_element(self, qtable_element, qvalue, key_idx, **kwargs):

        key_iterator = 0
        next_state_key = ""
        for state_key in kwargs.keys():
            if key_iterator == key_idx+1:
                next_state_key = state_key
                break
            key_iterator += 1

        if(key_idx+1 < len(kwargs.keys())):
            qtable_element[next_state_key] = OrderedDict()
            self.recursive_insert_qtable_element(qtable_element[next_state_key], qvalue, key_idx+1, **kwargs)
        else:
            qtable_element[next_state_key] = qvalue


    #
    #   choose the best actions in the decision array.
    #
    #
    def make_best_decision(self, decision_array, max_reward, max_rewarded_action, min_reward):
        for decision in decision_array:
            if(decision[1] > max_reward):
                max_reward = decision[1]
                max_rewarded_action = decision[0]
            if(decision[1] < min_reward):
                min_reward = decision[1]

    #
    #   choose the random action from the element's actions.
    #
    #
    def pick_random_action_from_object(self, element):
        random_action_name = None

        from random import randint
        random_action_idx = randint(0, len(element.Actions.values()) - 1)

        act_iterator = 0
        for act in element.Actions.values():
            if(act_iterator == random_action_idx):
                random_action_name = act.ActionName
                break
            act_iterator += 1
        return random_action_name

    #
    #   decision process by using the algorithm of reinforcement learning.
    #
    #
    def make_decision_by_reinforcement(self, element, **kwargs):

        decision_array = []

        for act in element.Actions.values():

            qtable_value = self.recursive_find_qtable_element(self.qtable_element[act], 1, **kwargs)

            if(qtable_value is None):
                # 보상값이 비어있다.
                pass
            else:
                # 보상값이 있음.
                decision_array_tuple = (act.ActionName, qtable_value)
                decision_array.append(decision_array_tuple)

        from SIMON import SIMONConstants
        max_reward = SIMONConstants.MINIMUM_CMP_VALUE()
        min_reward = SIMONConstants.MAXIMUM_CMP_VALUE()
        max_rewarded_action = None

        # chose the best action among those we could pick.
        self.make_best_decision(decision_array, max_reward, max_rewarded_action, min_reward)

        action_to_execute = None
        if max_reward == min_reward or max_rewarded_action is None:
            # 모든 보상값이 동일함. -> 랜덤으로 액션 선택.
            action_to_execute = self.pick_random_action_from_object(element)
        else:
            action_to_execute = max_rewarded_action

        return action_to_execute


    #
    #   update the q table element's value by using qlearning algorithm.
    #
    #
    def update_qtable(self, element, action, prev_state, next_state):
        # prev_state와 next_state의 구분은 이 Layer 윗단에서 해주는게 맞다.

        from SIMON import SIMONConstants
        max_q_value = SIMONConstants.MINIMUM_CMP_VALUE()

        # 재귀적으로 next state에서의 qvalue 의 최댓값을 찾음.
        for act in element.Actions.values():
            q_value = self.recursive_find_qtable_element(self.qtable[act], 1, **next_state)
            if q_value >= max_q_value:
                max_q_value = q_value

        # prev state 에서 action 을 했을 경우에 qtable 보상값을 update.
        updated_qvalue = self.reward + (self.gamma * max_q_value)
        self.recursive_insert_qtable_element(self.qtable[action], updated_qvalue, 1, **prev_state)


