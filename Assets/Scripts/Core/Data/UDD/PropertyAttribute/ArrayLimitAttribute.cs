using System;
using UnityEngine;

namespace IGT.Core {
    [AttributeUsage(AttributeTargets.Field)]
    public class ArrayLimitAttribute : PropertyAttribute {
        public int MaxLength { get; }
        public ArrayLimitAttribute(int maxLength) { MaxLength = maxLength; }
    }
}
