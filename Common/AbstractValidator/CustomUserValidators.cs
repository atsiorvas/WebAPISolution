using FluentValidation;
using System.Collections.Generic;

namespace Common.AbstractValidator {
    public static class CustomUserValidators {
        public static IRuleBuilderOptions<T, HashSet<TElement>>
            ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T,
                HashSet<TElement>> ruleBuilder,
            int num) {
            return ruleBuilder.Must(list => list.Count < num)
                .WithMessage("The list contains too many items");
        }
    }
}