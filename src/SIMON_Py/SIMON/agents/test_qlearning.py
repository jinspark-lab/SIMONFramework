__author__ = 'Park'

from SIMON.objects import SIMONGeneticObject


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


from SIMON import SIMONCollection

from SIMON.agents.SIMONQLearningAgent import SIMONQLearningAgent

qagent = SIMONQLearningAgent(100, 0.5, gpp, **gpp.States)

qagent.run_agent(gpp, None)

print(qagent.qtable)

