from SIMON.objects import SIMONGeneticObject

__author__ = 'PARKJINSANG'

from SIMON.SIMONManager import SIMONManager
from SIMON.objects.SIMONGeneticObject import SIMONGeneticObject

gObj = SIMONGeneticObject(None)
gObj.ObjectID = "MyObj"


sManager = SIMONManager("Test")


from SIMON import SIMONFunction
from SIMON import SIMONCollection

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

gObj.AssignProperty("TestDouble", 0.2, True)
gObj.AssignProperty("Attack", 3.2, True)
gObj.AssignProperty("Agile", 5.0, True)
gObj.AssignAction("Attack", "AttackAction", "AttackExecution", "AttackFitness", None)
gObj.AssignAction("Defence", "DefenceAction", "DefenceExecution", "DefenceFitness", None)
gObj.AssignDNA("Attack", WEAPON=100.9, STRENGTH=0.6, SPEED=12.4)
gObj.AssignDNA("Defence", STRENGTH=0.4, ARMOR=0.2)
gObj.ObjectFunctionName = "TestProperty"

gpp = SIMONGeneticObject(objectID="TempObj")
gpp.AssignID("RealID")
gpp.AssignProperty("HP", 100, True)
gpp.AssignProperty("Attack", 10, True)
gpp.AssignProperty("Mana", 0.5, True)
# gpp.AssignAction("Move", "MoveAction", "MoveExecution", "MoveFitness", None)
gpp.AssignAction("Attack", "AttackAction", "AttackExecution", "AttackFitness", None)
gpp.AssignDNA("Attack", WEAPON=0.9, STRENGTH=1.6, SPEED=0.4)
gpp.AssignAction("Defence", "DefenceAction", "DefenceExecution", "DefenceFitness", None)
gpp.AssignDNA("Defence", STRENGTH=1.0, ARMOR=1.0)
gpp.ObjectFunctionName = "TestProperty"

gsec = SIMONGeneticObject(objectID="sec")
gsec.AssignProperty("HP", 70, True)
gsec.AssignProperty("Attack", 8, True)
gsec.AssignProperty("Mana", 1, True)
gsec.AssignAction("Attack", "AttackAction", "AttackExecution", "AttackFitness", None)
gsec.AssignDNA("Attack", WEAPON=0.1, STRENGTH=3.0, SPEED=0.7)
gsec.ObjectFunctionName = "TestProperty"

gList = []
gList.append(gpp)
gList.append(gObj)
gList.append(gsec)

print("---- ActionMap Test : ")
map = sManager.create_actionmap(gList)


for learn_iter in range(1, 100):
    print("==============Run sprint==============")

    sManager.run_routine(1)

    print("==============Learn============")

    sManager.learn_model(gList)

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
