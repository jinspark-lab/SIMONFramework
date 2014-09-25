__author__ = 'Park'

from SIMON.objects.SIMONGeneticObject import SIMONGeneticObject


gObj = SIMONGeneticObject(None)
gObj.ObjectID = "MyObj"

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
