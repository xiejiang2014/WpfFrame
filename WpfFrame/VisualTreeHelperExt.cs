using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WpfFrame
{
    public static class VisualTreeHelperExt
    {
        /// <summary>
        /// 利用 VisualTreeHelper 寻找对象的子级对象
        /// <typeparam name="T"></typeparam>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T childT)
                {
                    yield return childT;
                }

                var childOfChildren = child.FindVisualChild<T>();

                foreach (var childOfChild in childOfChildren)
                {
                    yield return childOfChild;
                }
            }
        }

        public static IEnumerable<DependencyObject> FindVisualChild(this DependencyObject obj, Func<DependencyObject, bool> predicate)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (predicate(child))
                {
                    yield return child;
                }

                var childOfChildren = child.FindVisualChild(predicate);

                foreach (var childOfChild in childOfChildren)
                {
                    yield return childOfChild;
                }
            }
        }
    }
}