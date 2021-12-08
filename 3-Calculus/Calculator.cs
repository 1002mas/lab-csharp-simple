using System;
using ComplexAlgebra;

namespace Calculus
{
    /// <summary>
    /// A calculator for <see cref="Complex"/> numbers, supporting 2 operations ('+', '-').
    /// The calculator visualizes a single value at a time.
    /// Users may change the currently shown value as many times as they like.
    /// Whenever an operation button is chosen, the calculator memorises the currently shown value and resets it.
    /// Before resetting, it performs any pending operation.
    /// Whenever the final result is requested, all pending operations are performed and the final result is shown.
    /// The calculator also supports resetting.
    /// </summary>
    ///
    /// HINT: model operations as constants
    /// HINT: model the following _public_ properties methods
    /// HINT: - a property/method for the currently shown value
    /// HINT: - a property/method to let the user request the final result
    /// HINT: - a property/method to let the user reset the calculator
    /// HINT: - a property/method to let the user request an operation
    /// HINT: - the usual ToString() method, which is helpful for debugging
    /// HINT: - you may exploit as many _private_ fields/methods/properties as you like
    class Calculator
    {
        public const char OperationPlus = '+';
        public const char OperationMinus = '-';
        
        private char? _operation;
        private Complex _result;
        private Complex _value;
        private Complex _shownValue;

        //property for the currently shown value
        public Complex Value
        {
            get => _shownValue;
            set
            {
                _value = value;
                _shownValue = _value;
            }
        }

        //a property to let the user request an operation
        public char? Operation
        {
            set
            {
                //intermediate calculations
                if (_value != null)
                {
                    ComputeResult();
                    _shownValue = null;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                //set operation
                _operation = value;
            }
        }
        
        //a method to let the user request the final result
        public void ComputeResult()
        {
            if (_result != null)
            {
                switch (_operation)
                {
                    case OperationPlus:
                    {
                        _result = _result.Plus(_value); 
                        break;
                    }
                    case OperationMinus:
                    {
                        _result = _result.Minus(_value); 
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            else
            {
                _result = _value;
            }
            _shownValue = _result;
            _operation = null;
            _value = null;
        }
        
        //method to let the user reset the calculator
        public void Reset()
        {
            _operation = null;
            _value = null;
            _result = null;
            _shownValue = null;
        }

        public override string ToString() => $"{_shownValue}, {_operation}";
    }
}