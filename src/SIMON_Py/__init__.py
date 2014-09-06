__author__ = 'PARKJINSANG'

from SIMON.SIMONManager import SIMONManager
from SIMON.objects.SIMONGeneticObject import SIMONGeneticObject

gObj = SIMONGeneticObject(None)
gObj.ObjectID = "MyObj"


sManager = SIMONManager("Test")


from SIMON import SIMONFunction


@SIMONFunction.Register
def Foo1():
    return "Hoo"

@SIMONFunction.Register
def Foo2():
    return "Gaa"


@SIMONFunction.Register
def AttackAction(element, others):
    from random import randint
    return randint(0, 10)

@SIMONFunction.Register
def AttackExecution(element, others):
    from random import randint
    return randint(0, 10)

@SIMONFunction.Register
def AttackFitness(element, others):
    from random import randint
    return randint(0, 10)

@SIMONFunction.Register
def DefenceAction(element, others):
    from random import randint
    return randint(0, 5)

@SIMONFunction.Register
def DefenceExecution(element, others):
    from random import randint
    return randint(0, 5)

@SIMONFunction.Register
def DefenceFitness(element, others):
    from random import randint
    return randint(0, 5)

@SIMONFunction.Register
def MoveAction(element, others):
    from random import randint
    return randint(0, 100)
@SIMONFunction.Register
def MoveExecution(element, others):
    from random import randint
    return randint(0, 100)
@SIMONFunction.Register
def MoveFitness(element, others):
    from random import randint
    return randint(0, 100)
@SIMONFunction.Register
def TestProperty(element, others):
    from random import randint
    return randint(0, 100)

print(SIMONFunction.UserFunctionCollection)

gObj.AssignProperty("TestDouble", 0.2, True, "TestProperty")
gObj.AssignProperty("Attack", 3.2, True, "TestProperty")
gObj.AssignProperty("Agile", 5.0, True, "TestProperty")
gObj.AssignAction("Attack", "AttackAction", "AttackExecution", "AttackFitness", None)
gObj.AssignAction("Defence", "DefenceAction", "DefenceExecution", "DefenceFitness", None)
gObj.AssignDNA("Attack", WEAPON=100.9, STRENGTH=0.6, SPEED=12.4)
gObj.AssignDNA("Defence", STRENGTH=0.4, ARMOR=0.2)

gpp = SIMONGeneticObject(objectID="TempObj")
gpp.AssignID("RealID")
gpp.AssignProperty("HP", 100, True, "TestProperty")
gpp.AssignProperty("Attack", 10, True, "TestProperty")
gpp.AssignProperty("Mana", 0.5, True, "TestProperty")
# gpp.AssignAction("Move", "MoveAction", "MoveExecution", "MoveFitness", None)
gpp.AssignAction("Attack", "AttackAction", "AttackExecution", "AttackFitness", None)
gpp.AssignDNA("Attack", WEAPON=0.9, STRENGTH=1.6, SPEED=0.4)
gpp.AssignAction("Defence", "DefenceAction", "DefenceExecution", "DefenceFitness", None)
gpp.AssignDNA("Defence", STRENGTH=1.0, ARMOR=1.0)


gsec = SIMONGeneticObject(objectID="sec")
gsec.AssignProperty("HP", 70, True, "TestProperty")
gsec.AssignProperty("Attack", 8, True, "TestProperty")
gsec.AssignProperty("Mana", 1, True, "TestProperty")
gsec.AssignAction("Attack", "AttackAction", "AttackExecution", "AttackFitness", None)
gsec.AssignDNA("Attack", WEAPON=0.1, STRENGTH=3.0, SPEED=0.7)

gList = []
gList.append(gpp)
gList.append(gObj)
gList.append(gsec)

"""

print("---- ActionMap Test : ")
map = sManager.create_actionmap(gList)


for learn_iter in range(1, 100):
    print("==============Run sprint==============")

    sManager.run_routine()

    print("==============Learn============")

    sManager.learn_genetic_model(gList)

    print("Go")
    print(gpp.Actions["Attack"].ActionDNA)
    print(gObj.Actions["Attack"].ActionDNA)
    print(gObj.Actions["Defence"].ActionDNA)

    for pdna in gpp.PropertyDNA.values():
        print(pdna.PropertyValue)
    for gdna in gObj.PropertyDNA.values():
        print(gdna.PropertyValue)
    for gsna in gsec.PropertyDNA.values():
        print(gsna.PropertyValue)

#    for ckey, celement in SIMONCollection.SIMONObjectCollection.items():
#        print("[key] : " + ckey)
#        for val in celement.PropertyDNA.values():
#            print(val.PropertyValue)

print("Done")

"""
"""
from SIMON import SIMONStatusManager


SIMONStatusManager.mapping_state(A=1, B=2, C=3, D=4)
SIMONStatusManager.mapping_state(A=10, B=20, C=30, D=40)
SIMONStatusManager.mapping_state(A=1, B=2, C=3, D=4)
SIMONStatusManager.mapping_state(A=1, B=2, X=-3, Y=40)
SIMONStatusManager.mapping_state(A=1, B=2, C=3, D=4, E=0)
SIMONStatusManager.mapping_state(A=1, B=2, X=19, D='A')

d  = SIMONStatusManager.state_map

print(d)
"""

from SIMON import SIMONCollection

from SIMON.agents.SIMONQLearningAgent import SIMONQLearningAgent

qagent = SIMONQLearningAgent(100, 0.5, gpp, **gpp.States)

qagent.run_agent(gpp, None)

print(qagent.qtable)

