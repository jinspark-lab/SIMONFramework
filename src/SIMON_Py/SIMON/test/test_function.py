__author__ = 'Park'

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
