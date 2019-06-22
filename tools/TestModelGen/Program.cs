using modelgen;
using System;

namespace TestModelGen
{
    class Program
    {
        static void Main(string[] args)
        {
            var usingNamespaceSystem = string.Format(Formats.Namespace, "System");
            var usingNamespaceNumerics = string.Format(Formats.Namespace, "System.Numerics");
            var namespaces = string.Concat(
                usingNamespaceSystem, "\n", usingNamespaceNumerics);
            var propertySummary = string.Format(Formats.PropertySummary, "Gets or sets ID.");
            var property = string.Format(Formats.Property, "Id");
            var propertyWithSummary = string.Format(Formats.PropertyWithSummary, propertySummary, property);
            var classSummary = string.Format(Formats.ClassSummary, "Model for describing entity.");
            var classW = string.Format(Formats.Class, "Model", "DbModel", propertyWithSummary);
            var classWithSummary = string.Format(Formats.ClassWithSummary, classSummary, classW);
            var namespaceW = string.Format(Formats.NamespaceClause, "Tyche", classWithSummary);
            var file = string.Format(Formats.File, namespaces, namespaceW);
        }
    }
}
