using System.Reflection;
using System.Text;

namespace Domain.DTO;

public class Entidade
{
    public int? Id;

    public override string ToString()
    {
        Type type = GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

        StringBuilder stringBuilder = new StringBuilder();
        foreach (FieldInfo field in fields)
        {
            stringBuilder.Append($"{field.Name}: {field.GetValue(this)} ");
        }

        return stringBuilder.ToString();
    }
}
