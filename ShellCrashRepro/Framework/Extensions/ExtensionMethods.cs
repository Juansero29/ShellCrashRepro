using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EnigmatiKreations.Framework.Utils.Extensions
{
    public static class ExtensionMethods
    {
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        public static async void FireAndForgetSafeAsync(this Task task, ILog handler = null)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.Error(ex.Message);
            }
        }
        public static decimal ToInvariantDecimalNumber(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;
            if (value.Contains(','))
            {
                var provider = new CultureInfo("fr-FR");
                decimal.TryParse(value, NumberStyles.Number, provider, out decimal decimalFromComma);
                return decimalFromComma;
            }
            else
            {
                var provider = CultureInfo.InvariantCulture;
                decimal.TryParse(value, NumberStyles.Number, provider, out decimal decimalFromPoint);
                return decimalFromPoint;
            }
        }
        public static double ToInvariantDoubleNumber(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return default;
            if (value.Contains(','))
            {
                var provider = new CultureInfo("fr-FR");
                double.TryParse(value, NumberStyles.Number, provider, out double doubleFromComma);
                return doubleFromComma;
            }
            else
            {
                var provider = CultureInfo.InvariantCulture;
                double.TryParse(value, NumberStyles.Number, provider, out double doubleFromPoint);
                return doubleFromPoint;
            }
        }



        #region Finding Visual Elements
        public static T GetTemplateChild<T>(this Element parent, string name) where T : Element
        {
            if (parent == null)
                return null;

            T templateChild;

            foreach (var child in FindVisualChildren<Element>(parent, false))
            {
                templateChild = GetTemplateChild<T>(child, name);
                if (templateChild != null)
                    return templateChild;
            }

            try
            {
                templateChild = parent.FindByName<T>(name);
            }
            catch (InvalidOperationException)
            {
                templateChild = null;
            }

            return templateChild;
        }


        public static IEnumerable<T> FindVisualChildren<T>(this Element element, bool recursive = true) where T : Element
        {
            if (element != null && element is Layout)
            {
                var childrenProperty = element.GetType().GetProperty("InternalChildren", BindingFlags.Instance | BindingFlags.NonPublic);
                if (childrenProperty != null)
                {
                    var children = (IEnumerable<Element>)childrenProperty.GetValue(element);
                    foreach (var child in children)
                    {
                        if (child != null && child is T)
                        {
                            yield return (T)child;
                        }
                        if (recursive)
                        {
                            foreach (T childOfChild in FindVisualChildren<T>(child))
                            {
                                yield return childOfChild;
                            }
                        }
                    }
                }
            }

        }


        /// <summary>
        /// Recherche le premier parent du type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T FindVisualAncestor<T>(this Element parent) where T : Element
        {
            if (parent == null)
                return null;
            while (parent != null)
            {
                if (parent is T t)
                    return t;
                parent = parent.Parent;
            }
            return null;
        }

        /// <summary>
        /// Cherche le parent de type T.
        /// Null si pas trouvé.
        /// Si trouvé renvoi la liste de tous les ancestres. Le dernier de la liste correpondant a celui cherché
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<Element> FindVisualAncestorWithAncestorList<T>(this Element parent) where T : Element
        {
            return FindVisualAncestorWithAncestorList<T>(parent, false);
        }


        /// <summary>
        /// Cherche le parent de type T.
        /// Null si pas trouvé et giveAncestorFinded == false. sinon on retourne les ancetres trouvé quoi qu'il arrive
        /// Si trouvé renvoi la liste de tous les ancestres. Le dernier de la liste correpondant a celui cherché
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<Element> FindVisualAncestorWithAncestorList<T>(this Element parent, bool giveAncestorFinded) where T : Element
        {
            if (parent == null)
                return null;
            var ancestors = new List<Element>();
            while (parent != null)
            {
                ancestors.Add(parent);
                if (parent is T)
                    return ancestors;
                parent = parent.Parent;
            }
            if (giveAncestorFinded)
                return ancestors;
            return null;
        }
        #endregion
    }
}
