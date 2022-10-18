using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using static CommunityToolkit.Maui.Markup.GridRowsColumns;


namespace MauiAppFilterPanel.Controls;

public class ExpandableList : ContentView
{

    //private List<Item> values = new List<Item>();
    //private Item groupItem = new Item();

    //public string GroupItem
    //{
    //    get => groupItem.Key;
    //    set
    //    {
    //       groupItem.Key = value;
    //    }
    //}
    //public List<string> Values
    //{
    //    get
    //    {
    //        return values.Select(x => x.Key).ToList();
    //    }

    //    set
    //    {
    //        foreach (var vl in value)
    //            values.Add(new Item { Key = vl });
    //    }

    //}

    private Dictionary<string, string[]> filtredFilds = new Dictionary<string, string[]>();

    public Dictionary<string, string[]> FiltredFilds
    {
        get
        {
            return filtredFilds;
        }

        set
        {
            filtredFilds = value;
            foreach (var gr in filtredFilds)
                Initialize(gr);
        }

    }


    public ExpandableList()
    {
       
    }

    //public ExpandableList(string groupName, List<string> keyValuePairs)
    //{
    //    groupItem = new Item();
    //    values = new List<Item>();
    //    groupItem.Key = groupName;
    //    foreach (var value in keyValuePairs)
    //    {
    //        values.Add(new Item { Key = value });
    //    }
    //    Initialize();

    //}

    private void Initialize(KeyValuePair<string, string[]> group)
    {
        Item groupItem = new Item();
        groupItem.Key = group.Key;

        List<Item> values = new List<Item>();
        foreach (var val in group.Value)
            values.Add(new Item { Key = val });

        var groupNameView = new Grid
        {
            ColumnDefinitions = Columns.Define(Auto, Star, Star),
            ColumnSpacing = 2,
            Children =
            {
                new CheckBox{ BindingContext = groupItem}
                .Column(0).Row(0).Bind(".Value")
                .Invoke(checbox=>checbox.CheckedChanged+=Checbox_NameChanged),
                new Label{BindingContext = groupItem,
                    VerticalOptions = LayoutOptions.Center}
                .Column(1).Row(0).Bind(".Key"),
                new Image{Source = "expand_more.png"}
                .Size(15,25)
                 .Bind(Grid.IsVisibleProperty,
                 nameof(Item.IsExpanded),
                 converter: new InvertedBoolConverter())
                 .CenterVertical()
                .Column(2).Row(0),
                 new Image{Source = "expand_less.png"}
                .Size(15,25)
                .Bind(Grid.IsVisibleProperty, nameof(Item.IsExpanded))
                .CenterVertical()
                .Column(2).Row(0)
            }
        };
        var itemListView = new CollectionView
        {
            ItemsSource = values,
            ItemTemplate = new DataTemplate(() =>
            {
                Grid views = new Grid
                {
                    Padding = new Thickness(20, 0, 0, 0),
                    ColumnDefinitions = Columns.Define(Auto, Star),
                    Children =
                             {
                                new CheckBox().Column(0).Bind(".Value")
                                //.Bind(CheckBox.IsVisibleProperty, ".IsExpanded")
                                .Invoke(checbox=>checbox.CheckedChanged+=Checbox_ValueChanged),
                                new Label{ VerticalOptions = LayoutOptions.Center}
                                .Column(1).Bind(".Key")
                        //.Bind(CheckBox.IsVisibleProperty, ".IsExpanded")
                             }
                };
                return views;
            })
        };
        var view = new Grid
        {
            ColumnDefinitions = Columns.Define(Auto, Star),
            RowDefinitions = Rows.Define(Star, Auto),
            BindingContext = groupItem,
            Children =
            {
               groupNameView
               .Row(0)
               .TapGesture(()=>{
                    groupItem.IsExpanded=!groupItem.IsExpanded;
                }),
               itemListView
               .ColumnSpan(2)
               .Row(1)
               .Bind(Grid.IsVisibleProperty, ".IsExpanded")
            }
        };
        Content = view;

        void Checbox_NameChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                if (!values.Any(x => x.Value == true))
                    groupItem.Value = false;
                return;
            }
            if (e.Value == false)
                foreach (var v in values)
                    v.Value = false;

        }
        void Checbox_ValueChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                groupItem.Value = true;
                return;
            }
            if (!values.Any(x => x.Value == true))
            {
                groupItem.Value = false;
                return;
            }

        }
    }

   

    private class Item : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Item()
        {
            //key = "Name";
            value = false;
        }

        private string key;
        public string Key
        {
            get => key;
            set
            {
                if (key == value)
                    return;
                key = value;
                OnPropertyChanged();
            }
        }

        private bool value;
        public bool Value
        {
            get => value;
            set
            {
                if (this.value == value)
                    return;
                this.value = value;
                OnPropertyChanged();
            }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (this.isExpanded == value)
                    return;
                this.isExpanded = value;
                OnPropertyChanged();
            }
        }
    }
}