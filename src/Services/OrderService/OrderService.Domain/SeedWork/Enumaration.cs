﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.SeedWork
{
    public abstract class Enumaration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        protected Enumaration(int id, string name) => (Id, Name) = (id, name);
        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumaration
        {
            return typeof(T)
                    .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly)
                    .Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            if (obj is not Enumaration other)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(other.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();
        public static T FromValue<T>(int value) where T : Enumaration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumaration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumaration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new ArgumentException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public int CompareTo(object obj) => Id.CompareTo(((Enumaration)obj).Id);
    }
}
