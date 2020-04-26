using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using JetBrains.Annotations;

namespace Common {
    public class WeakList<T> where T : class {
        private readonly List<WeakReference<T>> list = new List<WeakReference<T>>();

        public void Add(T item) {
            list.Add(new WeakReference<T>(item));
        }

        public T this[int index] => list[index].GetOrNull();

        [CanBeNull]
        public T GetFirstFrom(int index) {
            try {
                var returnValue = list[index].GetOrNull();
                if (returnValue != null) return returnValue;
                list.RemoveAt(index);
                return GetFirstFrom(index);
            } catch (IndexOutOfRangeException) {
                return null;
            }
        }

        public WeakList<T> Clean() => 
            this.Also(_ => list.RemoveAll(it => !it.TryGetTarget(out var x)));

        public List<T> ToList() => list.ConvertAll(it => it.GetOrNull());

        public List<T> ToListClean() => ToList().Where(it => it != null).ToList();

        public List<T> FindAll(Predicate<T> match) => ToList().FindAll(match);

        public List<T> FindAllClean(Predicate<T> match) => ToListClean().FindAll(match);

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => ToList().ConvertAll(converter);
        
        public List<TOutput> ConvertAllClean<TOutput>(Converter<T, TOutput> converter) => ToListClean().ConvertAll(converter);
    }
}
