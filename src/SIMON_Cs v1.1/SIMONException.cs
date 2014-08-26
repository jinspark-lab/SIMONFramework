using System;
using System.Collections.Generic;
using System.Text;

namespace SIMONFramework
{

    /// <summary>
    /// Framework 에서 적합한 Key값을 찾지 못했을 경우 발생하는 예외를 정의합니다.
    /// </summary>
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException() : base() { }
        public KeyNotFoundException(string message) : base(message) { }
        public KeyNotFoundException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 에서 접근 권한이 발생했을 때, 예외 상황을 정의합니다.
    /// </summary>
    public class AccessViolationException : Exception
    {
        public AccessViolationException() : base() { }
        public AccessViolationException(string message) : base(message) { }
        public AccessViolationException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 의 자료구조 내에서 Index 오류가 발생 시 해당 예외를 정의합니다.
    /// </summary>
    public class IndexOutOfRangeException : Exception
    {
        public IndexOutOfRangeException() : base() { }
        public IndexOutOfRangeException(string message) : base(message) { }
        public IndexOutOfRangeException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 에서 입력된 Argument 인자목록에 대한 NULL 값 참조시 발생하는 예외를 정의합니다.
    /// </summary>
    public class ArgumentNullException : Exception
    {
        public ArgumentNullException() : base() { }
        public ArgumentNullException(string message) : base(message) { }
        public ArgumentNullException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 에서 현재 구현되지 않은 부분이나, 지원하지 않는 코드에 대한 접근 시 발생하는 예외를 정의합니다.
    /// </summary>
    public class NotImplementedException : Exception
    {
        public NotImplementedException() : base() { }
        public NotImplementedException(string message) : base(message) { }
        public NotImplementedException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 동작에 문제가 발생할 만한 값의 Overflow에 따른 예외 상황을 정의합니다.
    /// </summary>
    public class ValueOverflowException : Exception
    {
        public ValueOverflowException() : base() { }
        public ValueOverflowException(string message) : base(message) { }
        public ValueOverflowException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 동작에 문제가 발생할 만한 값의 Underflow에 따른 예외 상황을 정의합니다.
    /// </summary>
    public class ValueUnderflowException : Exception
    {
        public ValueUnderflowException() : base() { }
        public ValueUnderflowException(string message) : base(message) { }
        public ValueUnderflowException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 의 학습 루틴에서 발생하는 각종 예외 상황을 정의합니다.
    /// </summary>
    public class LearningFaultException : Exception
    {
        public LearningFaultException() : base() { }
        public LearningFaultException(string message) : base(message) { }
        public LearningFaultException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

    /// <summary>
    /// Framework 내 외부에서 잘못된 Algorithm Layer에 대한 참조 발생 시 발생하는 예외를 정의합니다.
    /// </summary>
    public class InvalidAlgorithmClassException : Exception
    {
        public InvalidAlgorithmClassException() : base() { }
        public InvalidAlgorithmClassException(string message) : base(message) { }
        public InvalidAlgorithmClassException(string message, Exception e) : base(message, e) { }

        public string ExceptionInfo { get; set; }
    }

}
