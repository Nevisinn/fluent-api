using System.Reflection;

namespace ObjectPrinting.Configs;

public class StringMemberPrintingConfig<TOwner> : MemberPrintingConfig<TOwner, string>
{
    private readonly MemberInfo memberInfo;
    private readonly PrintingConfig<TOwner> parent;

    public StringMemberPrintingConfig(PrintingConfig<TOwner> parent, MemberInfo memberInfo, PrintingContext context)
        : base(parent, memberInfo, context)
    {
        this.parent = parent;
        this.memberInfo = memberInfo;
    }

    public PrintingConfig<TOwner> Trim(int maxLength)
    {
        if (context.memberSerializers.TryGetValue(memberInfo, out var prevSerializer))
            context.memberSerializers[memberInfo] = obj =>
            {
                var intermediateResult = prevSerializer(obj);
                return intermediateResult.Length <= maxLength ? intermediateResult : intermediateResult[..maxLength];
            };
        else
            return Using(s => s.Length <= maxLength ? s : s[..maxLength]);

        return parent;
    }
}