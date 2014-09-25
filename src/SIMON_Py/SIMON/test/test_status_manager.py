__author__ = 'Park'


from SIMON import SIMONStatusManager


SIMONStatusManager.mapping_state(A=1, B=2, C=3, D=4)
SIMONStatusManager.mapping_state(A=10, B=20, C=30, D=40)
SIMONStatusManager.mapping_state(A=1, B=2, C=3, D=4)
SIMONStatusManager.mapping_state(A=1, B=2, X=-3, Y=40)
SIMONStatusManager.mapping_state(A=1, B=2, C=3, D=4, E=0)
SIMONStatusManager.mapping_state(A=1, B=2, X=19, D='A')

d  = SIMONStatusManager.state_map

print(d)

