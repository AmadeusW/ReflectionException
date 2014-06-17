using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace DemoSolution
{
    // Install-Package Microsoft.CodeAnalysis -Version 0.6.4033103-beta -Pre
    // Install-Package Microsoft.CodeAnalysis -Pre
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            System.Diagnostics.Debug.WriteLine("Loading solution...");
            var buildWorkspace = MSBuildWorkspace.Create();
            // Any solution will work:
            var solution = buildWorkspace.OpenSolutionAsync(@"C:\Users\Amadeus\Documents\GitHub\ReflectionException\DemoSolution\DemoSolution.sln").Result;

            System.Diagnostics.Debug.WriteLine("Analyzing solution...");
            foreach (var project in solution.Projects)
            {
                foreach (var document in project.Documents)
                {
                    var syntaxRoot = document.GetSyntaxRootAsync().Result;
                    var classes = syntaxRoot.DescendantNodes().OfType<ClassDeclarationSyntax>();

                    foreach (var currentClass in classes)
                    {
                        try
                        {
                            var model = document.GetSemanticModelAsync().Result;
                            // The following line crashes when using 
                            //  Microsoft.Bcl.Metadata.1.0.11-alpha and Roslyn 0.6
                            // it works fine when using
                            // Microsoft.Bcl.Metadata.1.0.9 - alpha
                            string fullName = model.GetDeclaredSymbol(currentClass).ToString();
                            System.Diagnostics.Debug.WriteLine("\t" + fullName);
                        }
                        catch (Exception ex)
                        {
                            Assert.Fail("We've got an excception " + ex);
                        }
                    }
                }
            }
        }
    }
}
