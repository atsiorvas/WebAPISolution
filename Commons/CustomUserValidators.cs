using FluentValidation;
using System.Collections.Generic;

namespace Common {
    public static class CustomUserValidators {
        public static IRuleBuilderOptions<T, List<TElement>>
            ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T,
                List<TElement>> ruleBuilder,
            int num) {
            return ruleBuilder.Must(list => list.Count < num)
                .WithMessage("The list contains too many items");
        }
    }
}