__author__ = 'Park'



# 이걸 클래스화해서 싱글톤으로 쓰게 하는 것보다는, 전역 변수로 이용하게 하는게 더 좋을 듯 하다.

UserFunctionCollection = {}

#
#   Decorator for registering user simon functions.
#
#
def Register(function):
    print("[OUT]Function Named " + function.__name__ + " has been added")
    UserFunctionCollection[function.__name__] = function





