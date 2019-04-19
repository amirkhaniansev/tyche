using System;

namespace Tyche.TycheDAL.Filtration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class FilterableAttribute : Attribute
    {
    }
}