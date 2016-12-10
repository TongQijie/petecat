using Petecat.DependencyInjection.Attribute;
using System;
namespace Petecat.Data
{
    [DependencyInjectable(Inference = typeof(IComparer), Singleton = true)]
    public class Comparer : IComparer
    {
        public int Compare(params Func<int>[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
            {
                return 0;
            }

            foreach (var condition in conditions)
            {
                var result = condition.Invoke();
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }
    }
}