﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OfficeManagment.Infrastructure
{
    public interface ISearchExpressionProvider
    {
        IEnumerable<string> GetOperators();

        ConstantExpression GetValue(string input);

        Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right);
    }
}
