__author__ = 'Park'

from collections import OrderedDict
import json

import SIMON

class SIMONObjectSerializer:

    ObjectContainer = OrderedDict()

    def SaveJSON(self, fileName, jsonData):
        with open(fileName, 'w', encoding='utf-8') as outfile:
            json.dump(jsonData, outfile)

    def LoadJSON(self, fileName):
        jsonData = open(fileName)
        data = json.load(jsonData)
        jsonData.close()
        return data

    # simon_object 객체를 JSON으로 Serializer 해줍니다.
    def ToJSON(self, simon_object):
        return json.dumps(simon_object, default=lambda o:simon_object.__dict__, sort_keys=True, indent=4 )

    # json_data 로부터 Deserialize 하여 simon_object로 업데이트 시킵니다.
    def FromJSON(self, simon_object, json_data):
        simon_object.__dict__ = json.loads(json_data)

    # json_data 로부터 객체를 생성해서 반환해줍니다. 뽑아낸 객체는 컨테이너에 담겨서 관리됩니다.
    def BuildObject(self, json_data):
        newObject = SIMON.SIMONGeneticObject.SIMONGeneticObject()
        self.FromJSON(newObject, json_data)
        self.ObjectContainer[newObject.ObjectID] = newObject
        return newObject

    # objectID를 키값으로 관리되는 컨테이너에서 해당 정보를 가진 개체를 생산합니다.
    def ProduceObject(self, objectID):
        newObject = self.ObjectContainer[objectID];
        return newObject

