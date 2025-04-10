using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Utils.ScriptHandler.Impl
{
    public class ControllersScriptsHandler : IScriptHandler
    {
        private const string INSTALL_ATTRIBUTE_NAME = "Install";
        
        private readonly InstallerTemplate _installers = new();

        public void HandleSyntaxRoot(SyntaxNode root)
        {
            var controllers = root.DescendantNodes()
                              .OfType<ClassDeclarationSyntax>()
                              .Where(HasTargetAttribute)
                              .ToList();

            if (controllers.Count <= 0)
                return;

            var stringNamespacesAsRawUsings = root.DescendantNodes()
                                                  .OfType<NamespaceDeclarationSyntax>()
                                                  .Select(u => u.Name!.ToString())
                                                  .ToArray();
            _installers.Namespaces.AddRange(stringNamespacesAsRawUsings);
            

            foreach (var controller in controllers)
            {
                var installAttribute = controller.AttributeLists
                    .SelectMany(al => al.Attributes)
                    .First(a => a.Name.ToString() == INSTALL_ATTRIBUTE_NAME);
                
                var arguments = installAttribute.ArgumentList!.Arguments;
                
                var priorityArg = arguments[0].Expression;
                var priority = EExecutionPriority.Urgent;
                if (priorityArg is MemberAccessExpressionSyntax memberAccess)
                {
                    var priorityStr = memberAccess.Name.ToString();
                    if (!Enum.TryParse(priorityStr, out priority))
                        continue;
                }
        
                var order = 0;
                if (arguments.Count > 1)
                {
                    var orderArg = arguments[1].Expression;
                    if (orderArg is LiteralExpressionSyntax literal && 
                        literal.Kind() == SyntaxKind.NumericLiteralExpression)
                    {
                        int.TryParse(literal.Token.ValueText, out order);
                    }
                }
                
                _installers.Counter++;

                if (!_installers.Container.ContainsKey(priority))
                    _installers.Container[priority] = new List<TypeElement>();

                _installers.Container[priority].Add(new TypeElement(controller.Identifier.Text, priority, order));
            }
        }

        public InstallerTemplate GetParsedInstallersTemplates() => _installers;

        private static bool HasTargetAttribute(BaseTypeDeclarationSyntax node)
        {
            return node.AttributeLists
                       .SelectMany(al => al.Attributes)
                       .Any(a => a.Name.ToString() == INSTALL_ATTRIBUTE_NAME);
        }
    }
}