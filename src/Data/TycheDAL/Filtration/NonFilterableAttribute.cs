using System;

namespace Tyche.TycheDAL.Filtration
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonFilterableAttribute : Attribute
    {
    }
}