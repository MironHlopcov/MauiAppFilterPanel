using CommunityToolkit.Maui.Markup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;


namespace MauiAppFilterPanel.Controls;

public class ExpandableList : ContentView
{
    public bool IsExpanded = false;
    private Item groupItem = new Item();
    private List<Item> values = new List<Item>();
  
    public ExpandableList()
    {
        for (int i = 0; i <= 10; i++)
        {
            values.Add(new Item
            {
                Key = i.ToString(),
                Value = false
            });
        }

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
                .TapGesture(()=>{
                    groupItem.IsExpanded=!groupItem.IsExpanded;
                })
                .Column(2).Row(0)
            }
        };
        var itemList = new CollectionView
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
            BindingContext= groupItem,
            Children =
            {
               groupNameView.Row(0),
               itemList.ColumnSpan(2).Row(1).Bind(Grid.IsVisibleProperty, ".IsExpanded")
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
            // else
            // groupItem.Value = false;
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
            key = "Name";
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