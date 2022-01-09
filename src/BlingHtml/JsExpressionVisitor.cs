
using System.Linq.Expressions;
using System.Text;

namespace BlingHtml;

public class JsExpressionVisitor: ExpressionVisitor
{
    protected override Expression VisitBinary(BinaryExpression node)
    {
        if(node.NodeType == ExpressionType.ArrayIndex)
        {
            Visit(node.Left);
            Write("[");
            Visit(node.Right);
            Write("]");
            return node;
        }

        var op = node.NodeType switch
        {
            ExpressionType.Add => "+",
            ExpressionType.And => "&",
            ExpressionType.Coalesce => "??",
            ExpressionType.Divide => "/",
            ExpressionType.Equal => "===",
            ExpressionType.ExclusiveOr => "^",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LeftShift => "<<",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.Modulo => "%",
            ExpressionType.Multiply => "*",
            ExpressionType.NotEqual => "!==",
            ExpressionType.Or => "||",
            ExpressionType.Power => "**",
            ExpressionType.RightShift => ">>",
            ExpressionType.Subtract => "-",
            ExpressionType.Assign => "=",
            ExpressionType.AddAssign => "+=",
            ExpressionType.AndAssign => "&&=",
            ExpressionType.DivideAssign => "/=",
            ExpressionType.ExclusiveOrAssign => "^=",
            ExpressionType.LeftShiftAssign => "<<=",
            ExpressionType.ModuloAssign => "%=",
            ExpressionType.MultiplyAssign => "*=",
            ExpressionType.OrAssign => "||=",
            ExpressionType.PowerAssign => "^=",
            ExpressionType.RightShiftAssign => ">>=",
            ExpressionType.SubtractAssign => "-=",
            _ => throw new NotImplementedException($"Unsupported binary expression type {node.NodeType}")
        };

        Write("(");
        Visit(node.Left);
        Write($" {op} ");
        Visit(node.Right);
        Write(")");

        return node;
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        Write(node.NodeType switch
        {
            ExpressionType.Convert => "convert(",
            ExpressionType.ArrayLength => string.Empty,
            ExpressionType.Negate => "-",
            ExpressionType.UnaryPlus => "+",
            ExpressionType.Not => "!(",
            ExpressionType.TypeIs => "typeIs(",
            ExpressionType.Unbox => "",
            ExpressionType.PreIncrementAssign => "++",
            ExpressionType.PreDecrementAssign => "--",
            ExpressionType.PostIncrementAssign => "",
            ExpressionType.PostDecrementAssign => "",
            ExpressionType.OnesComplement => "~(",
            _ => throw new NotImplementedException($"Unsupported unary expression type {node.NodeType}")
        });

        Visit(node.Operand);

        Write(node.NodeType switch
        {
            ExpressionType.Convert => $", \"{node.Type.Name}\")",
            ExpressionType.ArrayLength => ".length",
            ExpressionType.Not => ")",
            ExpressionType.TypeIs => $", \"{node.Type.Name}\")",
            ExpressionType.PostIncrementAssign => "++",
            ExpressionType.PostDecrementAssign => "--",
            ExpressionType.OnesComplement => ")",
            _ => ""
        });

        return node;
    }
        
    

    private void Write(string text) => _result.Append(text);
    

    private readonly StringBuilder _result = new();
}
