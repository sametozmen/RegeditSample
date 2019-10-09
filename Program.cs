using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace RegeditSample
{
    class Program
    {
        static void Main(string[] args)
        {
            returnValue();
        }




        public static void returnValue()
        {
            Registry.Users.GetSubKeyNames().Select(x => Registry.Users.OpenSubKey(x))
            .Traverse(key =>
            {
                if (key != null)
                {
                    return key.GetSubKeyNames().Select(subKey => key.OpenSubKey(subKey));
                }

                return null;
            })
            .ForEach(key =>
            {
                key.GetValueNames().ForEach(valueName => Console.WriteLine("{0}\\{1}:{2} ({3})", key, valueName, key.GetValue(valueName), key.GetValueKind(valueName)));
            });

            Console.ReadLine();
        }
    }
    public static class Extensions
    {
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
            foreach (T item in source)
            {
                yield return item;

                IEnumerable<T> seqRecurse = fnRecurse(item);

                if (seqRecurse != null)
                {
                    foreach (T itemRecurse in Traverse(seqRecurse, fnRecurse))
                    {
                        yield return itemRecurse;
                    }
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }




        }
    }
}
