__author__ = 'PARKJINSANG'

import json

class SIMONGene:

    @property
    def ElementName(self):
        return self._ElementName
    @ElementName.setter
    def ElementName(self, elementName):
        self._ElementName = elementName
    @ElementName.deleter
    def ElementName(self):
        del self._ElementName

    @property
    def ElementWeight(self):
        return self._ElementWeight
    @ElementWeight.setter
    def ElementWeight(self, elementWeight):
        self._ElementWeight = elementWeight
    @ElementWeight.deleter
    def ElementWeight(self):
        del self._ElementWeight

    def __init__(self, elementName = None, elementWeight = None):
        if(elementName is not None):
            self.ElementName = elementName
        if(elementWeight is not None):
            self.ElementWeight = elementWeight

    def ToJSON(self):
        return json.dumps(self, default=lambda o:o.__dict__, sort_keys=True, indent=4)