using System.Collections.Immutable;
using Alto.CodeAnalysis.Binding;
using Alto.CodeAnalysis.Syntax;

namespace Alto.CodeAnalysis.Lowering
{
    internal sealed class Lowerer : BoundTreeRewriter
    {
        private Lowerer()
        {
            
        }

        public static BoundStatement Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            return lowerer.RewriteStatement(statement);
        }

        protected override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            // for i = 0 to 10
            //    print i

            // --->

            //{
            //     var i = 0
            //     while (i <= upper)
            //     {
            //         print i
            //         i = i + 1
            //     }
            //}
            
            var variableDeclaration = new BoundVariableDeclaration(node.Variable, node.LowerBound);
            var variableExpression = new BoundVariableExpression(node.Variable);

            var condition = new BoundBinaryExpression(
                variableExpression,
                BoundBinaryOperator.Bind(SyntaxKind.LesserOrEqualsToken, typeof(int), typeof(int)), 
                node.UpperBound
            );
            
            var increment = new BoundExpressionStatement(
                new BoundAssignmentExpression(node.Variable, 
                    new BoundBinaryExpression(
                        variableExpression, 
                        BoundBinaryOperator.Bind(SyntaxKind.PlusToken, typeof(int), typeof(int)),
                        new BoundLiteralExpression(1)
                    )
                )
            );
            
            var whileBody = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(node.Body, increment));
            var whileStatement = new BoundWhileStatement(condition, whileBody);
            var result = new BoundBlockStatement(ImmutableArray.Create<BoundStatement>(variableDeclaration, whileStatement));

            return RewriteStatement(result);
        }
    }
}