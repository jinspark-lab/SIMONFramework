__author__ = 'PARKJINSANG'

import json

class SIMONGeneticProperty:
    PropertyName = ""
    PropertyValue = 0
    Heritability = False

    def __init__(self, propertyName = None, propertyValue = None, heritability = None):
        if(propertyName is not None):
            self.PropertyName = propertyName
        if(propertyValue is not None):
            self.PropertyValue = propertyValue
        if(heritability is not None):
            self.Heritability = heritability

    def ToJSON(self):
        return json.dumps(self, default=lambda o:o.__dict__, sort_keys=True, indent=4)

