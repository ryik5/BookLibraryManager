using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LibraryManager.Utils;

/// <summary>
/// Provides attached properties for MultiSelector controls to support multiple selection.
/// Can be used in xaml.
/// 
/// Example:
/// utils:MultiSelectorExtensions.SelectedItems="{Binding DataContext.SelectedBooks, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged, RelativeSource={RelativeSource AncestorType={x:Type UserControl}, AncestorLevel=2}}"
/// 
/// where utils:
/// xmlns:utils="clr-namespace:LibraryManager.Utils"
/// 
/// DataContext.SelectedBooks (must Observable collection):
/// ObservableCollection<Book> SelectedBooks
/// </summary>
/// <author>YR 2025-02-23</author>
public class MultiSelectorExtensions
{
    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached(
        "SelectedItems",
        typeof(INotifyCollectionChanged),
        typeof(MultiSelectorExtensions),
        new PropertyMetadata(default(INotifyCollectionChanged), OnSelectedItemsChanged));

    private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var multiSelectorControl = d as MultiSelector;

        NotifyCollectionChangedEventHandler handler = (sender, args) =>
        {
            if (multiSelectorControl != null)
            {
                var listSelectedItems = multiSelectorControl.SelectedItems;
                if (args.OldItems != null)
                {
                    foreach (var item in args.OldItems)
                    {
                        if (listSelectedItems.Contains(item))
                            listSelectedItems.Remove(item);
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (var item in args.NewItems)
                    {
                        if (!listSelectedItems.Contains(item))
                            listSelectedItems.Add(item);
                    }
                }
            }
        };


        if (e.OldValue == null && multiSelectorControl != null)
            multiSelectorControl.SelectionChanged += OnSelectionChanged;

        if (e.OldValue is INotifyCollectionChanged oldValue)
            oldValue.CollectionChanged -= handler;

        if (e.NewValue is INotifyCollectionChanged newValue)
            newValue.CollectionChanged += handler;
    }

    private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var d = sender as DependencyObject;

        if (GetSelectionChangedInProgress(d))
            return;

        SetSelectionChangedInProgress(d, true);

        dynamic selectedItems = GetSelectedItems(d);

        try
        {
            foreach (var item in e.RemovedItems.Cast<dynamic>().Where(item => selectedItems.Contains(item)))
            {
                selectedItems.Remove(item);
            }
        }
        catch { }

        try
        {
            foreach (var item in e.AddedItems.Cast<dynamic>().Where(item => !selectedItems.Contains(item)))
            {
                selectedItems.Add(item);
            }
        }
        catch { }

        SetSelectionChangedInProgress(d, false);
    }

    public static void SetSelectedItems(DependencyObject element, INotifyCollectionChanged value)
    {
        element.SetValue(SelectedItemsProperty, value);
    }

    public static INotifyCollectionChanged GetSelectedItems(DependencyObject element)
    {
        return (INotifyCollectionChanged)element.GetValue(SelectedItemsProperty);
    }

    private static readonly DependencyProperty SelectionChangedInProgressProperty = DependencyProperty.RegisterAttached(
        "SelectionChangedInProgress",
        typeof(bool),
        typeof(MultiSelectorExtensions),
        new PropertyMetadata(default(bool)));

    private static void SetSelectionChangedInProgress(DependencyObject element, bool value)
    {
        element.SetValue(SelectionChangedInProgressProperty, value);
    }

    private static bool GetSelectionChangedInProgress(DependencyObject element)
    {
        return (bool)element.GetValue(SelectionChangedInProgressProperty);
    }
}