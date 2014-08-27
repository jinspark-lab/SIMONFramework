__author__ = 'PARKJINSANG'


# dictionary 들을 담은 list.
state_map = []

from collections import OrderedDict

#
#   find whether the state is able to be mapped.
#
#
def find_mapped_state(states, mapping_key_list):

    for state in states:
        correct_flag = 1
        args_chk_iterator = 0
        if(len(state) != len(mapping_key_list)):
            continue
        for state_element_key in state.keys():
            if(mapping_key_list[args_chk_iterator] != state_element_key):
                correct_flag = 0
                break
            args_chk_iterator += 1
        if(correct_flag == 0):
            continue
        return True
    return False

#
#   mapping key_list and value_list from the user defined state dictionary.
#
#
def map_defined_state(mapping_key_list, mapping_value_list, **kwargs):
    mapping_dictionary = OrderedDict()
    for mapping_key, mapping_value in kwargs.items():
        mapping_key_list.append(mapping_key)
        mapping_value_list.append(mapping_value)
        mapping_dictionary[mapping_key] = mapping_value

    return mapping_dictionary

#
#   mapping user defined state dictionary to the state map.
#
#
def mapping_state(**kwargs):
    mapping_key_list = []
    mapping_value_list = []

    mapping_dictionary = map_defined_state(mapping_key_list, mapping_value_list, **kwargs)

    if(len(state_map) < 1):
        state_map.append([])
        state_map[len(state_map)-1].append(mapping_dictionary)
        pass

    not_in_state_map = True
    for states in state_map:
        if(find_mapped_state(states, mapping_key_list) is True):
            states.append(mapping_dictionary)
            not_in_state_map = False
            break
    if(not_in_state_map is True):
        state_map.append([])
        state_map[len(state_map)-1].append(mapping_dictionary)


