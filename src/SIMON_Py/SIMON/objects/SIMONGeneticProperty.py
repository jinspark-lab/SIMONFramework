__author__ = 'PARKJINSANG'

import json

class SIMONGeneticProperty:
    PropertyName = ""
    PropertyValue = 0
    Heritability = False

#    @property
#    def PropertyName(self):
#        return self._PropertyName
#    @PropertyName.setter
#    def PropertyName(self, propertyName):
#        self._PropertyName = propertyName
#    @PropertyName.deleter
#    def PropertyName(self):
#        del self._PropertyName

#    @property
#    def PropertyValue(self):
#        return self._PropertyValue
#    @PropertyValue.setter
#    def PropertyValue(self, propertyValue):
#        self._PropertyValue = propertyValue
#    @PropertyValue.deleter
#    def PropertyValue(self):
#        del self._PropertyValue

#    @property
#    def Heritability(self):
#        return self._Heritability
#    @Heritability.setter
#    def Heritability(self, heritability):
#        self._Heritability = heritability
#    @Heritability.deleter
#    def Heritability(self):
#        del self._Heritability

    def __init__(self, propertyName = None, propertyValue = None, heritability = None):
        if(propertyName is not None):
            self.PropertyName = propertyName
        if(propertyValue is not None):
            self.PropertyValue = propertyValue
        if(heritability is not None):
            self.Heritability = heritability

    def ToJSON(self):
        return json.dumps(self, default=lambda o:o.__dict__, sort_keys=True, indent=4)